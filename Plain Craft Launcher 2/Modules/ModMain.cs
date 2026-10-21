using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using FluentValidation;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using PCL.Core.App;
using PCL.Core.App.Configuration;
using PCL.Core.App.Localization;
using PCL.Core.Minecraft.IdentityModel.OAuth;
using PCL.Core.UI;
using PCL.Core.Utils;
using PCL.Core.Utils.OS;
using PCL.Core.Utils.Secret;

namespace PCL;

public static class ModMain
{
    public static FormMain? frmMain;

    public static FormMain FrmMainRequired =>
        frmMain ?? throw new InvalidOperationException("主窗口尚未初始化。");
    public static SplashScreen? frmStart;

    public static SplashScreen FrmStartRequired =>
        frmStart ?? throw new InvalidOperationException("frmStart 尚未初始化。");
    public static PageLaunchLeft? frmLaunchLeft;

    public static PageLaunchLeft FrmLaunchLeftRequired =>
        frmLaunchLeft ?? throw new InvalidOperationException("启动页尚未初始化。");
    public static PageLaunchRight? frmLaunchRight;

    public static PageLaunchRight FrmLaunchRightRequired =>
        frmLaunchRight ?? throw new InvalidOperationException("启动页尚未初始化。");
    public static PageLogLeft? frmLogLeft;

    public static PageLogLeft FrmLogLeftRequired =>
        frmLogLeft ?? throw new InvalidOperationException("frmLogLeft 尚未初始化。");
    public static PageLogRight? frmLogRight;

    public static PageLogRight FrmLogRightRequired =>
        frmLogRight ?? throw new InvalidOperationException("frmLogRight 尚未初始化。");
    public static PageSelectLeft? frmSelectLeft;

    public static PageSelectLeft FrmSelectLeftRequired =>
        frmSelectLeft ?? throw new InvalidOperationException("frmSelectLeft 尚未初始化。");
    public static PageSelectRight? frmSelectRight;

    public static PageSelectRight FrmSelectRightRequired =>
        frmSelectRight ?? throw new InvalidOperationException("frmSelectRight 尚未初始化。");
    public static PageSpeedLeft? frmSpeedLeft;

    public static PageSpeedLeft FrmSpeedLeftRequired =>
        frmSpeedLeft ?? throw new InvalidOperationException("frmSpeedLeft 尚未初始化。");
    public static PageSpeedRight? frmSpeedRight;

    public static PageSpeedRight FrmSpeedRightRequired =>
        frmSpeedRight ?? throw new InvalidOperationException("frmSpeedRight 尚未初始化。");
    public static PageToolsLeft? frmToolsLeft;

    public static PageToolsLeft FrmToolsLeftRequired =>
        frmToolsLeft ?? throw new InvalidOperationException("frmToolsLeft 尚未初始化。");
    public static PageToolsTest? frmToolsTest;

    public static PageToolsTest FrmToolsTestRequired =>
        frmToolsTest ?? throw new InvalidOperationException("frmToolsTest 尚未初始化。");
    public static PageDownloadLeft? frmDownloadLeft;

    public static PageDownloadLeft FrmDownloadLeftRequired =>
        frmDownloadLeft ?? throw new InvalidOperationException("下载页尚未初始化。");
    public static PageDownloadInstall? frmDownloadInstall;

    public static PageDownloadInstall FrmDownloadInstallRequired =>
        frmDownloadInstall ?? throw new InvalidOperationException("下载安装页尚未初始化。");
    public static PageDownloadClient? frmDownloadClient;

    public static PageDownloadClient FrmDownloadClientRequired =>
        frmDownloadClient ?? throw new InvalidOperationException("frmDownloadClient 尚未初始化。");
    public static PageDownloadOptiFine? frmDownloadOptiFine;

    public static PageDownloadOptiFine FrmDownloadOptiFineRequired =>
        frmDownloadOptiFine ?? throw new InvalidOperationException("frmDownloadOptiFine 尚未初始化。");
    public static PageDownloadLiteLoader? frmDownloadLiteLoader;

    public static PageDownloadLiteLoader FrmDownloadLiteLoaderRequired =>
        frmDownloadLiteLoader ?? throw new InvalidOperationException("frmDownloadLiteLoader 尚未初始化。");
    public static PageDownloadForge? frmDownloadForge;

    public static PageDownloadForge FrmDownloadForgeRequired =>
        frmDownloadForge ?? throw new InvalidOperationException("frmDownloadForge 尚未初始化。");
    public static PageDownloadNeoForge? frmDownloadNeoForge;

    public static PageDownloadNeoForge FrmDownloadNeoForgeRequired =>
        frmDownloadNeoForge ?? throw new InvalidOperationException("frmDownloadNeoForge 尚未初始化。");
    public static PageDownloadCleanroom? frmDownloadCleanroom;

    public static PageDownloadCleanroom FrmDownloadCleanroomRequired =>
        frmDownloadCleanroom ?? throw new InvalidOperationException("frmDownloadCleanroom 尚未初始化。");
    public static PageDownloadFabric? frmDownloadFabric;

    public static PageDownloadFabric FrmDownloadFabricRequired =>
        frmDownloadFabric ?? throw new InvalidOperationException("frmDownloadFabric 尚未初始化。");
    public static PageDownloadLabyMod? frmDownloadLabyMod;

    public static PageDownloadLabyMod FrmDownloadLabyModRequired =>
        frmDownloadLabyMod ?? throw new InvalidOperationException("frmDownloadLabyMod 尚未初始化。");
    public static PageDownloadLegacyFabric? frmDownloadLegacyFabric;

    public static PageDownloadLegacyFabric FrmDownloadLegacyFabricRequired =>
        frmDownloadLegacyFabric ?? throw new InvalidOperationException("frmDownloadLegacyFabric 尚未初始化。");
    public static PageDownloadMod? frmDownloadMod;

    public static PageDownloadMod FrmDownloadModRequired =>
        frmDownloadMod ?? throw new InvalidOperationException("frmDownloadMod 尚未初始化。");
    public static PageDownloadPack? frmDownloadPack;

    public static PageDownloadPack FrmDownloadPackRequired =>
        frmDownloadPack ?? throw new InvalidOperationException("frmDownloadPack 尚未初始化。");
    public static PageDownloadDataPack? frmDownloadDataPack;

    public static PageDownloadDataPack FrmDownloadDataPackRequired =>
        frmDownloadDataPack ?? throw new InvalidOperationException("frmDownloadDataPack 尚未初始化。");
    public static PageDownloadShader? frmDownloadShader;

    public static PageDownloadShader FrmDownloadShaderRequired =>
        frmDownloadShader ?? throw new InvalidOperationException("frmDownloadShader 尚未初始化。");
    public static PageDownloadResourcePack? frmDownloadResourcePack;

    public static PageDownloadResourcePack FrmDownloadResourcePackRequired =>
        frmDownloadResourcePack ?? throw new InvalidOperationException("frmDownloadResourcePack 尚未初始化。");
    public static PageDownloadWorld? frmDownloadWorld;

    public static PageDownloadWorld FrmDownloadWorldRequired =>
        frmDownloadWorld ?? throw new InvalidOperationException("frmDownloadWorld 尚未初始化。");
    public static PageDownloadCompFavorites? frmDownloadCompFavorites;

    public static PageDownloadCompFavorites FrmDownloadCompFavoritesRequired =>
        frmDownloadCompFavorites ?? throw new InvalidOperationException("frmDownloadCompFavorites 尚未初始化。");
    public static PageSetupLeft? frmSetupLeft;

    public static PageSetupLeft FrmSetupLeftRequired =>
        frmSetupLeft ?? throw new InvalidOperationException("frmSetupLeft 尚未初始化。");
    public static PageSetupLaunch? frmSetupLaunch;

    public static PageSetupLaunch FrmSetupLaunchRequired =>
        frmSetupLaunch ?? throw new InvalidOperationException("frmSetupLaunch 尚未初始化。");
    public static PageSetupUI? frmSetupUI;

    public static PageSetupUI FrmSetupUIRequired =>
        frmSetupUI ?? throw new InvalidOperationException("frmSetupUI 尚未初始化。");
    public static PageSetupGameManage? frmSetupGameManage;

    public static PageSetupGameManage FrmSetupGameManageRequired =>
        frmSetupGameManage ?? throw new InvalidOperationException("frmSetupGameManage 尚未初始化。");
    public static PageSetupUpdate? frmSetupUpdate;

    public static PageSetupUpdate FrmSetupUpdateRequired =>
        frmSetupUpdate ?? throw new InvalidOperationException("frmSetupUpdate 尚未初始化。");
    public static PageSetupJava? frmSetupJava;

    public static PageSetupJava FrmSetupJavaRequired =>
        frmSetupJava ?? throw new InvalidOperationException("frmSetupJava 尚未初始化。");
    public static PageSetupAbout? frmSetupAbout;

    public static PageSetupAbout FrmSetupAboutRequired =>
        frmSetupAbout ?? throw new InvalidOperationException("frmSetupAbout 尚未初始化。");
    public static PageSetupLog? frmSetupLog;

    public static PageSetupLog FrmSetupLogRequired =>
        frmSetupLog ?? throw new InvalidOperationException("frmSetupLog 尚未初始化。");
    public static PageSetupFeedback? frmSetupFeedback;

    public static PageSetupFeedback FrmSetupFeedbackRequired =>
        frmSetupFeedback ?? throw new InvalidOperationException("frmSetupFeedback 尚未初始化。");
    public static PageSetupLauncherLanguage? frmSetupLauncherLanguage;

    public static PageSetupLauncherLanguage FrmSetupLauncherLanguageRequired =>
        frmSetupLauncherLanguage ?? throw new InvalidOperationException("frmSetupLauncherLanguage 尚未初始化。");
    public static PageSetupLauncherMisc? frmSetupLauncherMisc;

    public static PageSetupLauncherMisc FrmSetupLauncherMiscRequired =>
        frmSetupLauncherMisc ?? throw new InvalidOperationException("frmSetupLauncherMisc 尚未初始化。");
    public static PageLoginAuth? frmLoginAuth;

    public static PageLoginAuth FrmLoginAuthRequired =>
        frmLoginAuth ?? throw new InvalidOperationException("frmLoginAuth 尚未初始化。");
    public static PageLoginMs? frmLoginMs;

    public static PageLoginMs FrmLoginMsRequired =>
        frmLoginMs ?? throw new InvalidOperationException("frmLoginMs 尚未初始化。");
    public static PageLoginProfile? frmLoginProfile;

    public static PageLoginProfile FrmLoginProfileRequired =>
        frmLoginProfile ?? throw new InvalidOperationException("frmLoginProfile 尚未初始化。");
    public static PageLoginProfileSkin? frmLoginProfileSkin;

    public static PageLoginProfileSkin FrmLoginProfileSkinRequired =>
        frmLoginProfileSkin ?? throw new InvalidOperationException("frmLoginProfileSkin 尚未初始化。");
    public static PageLoginOffline? frmLoginOffline;

    public static PageLoginOffline FrmLoginOfflineRequired =>
        frmLoginOffline ?? throw new InvalidOperationException("frmLoginOffline 尚未初始化。");
    public static PageInstanceLeft? frmInstanceLeft;

    public static PageInstanceLeft FrmInstanceLeftRequired =>
        frmInstanceLeft ?? throw new InvalidOperationException("实例页尚未初始化。");
    public static PageInstanceOverall? frmInstanceOverall;

    public static PageInstanceOverall FrmInstanceOverallRequired =>
        frmInstanceOverall ?? throw new InvalidOperationException("frmInstanceOverall 尚未初始化。");
    public static PageInstanceCompResource? frmInstanceMod;

    public static PageInstanceCompResource FrmInstanceModRequired =>
        frmInstanceMod ?? throw new InvalidOperationException("frmInstanceMod 尚未初始化。");
    public static PageInstanceModDisabled? frmInstanceModDisabled;

    public static PageInstanceModDisabled FrmInstanceModDisabledRequired =>
        frmInstanceModDisabled ?? throw new InvalidOperationException("frmInstanceModDisabled 尚未初始化。");
    public static PageInstanceCompJarInJar? frmInstanceModJarInJar;

    public static PageInstanceCompJarInJar FrmInstanceModJarInJarRequired =>
        frmInstanceModJarInJar ?? throw new InvalidOperationException("frmInstanceModJarInJar 尚未初始化。");
    public static PageInstanceScreenshot? frmInstanceScreenshot;

    public static PageInstanceScreenshot FrmInstanceScreenshotRequired =>
        frmInstanceScreenshot ?? throw new InvalidOperationException("frmInstanceScreenshot 尚未初始化。");
    public static PageInstanceSaves? frmInstanceSaves;

    public static PageInstanceSaves FrmInstanceSavesRequired =>
        frmInstanceSaves ?? throw new InvalidOperationException("frmInstanceSaves 尚未初始化。");
    public static PageInstanceCompResource? frmInstanceShader;

    public static PageInstanceCompResource FrmInstanceShaderRequired =>
        frmInstanceShader ?? throw new InvalidOperationException("frmInstanceShader 尚未初始化。");
    public static PageInstanceCompResource? frmInstanceSchematic;

    public static PageInstanceCompResource FrmInstanceSchematicRequired =>
        frmInstanceSchematic ?? throw new InvalidOperationException("frmInstanceSchematic 尚未初始化。");
    public static PageInstanceCompResource? frmInstanceResourcePack;

    public static PageInstanceCompResource FrmInstanceResourcePackRequired =>
        frmInstanceResourcePack ?? throw new InvalidOperationException("frmInstanceResourcePack 尚未初始化。");
    public static PageInstanceSetup? frmInstanceSetup;

    public static PageInstanceSetup FrmInstanceSetupRequired =>
        frmInstanceSetup ?? throw new InvalidOperationException("frmInstanceSetup 尚未初始化。");
    public static PageInstanceInstall? frmInstanceInstall;

    public static PageInstanceInstall FrmInstanceInstallRequired =>
        frmInstanceInstall ?? throw new InvalidOperationException("frmInstanceInstall 尚未初始化。");
    public static PageInstanceExport? frmInstanceExport;

    public static PageInstanceExport FrmInstanceExportRequired =>
        frmInstanceExport ?? throw new InvalidOperationException("frmInstanceExport 尚未初始化。");
    public static PageInstanceServer? frmInstanceServer;

    public static PageInstanceServer FrmInstanceServerRequired =>
        frmInstanceServer ?? throw new InvalidOperationException("frmInstanceServer 尚未初始化。");
    public static PageInstanceSavesLeft? frmInstanceSavesLeft;

    public static PageInstanceSavesLeft FrmInstanceSavesLeftRequired =>
        frmInstanceSavesLeft ?? throw new InvalidOperationException("frmInstanceSavesLeft 尚未初始化。");
    public static PageInstanceSavesInfo? frmInstanceSavesInfo;

    public static PageInstanceSavesInfo FrmInstanceSavesInfoRequired =>
        frmInstanceSavesInfo ?? throw new InvalidOperationException("frmInstanceSavesInfo 尚未初始化。");
    public static PageInstanceSavesDatapack? frmInstanceSavesDatapack;

    public static PageInstanceSavesDatapack FrmInstanceSavesDatapackRequired =>
        frmInstanceSavesDatapack ?? throw new InvalidOperationException("frmInstanceSavesDatapack 尚未初始化。");
    public static PageDownloadCompDetail? frmDownloadCompDetail;

    public static PageDownloadCompDetail FrmDownloadCompDetailRequired =>
        frmDownloadCompDetail ?? throw new InvalidOperationException("frmDownloadCompDetail 尚未初始化。");
    public static PageHomepageNewsView? frmHomepageNews;

    public static PageHomepageNewsView FrmHomepageNewsRequired =>
        frmHomepageNews ?? throw new InvalidOperationException("frmHomepageNews 尚未初始化。");

    public static MySlider? dragControl = null;
    private static int timer4Count;
    private static int timer150Count;

    /// <summary>
    ///     等待显示的弹窗。
    /// </summary>
    public static List<MyMsgBoxConverter> WaitingMyMsgBox { get; } = [];

    private static void TimerMain()
    {
        try
        {
            #region 每 50ms 执行一次的代码

            HintService.Tick();
            MyMsgBoxTick();
            frmMain?.DragTick();
            ModLoader.LoaderTaskbarProgressRefresh();
        }

        #endregion

        catch (Exception ex)
        {
            ModBase.Log(
                ex,
                "短程主时钟执行异常",
                ModBase.LogLevel.Critical,
                userSummary: Lang.Text("Main.Error.OperationFailed"));
        }

        timer4Count += 1;
        if (timer4Count == 4)
        {
            timer4Count = 0;
            try
            {
                #region 每 250ms 执行一次的代码
            }

            #endregion

            catch (Exception ex)
            {
                ModBase.Log(ex, "中程主时钟执行异常");
            }
        }

        timer150Count += 1;
        if (timer150Count == 150)
        {
            timer150Count = 0;
            try
            {
                #region 每 7.5s 执行一次的代码

                if (frmMain is { } main && main.BtnExtraApril_ShowCheck() && aprilDistance != 0)
                    main.BtnExtraApril.Ribble();
                // 以未知原因窗口被丢到一边去的修复（Top、Left = -25600），还有 #745
                ModBase.RunInUi(() =>
                {
                    if (frmMain is not { } main) return;
                    if (!main.Hidden)
                    {
                        if (main.Top < -9000) main.Top = 100d;
                        if (main.Left < -9000) main.Left = 100d;
                    }
                }); // 窗口拉至最大时 Left = -18.8
            }

            #endregion

            catch (Exception ex)
            {
                ModBase.Log(
                    ex,
                    "长程主时钟执行异常",
                    ModBase.LogLevel.Critical,
                    userSummary: Lang.Text("Main.Error.OperationFailed"));
            }
        }
    }

    public static void TimerMainStart()
    {
        ModBase.RunInNewThread(() =>
        {
            try
            {
                while (true)
                {
                    ModBase.RunInUiWait(TimerMain);
                    Thread.Sleep((int)Math.Round(50d * 0.98d));
                }
            }
            catch (Exception ex)
            {
                ModBase.Log(
                    ex,
                    "程序主时钟出错",
                    ModBase.LogLevel.Feedback,
                    userSummary: Lang.Text("Main.Error.OperationFailed"));
            }
        }, "Timer Main");
        if (!isAprilEnabled)
            return;
        ModBase.RunInNewThread(() =>
        {
            try
            {
                var lastTime = Environment.TickCount;
                while (true)
                {
                    if (lastTime != Environment.TickCount)
                    {
                        lastTime = Environment.TickCount;
                        ModBase.RunInUiWait(TimerFool);
                    }

                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                ModBase.Log(
                    ex,
                    "愚人节主时钟出错",
                    ModBase.LogLevel.Feedback,
                    userSummary: Lang.Text("Main.Error.OperationFailed"));
            }
        }, "Timer Main Fool");
    }

    #region 弹窗

    /// <summary>
    ///     存储弹窗信息的转换器。
    /// </summary>
    public class MyMsgBoxConverter
    {
        // 设置轮询 Url
        public string Button1 = "";

        /// <summary>
        ///     点击第一个按钮将执行该方法，不关闭弹窗。
        /// </summary>
        public Action? Button1Action;

        public string Button2 = "";

        /// <summary>
        ///     点击第二个按钮将执行该方法，不关闭弹窗。
        /// </summary>
        public Action? Button2Action;

        public string Button3 = "";

        /// <summary>
        ///     点击第三个按钮将执行该方法，不关闭弹窗。
        /// </summary>
        public Action? Button3Action;

        /// <summary>
        ///     输入模式：文本框的文本。
        ///     选择模式：需要放进去的 List(Of MyListItem)。
        ///     登录模式：登录步骤 1 中返回的 JSON。
        /// </summary>
        public object? Content;

        /// <summary>
        /// Optional provider-specific device-code polling callback used by OAuth providers other than Microsoft.
        /// </summary>
        public Func<JsonObject, CancellationToken, Task<AuthorizeResult?>>? DeviceCodePoll;

        public Func<AuthorizeResult, CancellationToken, Task>? LoginResultHandler;

        public Action<object?>? CompletionHandler;

        public bool ForceWait;

        /// <summary>
        ///     选择模式：是否允许勾选多个选项
        /// </summary>
        public bool MultiSelect;

        /// <summary>
        ///     有多个按钮时，是否给第一个按钮加高亮。
        /// </summary>
        public bool HighLight;

        /// <summary>
        ///     输入模式：提示文本。
        /// </summary>
        public string HintText = "";

        /// <summary>
        ///     弹窗是否已经关闭。
        /// </summary>
        public bool IsExited = false;

        public bool IsWarn;

        /// <summary>
        ///     输入模式：输入的文本。若点击了 非 第一个按钮，则为 Nothing。
        ///     选择模式：点击的按钮编号，从 1 开始。
        ///     登录模式：字符串数组 {AccessToken, RefreshToken} 或一个 Exception。
        /// </summary>
        public object? Result;

        public string Text = "";
        public string Title = "";
        public MyMsgBoxType Type;

        /// <summary>
        ///     输入模式：输入验证规则。
        /// </summary>
        public Collection<IValidator<string>> ValidateRules = [];

        public DispatcherFrame WaitFrame = new(true);

        public string AuthServerDefault = "";
        public IReadOnlyDictionary<string, string>? AuthServerPresets;
    }

    public enum MyMsgBoxType
    {
        Text,
        Select,
        Input,
        AuthServer,
        Login,
        Markdown
    }

    private static string GetDefaultDialogTitle() => Lang.Text("Common.Dialog.Title");

    private static string GetDefaultConfirmText() => Lang.Text("Common.Action.Confirm");

    private static string GetDefaultCancelText() => Lang.Text("Common.Action.Cancel");

    /// <summary>
    ///     显示弹窗，返回点击按钮的编号（从 1 开始）。
    /// </summary>
    /// <param name="title">弹窗的标题。</param>
    /// <param name="caption">弹窗的内容。</param>
    /// <param name="button1">显示的第一个按钮，默认为“确定”。</param>
    /// <param name="button2">显示的第二个按钮，默认为空。</param>
    /// <param name="button3">显示的第三个按钮，默认为空。</param>
    /// <param name="button1Action">点击第一个按钮将执行该方法，不关闭弹窗。</param>
    /// <param name="button2Action">点击第二个按钮将执行该方法，不关闭弹窗。</param>
    /// <param name="button3Action">点击第三个按钮将执行该方法，不关闭弹窗。</param>
    /// <param name="isWarn">是否为警告弹窗，若为 True，弹窗配色和背景会变为红色。</param>
    public static int MyMsgBox(string caption, string? title = null, string? button1 = null, string? button2 = "",
        string? button3 = "", bool isWarn = false, bool highLight = true, bool forceWait = false,
        Action? button1Action = null, Action? button2Action = null, Action? button3Action = null)
    {
        title ??= GetDefaultDialogTitle();
        button1 ??= GetDefaultConfirmText();
        button2 ??= "";
        button3 ??= "";
        // 将弹窗列入队列
        var converter = new MyMsgBoxConverter
        {
            Type = MyMsgBoxType.Text, Button1 = button1, Button2 = button2, Button3 = button3, Text = caption,
            IsWarn = isWarn, Title = title, HighLight = highLight, ForceWait = true, Button1Action = button1Action,
            Button2Action = button2Action, Button3Action = button3Action
        };
        WaitingMyMsgBox.Add(converter);
        if (ModBase.RunInUi())
            // 若为 UI 线程，立即执行弹窗刻， 避免快速（连点器）点击时多次弹窗
            MyMsgBoxTick();
        if (button2.Length > 0 || forceWait)
        {
            // 若有多个按钮则开始等待
            if (frmMain is null || (FrmMainRequired.PanMsg is null && ModBase.RunInUi()))
            {
                // 主窗体尚未加载，用老土的弹窗来替代
                WaitingMyMsgBox.Remove(converter);
                if (button2.Length > 0)
                {
                    var rawResult = Interaction.MsgBox(caption,
                        (MsgBoxStyle)((int)(button3.Length > 0 ? MsgBoxStyle.YesNoCancel : MsgBoxStyle.YesNo) +
                                      (int)(isWarn ? MsgBoxStyle.Critical : MsgBoxStyle.Question)), title);
                    switch (rawResult)
                    {
                        case MsgBoxResult.Yes:
                        {
                            converter.Result = 1;
                            break;
                        }
                        case MsgBoxResult.No:
                        {
                            converter.Result = 2;
                            break;
                        }
                        case MsgBoxResult.Cancel:
                        {
                            converter.Result = 3;
                            break;
                        }
                    }
                }
                else
                {
                    Interaction.MsgBox(caption,
                        (MsgBoxStyle)((int)MsgBoxStyle.OkOnly +
                                      (int)(isWarn ? MsgBoxStyle.Critical : MsgBoxStyle.Question)), title);
                    converter.Result = 1;
                }

                ModBase.Log("[Control] 主窗体加载完成前出现意料外的等待弹窗：" + button1 + "," + button2 + "," + button3,
                    ModBase.LogLevel.Debug);
            }
            else
            {
                try
                {
                    FrmMainRequired.DragStop();
                    ComponentDispatcher.PushModal();
                    Dispatcher.PushFrame(converter.WaitFrame);
                }
                finally
                {
                    ComponentDispatcher.PopModal();
                }
            }

            ModBase.Log($"[Control] 普通弹框返回：{converter.Result ?? "null"}");
            return converter.Result is int result ? result : 0;
        }

        // 不进行等待，直接返回
        return 1;
    }

    /// <summary>
    ///     显示弹窗，返回点击按钮的编号（从 1 开始）。
    /// </summary>
    /// <param name="title">弹窗的标题。</param>
    /// <param name="caption">弹窗的内容。</param>
    /// <param name="button1">显示的第一个按钮，默认为“确定”。</param>
    /// <param name="button2">显示的第二个按钮，默认为空。</param>
    /// <param name="button3">显示的第三个按钮，默认为空。</param>
    /// <param name="button1Action">点击第一个按钮将执行该方法，不关闭弹窗。</param>
    /// <param name="button2Action">点击第二个按钮将执行该方法，不关闭弹窗。</param>
    /// <param name="button3Action">点击第三个按钮将执行该方法，不关闭弹窗。</param>
    /// <param name="isWarn">是否为警告弹窗，若为 True，弹窗配色和背景会变为红色。</param>
    public static int MyMsgBoxMarkdown(string caption, string? title = null, string? button1 = null, string? button2 = "",
        string? button3 = "", bool isWarn = false, bool highLight = true, bool forceWait = false,
        Action? button1Action = null, Action? button2Action = null, Action? button3Action = null)
    {
        title ??= GetDefaultDialogTitle();
        button1 ??= GetDefaultConfirmText();
        button2 ??= "";
        button3 ??= "";
        // 将弹窗列入队列
        var converter = new MyMsgBoxConverter
        {
            Type = MyMsgBoxType.Markdown, Button1 = button1, Button2 = button2, Button3 = button3, Text = caption,
            IsWarn = isWarn, Title = title, HighLight = highLight, ForceWait = true, Button1Action = button1Action,
            Button2Action = button2Action, Button3Action = button3Action
        };
        WaitingMyMsgBox.Add(converter);
        if (ModBase.RunInUi())
            // 若为 UI 线程，立即执行弹窗刻， 避免快速（连点器）点击时多次弹窗
            MyMsgBoxTick();
        if (button2.Length > 0 || forceWait)
        {
            // 若有多个按钮则开始等待
            if (frmMain is null || (FrmMainRequired.PanMsg is null && ModBase.RunInUi()))
            {
                // 主窗体尚未加载，用老土的弹窗来替代
                WaitingMyMsgBox.Remove(converter);
                if (button2.Length > 0)
                {
                    var rawResult = Interaction.MsgBox(caption,
                        (MsgBoxStyle)((int)(button3.Length > 0 ? MsgBoxStyle.YesNoCancel : MsgBoxStyle.YesNo) +
                                      (int)(isWarn ? MsgBoxStyle.Critical : MsgBoxStyle.Question)), title);
                    switch (rawResult)
                    {
                        case MsgBoxResult.Yes:
                        {
                            converter.Result = 1;
                            break;
                        }
                        case MsgBoxResult.No:
                        {
                            converter.Result = 2;
                            break;
                        }
                        case MsgBoxResult.Cancel:
                        {
                            converter.Result = 3;
                            break;
                        }
                    }
                }
                else
                {
                    Interaction.MsgBox(caption,
                        (MsgBoxStyle)((int)MsgBoxStyle.OkOnly +
                                      (int)(isWarn ? MsgBoxStyle.Critical : MsgBoxStyle.Question)), title);
                    converter.Result = 1;
                }

                ModBase.Log("[Control] 主窗体加载完成前出现意料外的等待弹窗：" + button1 + "," + button2 + "," + button3,
                    ModBase.LogLevel.Debug);
            }
            else
            {
                try
                {
                    FrmMainRequired.DragStop();
                    ComponentDispatcher.PushModal();
                    Dispatcher.PushFrame(converter.WaitFrame);
                }
                finally
                {
                    ComponentDispatcher.PopModal();
                }
            }

            ModBase.Log($"[Control] 普通弹框返回：{converter.Result ?? "null"}");
            return converter.Result is int result ? result : 0;
        }

        // 不进行等待，直接返回
        return 1;
    }

    /// <summary>
    ///     显示输入框并返回输入的文本。若点击第二个按钮，则返回 Nothing。
    /// </summary>
    /// <param name="title">弹窗的标题。</param>
    /// <param name="validateRules">文本框的输入检测。</param>
    /// <param name="text">弹窗的介绍文本。</param>
    /// <param name="defaultInput">文本框的默认内容。</param>
    /// <param name="hintText">文本框的提示内容。</param>
    /// <param name="button1">显示的第一个按钮，默认为“确定”。</param>
    /// <param name="button2">显示的第二个按钮，默认为“取消”。</param>
    /// <param name="isWarn">是否为警告弹窗，若为 True，弹窗配色和背景会变为红色。</param>
    public static string MyMsgBoxInput(string title, string text = "", string defaultInput = "",
        Collection<IValidator<string>>? validateRules = null, string hintText = "", string? button1 = null,
        string? button2 = null, bool isWarn = false)
    {
        button1 ??= GetDefaultConfirmText();
        button2 ??= GetDefaultCancelText();
        // 将弹窗列入队列
        var converter = new MyMsgBoxConverter
        {
            Text = text, HintText = hintText, Type = MyMsgBoxType.Input,
            ValidateRules = validateRules ?? [], Button1 = button1, Button2 = button2,
            Content = defaultInput, IsWarn = isWarn, Title = title
        };
        WaitingMyMsgBox.Add(converter);
        // 虽然我也不知道这是啥但是能用就成了 :)
        try
        {
            frmMain?.DragStop();
            ComponentDispatcher.PushModal();
            Dispatcher.PushFrame(converter.WaitFrame);
        }
        finally
        {
            ComponentDispatcher.PopModal();
        }

        ModBase.Log($"[Control] 输入弹框返回：{converter.Result}");
        return converter.Result?.ToString();
    }

    public static string MyMsgBoxAuthServer(string defaultServer, IReadOnlyDictionary<string, string> presets,
        string? title = null)
    {
        var converter = new MyMsgBoxConverter
        {
            Type = MyMsgBoxType.AuthServer,
            Title = title ?? Lang.Text("Launch.Account.Auth.SelectServer"),
            Button1 = GetDefaultConfirmText(),
            Button2 = GetDefaultCancelText(),
            AuthServerDefault = defaultServer,
            AuthServerPresets = presets,
            ForceWait = true
        };
        WaitingMyMsgBox.Add(converter);
        try
        {
            frmMain?.DragStop();
            ComponentDispatcher.PushModal();
            Dispatcher.PushFrame(converter.WaitFrame);
        }
        finally
        {
            ComponentDispatcher.PopModal();
        }
        return converter.Result?.ToString() ?? string.Empty;
    }

    /// <summary>
    ///     显示选择框并返回选择的第几项（从 0 开始）。若点击第二个按钮，则返回 Nothing。
    /// </summary>
    /// <param name="title">弹窗的标题。</param>
    /// <param name="button1">显示的第一个按钮，默认为 “确定”。</param>
    /// <param name="button2">显示的第二个按钮，默认为空。</param>
    /// <param name="isWarn">是否为警告弹窗，若为 True，弹窗配色和背景会变为红色。</param>
    public static int? MyMsgBoxSelect(List<IMyRadio> selections, string? title = null, string? button1 = null,
        string? button2 = "", bool isWarn = false)
    {
        title ??= GetDefaultDialogTitle();
        button1 ??= GetDefaultConfirmText();
        button2 ??= "";
        // 将弹窗列入队列
        var converter = new MyMsgBoxConverter
        {
            Type = MyMsgBoxType.Select, Button1 = button1, Button2 = button2, Content = selections, IsWarn = isWarn,
            Title = title
        };
        WaitingMyMsgBox.Add(converter);
        // 虽然我也不知道这是啥但是能用就成了 :)
        try
        {
            if (frmMain is not null)
                FrmMainRequired.DragStop();
            ComponentDispatcher.PushModal();
            Dispatcher.PushFrame(converter.WaitFrame);
        }
        finally
        {
            ComponentDispatcher.PopModal();
        }

        ModBase.Log($"[Control] 选择弹框返回：{converter.Result ?? "null"}");
        return (int?)converter.Result;
    }

    /// <summary>
    ///     显示多选选择框并返回勾选的所有项索引（从 0 开始）。若点击第二个按钮，则返回 Nothing。
    /// </summary>
    /// <param name="selections">需要展示的可勾选列表项。</param>
    /// <param name="title">弹窗的标题。</param>
    /// <param name="button1">显示的第一个按钮，默认为 “确定”。</param>
    /// <param name="button2">显示的第二个按钮，默认为空。</param>
    /// <param name="isWarn">是否为警告弹窗，若为 True，弹窗配色和背景会变为红色。</param>
    public static List<int>? MyMsgBoxMultiSelect(List<MyListItem> selections, string? title = null,
        string? button1 = null, string? button2 = "", bool isWarn = false)
    {
        title ??= GetDefaultDialogTitle();
        button1 ??= GetDefaultConfirmText();
        button2 ??= "";
        // 将弹窗列入队列
        var converter = new MyMsgBoxConverter
        {
            Type = MyMsgBoxType.Select, MultiSelect = true, Button1 = button1, Button2 = button2, Content = selections,
            IsWarn = isWarn, Title = title
        };
        WaitingMyMsgBox.Add(converter);
        // 虽然我也不知道这是啥但是能用就成了 :)
        try
        {
            if (frmMain is not null)
                FrmMainRequired.DragStop();
            ComponentDispatcher.PushModal();
            Dispatcher.PushFrame(converter.WaitFrame);
        }
        finally
        {
            ComponentDispatcher.PopModal();
        }

        ModBase.Log($"[Control] 多选弹框返回：{converter.Result ?? "null"}");
        return (List<int>?)converter.Result;
    }


    public static void MyMsgBoxTick()
    {
        try
        {
            if (frmMain is null || FrmMainRequired.PanMsg is null || FrmMainRequired.WindowState == WindowState.Minimized)
                return;
            if (FrmMainRequired.PanMsg.Children.Count > 0)
            {
                // 弹窗中
                FrmMainRequired.PanMsgBackground.Visibility = Visibility.Visible;
            }
            else if (WaitingMyMsgBox.Any())
            {
                // 没有弹窗，显示一个等待的弹窗
                FrmMainRequired.PanMsgBackground.Visibility = Visibility.Visible;
                switch (WaitingMyMsgBox[0].Type)
                {
                    case MyMsgBoxType.Input:
                    {
                        FrmMainRequired.PanMsg.Children.Add(new MyMsgInput(WaitingMyMsgBox[0]));
                        break;
                    }
                    case MyMsgBoxType.Select:
                    {
                        FrmMainRequired.PanMsg.Children.Add(new MyMsgSelect(WaitingMyMsgBox[0]));
                        break;
                    }
                    case MyMsgBoxType.AuthServer:
                    {
                        FrmMainRequired.PanMsg.Children.Add(new MyMsgAuthServer(WaitingMyMsgBox[0]));
                        break;
                    }
                    case MyMsgBoxType.Text:
                    {
                        FrmMainRequired.PanMsg.Children.Add(new MyMsgText(WaitingMyMsgBox[0]));
                        break;
                    }
                    case MyMsgBoxType.Login:
                    {
                        FrmMainRequired.PanMsg.Children.Add(new MyMsgLogin(WaitingMyMsgBox[0]));
                        break;
                    }
                    case MyMsgBoxType.Markdown:
                    {
                        FrmMainRequired.PanMsg.Children.Add(new MyMsgMarkdown(WaitingMyMsgBox[0]));
                        break;
                    }
                }

                WaitingMyMsgBox.RemoveAt(0);
            }
            // 没有弹窗，没有等待的弹窗
            else if (!(FrmMainRequired.PanMsgBackground.Visibility == Visibility.Collapsed))
            {
                FrmMainRequired.PanMsgBackground.Visibility = Visibility.Collapsed;
            }
        }
        catch (Exception ex)
        {
            ModBase.Log(
                ex,
                "处理等待中的弹窗失败",
                ModBase.LogLevel.Feedback,
                userSummary: Lang.Text("Main.Error.OperationFailed"));
        }
    }

    public static void MsgBoxWrapper_OnShow(string message, string caption, ICollection<MsgBoxButtonInfo> buttons,
        MsgBoxTheme theme, bool block, ref int result)
    {
        var btnText1 = buttons.Count < 1 ? GetDefaultConfirmText() : buttons.ElementAt(0).Context;
        var btnAct1 = buttons.Count < 1 ? null : buttons.ElementAt(0).OnClick;
        var btnText2 = buttons.Count < 2 ? GetDefaultCancelText() : buttons.ElementAt(1).Context;
        var btnAct2 = buttons.Count < 2 ? null : buttons.ElementAt(1).OnClick;
        var btnText3 = buttons.Count < 3 ? "" : buttons.ElementAt(2).Context;
        var btnAct3 = buttons.Count < 3 ? null : buttons.ElementAt(2).OnClick;

        var isWarn = theme == MsgBoxTheme.Warning || theme == MsgBoxTheme.Error;

        result = MyMsgBox(message, caption, btnText1, btnText2, btnText3, isWarn, forceWait: block,
            button1Action: btnAct1, button2Action: btnAct2, button3Action: btnAct3);
    }

    #endregion

    #region 愚人节

    public static bool isAprilEnabled = DateTime.Now.Month == 4 && DateTime.Now.Day == 1;
    public static bool isAprilGiveup = false;
    private static Vector aprilSpeed = new(0d, 0d);
    private static int aprilIdieCount;
    private static Point aprilMousePosLast = new(0d, 0d);
    private static int aprilDistance;

    private static void TimerFool()
    {
        try
        {
            if (frmLaunchLeft is null || FrmLaunchLeftRequired.AprilPosTrans is null ||
                frmMain is not { lastMouseArg: { } mouseArg } main)
                return;
            if (isAprilGiveup || main.pageCurrent != FormMain.PageType.Launch ||
                ModAnimation.AniControlEnabled != 0 || !FrmLaunchLeftRequired.BtnLaunch.IsLoaded)
                return;

            // 计算是否空闲
            var mousePos = mouseArg.GetPosition(main);
            if (mousePos == aprilMousePosLast)
            {
                aprilIdieCount += 1;
            }
            else
            {
                aprilMousePosLast = mousePos;
                aprilIdieCount = 0;
            }

            // 计算躲避移动
            Vector direction;
            double distance;
            var buttonWidth = FrmLaunchLeftRequired.BtnLaunch.ActualWidth / 2d;
            var buttonHeight = FrmLaunchLeftRequired.BtnLaunch.ActualHeight / 2d;
            var vec = (Vector)(mouseArg.GetPosition(FrmLaunchLeftRequired.BtnLaunch) -
                               new Vector(buttonWidth, buttonHeight));
            var dir = new Vector(vec.X, vec.Y);
            dir.Normalize();
            direction = -dir;
            distance = new Vector(Math.Max(0d, Math.Abs(vec.X) - buttonWidth),
                Math.Max(0d, Math.Abs(vec.Y) - buttonHeight)).Length;
            var breathScale = Math.Sin(timer150Count / 37.5d * Math.PI);
            var acc = Math.Max(0d, breathScale * 0.25d - 0.65d - Math.Log((distance + 0.4d) / 200d)) * direction; // 加速度
            // 计算回归移动
            if (aprilIdieCount >= 64 * 5)
            {
                var safeDist = (Vector)(mouseArg.GetPosition(main.PanMain) -
                                        new Vector(buttonWidth, main.PanMain.ActualHeight - buttonHeight * 3d));
                var back = new Vector(FrmLaunchLeftRequired.AprilPosTrans.X, FrmLaunchLeftRequired.AprilPosTrans.Y);
                if (safeDist.Length > 250d && back.Length > 0.4d)
                {
                    acc -= back * 0.0005d;
                    back.Normalize();
                    acc -= back * 0.15d;
                }
            }

            // 回到边界
            var relative = FrmLaunchLeftRequired.BtnLaunch.TranslatePoint(new Point(0d, 0d), main.PanForm);
            if (relative.X < -buttonWidth * 2d)
            {
                FrmLaunchLeftRequired.AprilPosTrans.X += main.PanForm.ActualWidth + buttonWidth * 2d; // 离开左边界
                aprilSpeed.X -= 80d;
                if (relative.Y < 0d)
                    FrmLaunchLeftRequired.AprilPosTrans.Y += buttonHeight * 2.5d;
                else if (relative.Y > main.PanForm.ActualHeight - buttonHeight * 2d)
                    FrmLaunchLeftRequired.AprilPosTrans.Y -= buttonHeight * 2.5d;
            }
            else if (relative.X > main.PanForm.ActualWidth)
            {
                FrmLaunchLeftRequired.AprilPosTrans.X -= main.PanForm.ActualWidth + buttonWidth * 2d; // 离开右边界
                aprilSpeed.X += 80d;
                if (relative.Y < 0d)
                    FrmLaunchLeftRequired.AprilPosTrans.Y += buttonHeight * 2.5d;
                else if (relative.Y > main.PanForm.ActualHeight - buttonHeight * 2d)
                    FrmLaunchLeftRequired.AprilPosTrans.Y -= buttonHeight * 2.5d;
            }
            else if (relative.Y < -buttonHeight * 2d)
            {
                FrmLaunchLeftRequired.AprilPosTrans.Y += main.PanForm.ActualHeight + buttonHeight * 2d; // 离开上边界
                aprilSpeed.Y -= 25d;
                if (relative.X < 0d)
                    FrmLaunchLeftRequired.AprilPosTrans.X += buttonWidth * 2d;
                else if (relative.X > main.PanForm.ActualWidth - buttonWidth * 2d)
                    FrmLaunchLeftRequired.AprilPosTrans.X -= buttonWidth * 2d;
            }
            else if (relative.Y > main.PanForm.ActualHeight)
            {
                FrmLaunchLeftRequired.AprilPosTrans.Y -= main.PanForm.ActualHeight + buttonHeight * 2d; // 离开下边界
                aprilSpeed.Y += 25d;
                if (relative.X < 0d)
                    FrmLaunchLeftRequired.AprilPosTrans.X += buttonWidth * 2d;
                else if (relative.X > main.PanForm.ActualWidth - buttonWidth * 2d)
                    FrmLaunchLeftRequired.AprilPosTrans.X -= buttonWidth * 2d;
            }

            // 移动
            aprilSpeed = aprilSpeed * 0.8d + acc;
            var speedValue = Math.Min(60d, aprilSpeed.Length);
            if (speedValue < 0.01d)
                return;
            aprilSpeed.Normalize();
            aprilSpeed *= speedValue;
            aprilDistance = (int)Math.Round(aprilDistance + speedValue);
            FrmLaunchLeftRequired.AprilPosTrans.X += aprilSpeed.X;
            FrmLaunchLeftRequired.AprilPosTrans.Y += aprilSpeed.Y;
            // 大小改变
            FrmLaunchLeftRequired.AprilScaleTrans.ScaleX =
                ModBase.MathClamp(1d - (Math.Abs(direction.X) - Math.Abs(direction.Y)) * (speedValue / 160d), 0.2d,
                    1.8d);
            FrmLaunchLeftRequired.AprilScaleTrans.ScaleY =
                ModBase.MathClamp(1d - (Math.Abs(direction.Y) - Math.Abs(direction.X)) * (speedValue / 100d), 0.2d,
                    1.8d);
            // 放弃提示
            if (aprilDistance > 4000)
            {
                aprilDistance = -4000;
                switch (RandomUtils.NextInt(0, 3))
                {
                    case 0:
                    {
                        HintService.Hint("放弃吧！只需要点一下右下角的小白旗……");
                        break;
                    }
                    case 1:
                    {
                        HintService.Hint("看到右下角的那面小白旗了吗？");
                        break;
                    }
                    case 2:
                    {
                        HintService.Hint("这里建议点一下右下角的小白旗投降呢.jpg");
                        break;
                    }
                    case 3:
                    {
                        HintService.Hint("右下角的小白旗永远等着你……");
                        break;
                    }
                }
            }
        }

        catch (Exception ex)
        {
            ModBase.Log(
                ex,
                "愚人节移动出错",
                ModBase.LogLevel.Feedback,
                userSummary: Lang.Text("Main.Error.OperationFailed"));
        }
    }

    #endregion

    #region 系统

    /// <summary>
    ///     把某个 PCL 窗口拖到最前面。
    /// </summary>
    public static void ShowWindowToTop(nint handle)
    {
        try
        {
            PostMessage(handle, 400 * 16 + 2, 0L, 0L);
            SetForegroundWindow(handle); // 不在这里放不行，神秘 WinAPI，建议别动
        }
        catch (Exception ex)
        {
            ModBase.Log(
                ex,
                "设置窗口置顶失败",
                ModBase.LogLevel.Hint,
                userSummary: Lang.Text("Main.Error.OperationFailed"));
        }
    }

    [DllImport("user32", EntryPoint = "FindWindowA")]
    public static extern nint FindWindow(string className, string windowName);

    [DllImport("user32")]
    public static extern int SetForegroundWindow(nint hWnd);

    [DllImport("user32", EntryPoint = "PostMessageA")]
    private static extern bool PostMessage(nint hWnd, uint msg, long wParam, long lParam);

    /// <summary>
    ///     将特定程序设置为使用高性能显卡启动。
    ///     如果失败，则抛出异常。
    /// </summary>
    public static void SetGPUPreference(string executeable, bool wantHighPerformance = true)
    {
        const string GPU_PERFERENCE_REG_KEY = @"Software\Microsoft\DirectX\UserGpuPreferences";
        const string GPU_PERFERENCE_REG_VALUE_HIGH = "GpuPreference=2;";
        const string GPU_PERFERENCE_REG_VALUE_DEFAULT = "GpuPreference=0;";
        // Const GPU_PERFERENCE_REG_VALUE_POWER_SAVING As String = "GpuPreference=1;"

        var isCurrentHighPerformance = false;
        // 查看现有设置
        // 就知道 My.Computer，改个注册表 Microsoft.Win32.Registry 几年前的 API 了不用，还在这 My.Computer 都 5202 年了 My 你大爷
        using (var readOnlyKey = Registry.CurrentUser.OpenSubKey(GPU_PERFERENCE_REG_KEY, false))
        {
            if (readOnlyKey is not null)
            {
                var currentValue = readOnlyKey.GetValue(executeable);
                if (GPU_PERFERENCE_REG_VALUE_HIGH == (currentValue?.ToString() ?? "")) isCurrentHighPerformance = true;
            }
            else
            {
                // 创建父级键
                ModBase.Log("[System] 需要创建显卡设置的父级键");
                Registry.CurrentUser.CreateSubKey(GPU_PERFERENCE_REG_KEY);
            }
        }

        ModBase.Log($"[System] 当前程序 ({executeable}) 的显卡设置为高性能: {isCurrentHighPerformance}");
        if (isCurrentHighPerformance ^ wantHighPerformance)
            // 写入新设置
            using (var writeKey = Registry.CurrentUser.OpenSubKey(GPU_PERFERENCE_REG_KEY, true))
            {
                writeKey.SetValue(executeable,
                    wantHighPerformance ? GPU_PERFERENCE_REG_VALUE_HIGH : GPU_PERFERENCE_REG_VALUE_DEFAULT);
                ModBase.Log($"[System] 已调整程序 ({executeable}) 显卡设置: {wantHighPerformance}");
            }
    }

    /// <summary>
    /// 对替换标记进行处理。会对替换内容使用 EscapeHandler 进行转义。
    /// /// </summary>
    public static string ArgumentReplace(string text, Func<string, string>? escapeHandler = null, bool replaceTime = true) 
    {
    Func<string?, string> replacer = (s) =>
    {
        if (s is null) return "";
        if (escapeHandler is null) return s;
        if (s.Contains(":\\")) s = ModBase.ShortenPath(s);
        return escapeHandler(s);
    };
    
    // 基础
    text = text.Replace("{pcl_version}", replacer(ModBase.versionBaseName));
    text = text.Replace("{pcl_version_code}", replacer(ModBase.versionStandardCode));
    text = text.Replace("{pcl_version_branch}", replacer(ModBase.versionBranchName));
    text = text.Replace("{pcl_branch}", replacer(ModBase.versionBranchName));
    text = text.Replace("{identify}", replacer(Identify.LauncherId));
    text = text.Replace("{path}", replacer(Basics.ExecutableDirectory));
    text = text.Replace("{path_with_name}", replacer(Basics.ExecutableName));
    text = text.Replace("{path_temp}", replacer(ModBase.pathTemp));
    
    // 时间
    if (replaceTime) // 在窗口标题中，时间会被后续动态替换，所以此时不应该替换
    {
        text = text.Replace("{date}", replacer(Lang.Date(DateTime.Now, "d")));
        text = text.Replace("{time}", replacer(Lang.Date(DateTime.Now, "T")));
    }
    
    // Minecraft
    text = text.Replace("{java}", replacer(ModLaunch.mcLaunchJavaSelected?.Installation.JavaFolder));
    text = text.Replace("{minecraft}", replacer(ModFolder.mcFolderSelected));
    
    if (ModInstanceList.McMcInstanceSelected is not null)
    {
        text = text.Replace("{version_path}", replacer(ModInstanceList.McMcInstanceSelectedRequired.PathInstance));
        text = text.Replace("{verpath}", replacer(ModInstanceList.McMcInstanceSelectedRequired.PathInstance));
        text = text.Replace("{version_indie}", replacer(ModInstanceList.McMcInstanceSelectedRequired.PathIndie));
        text = text.Replace("{verindie}", replacer(ModInstanceList.McMcInstanceSelectedRequired.PathIndie));
        text = text.Replace("{name}", replacer(ModInstanceList.McMcInstanceSelectedRequired.Name));
        
        if (new[] { "unknown", "old", "pending" }.Contains(ModInstanceList.McMcInstanceSelectedRequired.Info.VanillaName))
        {
            text = text.Replace("{version}", replacer(ModInstanceList.McMcInstanceSelectedRequired.Name));
        }
        else
        {
            text = text.Replace("{version}", replacer(ModInstanceList.McMcInstanceSelectedRequired.Info.VanillaName));
        }
    }
    else
    {
        text = text.Replace("{version_path}", replacer(null));
        text = text.Replace("{verpath}", replacer(null));
        text = text.Replace("{version_indie}", replacer(null));
        text = text.Replace("{verindie}", replacer(null));
        text = text.Replace("{name}", replacer(null));
        text = text.Replace("{version}", replacer(null));
    }
    
    // 验证信息
    if (ModLaunch.mcLoginLoader.State == ModBase.LoadState.Finished)
    {
        text = text.Replace("{user}", replacer(ModLaunch.mcLoginLoader.output.Name));
        text = text.Replace("{uuid}", replacer(ModLaunch.mcLoginLoader.output.Uuid.ToLower()));
        
        switch (ModLaunch.mcLoginLoader.input.LoginType)
        {
            case ModLaunch.McLoginType.Legacy:
                text = text.Replace("{login}", replacer("离线"));
                break;
            case ModLaunch.McLoginType.Ms:
                text = text.Replace("{login}", replacer("正版"));
                break;
            case ModLaunch.McLoginType.Auth:
                text = text.Replace("{login}", replacer("Authlib-Injector"));
                break;
        }
    }
    else
    {
        text = text.Replace("{user}", replacer(null));
        text = text.Replace("{uuid}", replacer(null));
        text = text.Replace("{login}", replacer(null));
    }
    
    // 高级
    text = ModBase.RegexReplaceEach(text, @"\{hint\}", m => replacer(PageToolsTest.GetRandomHint()));
    text = ModBase.RegexReplaceEach(text, @"\{cave\}", m => replacer(PageToolsTest.GetRandomCave()));
    text = ModBase.RegexReplaceEach(text, @"\{setup:([a-zA-Z0-9]+)\}", m =>
    {
        if (ConfigService.TryGetConfigItemNoType(m.Groups[1].Value, out var item) && item.Source != ConfigSource.SharedEncrypt)
            return replacer(item.GetValueNoType(ModInstanceList.McMcInstanceSelected?.PathInstance)?.ToString() ?? "");
        return replacer("");
    });
    text = ModBase.RegexReplaceEach(text, @"\{varible:([^:\}]+)(?::([^\}]+))?\}", m => replacer(CustomEvent.GetCustomVariable(m.Groups[1].Value, m.Groups[2].Value)));
    text = ModBase.RegexReplaceEach(text, @"\{variable:([^:\}]+)(?::([^\}]+))?\}", m => replacer(CustomEvent.GetCustomVariable(m.Groups[1].Value, m.Groups[2].Value)));
    
    return text;
}
    #endregion

    #region 任务缓存

    private static bool isTaskTempCleared;
    private static bool isTaskTempClearing;

    /// <summary>
    ///     尝试清理任务缓存文件夹。
    ///     在整次运行中只会实际清理一次。
    /// </summary>
    public static void TryClearTaskTemp()
    {
        if (!isTaskTempCleared)
        {
            isTaskTempCleared = true;
            isTaskTempClearing = true;
            try
            {
                ModBase.Log("[System] 开始清理任务缓存文件夹");
                ModBase.DeleteDirectory(Path.Combine(SystemPaths.DriveLetter, "ProgramData", "PCL", "TaskTemp"));
                ModBase.DeleteDirectory($@"{ModBase.pathTemp}TaskTemp\");
                ModBase.Log("[System] 已清理任务缓存文件夹");
            }
            catch (Exception ex)
            {
                ModBase.Log(ex, "清理任务缓存文件夹失败");
            }
            finally
            {
                isTaskTempClearing = false;
            }
        }
        else if (isTaskTempClearing)
        {
            // 等待另一个清理步骤完成
            while (isTaskTempClearing)
                Thread.Sleep(1);
        }
    }

    /// <summary>
    ///     申请一个可用于任务缓存的临时文件夹，以 \ 结尾。这些文件夹无需进行后续清理。
    ///     若所有缓存位置均没有权限，会抛出异常。
    /// </summary>
    /// <param name="requireNonSpace">是否要求路径不包含空格。</param>
    public static string RequestTaskTempFolder(bool requireNonSpace = false)
    {
        TryClearTaskTemp();
        string resultFolder;
        do
        {
            try
            {
                resultFolder = $@"{ModBase.pathTemp}TaskTemp\{ModBase.GetUuid()}-{RandomUtils.NextInt(0, 1000000)}\";
                if (requireNonSpace && resultFolder.Contains(" "))
                    break; // 带空格
                Directory.CreateDirectory(resultFolder);
                ModBase.CheckPermissionWithException(resultFolder);
                return resultFolder;
            }
            catch
            {
            }
        } while (false);

        // 使用备用路径
        resultFolder =
            Path.Combine(SystemPaths.DriveLetter, "ProgramData", "PCL", "TaskTemp", $"{ModBase.GetUuid()}-{RandomUtils.NextInt(0, 1000000)}");
        Directory.CreateDirectory(resultFolder);
        ModBase.CheckPermission(resultFolder);
        return resultFolder;
    }

    #endregion
    
    public static void RaiseCustomEvent(DependencyObject control)
    {
        // 收集事件列表
        var events = CustomEventService.GetEvents(control).ToList();
        var eventType = CustomEventService.GetEventType(control);
        if (eventType != EventType.None)
            events.Add(new CustomEvent(eventType, CustomEventService.GetEventData(control)));

        if (!events.Any()) return;

        ModBase.RunInNewThread(() =>
            {
                foreach (var e in events)
                    e.Raise();
            }, $"执行自定义事件 {ModBase.GetUuid()}");
    }
}
