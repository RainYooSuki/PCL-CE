using System.IO;
using System.Net.Http;
using System.Text.Json.Nodes;
using PCL.Core.App.Localization;
using PCL.Core.IO.Net.Http;
using PCL.Core.Utils;
using PCL.Network;
using PCL.Network.Loaders;

namespace PCL;

/// <summary>
///     从当前项目仓库的 GitHub Releases 获取更新。
/// </summary>
public class UpdatesGitHubReleaseModel : IUpdateSource
{
    public const string RepositoryOwner = "RainYooSuki";
    public const string RepositoryName = "PCL-CE";
    public const string RepositoryUrl = "https://github.com/" + RepositoryOwner + "/" + RepositoryName;
    public const string ReleasesUrl = RepositoryUrl + "/releases";
    public const string LatestReleasePageUrl = ReleasesUrl + "/latest";
    public const string IssuesUrl = RepositoryUrl + "/issues";

    private const string RepositoryApiUrl =
        "https://api.github.com/repos/" + RepositoryOwner + "/" + RepositoryName;

    public const string IssuesApiUrl = RepositoryApiUrl + "/issues";
    private const string LatestReleaseApiUrl = RepositoryApiUrl + "/releases/latest";

    private readonly Dictionary<UpdateArch, VersionDataModel> _versionCache = new();
    private JsonObject? _release;

    public string SourceName { get; set; } = "GitHub Releases";

    public bool IsAvailable() => true;

    public bool RefreshCache()
    {
        using var response = CreateGitHubRequest(LatestReleaseApiUrl)
            .SendAsync(addMetedata: false).GetAwaiter().GetResult();

        var json = ModBase.GetJson(response.AsString());
        if (json is not JsonObject release || release["tag_name"] is null)
        {
            var message = (json as JsonObject)?["message"]?.ToString();
            throw new Exception(message ?? "无法获取 GitHub Release 信息");
        }

        _release = release;
        _versionCache.Clear();
        return true;
    }

    public VersionDataModel GetLatestVersion(UpdateChannel channel, UpdateArch arch)
    {
        if (_release is null) RefreshCache();
        if (_versionCache.TryGetValue(arch, out var cached)) return cached;
        if (_release is not { } release)
            throw new InvalidOperationException("无法获取 GitHub Release 信息");

        var version = BuildVersionData(release, arch);
        _versionCache[arch] = version;
        return version;
    }

    public bool IsLatest(UpdateChannel channel, UpdateArch arch, SemVer currentVersion)
    {
        RefreshCache();
        var latest = GetLatestVersion(channel, arch);
        return !string.IsNullOrWhiteSpace(latest.CommitSha) &&
               string.Equals(latest.CommitSha, ModBase.commitHashShort, StringComparison.OrdinalIgnoreCase);
    }

    public VersionAnnouncementDataModel GetAnnouncementList()
    {
        // 当前仓库使用 GitHub Releases，不再依赖旧公告服务器。
        return new VersionAnnouncementDataModel
        {
            Content = new List<VersionAnnouncementContentModel>()
        };
    }

    public List<ModLoader.LoaderBase> GetDownloadLoader(UpdateChannel channel, UpdateArch arch, string output)
    {
        var latest = GetLatestVersion(channel, arch);
        var downloadUrl = latest.DownloadUrl;
        if (string.IsNullOrWhiteSpace(downloadUrl))
            throw new Exception("GitHub Release 中没有适用于当前平台的更新文件");

        var loaders = new List<ModLoader.LoaderBase>();
        loaders.Add(new ModLoader.LoaderTask<int, List<DownloadFile>>(Lang.Text("Update.Task.GetDownloadInfo"),
            load =>
            {
                load.output = new List<DownloadFile>
                {
                    new(new[] { downloadUrl }, output)
                };
            }));
        loaders.Add(new LoaderDownload(Lang.Text("Update.Task.DownloadUpdateFile"), new List<DownloadFile>()));
        return loaders;
    }

    private VersionDataModel BuildVersionData(JsonObject release, UpdateArch arch)
    {
        var tag = release["tag_name"]?.ToString()?.Trim();
        if (string.IsNullOrWhiteSpace(tag))
            throw new Exception("GitHub Release 缺少 Tag 名称");

        var assets = release["assets"]?.AsArray() ??
                     throw new Exception("GitHub Release 缺少 Assets 信息");
        var archName = arch == UpdateArch.arm64 ? "ARM64" : "x64";
        var exeAsset = assets.OfType<JsonObject>().FirstOrDefault(asset =>
        {
            var name = asset["name"]?.ToString();
            return name is not null && name.EndsWith($"_{archName}.exe", StringComparison.OrdinalIgnoreCase);
        }) ?? throw new Exception($"GitHub Release 中没有 {archName} 更新文件");

        var exeName = exeAsset["name"]?.ToString();
        var downloadUrl = exeAsset["browser_download_url"]?.ToString();
        if (string.IsNullOrWhiteSpace(exeName) || string.IsNullOrWhiteSpace(downloadUrl))
            throw new Exception("GitHub Release 更新文件信息不完整");

        var releasePageUrl = release["html_url"]?.ToString() ?? $"{ReleasesUrl}/tag/{tag}";
        return new VersionDataModel
        {
            Source = SourceName,
            VersionName = tag,
            Changelog = release["body"]?.ToString() ?? "",
            Sha256 = ResolveSha256(assets, exeAsset, exeName),
            CommitSha = ExtractCommitSha(tag),
            DownloadUrl = downloadUrl,
            ReleasePageUrl = releasePageUrl
        };
    }

    private static string ResolveSha256(JsonArray assets, JsonObject exeAsset, string exeName)
    {
        var digest = exeAsset["digest"]?.ToString();
        if (digest is not null && digest.StartsWith("sha256:", StringComparison.OrdinalIgnoreCase))
            return digest["sha256:".Length..];

        var checksumName = Path.ChangeExtension(exeName, ".sha256");
        var checksumAsset = assets.OfType<JsonObject>().FirstOrDefault(asset =>
            string.Equals(asset["name"]?.ToString(), checksumName, StringComparison.OrdinalIgnoreCase));
        var checksumUrl = checksumAsset?["browser_download_url"]?.ToString();
        if (string.IsNullOrWhiteSpace(checksumUrl)) return "";

        try
        {
            using var response = CreateGitHubRequest(checksumUrl)
                .SendAsync(addMetedata: false).GetAwaiter().GetResult();
            var checksumText = response.AsString();
            foreach (var line in checksumText.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2 &&
                    string.Equals(Path.GetFileName(parts[1]), exeName, StringComparison.OrdinalIgnoreCase))
                    return parts[0];
            }

            return checksumText.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault() ?? "";
        }
        catch
        {
            // 校验文件缺失或暂时无法读取时仍允许更新，下载后的校验会自动跳过。
            return "";
        }
    }

    private static HttpRequestMessage CreateGitHubRequest(string url) =>
        HttpRequest.Create(url)
            .WithHeader("User-Agent", "RainYooSuki-PCL-CE")
            .WithHeader("Accept", "application/vnd.github+json");

    private static string? ExtractCommitSha(string tag)
    {
        var parts = tag.TrimStart('v', 'V').Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 4 ||
            !int.TryParse(parts[0], out _) ||
            !int.TryParse(parts[1], out _) ||
            !int.TryParse(parts[2], out _))
            return null;

        var candidate = parts[^1];
        return candidate.Length is >= 7 and <= 40 && candidate.All(char.IsAsciiHexDigit)
            ? candidate
            : null;
    }
}
