using System.Text;

namespace CustomToolbox.BilibiliApi.Extensions;

/// <summary>
/// 字串擴充功能
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// 轉換成 Base64 字串
    /// </summary>
    /// <param name="value">字串</param>
    /// <returns>字串</returns>
    public static string ToBase64String(this string value)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
    }

    /// <summary>
    /// 轉換成一般字串
    /// </summary>
    /// <param name="value">字串</param>
    /// <returns>字串</returns>
    public static string FromBase64String(this string value)
    {
        return Encoding.UTF8.GetString(Convert.FromBase64String(value));
    }
}