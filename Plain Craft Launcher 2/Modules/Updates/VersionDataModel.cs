namespace PCL;

public class VersionDataModel
{
    public string Changelog { get; set; } = "";
    public string Sha256 { get; set; } = "";
    public string Source { get; set; } = "";
    public string VersionName { get; set; } = "";

    /// <summary>
    /// Release 对应源码提交的短 SHA；旧更新源可以为空。
    /// </summary>
    public string? CommitSha { get; set; }

    /// <summary>
    /// 当前平台更新程序的下载地址。
    /// </summary>
    public string? DownloadUrl { get; set; }

    /// <summary>
    /// GitHub Release 页面地址。
    /// </summary>
    public string? ReleasePageUrl { get; set; }
}