using CustomToolbox.Common.Extensions;
using ModernWpf;

namespace CustomToolbox.Common.Utils;

/// <summary>
/// 應用程式主題工具
/// </summary>
public class AppThemeUtil
{
    /// <summary>
    /// 設定應用程式使用的主題
    /// </summary>
    /// <param name="applicationTheme">ApplicationTheme，預設值是 null</param>
    /// <returns>字串</returns>
    public static string SetAppTheme(ApplicationTheme? applicationTheme = null)
    {
        string message = string.Empty;

        try
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.AppTheme))
            {
                Properties.Settings.Default.AppTheme = nameof(ApplicationTheme.Light);
                Properties.Settings.Default.Save();
            }

            applicationTheme ??= Properties.Settings.Default.AppTheme switch
            {
                nameof(ApplicationTheme.Light) => ApplicationTheme.Light,
                nameof(ApplicationTheme.Dark) => ApplicationTheme.Dark,
                _ => ApplicationTheme.Light
            };


            string value = applicationTheme?.ToString() ?? string.Empty;

            if (Properties.Settings.Default.AppTheme != value)
            {
                Properties.Settings.Default.AppTheme = value;
                Properties.Settings.Default.Save();
            }

            ThemeManager.Current.ApplicationTheme = applicationTheme;
        }
        catch (Exception ex)
        {
            message = ex.GetExceptionMessage();
        }

        return message;
    }

    /// <summary>
    /// 取得應用程式設定使用的主題
    /// </summary>
    /// <returns>ApplicationTheme</returns>
    public static ApplicationTheme GetAppTheme()
    {
        return Properties.Settings.Default.AppTheme switch
        {
            nameof(ApplicationTheme.Light) => ApplicationTheme.Light,
            nameof(ApplicationTheme.Dark) => ApplicationTheme.Dark,
            _ => ApplicationTheme.Light
        };
    }

    /// <summary>
    ///  取得應用程式的主題
    /// </summary>
    /// <param name="value">字串，值</param>
    /// <returns>ApplicationTheme</returns>
    public static ApplicationTheme GetAppTheme(string value)
    {
        return value switch
        {
            nameof(ApplicationTheme.Light) => ApplicationTheme.Light,
            nameof(ApplicationTheme.Dark) => ApplicationTheme.Dark,
            _ => ApplicationTheme.Light
        };
    }
}