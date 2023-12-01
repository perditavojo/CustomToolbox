using static CustomToolbox.Common.Sets.EnumSet;
using System.Diagnostics;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;

namespace CustomToolbox.Common.Extensions;

/// <summary>
/// YoutubeDL 的擴充方法
/// </summary>
public static class YoutubeDLExtension
{
    /// <summary>
    /// 執行更新至
    /// <para>自定義標籤的範例：stable@2023.07.06</para>
    /// </summary>
    /// <param name="youtubeDL">YoutubeDL</param>
    /// <param name="ytDlpUpdateChannelType">YtDlpUpdateChannelType，預設值為 YtDlpUpdateChannelType.Stable</param>
    /// <param name="customTag">字串，自定義的標籤，預設值為空白</param>
    /// <returns>Task&lt;string&gt;</returns>
    public static async Task<string> RunUpdateTo(
        this YoutubeDL youtubeDL,
        YtDlpUpdateChannelType ytDlpUpdateChannelType = YtDlpUpdateChannelType.Stable,
        string customTag = "")
    {
        string output = string.Empty;

        YoutubeDLProcess youtubeDLProcess = new(youtubeDL.YoutubeDLPath);

        youtubeDLProcess.OutputReceived += delegate (object? o, DataReceivedEventArgs e)
        {
            // 後補分隔符號。
            if (!string.IsNullOrEmpty(output))
            {
                output += "｜";
            }

            output += e.Data ?? string.Empty;  
        };

        // 有些訊息會出在 ErrorReceived。
        youtubeDLProcess.ErrorReceived += delegate (object? o, DataReceivedEventArgs e)
        {
            // 後補分隔符號。
            if (!string.IsNullOrEmpty(output))
            {
                output += "｜";
            }

            output += e.Data ?? string.Empty;
        };

        OptionSet optionSet = new();

        string updateChannelType = ytDlpUpdateChannelType switch
        {
            YtDlpUpdateChannelType.Stable => ytDlpUpdateChannelType.GetLowerString(),
            YtDlpUpdateChannelType.Nightly => ytDlpUpdateChannelType.GetLowerString(),
            YtDlpUpdateChannelType.Master => ytDlpUpdateChannelType.GetLowerString(),
            YtDlpUpdateChannelType.Custom => customTag,
            _ => YtDlpUpdateChannelType.Stable.GetLowerString()
        };

        if (string.IsNullOrEmpty(updateChannelType))
        {
            updateChannelType = YtDlpUpdateChannelType.Stable.GetLowerString();
        }

        optionSet.AddCustomOption("--update-to", updateChannelType);

        await youtubeDLProcess.RunAsync(null, optionSet);

        return output;
    }
}