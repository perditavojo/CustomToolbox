using ConsoleTableExt;
using CustomToolbox.BiliBili.Funcs;
using CustomToolbox.Bilibili.Models;
using CustomToolbox.Common.Models;
using static CustomToolbox.Common.Sets.EnumSet;
using Humanizer;
using OpenCCNET;
using System.IO;
using System.Text.Json;
using Xabe.FFmpeg;
using YoutubeDLSharp;
using YoutubeDLSharp.Metadata;

namespace CustomToolbox.Common.Sets;

/// <summary>
/// 作業組
/// </summary>
internal class OperationSet
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
    /// 獲取短片資訊
    /// </summary>
    /// <param name="url">字串，影片的網址</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task FetchClipInfo(string url, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            YoutubeDL ytdl = ExternalProgram.GetYoutubeDL();

            RunResult<VideoData> result = await ytdl.RunVideoDataFetch(
                url: url,
                ct: ct,
                overrideOptions: ExternalProgram.GetConfiguredOptionSet());

            if (result.Success)
            {
                _WMain?.WriteLog(MsgSet.MsgClipInfoFetchSuceed);

                VideoData videoData = result.Data;

                string fmtResult = $"{Environment.NewLine}" +
                    $"{MsgSet.VideoDataChannel}{videoData.Channel}{Environment.NewLine}" +
                    $"{MsgSet.VideoDataUploader}{videoData.Uploader}{Environment.NewLine}" +
                    $"{MsgSet.VideoDataUploadDate}{videoData.UploadDate}{Environment.NewLine}" +
                    $"{MsgSet.VideoDataID}{videoData.ID}{Environment.NewLine}" +
                    $"{MsgSet.VideoDataTitle}{videoData.Title}{Environment.NewLine}" +
                    $"{MsgSet.VideoDataViewCount}{videoData.ViewCount}{Environment.NewLine}" +
                    $"{MsgSet.VideoDataDescription}{Environment.NewLine}" +
                    // 替換行字元至 Environment.NewLine。
                    $"{VariableSet.RegexAscii.Replace(videoData.Description, Environment.NewLine)}" +
                    Environment.NewLine +
                    $"{MsgSet.VideoDataFormats}{Environment.NewLine}";

                List<List<object>> tableFormats = new();

                if (videoData.Formats.Length > 0)
                {
                    foreach (FormatData formatData in videoData.Formats)
                    {
                        string frameRate = string.Empty,
                            videoBitrate = string.Empty,
                            audioBitrate = string.Empty,
                            audioSamplingRate = string.Empty,
                            audioChannels = string.Empty,
                            bitrate = string.Empty,
                            fileSize = string.Empty;

                        if (formatData.FrameRate != null)
                        {
                            double fr = Math.Round(
                               formatData.FrameRate.Value,
                               MidpointRounding.AwayFromZero);

                            if (fr > 0.0f)
                            {
                                frameRate = fr.ToString();
                            }
                        }

                        if (formatData.VideoBitrate != null)
                        {
                            double vbr = Math.Round(
                                formatData.VideoBitrate.Value,
                                MidpointRounding.AwayFromZero);

                            if (vbr > 0.0d)
                            {
                                videoBitrate = $"{vbr} kbps";
                            }
                        }

                        if (formatData.AudioBitrate != null)
                        {
                            double abr = Math.Round(
                                formatData.AudioBitrate.Value,
                                MidpointRounding.AwayFromZero);

                            if (abr > 0.0d)
                            {
                                audioBitrate = $"{abr} kbps";
                            }
                        }

                        if (formatData.AudioSamplingRate != null)
                        {
                            double asr = Math.Round(
                                formatData.AudioSamplingRate.Value,
                                MidpointRounding.AwayFromZero) / 1000;

                            if (asr > 0.0d)
                            {
                                audioSamplingRate = $"{asr} kHz";
                            }
                        }

                        if (formatData.AudioChannels != null)
                        {
                            audioChannels = $"{formatData.AudioChannels} ch";
                        }

                        if (formatData.Bitrate != null)
                        {
                            double br = Math.Round(
                                formatData.Bitrate.Value,
                                MidpointRounding.AwayFromZero);

                            if (br > 0.0d)
                            {
                                bitrate = $"{br} kpbs";
                            }
                        }

                        if (formatData.FileSize != null)
                        {
                            fileSize = formatData.FileSize.Value.Bytes().ToString();
                        }
                        else
                        {
                            if (formatData.ApproximateFileSize != null)
                            {
                                fileSize = $"~{formatData.ApproximateFileSize.Value.Bytes()}";
                            }
                        }

                        tableFormats.Add(new List<object>
                        {
                            formatData.Format,
                            formatData.Extension,
                            frameRate,
                            audioChannels,
                            fileSize,
                            bitrate,
                            formatData.Protocol,
                            formatData.VideoCodec,
                            videoBitrate,
                            formatData.AudioCodec,
                            audioBitrate,
                            audioSamplingRate,
                            formatData.ContainerFormat,
                            formatData.DynamicRange,
                        });
                    }

                    // 因為排版問題，此處字串不提供 i18n 化，強制使用英文。
                    fmtResult += ConsoleTableBuilder
                        .From(tableFormats)
                        .WithColumn(new List<string>
                        {
                            "Format",
                            "Extension",
                            "Frame rate",
                            "Audio channels",
                            "File size",
                            "Bitrate",
                            "Protocol",
                            "Video codec",
                            "Video bitrate",
                            "Audio codec",
                            "Audio bitrate",
                            "Audio sampling rate",
                            "Container format",
                            "Dynamic range",
                        })
                        .WithFormat(ConsoleTableBuilderFormat.MarkDown)
                        .Export()
                        .ToString();
                }

                if (Properties.Settings.Default.OpenCCS2TWP)
                {
                    fmtResult = ZhConverter.HansToTW(fmtResult, true);
                }

                _WMain?.WriteLog(fmtResult);
            }
            else
            {
                _WMain?.WriteLog(MsgSet.MsgClipInfoFetchFailed);

                string[] errors = result.ErrorOutput;
                string errMsg = string.Join(Environment.NewLine, errors);

                _WMain?.WriteLog(errMsg);
            }
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(ex.Message);
        }
    }

    /// <summary>
    /// 燒錄字幕檔
    /// </summary>
    /// <param name="videoFilePath">字串，視訊檔案的路徑</param>
    /// <param name="subtitleFilePath">字串，字幕檔案的路徑</param>
    /// <param name="outputPath">字串，輸出檔案的路徑</param>
    /// <param name="encoding">字串，字幕檔案的文字編碼，預設值為 "UTF-8"</param>
    /// <param name="applyFontSetting">布林值，套用字型設定，預設值為 false</param>
    /// <param name="fontName">字串，字型名稱，預設值為 null</param>
    /// <param name="useHardwareAcceleration">布林值，是否使用硬體加速解編碼，預設值為 false</param>
    /// <param name="hardwareAcceleratorType">EnumSet.HardwareAcceleratorType，硬體的類型，預設是 EnumSet.HardwareAcceleratorType.Intel</param>
    /// <param name="deviceNo">數值，GPU 裝置的 ID 值，預設為 0</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task DoBurnInSubtitle(
        string videoFilePath,
        string subtitleFilePath,
        string outputPath,
        string encoding = "UTF-8",
        bool applyFontSetting = false,
        string? fontName = null,
        bool useHardwareAcceleration = false,
        HardwareAcceleratorType hardwareAcceleratorType = HardwareAcceleratorType.Intel,
        int deviceNo = 0,
        CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            string videoExtName = Path.GetExtension(videoFilePath);

            // 取得影片的資訊。
            IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(videoFilePath, ct);

            switch (videoExtName)
            {
                case ".mp4":
                    {
                        IConversion conversion = ExternalProgram.GetBurnInSubtitleConversion(
                            mediaInfo,
                            subtitleFilePath,
                            outputPath,
                            encoding,
                            applyFontSetting,
                            fontName,
                            useHardwareAcceleration,
                            hardwareAcceleratorType,
                            deviceNo);

                        IConversionResult conversionResult = await conversion.Start(ct);

                        ExternalProgram.WriteConversionResult(conversionResult);
                    }

                    break;
                case ".mkv":
                    {
                        // 取得字幕檔的資訊。
                        IMediaInfo subtitleInfo = await FFmpeg.GetMediaInfo(subtitleFilePath, ct);

                        IConversion conversion = ExternalProgram.GetAddSubtitleStreamConversion(
                            mediaInfo,
                            subtitleInfo,
                            outputPath,
                            useHardwareAcceleration,
                            hardwareAcceleratorType,
                            deviceNo);

                        IConversionResult conversionResult = await conversion.Start(ct);

                        ExternalProgram.WriteConversionResult(conversionResult);
                    }

                    break;
                default:
                    _WMain?.WriteLog(MsgSet.GetFmtStr(
                        MsgSet.MsgErrorOccured,
                        MsgSet.MsgSelectedVideoNonSupported));

                    break;
            }
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(ex.Message);
        }
    }

    /// <summary>
    /// 執行產生 Bilibili 指定使用者的短片清單檔案
    /// </summary>
    /// <param name="mid">字串，目標使用者的 mid</param>
    /// <param name="exportJsonc">布林值，是否匯出 *.jsonc 格式，預設值為 false</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task DoGenerateB23ClipList(
        string mid,
        bool exportJsonc = false,
        CancellationToken ct = default)
    {
        try
        {
            // TODO: 2022-12-27 待修改完成。
            ct.ThrowIfCancellationRequested();

            // 取標籤資訊。
            ReceivedObject<TList> receivedTList = await SpaceFunction.GetTList(mid);

            if (receivedTList.Code != 0)
            {
                string message = receivedTList.Message ?? "作業失敗，發生非預期的錯誤";

                _WMain?.WriteLog(message);

                return;
            }

            List<TidData> tidDataSet = new();

            TList? tlist = receivedTList.Data;

            // 當 tlist 等於 null，則表示沒有取到有效的標籤資訊。
            if (tlist == null)
            {
                _WMain?.WriteLog($"資料解析失敗，沒有取到有效的標籤資訊。已取消產製 Bilibili 使用者（{mid}）的短片清單檔案");

                return;
            }

            // 理論上應該只抓的到音樂標籤。
            SetTidDataList(tidDataSet, tlist);

            if (tidDataSet.Count <= 0)
            {
                _WMain?.WriteLog($"資料解析失敗，沒有取到有效的標籤資訊。已取消產製 Bilibili 使用者（{mid}）的短片清單檔案");

                return;
            }

            _WMain?.WriteLog($"正在準備產製 Bilibili 使用者（{mid}）的短片清單檔案。");

            List<ClipData> originDataSource = new();

            int no = 1;

            foreach (TidData tidData in tidDataSet)
            {
                ct.ThrowIfCancellationRequested();

                int tid = tidData.TID, ps = 50;

                _WMain?.WriteLog($"正在處裡標籤「{tidData.Name}（{tidData.TID}）」的資料。");

                // 取得分頁資訊。
                ReceivedObject<Page> receivedPage = await SpaceFunction.GetPage(mid, tid);

                if (receivedPage.Code != 0)
                {
                    string message = receivedPage.Message ?? "作業失敗，發生非預期的錯誤";

                    _WMain?.WriteLog(message);

                    return;
                }

                // 取得此標籤下的影片數量。
                int videoCount = receivedPage.Data?.Count ?? -1;

                if (videoCount <= 0)
                {
                    return;
                }

                int pages = videoCount / ps, remainder = videoCount % ps;

                if (remainder > 0)
                {
                    pages++;
                }

                int processCount = 1;

                for (int pn = 1; pn <= pages; pn++)
                {
                    ct.ThrowIfCancellationRequested();

                    _WMain?.WriteLog($"正在處裡第 {pn}/{pages} 頁的資料。");

                    ReceivedObject<List<VList>> receivedVLists = await SpaceFunction.GetVList(mid, tid, pn, ps);

                    if (receivedVLists.Code != 0 ||
                        receivedVLists.Data == null)
                    {
                        string message = receivedPage.Message ?? "作業失敗，發生非預期的錯誤";

                        _WMain?.WriteLog(message);

                        continue;
                    }

                    foreach (VList vlist in receivedVLists.Data)
                    {
                        ct.ThrowIfCancellationRequested();

                        _WMain?.WriteLog($"正在處裡第 {processCount}/{videoCount} 部影片的資料。");

                        processCount++;

                        // 強制將網址掛上 "/p1"，以免部分有多分頁的影片會無法被解析。
                        string url = $"https://b23.tv/{vlist?.Bvid}/p1",
                            title = vlist?.Title ?? string.Empty;

                        // 處理 title。
                        if (!string.IsNullOrEmpty(title))
                        {
                            // 排除 title 內會破壞 JSON 字串結構的內容。
                            title = string.Join(" ", title.Split(Path.GetInvalidFileNameChars()));

                            // 判斷是否有啟用 OpenCC。
                            if (Properties.Settings.Default.OpenCCS2TWP)
                            {
                                // 透過 OpenCC 轉換成正體中文。
                                title = ZhConverter.HansToTW(title, true);
                            }
                        }

                        string length = vlist?.Length ?? string.Empty;

                        length = CommonFunction.GetFormattedLength(length);

                        TimeSpan endTime = TimeSpan.Parse(length);

                        double endSeconds = endTime.TotalSeconds;

                        // 有多個 Part 的影片，時間會全部加總在一起。
                        // 根據網路資料取平均值，一首歌大約 4 分鐘。
                        if (endTime.TotalMinutes > 4)
                        {
                            // 將結束秒數直接歸零。
                            endSeconds = 0;
                        }

                        // 判斷 dataSource 是否已存在同樣的資料。
                        if (originDataSource.Any(n => n.VideoUrlOrID == url))
                        {
                            continue;
                        }

                        originDataSource.Add(new ClipData()
                        {
                            VideoUrlOrID = url,
                            No = no,
                            Name = title,
                            StartTime = TimeSpan.FromSeconds(0),
                            EndTime = TimeSpan.FromSeconds(endSeconds),
                            SubtitleFileUrl = string.Empty,
                            IsAudioOnly = false,
                            IsLivestream = false
                        });

                        no++;
                    }
                }

                _WMain?.WriteLog(
                    $"Bilibili 使用者（{mid}）在此 tid（{tid}）下共有 {videoCount} 部影片，" +
                    $"已成功處理 {originDataSource.Count} 部影片的資料。");
            }

            if (originDataSource.Count <= 0)
            {
                _WMain?.WriteLog($"資料解析失敗，無法產生 Bilibili 使用者（{mid}）的秒數播放清單檔案。");

                return;
            }

            // 短片清單檔案儲存的路徑。
            string savedPath = Path.Combine(
                VariableSet.ClipListsFolderPath,
                $"{(exportJsonc ? $"{mid}" : $"ClipList_{mid}")}" +
                $".{(exportJsonc ? "jsonc" : "json")}");

            using FileStream fileStream = new(
                savedPath,
                new FileStreamOptions()
                {
                    Access = FileAccess.ReadWrite,
                    Mode = FileMode.Create,
                    Share = FileShare.ReadWrite
                });

            List<List<object>> newDataSource = new();

            foreach (ClipData clipData in originDataSource)
            {
                newDataSource.Add(new List<object>
                {
                    clipData.VideoUrlOrID ?? string.Empty,
                    clipData.StartTime.TotalSeconds,
                    clipData.EndTime.TotalSeconds,
                    clipData.Name ?? string.Empty,
                    clipData.SubtitleFileUrl ?? string.Empty
                });
            }

            object outDataSource = exportJsonc ? newDataSource : originDataSource;

            await JsonSerializer.SerializeAsync(
                fileStream,
                outDataSource,
                VariableSet.SharedJSOptions,
                ct);

            await fileStream.DisposeAsync();

            _WMain?.WriteLog($"已產生 Bilibili 使用者（{mid}）的短片清單檔案：{savedPath}");

            // 開啟 ClipLists 資料夾。
            CustomFunction.OpenFolder(VariableSet.ClipListsFolderPath);
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(ex.Message);
        }
    }

    /// <summary>
    /// 設定 List<TidData>
    /// </summary>
    /// <param name="dataSet">List&lt;TidData&gt;</param>
    /// <param name="tlist">TList</param>
    private static void SetTidDataList(List<TidData> dataSet, TList tlist)
    {
        // 只允許下列的 Tag 的資料。
        //※有些 Up 主會將音樂相關的影片放於動畫 Tag 下。

        if (tlist.Tag3 != null)
        {
            dataSet.Add(new TidData()
            {
                TID = tlist.Tag3.Tid,
                Name = tlist.Tag3.Name
            });
        }

        if (tlist.Tag28 != null)
        {
            dataSet.Add(new TidData()
            {
                TID = tlist.Tag28.Tid,
                Name = tlist.Tag28.Name
            });
        }

        if (tlist.Tag31 != null)
        {
            dataSet.Add(new TidData()
            {
                TID = tlist.Tag31.Tid,
                Name = tlist.Tag31.Name
            });
        }

        if (tlist.Tag59 != null)
        {
            dataSet.Add(new TidData()
            {
                TID = tlist.Tag59.Tid,
                Name = tlist.Tag59.Name
            });
        }

        if (tlist.Tag193 != null)
        {
            dataSet.Add(new TidData()
            {
                TID = tlist.Tag193.Tid,
                Name = tlist.Tag193.Name
            });
        }

        if (tlist.Tag29 != null)
        {
            dataSet.Add(new TidData()
            {
                TID = tlist.Tag29.Tid,
                Name = tlist.Tag29.Name
            });
        }
    }
}