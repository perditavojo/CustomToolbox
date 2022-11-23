using System.Windows;

namespace CustomToolbox.Extensions;

/// <summary>
/// Window 的擴充方法
/// </summary>
internal static class WindowExtension
{
    /// <summary>
    /// Window 是否正在顯示
    /// </summary>
    /// <param name="window">Window</param>
    /// <returns>布林值</returns>
    public static bool IsShowing(this Window window)
    {
        return System.Windows.Application.Current.Windows.Cast<Window>().Any(n => n == window);
    }
}