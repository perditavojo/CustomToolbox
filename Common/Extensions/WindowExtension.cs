using Application = System.Windows.Application;
using System.Windows;

namespace CustomToolbox.Common.Extensions;

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
        return Application.Current.Windows.Cast<Window>().Any(n => n == window);
    }
}