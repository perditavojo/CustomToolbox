using Application = System.Windows.Application;
using ConsoleTableExt;
using CustomToolbox.BilibiliApi.Funcs;
using CustomToolbox.BilibiliApi.Models;
using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Utils;
using static CustomToolbox.Common.Sets.EnumSet;
using Humanizer;
using Label = System.Windows.Controls.Label;
using Microsoft.Playwright;
using OpenCCNET;
using ProgressBar = ModernWpf.Controls.ProgressBar;
using System.IO;
using System.Text.Json;
using Xabe.FFmpeg;
using YoutubeDLSharp;
using YoutubeDLSharp.Metadata;
using YoutubeDLSharp.Options;

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
    /// LOperation
    /// </summary>
    private static Label? _LOperation = null;

    /// <summary>
    /// PBProgress
    /// </summary>
    private static ProgressBar? _PBProgress = null;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="wMain">WMain</param>
    public static void Init(WMain wMain)
    {
        _WMain = wMain;
        _LOperation = _WMain.LOperation;
        _PBProgress = _WMain.PBProgress;
    }

    /// <summary>
    /// 執行獲取短片資訊
    /// </summary>
    /// <param name="url">字串，影片的網址</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task DoFetchClipInfo(string url, CancellationToken ct = default)
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
    /// 執行下載短片
    /// </summary>
    /// <param name="clipData">ClipData</param>
    /// <param name="isFullDownloadFirst">布林值，是否先下載完整短片，預設值為 false</param>
    /// <param name="useHardwareAcceleration">布林值，是否使用硬體加速解編碼，預設值為 false</param>
    /// <param name="hardwareAcceleratorType">HardwareAcceleratorType，硬體加速的類型，預設值為 HardwareAcceleratorType.Intel</param>
    /// <param name="deviceNo">數值，GPU 裝置的 ID 值，預設為 0</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task DoDownloadClip(
        ClipData clipData,
        bool isFullDownloadFirst = false,
        bool useHardwareAcceleration = false,
        HardwareAcceleratorType hardwareAcceleratorType = HardwareAcceleratorType.Intel,
        int deviceNo = 0,
        CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            string fileName = string.Join(
                "_",
                $"{clipData.No}.{clipData.Name}".Split(Path.GetInvalidFileNameChars()) ??
                Array.Empty<string>());

            YoutubeDL ytdl = ExternalProgram.GetYoutubeDL();

            OptionSet configuredOptionSet = ExternalProgram.GetConfiguredOptionSet(
                    startSeconds: clipData.StartTime.TotalSeconds,
                    endSeconds: clipData.EndTime.TotalSeconds,
                    fileName: fileName,
                    isAudioOnly: clipData.IsAudioOnly,
                    isFullDownloadFirst: isFullDownloadFirst);

            RunResult<string> runResult = await ytdl.RunVideoDownload(
                url: clipData.VideoUrlOrID,
                mergeFormat: DownloadMergeFormat.Unspecified,
                recodeFormat: VideoRecodeFormat.None,
                progress: GetDownloadProgress(),
                output: GetOutputProgress(),
                ct: ct,
                overrideOptions: configuredOptionSet);

            if (runResult.Success)
            {
                _WMain?.WriteLog(runResult.Data);

                // 計算間隔秒數。
                double durationSeconds = clipData.EndTime.TotalSeconds -
                    clipData.StartTime.TotalSeconds;

                // 當為下載完整短片且 durationSeconds 的值大於 0 時，
                // 需要再透過 FFmpeg 來分割短片檔案。
                if (isFullDownloadFirst)
                {
                    if (durationSeconds > 0)
                    {
                        string sourceFilePath = Path.GetFullPath(runResult.Data),
                            sourceFileName = Path.GetFileNameWithoutExtension(sourceFilePath),
                            sourceExtName = Path.GetExtension(sourceFilePath),
                            outputFileName = string.IsNullOrEmpty(fileName) ?
                                $"{sourceFileName}_Fixed{sourceExtName}" :
                                $"{fileName}_Fixed{sourceExtName}",
                            outputFilePath = Path.Combine(VariableSet.DownloadsFolderPath, outputFileName);

                        IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(sourceFilePath, ct);

                        IConversion conversion = ExternalProgram.GetConversion(
                               mediaInfo,
                               clipData.StartTime.TotalSeconds,
                               clipData.EndTime.TotalSeconds,
                               outputFilePath,
                               fixDuration: true,
                               useCodecCopy: false,
                               useHardwareAcceleration,
                               hardwareAcceleratorType,
                               deviceNo,
                               clipData.IsAudioOnly);

                        // 移除全部的 Meta 資料，以利傻瓜式操作。
                        //conversion.AddParameter("-map_metadata -1");

                        IConversionResult conversionResult = await conversion.Start(ct);

                        ExternalProgram.WriteConversionResult(conversionResult);

                        // TODO: 2023-01-13 待完成。
                    }
                }
            }
            else
            {
                if (runResult.ErrorOutput.Any())
                {
                    string errMsg = string.Join(
                        Environment.NewLine,
                        runResult.ErrorOutput);

                    _WMain?.WriteLog(errMsg);
                }
            }
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(ex.Message);
        }
        finally
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (_LOperation == null || _PBProgress == null)
                {
                    return;
                }

                _LOperation.Content = string.Empty;
                _PBProgress.Value = 0.0d;
            }));
        }
    }

    /// <summary>
    /// 執行燒錄字幕檔
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
            ct.ThrowIfCancellationRequested();

            // 取標籤資訊。
            ReceivedObject<TList> receivedTList = await SpaceFunction.GetTList(mid);

            if (receivedTList.Code != 0)
            {
                string message = $"[{receivedTList.Code}] {receivedTList.Message}" ?? MsgSet.MsgJobFailedAndErrorOccurred;

                _WMain?.WriteLog(message);

                return;
            }

            List<TidData> tidDataSet = new();

            TList? tlist = receivedTList.Data;

            // 當 tlist 等於 null，則表示沒有取到有效的標籤資訊。
            if (tlist == null)
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgDataParsingFailedAndCanceled,
                    mid));

                return;
            }

            // 理論上應該只抓的到音樂標籤。
            SetTidDataList(tidDataSet, tlist);

            if (tidDataSet.Count <= 0)
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgDataParsingFailedAndCanceled,
                    mid));

                return;
            }

            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgPrepToProduceClipListFile,
                mid));

            List<ClipData> originDataSource = new();

            int no = 1;

            foreach (TidData tidData in tidDataSet)
            {
                ct.ThrowIfCancellationRequested();

                int tid = tidData.TID, ps = 50;

                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgProcessingDataForTag,
                    tidData.Name ?? string.Empty,
                    tidData.TID.ToString()));

                // 取得分頁資訊。
                ReceivedObject<Page> receivedPage = await SpaceFunction.GetPage(mid, tid);

                if (receivedPage.Code != 0)
                {
                    string message = receivedPage.Message ?? MsgSet.MsgJobFailedAndErrorOccurred;

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

                    _WMain?.WriteLog(MsgSet.GetFmtStr(
                        MsgSet.MsgProcessingDataForPage,
                        pn.ToString(),
                        pages.ToString()));

                    ReceivedObject<List<VList>> receivedVLists = await SpaceFunction.GetVList(
                        mid,
                        tid,
                        pn,
                        ps);

                    if (receivedVLists.Code != 0 ||
                        receivedVLists.Data == null)
                    {
                        string message = receivedPage.Message ?? MsgSet.MsgJobFailedAndErrorOccurred;

                        _WMain?.WriteLog(message);

                        continue;
                    }

                    foreach (VList vlist in receivedVLists.Data)
                    {
                        ct.ThrowIfCancellationRequested();

                        _WMain?.WriteLog(MsgSet.GetFmtStr(
                            MsgSet.MsgProcessingDataForVideo,
                            processCount.ToString(),
                            videoCount.ToString()));

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

                        // 檢查影片的標題是否包含排除字詞。
                        if (!CheckVideoTitle(title))
                        {
                            _WMain?.WriteLog(MsgSet.GetFmtStr(MsgSet.MsgSkipThisVideo, title));

                            continue;
                        }

                        string length = vlist?.Length ?? string.Empty;

                        length = CommonFunction.GetFormattedLength(length);

                        TimeSpan endTime = TimeSpan.Parse(length);

                        double endSeconds = endTime.TotalSeconds;

                        // 有多個 Part 的影片，時間會全部加總在一起。
                        // 根據網路資料取平均值，一首歌大約 4 分鐘。
                        if (endTime.TotalMinutes > Properties.Settings.Default.B23ClipListMaxMinutes)
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

                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgProcessResult,
                    mid,
                    tid.ToString(),
                    videoCount.ToString(),
                    originDataSource.Count.ToString()));
            }

            if (originDataSource.Count <= 0)
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgDataParsingFailedAndCantCreateClipListFile,
                    mid));

                return;
            }

            // 反向排序 List，讓最舊的資料排第一筆。
            originDataSource.Reverse();

            // 重新更新 no。
            for (int i = 0; i < originDataSource.Count; i++)
            {
                originDataSource[i].No = i + 1;
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

            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgClipListFileGeneratedFor,
                mid,
                savedPath));

            // 開啟 ClipLists 資料夾。
            CustomFunction.OpenFolder(VariableSet.ClipListsFolderPath);
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(ex.Message);
        }
    }

    /// <summary>
    /// 執行拍攝 YouTube 訂閱者數量截圖
    /// </summary>
    /// <param name="channelId">字串，YouTube 頻道的 ID</param>
    /// <param name="inputSavedPath">字串，截圖的儲存路徑，預設值為空白</param>
    /// <param name="customSubscriberAmount">數值，自定義頻道訂閱者數量，預設值為 -1</param>
    /// <param name="useTranslate">布林值，套用中文翻譯，預設值為 false</param>
    /// <param name="useClip">布林值，將截圖裁切成正方形，預設值為 false</param>
    /// <param name="addTimestamp">布林值，加入日期戳記，預設值為 false</param>
    /// <param name="customTimestamp">字串，自定義的日期戳記，預設值為空白</param>
    /// <param name="blurBackground">布林值，模糊背景，預設值為 false</param>
    /// <param name="forceChromium">布林值，強制使用 Chromium，預設值為 false</param>
    /// <param name="isDevelopmentMode">布林值，開發模式，預設值為 false</param>
    /// <returns>Task</returns>
    public static async Task DoTakeYtscScrnshot(
        string channelId,
        string inputSavedPath = "",
        decimal customSubscriberAmount = -1,
        bool useTranslate = false,
        bool useClip = false,
        bool addTimestamp = false,
        string customTimestamp = "",
        bool blurBackground = false,
        bool forceChromium = false,
        bool isDevelopmentMode = false)
    {
        try
        {
            // 偵測 Playwright 使用的網頁瀏覽器。
            string browserChannel = await PlaywrightUtil.DetectBrowser(forceChromium);

            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgYtscToolUsedBrowserChannel,
                browserChannel));

            using IPlaywright playwright = await Playwright.CreateAsync();

            await using IBrowser browser = await playwright.Chromium.LaunchAsync(new()
            {
                Channel = browserChannel,
                Headless = !isDevelopmentMode
            });

            IPage page = await browser.NewPageAsync();

            // 瀏覽 YouTube Subscriber Counter 網站。
            await page.GotoAsync($"{UrlSet.YTSubscriberCounterUrl}{channelId}");

            // 隱藏 Header 區塊。
            await page.EvalOnSelectorAsync(
                SelectorSet.HeaderBlock,
                ExpressionSet.HideElement);

            // 判斷是否要裁切截圖。
            if (useClip)
            {
                // 取頁面上的頻道名稱文字。
                string channelName = await page.InnerTextAsync(SelectorSet.ChannelNameBlock);

                channelName = channelName.TrimEnd();

                // 判斷是否需要截斷頻道名稱。
                if (channelName.Length > VariableSet.SplitLength)
                {
                    int[] intArray = PlaywrightUtil.GetIntArray(channelName);

                    string[]? nameArray = channelName.Split(intArray);

                    if (nameArray != null)
                    {
                        if (nameArray.Length < VariableSet.ChannelNameRowLimit)
                        {
                            string newChannelName = string.Empty;

                            foreach (string str in nameArray)
                            {
                                newChannelName += $"<div>{str}</div>";
                            }

                            if (!string.IsNullOrEmpty(newChannelName))
                            {
                                // 替換文字。
                                await page.EvalOnSelectorAsync(
                                    SelectorSet.ChannelNameBlock,
                                    ExpressionSet.ChangeChannelNameBlock.Replace("{Value}", newChannelName));
                            }
                        }
                        else
                        {
                            // 當超出限制列數時，取消裁切截圖。
                            useClip = false;

                            _WMain?.WriteLog(MsgSet.GetFmtStr(
                                MsgSet.MsgYtscToolChNameTooLongCancelCut,
                                VariableSet.ChannelNameRowLimit.ToString()));
                        }
                    }
                }
            }

            // 判斷是否要翻譯文字。
            if (useTranslate)
            {
                // 替換文字。
                await page.EvalOnSelectorAsync(
                    SelectorSet.PrefixText,
                    ExpressionSet.ChangePrefixText);

                // 替換文字。
                await page.EvalOnSelectorAsync(
                    SelectorSet.SuffixText,
                    ExpressionSet.ChangeSuffixText);
            }

            // 判斷是否要加入日期戳記
            if (addTimestamp)
            {
                // 當 customTimestamp 為空值時，使用今日。
                if (string.IsNullOrEmpty(customTimestamp))
                {
                    customTimestamp = DateTime.Now.ToShortDateString();
                }

                // 統一使用 "."。
                customTimestamp = customTimestamp
                    .Replace("/", ".")
                    .Replace("-", ".")
                    .Replace(" ", ".");

                // 替換文字。
                await page.EvalOnSelectorAsync(
                    useTranslate ? SelectorSet.TranslateSuffixText : SelectorSet.SuffixText,
                    ExpressionSet.ChangeSuffixText1Dot5.Replace("{Value}", customTimestamp));
            }

            // 點選對話視窗的按鈕。
            await page.ClickAsync(SelectorSet.AlertDialogButton);

            // 判斷是否要模糊背景。
            if (blurBackground)
            {
                // 點選設定按鈕。
                await page.EvalOnSelectorAsync(SelectorSet.SettingButton, ExpressionSet.ClickElement);

                // 點選模糊背景。
                await page.EvalOnSelectorAsync(SelectorSet.BlurBackgroundCheckbox, ExpressionSet.ClickElement);

                // 點選關閉按鈕。
                await page.EvalOnSelectorAsync(SelectorSet.CloseButton, ExpressionSet.ClickElement);
            }

            // 隱藏 Chart 區塊。
            await page.EvalOnSelectorAsync(
                SelectorSet.ChartBlock,
                ExpressionSet.HideElement);

            // 判斷是否有設定自定義訂閱數。
            if (customSubscriberAmount > 0)
            {
                string acutalNumber = string.Format("{0:N0}", customSubscriberAmount);

                string tempHtml = string.Empty;

                foreach (char c in acutalNumber.ToCharArray())
                {
                    tempHtml += $"{PlaywrightUtil.SetHTML(c)}";
                }

                string expression = ExpressionSet.ChangeSubscribersBlock
                    .Replace("{Value}", tempHtml);

                // 設定自定義訂閱數。
                await page.EvalOnSelectorAsync(SelectorSet.SubscribersBlock, expression);
            }
            else
            {
                // 取頁面上的訂閱者數字。
                string subscriberAmount = await page.InnerTextAsync(SelectorSet.SubscribersBlock);

                // 將字串後處理。
                // 將字串後處理。
                subscriberAmount = subscriberAmount
                    .Replace("\r", string.Empty)
                    .Replace("\n", string.Empty)
                    .Replace("\t", string.Empty)
                    .Replace(",", string.Empty);

                // 將處理過的字串轉換成 decimal。
                customSubscriberAmount = decimal.TryParse(subscriberAmount, out decimal parsedResult) ?
                    parsedResult : -1;
            }

            // 延後執行，讓對話視窗有時間關閉。
            Thread.Sleep(VariableSet.SleepMs);

            // 截圖的儲存路徑。
            string savedPath = string.IsNullOrEmpty(inputSavedPath) ?
                Path.Combine(
                    $@"C:\Users\{Environment.UserName}\Desktop",
                    $"Screenshot_{DateTime.Now:yyyyMMddHHmmss}.png") :
                inputSavedPath;

            // 當變數值相等時才判斷目的地的資料夾是否存在。
            if (savedPath == inputSavedPath)
            {
                string? folderPath = Path.GetDirectoryName(savedPath) ?? string.Empty;

                if (!string.IsNullOrEmpty(folderPath) &
                    !Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }

            // 取得副檔名。
            string extName = Path.GetExtension(savedPath);

            // 拍攝頁面的截圖。
            await page.ScreenshotAsync(new()
            {
                Path = savedPath,
                Timeout = VariableSet.ScreenshotTimeout,
                Clip = useClip ? PlaywrightUtil.GetClip(customSubscriberAmount) : null,
                Type = extName == ".png" ? ScreenshotType.Png : ScreenshotType.Jpeg
            });

            _WMain?.WriteLog(MsgSet.MsgYtscToolTakeScreenshotFinished);
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgYtscToolFileSaveAt,
                savedPath));

            // 僅供測試使用。
            if (isDevelopmentMode)
            {
                Thread.Sleep(VariableSet.DevSleepMs);
            }
        }
        catch (Exception ex)
        {
            // 只有可以 Cancel 的才只輸出 ex.Message.ToString()。
            _WMain?.WriteLog(ex.ToString());
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

    /// <summary>
    /// 檢查標題
    /// </summary>
    /// <param name="value">字串，影片的標題</param>
    /// <returns>布林值</returns>
    private static bool CheckVideoTitle(string value)
    {
        bool isOkay = true;

        string[] excludedPhrases = Properties.Settings.Default
            .B23ClipListExcludedPhrases
            .Split(
                ";".ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);

        foreach (string phrase in excludedPhrases)
        {
            if (value.Contains(phrase))
            {
                isOkay = false;

                break;
            }
        }

        return isOkay;
    }

    /// <summary>
    /// 取得 Progress&lt;DownloadProgress&gt;
    /// </summary>
    /// <returns>Progress&lt;DownloadProgress&gt;</returns>
    private static Progress<DownloadProgress> GetDownloadProgress()
    {
        return new Progress<DownloadProgress>(downloadProgress =>
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (_LOperation == null || _PBProgress == null)
                {
                    return;
                }

                double currentPercent = downloadProgress.Progress * 100;

                _PBProgress.Value = currentPercent;

                string message = $"({downloadProgress.State})";

                message += $" 影片索引值：{downloadProgress.VideoIndex}";

                if (!string.IsNullOrEmpty(downloadProgress.DownloadSpeed))
                {
                    message += $" 下載速度：{downloadProgress.DownloadSpeed}";
                }

                if (!string.IsNullOrEmpty(downloadProgress.ETA))
                {
                    message += $" 剩餘時間：{downloadProgress.ETA}";
                }

                if (!string.IsNullOrEmpty(downloadProgress.TotalDownloadSize))
                {
                    message += $" 檔案大小：{downloadProgress.TotalDownloadSize}";
                }

                if (!string.IsNullOrEmpty(downloadProgress.Data))
                {
                    message += $" 資料：{downloadProgress.Data}";
                }

                _LOperation.Content = message;
            }));
        });
    }

    /// <summary>
    /// 取得 Progress&lt;string&gt;
    /// </summary>
    /// <returns>Progress&lt;string&gt;</returns>
    private static Progress<string> GetOutputProgress()
    {
        string tempFrag = string.Empty,
            tempValue = string.Empty;

        return new Progress<string>(value =>
        {
            value = value.TrimEnd();

            // 手動減速機制，針對 "(frag " 開的字串進行減速。
            int starIdx = value.LastIndexOf("(frag ");

            if (starIdx > -1)
            {
                string fragPart = value[starIdx..]
                    .Replace("(frag ", string.Empty)
                    .Replace(")", string.Empty);

                if (tempFrag != fragPart)
                {
                    _WMain?.WriteLog(value);

                    tempFrag = fragPart;
                }
            }
            else
            {
                // 手動減速機制，讓前後一樣的字串不輸出。
                if (value != tempValue)
                {
                    _WMain?.WriteLog(value);
                }
            }

            tempValue = value;
        });
    }
}