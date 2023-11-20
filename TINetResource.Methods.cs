using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Sets;
using CustomToolbox.Common.Utils;
using Downloader;
using Serilog.Events;
using System.IO;
using System.Text.Json;

namespace CustomToolbox;

/// <summary>
/// TINetResource 的方法
/// </summary>
public partial class WMain
{
    /// <summary>
    /// 初始化網路資源
    /// </summary>
    private void InitNetResurce()
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(async () =>
            {
                CBNetPlaylists.ItemsSource = null;

                List<ClipListData> dataSource1 = await ClipListUtil.GetNetPlaylists();

                CBNetPlaylists.ItemsSource = dataSource1;
                CBNetPlaylists.DisplayMemberPath = nameof(ClipListData.Name);
                CBNetPlaylists.SelectedValuePath = nameof(ClipListData.Path);

                if (dataSource1.Count > 0)
                {
                    CBNetPlaylists.SelectedIndex = 0;
                }

                CBLocalClipLists.ItemsSource = null;

                List<ClipListData> dataSource2 = ClipListUtil.GetLocalClipLists();

                CBLocalClipLists.ItemsSource = dataSource2;
                CBLocalClipLists.DisplayMemberPath = nameof(ClipListData.Name);
                CBLocalClipLists.SelectedValuePath = nameof(ClipListData.Path);

                if (dataSource2.Count > 0)
                {
                    CBLocalClipLists.SelectedIndex = 0;
                }
            }));
        }
        catch (Exception ex)
        {
            WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 取得 *.lrc 檔案的網址
    /// </summary>
    /// <param name="videoID">字串，影片的 ID 值</param>
    /// <param name="videoName">字串，影片的名稱</param>
    /// <param name="startSeconds">字串，開始秒數</param>
    /// <returns>字串陣列</returns>
    private async Task<string[]> GetLrcFileUrl(
        string videoID,
        string videoName,
        string startSeconds)
    {
        string lrcFileUrl = string.Empty, offsetSeconds = "0";

        try
        {
            DownloadService downloadService = DownloaderUtil.GetDownloadService();

            Stream stream = await downloadService
                .DownloadFileTaskAsync(PlaylistUrlSet.YCPLyricsJsonUrl);

            List<List<object>>? dataSet = JsonSerializer
                .Deserialize<List<List<object>>>(stream, VariableSet.SharedJSOptions);

            if (dataSet != null)
            {
                List<object>? lyricData = dataSet
                    .FirstOrDefault(n => n[0].ToString() == videoID &&
                        n[1].ToString() == startSeconds);

                if (lyricData != null)
                {
                    if (int.TryParse(lyricData[2].ToString(), out int songID))
                    {
                        if (songID > 0)
                        {
                            WriteLog(
                                message: MsgSet.GetFmtStr(
                                    MsgSet.MsgSongID,
                                    songID.ToString()));

                            lrcFileUrl = PlaylistUrlSet.YCPLrcFileTemplateUrl
                                .Replace("[SongId]", songID.ToString());

                            WriteLog(
                                message: MsgSet.GetFmtStr(
                                    MsgSet.MsgFoundAvailableLrcFile,
                                    videoID,
                                    videoName,
                                    lrcFileUrl));

                            string tempOffsetSecnds = lyricData[4].ToString() ?? "0";

                            if (int.TryParse(startSeconds, out int iStartSecnds) &&
                                int.TryParse(tempOffsetSecnds, out int iOffsetSecnds))
                            {
                                WriteLog(
                                    message: MsgSet.GetFmtStr(
                                        MsgSet.MsgStartSeconds,
                                        iStartSecnds.ToString()));
                                WriteLog(
                                    message: MsgSet.GetFmtStr(
                                        MsgSet.MsgLrcFileOffsetSeconds,
                                        iOffsetSecnds.ToString()));

                                offsetSeconds = (iStartSecnds + iOffsetSecnds).ToString();
                            }
                            else
                            {
                                offsetSeconds = "0";
                            }

                            WriteLog(
                                message: MsgSet.GetFmtStr(
                                    MsgSet.MsgLrcFileActualOffsetSeconds,
                                    offsetSeconds.ToString()));
                        }
                        else if (songID == 0)
                        {
                            WriteLog(
                                message: MsgSet.GetFmtStr(
                                    MsgSet.MsgManualDisabledLyricSearch,
                                    videoID,
                                    videoName),
                                logEventLevel: LogEventLevel.Warning);
                        }
                        else if (songID == -1)
                        {
                            WriteLog(
                                message: MsgSet.GetFmtStr(
                                    MsgSet.MsgSongFindingFailed,
                                    videoID,
                                    videoName),
                                logEventLevel: LogEventLevel.Warning);
                        }
                        else
                        {
                            // result < -1。
                            WriteLog(
                                message: MsgSet.GetFmtStr(
                                    MsgSet.MsgFoundSongButNoLyric,
                                    videoID,
                                    videoName,
                                    songID.ToString()),
                                logEventLevel: LogEventLevel.Warning);
                        }
                    }
                }
                else
                {
                    WriteLog(
                        message: MsgSet.GetFmtStr(
                            MsgSet.MsgCanNotFindLrcFileInfo,
                            videoID,
                            videoName),
                        logEventLevel: LogEventLevel.Error);
                }
            }
        }
        catch (Exception ex)
        {
            WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }

        return
        [
            lrcFileUrl,
            offsetSeconds
        ];
    }

    /// <summary>
    /// 初始化排除字詞
    /// </summary>
    private void InitB23ClipListExcludedPhrases()
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                char[] separators = [';'];

                string[] tempValue = Properties.Settings.Default
                    .B23ClipListExcludedPhrases.Split(
                        separators,
                        StringSplitOptions.RemoveEmptyEntries);

                string value = string.Join(Environment.NewLine, tempValue);

                TBB23ClipListExcludedPhrases.Text = value;
            }));
        }
        catch (Exception ex)
        {
            WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }
}