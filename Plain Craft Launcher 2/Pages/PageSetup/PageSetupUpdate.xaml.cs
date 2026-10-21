using System.Windows;
using System.Windows.Input;
using PCL.Core.Utils;
using PCL.Core.App.Localization;
using PCL.Core.Utils.OS;

namespace PCL;

public partial class PageSetupUpdate
{
    public VersionDataModel? updateInfo;

    public PageSetupUpdate()
    {
        InitializeComponent();
        Loaded += (_, _) => Init();
    }

    private void Init()
    {
        TextCurrentVersion.Text = "PCL CE " + VersionNameFormat(ModBase.releaseVersion);
        CheckUpdate();
    }

    private async Task<UpdateStatus> IsLatestAsync()
    {
        try
        {
            if (await UpdateManager.remoteServer.IsLatestAsync(
                    UpdateChannel.stable,
                    SystemInfo.IsArm64System ? UpdateArch.arm64 : UpdateArch.x64,
                    SemVer.Parse(ModBase.versionBaseName)))
            {
                ModBase.Log("[Update] 已是最新版本");
                return UpdateStatus.Latest;
            }

            ModBase.Log("[Update] 有可用的新版本");
            return UpdateStatus.Available;
        }
        catch (Exception ex)
        {
            ModBase.Log(
                ex,
                Lang.Text("Setup.Update.Error.NetworkFailed"),
                ModBase.LogLevel.Hint,
                userSummary: Lang.Text("Setup.Update.Error.NetworkFailed"));
            return UpdateStatus.Error;
        }
    }

    public async void CheckUpdate()
    {
        ModBase.Log("[Update] 开始检查更新");
        updateInfo = null;
        CardUpdate.Visibility = Visibility.Collapsed;
        CardCheck.Visibility = Visibility.Visible;
        TextCurrentDesc.Text = Lang.Text("Setup.Update.Checking");
        BtnCheckAgain.IsEnabled = false;
        switch (await IsLatestAsync())
        {
            case UpdateStatus.Available:
            {
                Exception? checkUpdateEx = null;
                try
                {
                    var latest = UpdateManager.remoteServer.GetLatestVersion(
                        UpdateChannel.stable, SystemInfo.IsArm64System ? UpdateArch.arm64 : UpdateArch.x64);
                    updateInfo = latest;
                    TextUpdateName.Text = "PCL CE " + VersionNameFormat(latest.VersionName);
                    var changelog = latest.Changelog;
                    var summary = changelog.Between("<summary>", "</summary>");
                    if (!string.IsNullOrWhiteSpace(summary))
                        TextChangelog.Text = summary.Trim();
                    else if (!string.IsNullOrWhiteSpace(changelog))
                        TextChangelog.Text = changelog.Length > 500 ? changelog[..500] + "..." : changelog;
                    else
                        TextChangelog.Text = Lang.Text("Setup.Update.Changelog.Empty");
                }
                catch (Exception ex)
                {
                    checkUpdateEx = ex;
                }

                BtnCheckAgain.IsEnabled = true;
                if (updateInfo is null)
                {
                    TextCurrentDesc.Text = Lang.Text("Setup.Update.CheckFailed");
                    if (checkUpdateEx is not null)
                        ModBase.Log(
                            checkUpdateEx,
                            "[Update] 检查更新失败",
                            ModBase.LogLevel.Msgbox,
                            userSummary: Lang.Text("Update.Check.Failed"));
                    else
                        ModBase.Log(
                            "[Update] 检查更新失败",
                            ModBase.LogLevel.Msgbox,
                            userSummary: Lang.Text("Update.Check.Failed"));
                    return;
                }

                if (UpdateManager.updateLoader is not null && UpdateManager.updateLoader.State == ModBase.LoadState.Loading)
                {
                    BtnUpdate_Timer();
                    BtnUpdate.IsEnabled = false;
                }
                else if (UpdateManager.isUpdateWaitingRestart)
                {
                    BtnUpdate.Text = Lang.Text("Setup.Update.RestartInstall");
                    BtnUpdate.IsEnabled = true;
                }
                else
                {
                    BtnUpdate.Text = Lang.Text("Setup.Update.Install");
                    BtnUpdate.IsEnabled = true;
                }

                CardUpdate.Visibility = Visibility.Visible;
                CardCheck.Visibility = Visibility.Collapsed;
                break;
            }
            case UpdateStatus.Latest:
            {
                CardUpdate.Visibility = Visibility.Collapsed;
                CardCheck.Visibility = Visibility.Visible;
                BtnCheckAgain.IsEnabled = true;
                TextCurrentDesc.Text = Lang.Text("Setup.Update.Latest");
                break;
            }
            case UpdateStatus.Error:
            {
                CardUpdate.Visibility = Visibility.Collapsed;
                CardCheck.Visibility = Visibility.Visible;
                BtnCheckAgain.IsEnabled = true;
                TextCurrentDesc.Text = Lang.Text("Setup.Update.CheckFailed");
                break;
            }
        }
    }

    public void BtnUpdate_Timer()
    {
        while (UpdateManager.updateLoader is not null && UpdateManager.updateLoader.State == ModBase.LoadState.Loading)
        {
            ModBase.RunInUi(() => BtnUpdate.Text = Lang.Number(UpdateManager.updateLoader.Progress, "P2"));
            Thread.Sleep(200);
        }
    }

    private void BtnUpdate_Click(object sender, MouseButtonEventArgs e)
    {
        if (UpdateManager.isUpdateWaitingRestart) UpdateManager.UpdateRestart(true);
        // 开始更新流程
        UpdateManager.UpdateStart(UpdateEnums.UpdateType.UpdateNow);
    }

    private void BtnChangelogDetail_Click(object sender, EventArgs e)
    {
        if (updateInfo is null)
            ModMain.MyMsgBox(Lang.Text("Setup.Update.Changelog.Unavailable"), Lang.Text("Setup.Update.Changelog.Title"));
        else
            ModMain.MyMsgBoxMarkdown(updateInfo.Changelog, Lang.Text("Setup.Update.Changelog.Title"));
    }

    private void BtnChangelog_Click(object sender, MouseButtonEventArgs e)
    {
        ModBase.OpenWebsite(updateInfo?.ReleasePageUrl ?? UpdatesGitHubReleaseModel.LatestReleasePageUrl);
    }

    public string VersionNameFormat(string str)
    {
        str = str.TrimStart('v', 'V');
        if (!str.Contains("-"))
            return str;
        var add = str.AfterLast("-");
        str = str.BeforeLast("-");
        return $"{str} {add.Replace(".", " ").Replace("beta", "Beta").Replace("rc", "RC")}";
    }

    private void BtnCheckAgain_OnClick(object sender, MouseButtonEventArgs e)
    {
        CheckUpdate();
    }

    private enum UpdateStatus
    {
        Checking = 0,
        Available = 1,
        Error = 2,
        Latest = 3
    }
}
