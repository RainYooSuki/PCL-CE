using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using PCL.Core.App;
using PCL.Core.App.Configuration;
using PCL.Core.IO.Net.Http;
using PCL.Core.UI.Theme;
using PCL.Core.Utils.Exts;
using PCL.Network;

using PCL.Core.App.Localization;
namespace PCL;

public class ModSetup
{
    public ModSetup()
    {
        // === Hide Group ===
        ConfigService.RegisterObserver(Config.Preference.Hide,
            new ConfigObserver(ConfigEvent.Changed, _ => PageSetupUI.HiddenRefresh()));

        // === Launch ===
        Config.Launch.MemoryAllocationModeConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, LaunchRamType)));
        States.Game.SelectedFolderConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => LaunchFolderSelect((string)(e.Value ?? ""))));
        States.Game.SelectedInstanceConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => LaunchInstanceSelect((string)(e.Value ?? ""))));

        // === Tool ===
        Config.Download.ThreadLimitConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, ToolDownloadThread)));
        Config.Download.FileConnectionLimitConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, ToolDownloadFileConnection)));
        Config.Download.SpeedLimitConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, ToolDownloadSpeed)));

        // === UI - Launcher ===
        Config.Preference.Theme.WindowOpacityConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, UiLauncherTransparent)));
        Config.Preference.Theme.ThemeSelectedConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, UiLauncherTheme)));
        Config.Preference.Background.BackgroundColorfulConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyBool(e, UiBackgroundColorful)));
        Config.Preference.LockWindowSizeConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyBool(e, UiLockWindowSize)));

        // UI - Video Background
        Config.Preference.Background.AutoPauseVideoConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyBool(e, UiAutoPauseVideo)));

        // UI - Background Image
        Config.Preference.Background.WallpaperOpacityConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, UiBackgroundOpacity)));
        Config.Preference.Background.WallpaperBlurRadiusConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, UiBackgroundBlur)));
        Config.Preference.Background.WallpaperSuitModeConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, UiBackgroundSuit)));

        // UI - Font
        Config.Preference.FontConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => UiFont((string)(e.Value ?? ""))));

        // UI - Homepage
        Config.Preference.Homepage.TypeConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, UiCustomType)));

        // UI - Blur
        Config.Preference.Blur.IsEnabledConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyBool(e, UiBlur)));
        Config.Preference.Blur.RadiusConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, UiBlurValue)));
        Config.Preference.Blur.SamplingRateConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, UiBlurSamplingRate)));
        Config.Preference.Blur.KernelTypeConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, UiBlurType)));

        // UI - Title Bar
        Config.Preference.WindowTitleTypeConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, UiLogoType)));
        Config.Preference.WindowTitleCustomTextConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => UiLogoText((string)(e.Value ?? ""))));
        Config.Preference.TopBarLeftAlignConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyBool(e, UiLogoLeft)));

        // === System ===
        Config.Debug.EnabledConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyBool(e, SystemDebugMode)));
        Config.Debug.AnimationSpeedConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, SystemDebugAnim)));
        Config.Network.HttpProxy.CustomAddressConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => SystemHttpProxy((string)(e.Value ?? ""))));
        Config.Network.HttpProxy.TypeConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, SystemHttpProxyType)));
        Config.Network.HttpProxy.CustomUsernameConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => SystemHttpProxyCustomUsername((string)(e.Value ?? ""))));
        Config.Network.HttpProxy.CustomPasswordConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => SystemHttpProxyCustomPassword((string)(e.Value ?? ""))));

        // === Version ===
        Config.Instance.MemorySolutionConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, VersionRamType)));
        Config.InstanceAuth.LoginRequirementSolutionConfig.Observe(new ConfigObserver(ConfigEvent.Changed,
            e => ApplyInt(e, VersionServerLogin)));
    }

    private static void ApplyInt(ConfigEventArgs e, Action<int> apply)
    {
        if (e.Value is int value)
            apply(value);
    }

    private static void ApplyBool(ConfigEventArgs e, Action<bool> apply)
    {
        if (e.Value is bool value)
            apply(value);
    }

    /// <summary>
    ///     主动应用所有当前配置值。
    /// </summary>
    public static void ApplyAll()
    {
        // Launch
        LaunchRamType(Config.Launch.MemoryAllocationMode);

        // Tool
        ToolDownloadThread(Config.Download.ThreadLimit);
        ToolDownloadFileConnection(Config.Download.FileConnectionLimit);
        ToolDownloadSpeed(Config.Download.SpeedLimit);

        // UI - Launcher
        UiLauncherTransparent(Config.Preference.Theme.WindowOpacity);
        UiLauncherTheme(Config.Preference.Theme.ThemeSelected);
        UiBackgroundColorful(Config.Preference.Background.BackgroundColorful);
        UiLockWindowSize(Config.Preference.LockWindowSize);

        // UI - Video Background
        UiAutoPauseVideo(Config.Preference.Background.AutoPauseVideo);

        // UI - Background Image
        UiBackgroundOpacity(Config.Preference.Background.WallpaperOpacity);
        UiBackgroundBlur(Config.Preference.Background.WallpaperBlurRadius);
        UiBackgroundSuit(Config.Preference.Background.WallpaperSuitMode);

        // UI - Font
        UiFont(Config.Preference.Font);

        // UI - Homepage
        UiCustomType(Config.Preference.Homepage.Type);

        // UI - Blur
        if (Config.Preference.Blur.IsEnabled)
        {
            UiBlurValue(Config.Preference.Blur.Radius);
            UiBlurSamplingRate(Config.Preference.Blur.SamplingRate);
            UiBlurType(Config.Preference.Blur.KernelType);
        }
        else
        {
            UiBlurValue(0);
        }

        UiBlur(Config.Preference.Blur.IsEnabled);

        // UI - Title Bar
        UiLogoType((int)Config.Preference.WindowTitleType);
        UiLogoText(Config.Preference.WindowTitleCustomText);
        UiLogoLeft(Config.Preference.TopBarLeftAlign);

        // UI - Hide
        PageSetupUI.HiddenRefresh();

        // System
        SystemDebugMode(Config.Debug.Enabled);
        SystemDebugAnim(Config.Debug.AnimationSpeed);
        SystemHttpProxy(Config.Network.HttpProxy.CustomAddress);
        SystemHttpProxyType(Config.Network.HttpProxy.Type);
        SystemHttpProxyCustomUsername(Config.Network.HttpProxy.CustomUsername);
        SystemHttpProxyCustomPassword(Config.Network.HttpProxy.CustomPassword);
    }

    #region Launch

    // 切换选择
    public static void LaunchInstanceSelect(string value)
    {
        ModBase.Log("[Setup] 当前选择的 Minecraft 版本：" + value);
        ModBase.WriteIni(ModFolder.mcFolderSelected + "PCL.ini", "Version", value);
    }

    public static void LaunchFolderSelect(string value)
    {
        ModBase.Log("[Setup] 当前选择的 Minecraft 文件夹：" + value.Replace("$", ModBase.exePath));
        ModFolder.mcFolderSelected = value.Replace("$", ModBase.exePath);
    }

    // 游戏内存
    public static void LaunchRamType(int type)
    {
        if (ModMain.frmSetupLaunch is null)
            return;
        ModMain.FrmSetupLaunchRequired.RamType(type);
    }

    #endregion

    #region Tool

    public static void ToolDownloadThread(int value)
    {
        ModNet.NetTaskConnectionLimit = Math.Clamp(value + 1, 1, ModNet.NetTaskConnectionLimitMax);
        DownloadResourceManager.OnConnectionLimitChanged();
    }

    public static void ToolDownloadFileConnection(int value)
    {
        ModNet.NetTaskSingleFileConnectionLimit = Math.Clamp(value + 1, 1,
            ModNet.NetTaskSingleFileConnectionLimitMax);
    }

    public static void ToolDownloadSpeed(int value)
    {
        ModNet.NetTaskSpeedLimitHigh = value switch
        {
            <= 14 => (long)Math.Round((value + 1) * 0.1d * 1024d * 1024d),
            <= 31 => (long)Math.Round((value - 11) * 0.5d * 1024d * 1024d),
            <= 41 => (value - 21) * 1024 * 1024L,
            _ => -1
        };
    }

    #endregion

    #region UI

    // 启动器
    public static void UiLauncherTransparent(int value)
    {
        ModMain.FrmMainRequired.Opacity = value / 1000d + 0.4d;
    }

    public static void UiLauncherTheme(int value)
    {
        ThemeManager.ThemeRefresh(value);
    }

    public static void UiBackgroundColorful(bool value)
    {
        ThemeManager.ThemeRefresh();
    }

    public static void UiLockWindowSize(bool value)
    {
        if (value)
            ModMain.FrmMainRequired.RemoveResizer();
        else
            ModMain.FrmMainRequired.AddResizer();
    }

    // 视频背景
    public static void UiAutoPauseVideo(bool value)
    {
        if (!value)
        {
            ModVideoBack.ForcePlay = true;
            ModVideoBack.VideoPlay();
        }
        else
        {
            ModVideoBack.ForcePlay = false;
            if (ModVideoBack.IsGaming)
                ModVideoBack.VideoPause();
        }
    }

    // 背景图片
    public static void UiBackgroundOpacity(int value)
    {
        ModMain.FrmMainRequired.ImgBack.Opacity = value / 1000d;
    }

    public static void UiBackgroundBlur(int value)
    {
        ModMain.FrmMainRequired.ImgBack.Effect = value == 0 ? null : new BlurEffect { Radius = value + 1 };
        ModMain.FrmMainRequired.ImgBack.Margin = new Thickness(-(value + 1) / 1.8d);
    }

    public static void UiBackgroundSuit(int value)
    {
        if (ModMain.FrmMainRequired.ImgBack.Background is null)
            return;
        var width = ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ImageSource.Width;
        var height = ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ImageSource.Height;
        if (value == 0)
        {
            // 智能：当图片较小时平铺，较大时适应
            if (width < ModMain.FrmMainRequired.PanMain.ActualWidth / 2d && height < ModMain.FrmMainRequired.PanMain.ActualHeight / 2d)
                value = 4; // 平铺
            else
                value = 2; // 适应
        }

        ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).TileMode = TileMode.None;
        ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).Viewport = new Rect(0d, 0d, 1d, 1d);
        ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
        switch (value)
        {
            case 1: // 居中
            {
                ModMain.FrmMainRequired.ImgBack.HorizontalAlignment = HorizontalAlignment.Center;
                ModMain.FrmMainRequired.ImgBack.VerticalAlignment = VerticalAlignment.Center;
                ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).Stretch = Stretch.None;
                ModMain.FrmMainRequired.ImgBack.Width = ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ImageSource.Width;
                ModMain.FrmMainRequired.ImgBack.Height = ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ImageSource.Height;
                break;
            }
            case 2: // 适应
            {
                ModMain.FrmMainRequired.ImgBack.HorizontalAlignment = HorizontalAlignment.Stretch;
                ModMain.FrmMainRequired.ImgBack.VerticalAlignment = VerticalAlignment.Stretch;
                ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).Stretch = Stretch.UniformToFill;
                ModMain.FrmMainRequired.ImgBack.Width = double.NaN;
                ModMain.FrmMainRequired.ImgBack.Height = double.NaN;
                break;
            }
            case 3: // 拉伸
            {
                ModMain.FrmMainRequired.ImgBack.HorizontalAlignment = HorizontalAlignment.Stretch;
                ModMain.FrmMainRequired.ImgBack.VerticalAlignment = VerticalAlignment.Stretch;
                ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).Stretch = Stretch.Fill;
                ModMain.FrmMainRequired.ImgBack.Width = double.NaN;
                ModMain.FrmMainRequired.ImgBack.Height = double.NaN;
                break;
            }
            case 4: // 平铺
            {
                ModMain.FrmMainRequired.ImgBack.HorizontalAlignment = HorizontalAlignment.Stretch;
                ModMain.FrmMainRequired.ImgBack.VerticalAlignment = VerticalAlignment.Stretch;
                ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).Stretch = Stretch.None;
                ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).TileMode = TileMode.Tile;
                ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).Viewport = new Rect(0d, 0d,
                    ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ImageSource.Width,
                    ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ImageSource.Height);
                ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ViewportUnits = BrushMappingMode.Absolute;
                ModMain.FrmMainRequired.ImgBack.Width = double.NaN;
                ModMain.FrmMainRequired.ImgBack.Height = double.NaN;
                break;
            }
            case 5: // 左上
            {
                ModMain.FrmMainRequired.ImgBack.HorizontalAlignment = HorizontalAlignment.Left;
                ModMain.FrmMainRequired.ImgBack.VerticalAlignment = VerticalAlignment.Top;
                ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).Stretch = Stretch.None;
                ModMain.FrmMainRequired.ImgBack.Width = ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ImageSource.Width;
                ModMain.FrmMainRequired.ImgBack.Height = ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ImageSource.Height;
                break;
            }
            case 6: // 右上
            {
                ModMain.FrmMainRequired.ImgBack.HorizontalAlignment = HorizontalAlignment.Right;
                ModMain.FrmMainRequired.ImgBack.VerticalAlignment = VerticalAlignment.Top;
                ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).Stretch = Stretch.None;
                ModMain.FrmMainRequired.ImgBack.Width = ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ImageSource.Width;
                ModMain.FrmMainRequired.ImgBack.Height = ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ImageSource.Height;
                break;
            }
            case 7: // 左下
            {
                ModMain.FrmMainRequired.ImgBack.HorizontalAlignment = HorizontalAlignment.Left;
                ModMain.FrmMainRequired.ImgBack.VerticalAlignment = VerticalAlignment.Bottom;
                ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).Stretch = Stretch.None;
                ModMain.FrmMainRequired.ImgBack.Width = ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ImageSource.Width;
                ModMain.FrmMainRequired.ImgBack.Height = ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ImageSource.Height;
                break;
            }
            case 8: // 右下
            {
                ModMain.FrmMainRequired.ImgBack.HorizontalAlignment = HorizontalAlignment.Right;
                ModMain.FrmMainRequired.ImgBack.VerticalAlignment = VerticalAlignment.Bottom;
                ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).Stretch = Stretch.None;
                ModMain.FrmMainRequired.ImgBack.Width = ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ImageSource.Width;
                ModMain.FrmMainRequired.ImgBack.Height = ((ImageBrush)ModMain.FrmMainRequired.ImgBack.Background).ImageSource.Height;
                break;
            }
        }
    }

    // 字体
    public static void UiFont(string value)
    {
        try
        {
            ModBase.SetLaunchFont(value);
        }
        catch (Exception ex)
        {
            ModBase.Log(
                ex,
                "字体加载失败",
                ModBase.LogLevel.Hint,
                userSummary: Lang.Text("Setup.Error.OperationFailed"));
        }
    }

    // 主页
    public static void UiCustomType(int value)
    {
        if (ModMain.frmSetupUI is null)
            return;
        switch (value)
        {
            case 0: // 无
            {
                ModMain.FrmSetupUIRequired.PanCustomPreset.Visibility = Visibility.Collapsed;
                ModMain.FrmSetupUIRequired.PanCustomLocal.Visibility = Visibility.Collapsed;
                ModMain.FrmSetupUIRequired.PanCustomNet.Visibility = Visibility.Collapsed;
                ModMain.FrmSetupUIRequired.HintCustom.Visibility = Visibility.Collapsed;
                ModMain.FrmSetupUIRequired.HintCustomWarn.Visibility = Visibility.Collapsed;
                break;
            }
            case 1: // 本地
            {
                ModMain.FrmSetupUIRequired.PanCustomPreset.Visibility = Visibility.Collapsed;
                ModMain.FrmSetupUIRequired.PanCustomLocal.Visibility = Visibility.Visible;
                ModMain.FrmSetupUIRequired.PanCustomNet.Visibility = Visibility.Collapsed;
                ModMain.FrmSetupUIRequired.HintCustom.Visibility = Visibility.Visible;
                ModMain.FrmSetupUIRequired.HintCustomWarn.Visibility =
                    States.Hint.UntrustedHomepage ? Visibility.Collapsed : Visibility.Visible;
                ModMain.FrmSetupUIRequired.HintCustom.Text =
                    Lang.Text("Setup.Ui.Homepage.LocalFile.Hint");
                CustomEventService.SetEventType(ModMain.FrmSetupUIRequired.HintCustom, EventType.None);
                break;
            }
            case 2: // 联网
            {
                ModMain.FrmSetupUIRequired.PanCustomPreset.Visibility = Visibility.Collapsed;
                ModMain.FrmSetupUIRequired.PanCustomLocal.Visibility = Visibility.Collapsed;
                ModMain.FrmSetupUIRequired.PanCustomNet.Visibility = Visibility.Visible;
                ModMain.FrmSetupUIRequired.HintCustom.Visibility = Visibility.Visible;
                ModMain.FrmSetupUIRequired.HintCustomWarn.Visibility =
                    States.Hint.UntrustedHomepage ? Visibility.Collapsed : Visibility.Visible;
                ModMain.FrmSetupUIRequired.HintCustom.Text =
                    Lang.Text("Setup.Ui.Homepage.NetUpdate.Hint");
                CustomEventService.SetEventType(ModMain.FrmSetupUIRequired.HintCustom, EventType.OpenUrl);
                CustomEventService.SetEventData(ModMain.FrmSetupUIRequired.HintCustom,
                    "https://github.com/RainYooSuki/PCL-CE/discussions");
                break;
            }
            case 3: // 预设
            {
                ModMain.FrmSetupUIRequired.PanCustomPreset.Visibility = Visibility.Visible;
                ModMain.FrmSetupUIRequired.PanCustomLocal.Visibility = Visibility.Collapsed;
                ModMain.FrmSetupUIRequired.PanCustomNet.Visibility = Visibility.Collapsed;
                ModMain.FrmSetupUIRequired.HintCustom.Visibility = Visibility.Collapsed;
                ModMain.FrmSetupUIRequired.HintCustomWarn.Visibility = Visibility.Collapsed;
                break;
            }
        }

        ModMain.FrmSetupUIRequired.CardCustom.TriggerForceResize();
    }

    // 高级材质
    public static void UiBlur(bool value)
    {
        if (ModMain.frmSetupUI is null)
            return;

        ModMain.FrmSetupUIRequired.PanBlurValue.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        UiBlurValue(value ? Config.Preference.Blur.Radius : 0);
    }

    public static void UiBlurValue(int value)
    {
        System.Windows.Application.Current.Resources["BlurRadius"] = value * 1.0d;
    }

    public static void UiBlurSamplingRate(int value)
    {
        System.Windows.Application.Current.Resources["BlurSamplingRate"] = value * 0.01d;
    }

    public static void UiBlurType(int value)
    {
        System.Windows.Application.Current.Resources["BlurType"] = (KernelType)value;
    }

    // 顶部栏
    public static void UiLogoType(int value)
    {
        if (ThemeService.CurrentTheme == ColorTheme.HmclBlue) value = 4;
        switch (value)
        {
            case 0: // 无
            {
                ModMain.FrmMainRequired.ShapeTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.BtnTitleHelp.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.ShapeHMCLTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.LabTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.ImageTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.CELogo.Visibility = Visibility.Collapsed;
                if (ModMain.frmSetupUI is not null)
                {
                    ModMain.FrmSetupUIRequired.CheckLogoLeft.Visibility = Visibility.Visible;
                    ModMain.FrmSetupUIRequired.PanLogoText.Visibility = Visibility.Collapsed;
                    ModMain.FrmSetupUIRequired.PanLogoChange.Visibility = Visibility.Collapsed;
                }

                break;
            }
            case 1: // 默认
            {
                ModMain.FrmMainRequired.ShapeTitleLogo.Visibility = Visibility.Visible;
                ModMain.FrmMainRequired.BtnTitleHelp.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.ShapeHMCLTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.LabTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.ImageTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.CELogo.Visibility = Visibility.Visible;
                if (ModMain.frmSetupUI is not null)
                {
                    ModMain.FrmSetupUIRequired.CheckLogoLeft.Visibility = Visibility.Collapsed;
                    ModMain.FrmSetupUIRequired.PanLogoText.Visibility = Visibility.Collapsed;
                    ModMain.FrmSetupUIRequired.PanLogoChange.Visibility = Visibility.Collapsed;
                }

                break;
            }
            case 2: // 文本
            {
                ModMain.FrmMainRequired.ShapeTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.BtnTitleHelp.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.ShapeHMCLTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.LabTitleLogo.Visibility = Visibility.Visible;
                ModMain.FrmMainRequired.ImageTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.CELogo.Visibility = Visibility.Visible;
                if (ModMain.frmSetupUI is not null)
                {
                    ModMain.FrmSetupUIRequired.CheckLogoLeft.Visibility = Visibility.Collapsed;
                    ModMain.FrmSetupUIRequired.PanLogoText.Visibility = Visibility.Visible;
                    ModMain.FrmSetupUIRequired.PanLogoChange.Visibility = Visibility.Collapsed;
                }

                _ = Config.Preference.WindowTitleCustomText;
                break;
            }
            case 3: // 图片
            {
                ModMain.FrmMainRequired.ShapeTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.BtnTitleHelp.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.ShapeHMCLTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.LabTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.ImageTitleLogo.Visibility = Visibility.Visible;
                ModMain.FrmMainRequired.CELogo.Visibility = Visibility.Visible;
                if (ModMain.frmSetupUI is not null)
                {
                    ModMain.FrmSetupUIRequired.CheckLogoLeft.Visibility = Visibility.Collapsed;
                    ModMain.FrmSetupUIRequired.PanLogoText.Visibility = Visibility.Collapsed;
                    ModMain.FrmSetupUIRequired.PanLogoChange.Visibility = Visibility.Visible;
                }

                try
                {
                    ModMain.FrmMainRequired.ImageTitleLogo.Source = ModBase.exePath + @"PCL\Logo.png";
                }
                catch (Exception ex)
                {
                    ModMain.FrmMainRequired.ImageTitleLogo.Source = null;
                    ModBase.Log(
                        ex,
                        "显示标题栏图片失败",
                        ModBase.LogLevel.Msgbox,
                        userSummary: Lang.Text("Setup.Error.OperationFailed"));
                }

                break;
            }
            case 4: //HMCL (愚人节)
                ModMain.FrmMainRequired.ShapeTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.ShapeHMCLTitleLogo.Visibility = Visibility.Visible;
                ModMain.FrmMainRequired.LabTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.ImageTitleLogo.Visibility = Visibility.Collapsed;
                ModMain.FrmMainRequired.BtnTitleHelp.Visibility = Visibility.Visible;
                if (ModMain.frmSetupUI is not null) 
                {
                    ModMain.FrmSetupUIRequired.CheckLogoLeft.Visibility = Visibility.Collapsed;
                    ModMain.FrmSetupUIRequired.PanLogoText.Visibility = Visibility.Collapsed;
                    ModMain.FrmSetupUIRequired.PanLogoChange.Visibility = Visibility.Collapsed;
                }

                break;
        }

        UiLogoLeft(Config.Preference.TopBarLeftAlign);
        if (ModMain.frmSetupUI is not null)
            ModMain.FrmSetupUIRequired.CardLogo.TriggerForceResize();
    }

    public static void UiLogoText(string value)
    {
        ModMain.FrmMainRequired.LabTitleLogo.Text = value;
    }

    public static void UiLogoLeft(bool value)
    {
        ModMain.FrmMainRequired.PanTitleMain.ColumnDefinitions[0].Width = new GridLength(
            value && Config.Preference.WindowTitleType == LauncherTitleType.None ? 0 : 1,
            GridUnitType.Star);
    }

    #endregion

    #region System

    // 调试选项
    public static void SystemDebugMode(bool value)
    {
        ModBase.modeDebug = value;
    }

    public static void SystemDebugAnim(int value)
    {
        ModAnimation.aniSpeed = value >= 30
            ? 200d
            : ModBase.MathClamp(value * 0.1d + 0.1d, 0.1d, 3d);
    }

    public static void SystemHttpProxy(string value)
    {
        if (value.IsNullOrWhiteSpace()) return;
        try
        {
            HttpProxyManager.Instance.CustomProxyAddress = new Uri(value);
        }
        catch (Exception ex)
        {
            ModBase.Log(ex, "HTTP 代理应用出错");
        }
    }

    public static void SystemHttpProxyType(int value)
    {
        var mode = (HttpProxyManager.ProxyMode)value;
        HttpProxyManager.Instance.Mode = Enum.IsDefined(mode)
            ? mode
            : HttpProxyManager.Instance.Mode;
    }

    public static void SystemHttpProxyCustomUsername(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            var password = Config.Network.HttpProxy.CustomPassword;
            HttpProxyManager.Instance.Credentials = new NetworkCredential(value, password);
        }
        else
        {
            HttpProxyManager.Instance.Credentials = null;
        }
    }

    public static void SystemHttpProxyCustomPassword(string value)
    {
        var username = Config.Network.HttpProxy.CustomUsername;
        HttpProxyManager.Instance.Credentials = !string.IsNullOrEmpty(username)
            ? new NetworkCredential(username, value)
            : null;
    }

    #endregion

    #region Version

    // 游戏内存
    public static void VersionRamType(int type)
    {
        if (ModMain.frmInstanceSetup is null)
            return;
        ModMain.FrmInstanceSetupRequired.RamType(type);
    }

    // 服务器
    public static void VersionServerLogin(int type)
    {
        if (ModMain.frmInstanceSetup is null)
            return;
        // 为第三方登录清空缓存以更新描述
        ModBase.WriteIni(ModFolder.mcFolderSelected + "PCL.ini", "InstanceCache", "");
        if (PageInstanceLeft.McInstance is null)
            return;
        PageInstanceLeft.McInstance = new McInstance(PageInstanceLeft.McInstanceRequired.Name).Load();
        ModLoader.LoaderFolderRun(ModInstanceList.mcInstanceListLoader, ModFolder.mcFolderSelected,
            ModLoader.LoaderFolderRunType.ForceRun, 1, @"versions\");
    }

    #endregion
}
