using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Models.NetPlaylist;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Sets;
using Downloader;
using Serilog.Events;
using System.IO;
using System.Text.Json;

namespace CustomToolbox.Common.Utils;

/// <summary>
/// 短片清單工具
/// </summary>
public class ClipListUtil
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
    /// 取得網路播放清單資料
    /// </summary>
    /// <returns>Task&lt;List&lt;ClipListData&gt;&gt;</returns>
    public static async Task<List<ClipListData>> GetNetPlaylists()
    {
        List<ClipListData> outputList = new();

        try
        {
            string[] urls =
            {
                PlaylistUrlSet.YCPPlaylistsJsonUrl,
                PlaylistUrlSet.FCPPlaylistsJsonUrl,
                PlaylistUrlSet.FCPB23PlaylistsJsonUrl
            };

            foreach (string url in urls)
            {
                DownloadService downloadService = DownloaderUtil.GetDownloadService();

                Stream stream = await downloadService.DownloadFileTaskAsync(url);

                List<Playlists>? dataSet = JsonSerializer
                    .Deserialize<List<Playlists>>(
                        stream,
                        VariableSet.SharedJSOptions);

                if (dataSet != null)
                {
                    // 2022-10-03
                    // name 有 backup 的，通常都是放置於 OneDrive，目前 libmpv + yt-dlp 並不支援 OneDrive。
                    // twitcasting 則是不支援 seek。

                    // 過濾資料。
                    dataSet = dataSet.Where(n => !string.IsNullOrEmpty(n.Name) &&
                        !n.Name.ToLower().Contains("backup") &&
                        !string.IsNullOrEmpty(n.NameDisplay) &&
                        !string.IsNullOrEmpty(n.Route) &&
                        n.Tag != null &&
                        !n.Tag.Contains("onedrive") &&
                        !n.Tag.Contains("twitcasting"))
                        .ToList();

                    foreach (Playlists playlists in dataSet)
                    {
                        string text = $"{playlists.NameDisplay}", playlistFileUrl = string.Empty;

                        if (url.Contains(PlaylistUrlSet.YCPBaseUrl))
                        {
                            text = $"[YoutubeClipPlaylist] {text}";

                            playlistFileUrl = $"{PlaylistUrlSet.YCPBaseUrl}{playlists.Route}";
                        }
                        else if (url.Contains(PlaylistUrlSet.FCPBaseUrl))
                        {
                            if (playlists.Tag != null &&
                                playlists.Tag.Contains("bilibili"))
                            {
                                text = $"[rubujo/CustomPlaylist] (Bilibili) {text}";
                            }
                            else
                            {
                                text = $"[rubujo/CustomPlaylist] {text}";
                            }

                            playlistFileUrl = $"{PlaylistUrlSet.FCPBaseUrl}{playlists.Route}";
                        }
                        else
                        {
                            playlistFileUrl = playlists.Route ?? string.Empty;
                        }

                        if (!string.IsNullOrEmpty(playlistFileUrl))
                        {
                            if (playlists.Maintainer != null &&
                                !string.IsNullOrEmpty(playlists.Maintainer.Name) &&
                                playlists.Maintainer.Name != "AutoGenerator")
                            {
                                text += $" {MsgSet.GetFmtStr(MsgSet.TemplateMaintainer, playlists.Maintainer.Name)}";
                            }

                            outputList.Add(new ClipListData(text, playlistFileUrl));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }

        outputList.Insert(0, new ClipListData(MsgSet.SelectPlease, string.Empty));

        return outputList;
    }

    /// <summary>
    /// 取得本地的短片清單資料
    /// </summary>
    /// <returns>Task&lt;List&lt;ClipListData&gt;&gt;</returns>
    public static List<ClipListData> GetLocalClipLists()
    {
        List<ClipListData> outputList = new();

        try
        {
            // 取得 ClipLists 資料夾內的短片清單檔案。
            List<string> files = CustomFunction
                .EnumerateFiles(
                    VariableSet.ClipListsFolderPath,
                    VariableSet.AllowedExts,
                    SearchOption.AllDirectories)
            .ToList();

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                outputList.Add(new ClipListData(fileName, filePath));
            }
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }

        outputList.Insert(0, new ClipListData(MsgSet.SelectPlease, string.Empty));

        return outputList;
    }

    /// <summary>
    /// 取得網路播放清單資料
    /// </summary>
    /// <returns>Task&lt;List&lt;List&lt;object&gt;&gt;&gt;</returns>
    public static async Task<List<List<object>>> GetNetPlaylist(string url)
    {
        List<List<object>> outputList = new();

        try
        {
            DownloadService downloadService = DownloaderUtil.GetDownloadService();

            Stream stream = await downloadService.DownloadFileTaskAsync(url);

            List<List<object>>? dataSet = JsonSerializer
                .Deserialize<List<List<object>>>(
                    stream,
                    VariableSet.SharedJSOptions);

            if (dataSet != null)
            {
                outputList.AddRange(dataSet);
            }
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }

        return outputList;
    }
}