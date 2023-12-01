using static CustomToolbox.Common.Sets.EnumSet;

namespace CustomToolbox.Common.Extensions;

/// <summary>
/// enum 的擴充方法
/// </summary>
public static class EnumExtension
{
    /// <summary>
    /// 取得小寫字串
    /// </summary>
    /// <param name="ytDlpUpdateChannelType">YtDlpUpdateChannelType</param>
    /// <returns>字串</returns>
    public static string GetLowerString(this YtDlpUpdateChannelType ytDlpUpdateChannelType)
    {
        return ytDlpUpdateChannelType.ToString().ToLowerInvariant();
    }
}