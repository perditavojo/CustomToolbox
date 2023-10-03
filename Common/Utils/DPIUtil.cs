using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace CustomToolbox.Common.Utils;

/// <summary>
/// DPI 工具
/// <para>Source: https://stackoverflow.com/a/32888078</para>
/// <para>Author: Simon Mourier</para>
/// <para>License: CC BY-SA 4.0</para>
/// <para>CC BY-SA 4.0: https://creativecommons.org/licenses/by-sa/4.0/</para>
/// <para>Note: This class considers dpix = dpiy.</para>
/// </summary>
public static class DPIUtil
{
    /// <summary>
    /// 取得視窗的 DPI
    /// <para>You should always use this one and it will fallback if necessary。</para>
    /// <para>Reference: https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdpiforwindow</para>
    /// </summary>
    /// <param name="hwnd">IntPtr</param>
    /// <returns>數值</returns>
    public static int GetDpiForWindow(IntPtr hwnd)
    {
        nint hModule = LoadLibrary("user32.dll");
        // Windows 10 1607.
        nint ptr = GetProcAddress(hModule, lpProcName: "GetDpiForWindow");

        if (ptr == IntPtr.Zero)
        {
            return GetDpiForNearestMonitor(hwnd);
        }

        return Marshal.GetDelegateForFunctionPointer<GetDpiForWindowFn>(ptr)(hwnd);
    }

    /// <summary>
    /// 取得距離最近的螢幕的 DPI
    /// </summary>
    /// <param name="hwnd">IntPtr</param>
    /// <returns>數值</returns>
    public static int GetDpiForNearestMonitor(IntPtr hwnd) => GetDpiForMonitor(GetNearestMonitorFromWindow(hwnd));

    /// <summary>
    /// 取得距離最近的螢幕的 DPI
    /// </summary>
    /// <param name="x">數值，X</param>
    /// <param name="y">數值，Y</param>
    /// <returns>數值</returns>
    public static int GetDpiForNearestMonitor(int x, int y) => GetDpiForMonitor(GetNearestMonitorFromPoint(x, y));

    /// <summary>
    /// 取得螢幕的 DPI
    /// </summary>
    /// <param name="monitor">IntPtr</param>
    /// <param name="type">MonitorDpiType，預設值為 MonitorDpiType.Effective</param>
    /// <returns>數值</returns>
    public static int GetDpiForMonitor(IntPtr monitor, MonitorDpiType type = MonitorDpiType.Effective)
    {
        nint hModule = LoadLibrary("shcore.dll");
        // Windows 8.1.
        nint ptr = GetProcAddress(hModule, lpProcName: "GetDpiForMonitor");

        if (ptr == IntPtr.Zero)
        {
            return GetDpiForDesktop();
        }

        int hr = Marshal.GetDelegateForFunctionPointer<GetDpiForMonitorFn>(ptr)(monitor, type, out int x, out int y);

        if (hr < 0)
        {
            return GetDpiForDesktop();
        }

        return x;
    }

    /// <summary>
    /// 取得桌面的 DPI
    /// </summary>
    /// <returns>數值</returns>
    public static int GetDpiForDesktop()
    {
        int hr = D2D1CreateFactory(
            D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED,
            typeof(ID2D1Factory).GUID,
            IntPtr.Zero,
            out ID2D1Factory factory);

        if (hr < 0)
        {
            // We really hit the ground, don't know what to do next!
            return 96;
        }

        // Windows 7.
        factory.GetDesktopDpi(out float x, out float _);

        Marshal.ReleaseComObject(factory);

        return (int)x;
    }

    /// <summary>
    /// 取得桌面螢幕
    /// </summary>
    /// <returns>IntPtr</returns>
    public static IntPtr GetDesktopMonitor() =>
        GetNearestMonitorFromWindow(GetDesktopWindow());

    /// <summary>
    /// 取得殼層螢幕
    /// </summary>
    /// <returns>IntPtr</returns>
    public static IntPtr GetShellMonitor() =>
        GetNearestMonitorFromWindow(GetShellWindow());

    /// <summary>
    /// 取得視窗的距離最近的螢幕
    /// </summary>
    /// <param name="hwnd">IntPtr</param>
    /// <returns>IntPtr</returns>
    public static IntPtr GetNearestMonitorFromWindow(IntPtr hwnd) =>
        MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

    /// <summary>
    /// 取得點的距離最近的螢幕
    /// </summary>
    /// <param name="x">數值，X</param>
    /// <param name="y">數值，Y</param>
    /// <returns>IntPtr</returns>
    public static IntPtr GetNearestMonitorFromPoint(int x, int y) =>
        MonitorFromPoint(new POINT { x = x, y = y }, MONITOR_DEFAULTTONEAREST);

    /// <summary>
    /// 委派：取得視窗 FN 的 DPI
    /// </summary>
    /// <param name="hwnd">IntPtr</param>
    /// <returns>數值</returns>
    private delegate int GetDpiForWindowFn(IntPtr hwnd);

    /// <summary>
    /// 委派：取得視窗 FN 的 DPI
    /// </summary>
    /// <param name="hmonitor">IntPtr</param>
    /// <param name="dpiType">MonitorDpiType</param>
    /// <param name="dpiX">數值，X 的DPI</param>
    /// <param name="dpiY">數值，Y 的DPI</param>
    /// <returns>數值</returns>
    private delegate int GetDpiForMonitorFn(IntPtr hmonitor, MonitorDpiType dpiType, out int dpiX, out int dpiY);

    /// <summary>
    /// 常數：MONITOR_DEFAULTTONEAREST
    /// </summary>
    private const int MONITOR_DEFAULTTONEAREST = 2;

    /// <summary>
    /// 載入函式庫
    /// </summary>
    /// <param name="lpLibFileName">字串，函式庫檔案的名稱</param>
    /// <returns>IntPtr</returns>
    [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
    [SuppressMessage("Interoperability", "SYSLIB1054:使用 'LibraryImportAttribute' 而非 'DllImportAttribute' 在編譯時間產生 P/Invoke 封送處理程式碼", Justification = "<暫止>")]
    [SuppressMessage("Globalization", "CA2101:指定 P/Invoke 字串引數的封送處理", Justification = "<暫止>")]
    private static extern IntPtr LoadLibrary(string lpLibFileName);

    /// <summary>
    /// 取得進程的地址
    /// </summary>
    /// <param name="hModule">IntPtr，模組</param>
    /// <param name="lpProcName">字串，進程的名稱</param>
    /// <returns>IntPtr</returns>
    [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
    [SuppressMessage("Interoperability", "SYSLIB1054:使用 'LibraryImportAttribute' 而非 'DllImportAttribute' 在編譯時間產生 P/Invoke 封送處理程式碼", Justification = "<暫止>")]
    [SuppressMessage("Globalization", "CA2101:指定 P/Invoke 字串引數的封送處理", Justification = "<暫止>")]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    /// <summary>
    /// 來自點的螢幕
    /// </summary>
    /// <param name="pt">POINT</param>
    /// <param name="flags">數值</param>
    /// <returns>IntPtr</returns>
    [DllImport("user32")]
    [SuppressMessage("Interoperability", "SYSLIB1054:使用 'LibraryImportAttribute' 而非 'DllImportAttribute' 在編譯時間產生 P/Invoke 封送處理程式碼", Justification = "<暫止>")]
    private static extern IntPtr MonitorFromPoint(POINT pt, int flags);

    /// <summary>
    /// 來自視窗的螢幕
    /// </summary>
    /// <param name="hwnd">IntPtr</param>
    /// <param name="flags">數值</param>
    /// <returns>IntPtr</returns>
    [DllImport("user32")]
    [SuppressMessage("Interoperability", "SYSLIB1054:使用 'LibraryImportAttribute' 而非 'DllImportAttribute' 在編譯時間產生 P/Invoke 封送處理程式碼", Justification = "<暫止>")]
    private static extern IntPtr MonitorFromWindow(IntPtr hwnd, int flags);

    /// <summary>
    /// 取得桌面視窗
    /// </summary>
    /// <returns>IntPtr</returns>
    [DllImport("user32")]
    [SuppressMessage("Interoperability", "SYSLIB1054:使用 'LibraryImportAttribute' 而非 'DllImportAttribute' 在編譯時間產生 P/Invoke 封送處理程式碼", Justification = "<暫止>")]
    private static extern IntPtr GetDesktopWindow();

    /// <summary>
    /// 取得殼層視窗
    /// </summary>
    /// <returns>IntPtr</returns>
    [DllImport("user32")]
    [SuppressMessage("Interoperability", "SYSLIB1054:使用 'LibraryImportAttribute' 而非 'DllImportAttribute' 在編譯時間產生 P/Invoke 封送處理程式碼", Justification = "<暫止>")]
    private static extern IntPtr GetShellWindow();

    /// <summary>
    /// 點
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    private partial struct POINT
    {
        public int x;
        public int y;
    }

    [DllImport("d2d1")]
    [SuppressMessage("Interoperability", "SYSLIB1054:使用 'LibraryImportAttribute' 而非 'DllImportAttribute' 在編譯時間產生 P/Invoke 封送處理程式碼", Justification = "<暫止>")]
    private static extern int D2D1CreateFactory(
        D2D1_FACTORY_TYPE factoryType,
        [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
        IntPtr pFactoryOptions,
        out ID2D1Factory ppIFactory);

    /// <summary>
    /// 列舉：D2D1_FACTORY_TYPE
    /// </summary>
    private enum D2D1_FACTORY_TYPE
    {
        D2D1_FACTORY_TYPE_SINGLE_THREADED = 0,
        D2D1_FACTORY_TYPE_MULTI_THREADED = 1,
    }

    /// <summary>
    /// ID2D1Factory
    /// </summary>
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
        Guid("06152247-6f50-465a-9245-118bfd3b6007")]
    private interface ID2D1Factory
    {
        int ReloadSystemMetrics();

        [PreserveSig]
        void GetDesktopDpi(out float dpiX, out float dpiY);

        // 其餘的因為不需要所以不實做。
    }
}

/// <summary>
/// 列舉：螢幕的 DPI 類型
/// </summary>
public enum MonitorDpiType
{
    Effective = 0,
    Angular = 1,
    Raw = 2,
}