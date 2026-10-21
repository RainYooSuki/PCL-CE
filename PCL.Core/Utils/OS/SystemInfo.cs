using System;
using System.Runtime.InteropServices;

namespace PCL.Core.Utils.OS;

public static class SystemInfo
{
    /// <summary>
    /// 是否为 32 位系统。
    /// </summary>
    public static readonly bool Is32BitSystem = !Environment.Is64BitOperatingSystem;

    /// <summary>
    /// 是否为 ARM64 架构。
    /// </summary>
    public static readonly bool IsArm64System = RuntimeInformation.OSArchitecture == Architecture.Arm64;

    /// <summary>
    /// 是否使用 GBK 编码。
    /// </summary>
    public static readonly bool IsGBKEncoding = KernelInterop.GetACP() == 936;

    /// <summary>
    /// 系统信息描述，例如 Microsoft Windows 11 专业工作站版 10.0.22635.0
    /// </summary>
    public static readonly string OSInfo = $"{RuntimeInformation.OSDescription} {Environment.OSVersion.Version}";

    /// <summary>
    /// 获取主显示器的当前刷新率（Hz）。无法获取时返回 60。
    /// </summary>
    public static int GetPrimaryDisplayRefreshRate()
    {
        try
        {
            var devMode = new DevMode
            {
                dmDeviceName = string.Empty,
                dmFormName = string.Empty,
                dmSize = (short)Marshal.SizeOf<DevMode>()
            };
            if (EnumDisplaySettings(null, EnumCurrentSettings, ref devMode) && devMode.dmDisplayFrequency > 1)
                return Math.Clamp(devMode.dmDisplayFrequency, 1, 1000);
        }
        catch
        {
            // 获取显示模式失败时使用安全的默认值。
        }

        return 60;
    }

    private const int EnumCurrentSettings = -1;

    [DllImport("user32.dll", EntryPoint = "EnumDisplaySettingsW", CharSet = CharSet.Unicode)]
    private static extern bool EnumDisplaySettings(string? deviceName, int modeNum, ref DevMode devMode);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct DevMode
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;
        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;
        public int dmPositionX;
        public int dmPositionY;
        public int dmDisplayOrientation;
        public int dmDisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;
        public short dmLogPixels;
        public int dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;
        public int dmPanningWidth;
        public int dmPanningHeight;
    }
}