using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Sets;
using Downloader;
using OpenCCNET;
using Serilog.Events;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CustomToolbox.Common.Utils;

/// <summary>
/// LyRiCs 工具
/// <para>Source: https://github.com/YoutubeClipPlaylist/YoutubeClipPlaylist/blob/master/src/Helper/LyricHelper.ts</para>
/// <para>Author: 陳鈞</para>
/// <para>License: MIT License</para>
/// <para>MIT License: https://github.com/YoutubeClipPlaylist/YoutubeClipPlaylist/blob/master/LICENSE</para>
/// <para>Source: https://www.cnblogs.com/Wayou/p/sync_lyric_with_html5_audio.html</para>
/// <para>Author: 刘哇勇</para>
/// <para>License: CC BY-NC-SA</para>
/// <para>Source: https://github.com/AioiLight/LRCDotNet/blob/master/LRCDotNet/Parser.cs</para>
/// <para>Author: AioiLight</para>
/// <para>License: MIT License</para>
/// <para>MIT License: https://github.com/AioiLight/LRCDotNet/blob/master/LICENSE</para>
/// <para>Source: https://github.com/OpportunityLiu/LrcParser</para>
/// <para>Author: OpportunityLiu</para>
/// <para>License: Apache License 2.0</para>
/// <para>Apache License 2.0: https://github.com/OpportunityLiu/LrcParser/blob/master/LICENSE</para>
/// </summary>
public partial class LyricsUtil
{
    /// <summary>
    /// WMain
    /// </summary>
    private static WMain? _WMain = null;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="wMain">WMain</param>
    public static void Init(WMain wMain)
    {
        _WMain = wMain;
    }

    /// <summary>
    /// 取得處理過的 *.lrc 檔案的路徑
    /// </summary>
    /// <param name="url">字串，*.lrc 檔案的網址</param>
    /// <param name="translateToTChinese">布林值，轉換成正體中文，預設值為 false</param>
    /// <returns>字串，*.lrc 檔案的路徑</returns>
    [SuppressMessage("GeneratedRegex", "SYSLIB1045:轉換為 'GeneratedRegexAttribute'。", Justification = "<暫止>")]
    public static async Task<string> GetProcessedLrcFilePath(
        string url,
        bool translateToTChinese = false)
    {
        string path = string.Empty;

        try
        {
            // 從網址取得檔案名稱。
            string fileName = url[(url.LastIndexOf('/') + 1)..];

            if (string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }

            // 組合成下載路徑。
            path = Path.Combine(VariableSet.LyricsFolderPath, fileName);

            // 判斷是否已有本地檔案的存在。
            if (File.Exists(path))
            {
                DateTime dtCreateTime = File.GetCreationTime(path);

                // 當是今天建立的檔案時，則直接回傳檔案的路徑。
                if (dtCreateTime.Date >= DateTime.Now.Date)
                {
                    return path;
                }
            }

            DownloadService downloadService = DownloaderUtil.GetDownloadService(
                action: new Action(() =>
                {
                    if (File.Exists(path))
                    {
                        string rawContent = File.ReadAllText(path);

                        if (translateToTChinese)
                        {
                            rawContent = ZhConverter.HansToTW(rawContent, true);
                        }

                        // 將文字拆分成字串陣列。
                        string[] lines = rawContent.Split("\r\n\u0085\u2028\u2029".ToCharArray(),
                                StringSplitOptions.RemoveEmptyEntries)
                            .Select(n => n.TrimStart().TrimEnd())
                            .ToArray();

                        Regex regex = new("\\[(?<m>\\d+):(?<s>\\d+)([:.](?<ms>\\d+))*\\]");

                        // 暫存用的字串列表。
                        List<string> tempList = [];

                        // 前處理字串。（忽略 ID tags）
                        foreach (string line in lines)
                        {
                            // 取得歌詞的部分。
                            string text = regex.Replace(line, string.Empty);

                            // 排除沒有時間的行。
                            if (regex.IsMatch(line))
                            {
                                tempList.Add(line);
                            }
                        }

                        List<LyricData> outputList = [];

                        foreach (string item in tempList)
                        {
                            // 取得歌詞的部分。
                            string text = regex.Replace(item, string.Empty);

                            MatchCollection matches = regex.Matches(item);

                            foreach (Match singleMatch in matches.Cast<Match>())
                            {
                                // 只處理結果為成功的資料。
                                if (singleMatch.Success)
                                {
                                    int minutes = int.TryParse(singleMatch.Groups["m"].Value, out int rMinutes) ? rMinutes : 0,
                                        seconds = int.TryParse(singleMatch.Groups["s"].Value, out int rSeconds) ? rSeconds : 0,
                                        milliseconds = int.TryParse(singleMatch.Groups["ms"].Value.PadRight(3, '0'), out int rMilliseconds) ? rMilliseconds : 0;

                                    TimeSpan timeSpan = new(0, 0, minutes, seconds, milliseconds);

                                    outputList.Add(new LyricData()
                                    {
                                        Time = timeSpan,
                                        Text = text
                                    });
                                }
                            }
                        }

                        outputList.Sort((m, n) => m.Time.CompareTo(n.Time));

                        StringBuilder stringBuilder = new();

                        foreach (LyricData lyricData in outputList)
                        {
                            stringBuilder.AppendLine($"[{lyricData.Time:mm\\:ss\\.ff}]{lyricData.Text}");
                        }

                        using StreamWriter streamWriter = new(path, false);

                        streamWriter.Write(stringBuilder.ToString()
                            .TrimEnd(Environment.NewLine.ToCharArray()));
                        streamWriter.Close();
                    }
                }));

            await downloadService.DownloadFileTaskAsync(url, path);
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }

        return path;
    }
}