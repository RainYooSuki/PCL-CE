using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using PCL.Core.App;
using PCL.Core.App.Localization;
using PCL.Core.UI;
using System.Globalization;

namespace PCL;

public partial class PageLogRight
{
    public Run labDebug = new();
    public Run labError = new();
    public Run labFatal = new();
    public Run labInfo = new();
    public Run labWarn = new();

    public PageLogRight()
    {
        Initialized += (_, _) => Init();
        Loaded += PageLogRight_Loaded;
        InitializeComponent();
    }

    public void Init()
    {
        PanLogCard.Inlines.Clear();
        PanLogCard.Inlines.Add(new Run(Lang.Text("LogPage.Title")));
        PanLogCard.Inlines.Add(new Run(" | "));
        labDebug = new Run($"0 {Lang.Text("LogPage.Level.Debug")}")
            { Foreground = (Brush)System.Windows.Application.Current.Resources["ColorBrushDebug"] };
        PanLogCard.Inlines.Add(labDebug);
        PanLogCard.Inlines.Add(new Run(" | "));
        labInfo = new Run($"0 {Lang.Text("LogPage.Level.Info")}")
        {
            Foreground =
                (Brush)System.Windows.Application.Current.Resources[
                    ThemeManager.IsDarkMode ? "ColorBrushInfoDark" : "ColorBrushInfo"]
        };
        PanLogCard.Inlines.Add(labInfo);
        PanLogCard.Inlines.Add(new Run(" | "));
        labWarn = new Run($"0 {Lang.Text("LogPage.Level.Warn")}")
            { Foreground = (Brush)System.Windows.Application.Current.Resources["ColorBrushWarn"] };
        PanLogCard.Inlines.Add(labWarn);
        PanLogCard.Inlines.Add(new Run(" | "));
        labError = new Run($"0 {Lang.Text("LogPage.Level.Error")}")
            { Foreground = (Brush)System.Windows.Application.Current.Resources["ColorBrushError"] };
        PanLogCard.Inlines.Add(labError);
        PanLogCard.Inlines.Add(new Run(" | "));
        labFatal = new Run($"0 {Lang.Text("LogPage.Level.Fatal")}")
            { Foreground = (Brush)System.Windows.Application.Current.Resources["ColorBrushFatal"] };
        PanLogCard.Inlines.Add(labFatal);
    }

    private void PageLogRight_Loaded(object sender, RoutedEventArgs e)
    {
        ModAnimation.AniControlEnabled += 1;
        Reload();
        ModAnimation.AniControlEnabled -= 1;
    }

    public void Reload()
    {
        // 初始化
        if (ModMain.FrmLogLeftRequired.currentLog is null || ModMain.FrmLogLeftRequired.currentUuid <= 0 ||
            ModMain.FrmLogLeftRequired.shownLogs.Count == 0)
        {
            ModMain.FrmMainRequired.PageChange(ModMain.FrmMainRequired.pageCurrent);
            return;
        }

        PanAllBack.Visibility = Visibility.Visible;
        CardOperation.Visibility = Visibility.Visible;
        BtnOperationKill.IsEnabled = !ModMain.FrmLogLeftRequired.CurrentLogRequired.gameProcess.HasExited;
        BtnOperationExportStackDump.IsEnabled = !ModMain.FrmLogLeftRequired.CurrentLogRequired.gameProcess.HasExited &&
                                                !string.IsNullOrWhiteSpace(ModMain.FrmLogLeftRequired.CurrentLogRequired.jStackPath);
        SliderMaxLog.Value = Config.System.MaxGameLog;
        // y = 10x + 50 (0 <= x <= 5, 50 <= y <= 100)
        // y = 50x - 150 (5 < x <= 13, 100 < y <= 500)
        // y = 100x - 800 (13 < x <= 28, 500 < y <= 2000)
        SliderMaxLog.getHintText = new Func<object, object>(v =>
        {
            return v switch
            {
                _ when (int)v <= 5 => ((int)v * 10 + 50).ToString(),
                _ when (int)v <= 13 => ((int)v * 50 - 150).ToString(),
                _ when (int)v <= 28 => ((int)v * 100 - 800).ToString(),
                _ => Lang.Text("LogPage.MaxLines.Unlimited")
            };
        });
        // 绑定日志输出
        PanLog.Document = ModMain.FrmLogLeftRequired.flowDocuments[ModMain.FrmLogLeftRequired.currentUuid];
        // 绑定事件
        ModMain.FrmLogLeftRequired.CurrentLogRequired.LogOutput += OnLogOutput;
        ModMain.FrmLogLeftRequired.CurrentLogRequired.GameExit += OnGameExit;
        RefreshLabText();
    }

    private void RefreshLabText()
    {
        // 刷新计数器

        labFatal.Text = $"{ModMain.FrmLogLeftRequired.CurrentLogRequired.countFatal} {Lang.Text("LogPage.Level.Fatal")}";
        labError.Text = $"{ModMain.FrmLogLeftRequired.CurrentLogRequired.countError} {Lang.Text("LogPage.Level.Error")}";
        labWarn.Text = $"{ModMain.FrmLogLeftRequired.CurrentLogRequired.countWarn} {Lang.Text("LogPage.Level.Warn")}";
        labInfo.Text = $"{ModMain.FrmLogLeftRequired.CurrentLogRequired.countInfo} {Lang.Text("LogPage.Level.Info")}";
        labDebug.Text = $"{ModMain.FrmLogLeftRequired.CurrentLogRequired.countDebug} {Lang.Text("LogPage.Level.Debug")}";
    }

    private void OnLogOutput(ModWatcher.Watcher sender, ModWatcher.LogOutputEventArgs e)
    {
        ModBase.RunInUi(() =>
        {
            if (ModMain.FrmLogLeftRequired.currentLog is not null)
            {
                if (CheckAutoScroll.Checked == true) PanBack.ScrollToBottom();
                RefreshLabText();
            }
        });
    }

    #region 滑动条

    private void SliderMaxLog_ValueChanged(object o, bool user)
    {
        var sender = (MySlider)o;
        Config.System.MaxGameLog = sender.Value;
        if (ModMain.frmSetupLauncherMisc is null)
            return;
        ModMain.FrmSetupLauncherMiscRequired.SliderMaxLog.Value = sender.Value;
    }

    #endregion

    #region 卡片按钮

    private void BtnOperationClear_Click(object sender, ModBase.RouteEventArgs e)
    {
        ModMain.FrmLogLeftRequired.flowDocuments[ModMain.FrmLogLeftRequired.currentUuid].Blocks.Clear();
    }

    private void BtnOperationExport_Click(object sender, ModBase.RouteEventArgs e)
    {
        var savePath = SystemDialogs.SelectSaveFile(Lang.Text("LogPage.Export.SelectLocation"),
            Lang.Text("LogPage.Export.GameLog.FileName", ModMain.FrmLogLeftRequired.CurrentLogRequired.version.Name),
            Lang.Text("LogPage.Export.GameLog.Filter"));
        if (savePath.Length < 3)
            return;
        File.WriteAllLines(savePath, ModMain.FrmLogLeftRequired.CurrentLogRequired.fullLog);
        HintService.Hint(Lang.Text("LogPage.Export.Success"), HintType.Success);
        ModBase.OpenExplorer(savePath);
    }

    private void BtnOperationKill_Click(object sender, ModBase.RouteEventArgs e)
    {
        if (ModMain.FrmLogLeftRequired.CurrentLogRequired.State <= ModWatcher.Watcher.MinecraftState.Running)
        {
            ModMain.FrmLogLeftRequired.CurrentLogRequired.Kill();
            HintService.Hint(Lang.Text("LogPage.Action.GameClosed", ModMain.FrmLogLeftRequired.CurrentLogRequired.version.Name),
                HintType.Success);
        }
    }

    private void BtnOperationExportStackDump_Click(object sender, ModBase.RouteEventArgs e)
    {
        var formattedDate = DateTime.Now.ToString("G", CultureInfo.InvariantCulture)
            .Replace("/", "-")
            .Replace(":", ".")
            .Replace(" ", "_");
        var savePath = SystemDialogs.SelectSaveFile(Lang.Text("LogPage.Export.SelectLocation"),
            Lang.Text("LogPage.ExportStack.FileName", formattedDate),
            Lang.Text("LogPage.ExportStack.Filter"));
        if (savePath.Length < 3)
            return;
        HintService.Hint(Lang.Text("LogPage.ExportStack.Progress"));
        BtnOperationExportStackDump.IsEnabled = false;
        ModBase.RunInNewThread(() =>
        {
            var dump = ModMain.FrmLogLeftRequired.CurrentLogRequired.ExportStackDump(savePath);
            File.WriteAllLines(savePath, dump);
            ModBase.RunInUi(() =>
            {
                HintService.Hint(Lang.Text("LogPage.ExportStack.Success"), HintType.Success);
                BtnOperationExportStackDump.IsEnabled = true;
            });
            ModBase.OpenExplorer(savePath);
        });
    }

    private void OnGameExit()
    {
        ModBase.RunInUi(() => BtnOperationKill.IsEnabled = false);
        ModBase.RunInUi(() => BtnOperationExportStackDump.IsEnabled = false);
    }

    #endregion
}
