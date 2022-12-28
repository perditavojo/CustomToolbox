using Application = System.Windows.Application;
using static CustomToolbox.Common.Sets.EnumSet;
using CustomToolbox.Common.Sets;
using CustomToolbox.Common.Utils;
using Label = System.Windows.Controls.Label;
using ProgressBar = ModernWpf.Controls.ProgressBar;
using System.Diagnostics;
using System.IO;
using Xabe.FFmpeg.Events;
using Xabe.FFmpeg;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;

namespace CustomToolbox.Common;

/// <summary>
/// 外部程式
/// </summary>
internal class ExternalProgram
{
    /// <summary>
    /// WMain
    /// </summary>
    private static WMain? _WMain = null;

    /// <summary>
    /// LYtDlpVersion
    /// </summary>
    private static Label? _LYtDlpVersion = null;

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
        _LYtDlpVersion = _WMain.LYtDlpVersion;
        _LOperation = _WMain.LOperation;
        _PBProgress = _WMain.PBProgress;
    }

    /// <summary>
    /// 檢查相依性檔案
    /// </summary>
    /// <param name="action">Action</param>
    /// <param name="isForceDownload">布林值，是否強制下載，預設值為 false</param>
    public static async void CheckDependencyFiles(Action? action, bool isForceDownload = false)
    {
        try
        {
            #region 檢查資料夾

            string[] folders =
            {
                VariableSet.BinsFolderPath,
                VariableSet.DownloadsFolderPath,
                VariableSet.ClipListsFolderPath,
                VariableSet.LogsFolderPath,
                VariableSet.LyricsFolderPath
            };

            foreach (string folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);

                    _WMain?.WriteLog(MsgSet.GetFmtStr(
                        MsgSet.MsgCreated,
                        folder));
                }
            }

            #endregion

            #region 檢查 yt-dlp

            if (!File.Exists(VariableSet.YtDlpPath) ||
                isForceDownload)
            {
                await DownloaderUtil.DownloadYtDlp();
            }
            else
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgFound,
                    VariableSet.YtDlpExecName));

                // 當今天的日期晚於設定檔中的日期時。
                if (DateTime.Now.Date > Properties.Settings.Default.YtDlpCheckTime.Date)
                {
                    UpdateYtDlp();
                }
                else
                {
                    SetYtDlpVersion();

                    _WMain?.WriteLog(MsgSet.GetFmtStr(
                        MsgSet.MsgYtDlpNotice,
                        $"{Properties.Settings.Default.YtDlpCheckTime:yyyy/MM/dd HH:mm:ss}"));
                }
            }

            #endregion

            #region 檢查 yt-dlp.conf

            if (!File.Exists(VariableSet.YtDlpConfPath) ||
                isForceDownload)
            {
                GetOptionSet(forceCreate: true);
            }
            else
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgFound,
                    VariableSet.YtDlpConfName));
            }

            #endregion

            #region 檢查 FFmpeg

            if ((!File.Exists(VariableSet.FFmpegPath) ||
                !File.Exists(VariableSet.FFprobePath)) ||
                isForceDownload)
            {
                await DownloaderUtil.DownloadFFmpeg();
            }
            else
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                        MsgSet.MsgFound,
                        $"{VariableSet.FFmpegExecName}, " +
                        $"{VariableSet.FFprobeExecName}"));
            }

            #endregion

            #region 檢查 sub_charenc_parameters.txt

            if (!File.Exists(VariableSet.SubCharencParametersTxtPath) ||
                isForceDownload)
            {
                await DownloaderUtil.DownloadSubCharencParametersTxt();
            }
            else
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgFound,
                    VariableSet.SubCharencParametersTxtFileName));
            }

            #endregion

            #region 檢查 aria2

            if (!File.Exists(VariableSet.Aria2Path) ||
                isForceDownload)
            {
                await DownloaderUtil.DownloadAria2();
            }
            else
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgFound,
                    VariableSet.Aria2ExecName));
            }

            #endregion

            #region 檢查 mpv.conf

            if (!File.Exists(VariableSet.MpvConfPath) ||
                isForceDownload)
            {
                CreateDefaultMpvConf();
            }
            else
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgFound,
                    VariableSet.MpvConfFileName));
            }

            #endregion

            #region 檢查 libmpv

            if (!File.Exists(VariableSet.LibMpvPath) ||
                isForceDownload)
            {
                await DownloaderUtil.DownloadLibMpv();
            }
            else
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgFound,
                    VariableSet.LibMpvDllFileName));
            }

            #endregion

            #region 檢查 ytdl_hook.lua

            if (!File.Exists(VariableSet.YtDlHookLuaPath) ||
                isForceDownload)
            {
                await DownloaderUtil.DownloadYtDlHookLua();
            }
            else
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgFound,
                    VariableSet.YtDlHookLuaFileName));
            }

            #endregion

            action?.Invoke();
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 更新 yt-dlp
    /// </summary>
    public static async void UpdateYtDlp()
    {
        YoutubeDL ytdl = GetYoutubeDL();

        string result = await ytdl.RunUpdate();

        _WMain?.WriteLog(result);

        // 當 yt-dlp 更新後，才需要提示要更新 FFmpeg。
        if (result.Contains("Updated yt-dlp to version"))
        {
            _WMain?.WriteLog(MsgSet.MsgUpdateFFmpeg);
        }

        Properties.Settings.Default.YtDlpCheckTime = DateTime.Now;
        Properties.Settings.Default.Save();

        SetYtDlpVersion();
    }

    /// <summary>
    /// 設定顯示 yt-dlp 的版本號
    /// </summary>
    public static void SetYtDlpVersion()
    {
        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
        {
            YoutubeDL ytdl = GetYoutubeDL();

            if (_LYtDlpVersion != null)
            {
                _LYtDlpVersion.Content = ytdl.Version;
            }
        }));
    }

    /// <summary>
    /// 取得 YoutubeDL
    /// </summary>
    /// <returns>YoutubeDL</returns>
    public static YoutubeDL GetYoutubeDL()
    {
        return new YoutubeDL()
        {
            FFmpegPath = VariableSet.FFmpegPath,
            IgnoreDownloadErrors = true,
            OutputFolder = VariableSet.DownloadsFolderPath,
            OverwriteFiles = true,
            RestrictFilenames = false,
            YoutubeDLPath = VariableSet.YtDlpPath,
        };
    }

    /// <summary>
    /// 建立預設的 mpv.conf
    /// </summary>
    public static void CreateDefaultMpvConf()
    {
        if (File.Exists(VariableSet.MpvConfPath))
        {
            File.Delete(VariableSet.MpvConfPath);

            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgDeleted,
                VariableSet.MpvConfFileName));
        }

        using StreamWriter streamWriter = new(VariableSet.MpvConfPath, false);

        streamWriter.Write(VariableSet.MpvConfTemplate);

        _WMain?.WriteLog(MsgSet.GetFmtStr(
            MsgSet.MsgCreated,
            VariableSet.MpvConfFileName));
    }

    /// <summary>
    /// 取得 OptionSet
    /// </summary>
    /// <param name="forceCreate">布林值，是否強制建立，預設值為 false</param>
    /// <returns>OptionSet</returns>
    public static OptionSet GetOptionSet(bool forceCreate = false)
    {
        if (!File.Exists(VariableSet.YtDlpConfPath) ||
            forceCreate)
        {
            if (File.Exists(VariableSet.YtDlpConfPath))
            {
                File.Delete(VariableSet.YtDlpConfPath);

                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgDeleted,
                    VariableSet.YtDlpConfName));
            }

            OptionSet optionSet = new()
            {
                ConcurrentFragments = Properties.Settings.Default.YtDlpConcurrentFragments,
                FfmpegLocation = VariableSet.FFmpegPath,
                Format = Properties.Settings.Default.YtDlpDefaultFormat,
                Output = VariableSet.YtDlpDefaultOutput,
                RemoveCacheDir = true,
                SkipUnavailableFragments = true,
                SubLangs = Properties.Settings.Default.YtDlpSubLangs
            };

            optionSet.WriteConfigFile(VariableSet.YtDlpConfPath);

            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgCreated,
                VariableSet.YtDlpConfName));
        }

        OptionSet loadedOptionSet = OptionSet
            .LoadConfigFile(VariableSet.YtDlpConfPath);

        return loadedOptionSet;
    }

    /// <summary>
    /// 取得設定過的 OptionSet
    /// </summary>
    /// <param name="startSeconds">數值，開始秒數，預設值為 0.0d</param>
    /// <param name="endSeconds">數值，結束秒數，預設值為 0.0d</param>
    /// <returns>OptionSet</returns>
    public static OptionSet GetConfiguredOptionSet(
        double startSeconds = 0.0d,
        double endSeconds = 0.0d)
    {
        OptionSet optionSet = GetOptionSet();

        // 因 yt-dlp.exe 的文字編碼機制問題，故採用下列格式的名稱。
        string newOutput = Path.Combine(
            VariableSet.DownloadsFolderPath,
            $"%(id)s_{DateTime.Now:yyyyMMddHHmmss}.%(ext)s");

        // 暫時替換掉預設的設定值。
        optionSet.Output = newOutput;

        // 計算間隔秒數。
        double durationSeconds = startSeconds - endSeconds;

        // 當 durationSeconds 為負數時，執行下載影片的片段。
        if (durationSeconds < 0)
        {
            // 參考來源：https://www.reddit.com/r/youtubedl/wiki/howdoidownloadpartsofavideo/
            optionSet.Downloader = "ffmpeg";
            optionSet.DownloaderArgs = $"ffmpeg_i:-ss {startSeconds} -to {endSeconds}";

            // 為避免 Xabe.FFmpeg 轉檔時發生錯誤，故停用下列設定。
            optionSet.EmbedMetadata = false;
            optionSet.EmbedThumbnail = false;
            optionSet.EmbedSubs = false;
        }

        return optionSet;
    }

    /// <summary>
    /// 取得 IConversion
    /// </summary>
    /// <param name="mediaInfo">IMediaInfo</param>
    /// <param name="startSeconds">數值，開始秒數</param>
    /// <param name="endSeconds">數值，結束秒數</param>
    /// <param name="output">字串，檔案輸出的路徑</param>
    /// <param name="fixDuration">布林值，是否執行使用步驟，預設值為 false</param>
    /// <param name="useCodecCopy">布林值，是否使用 -c:v copy -c:a copy，預設值為 true</param>
    /// <param name="useHardwareAcceleration">布林值，是否使用硬體加速解編碼，預設值為 false</param>
    /// <param name="hardwareAcceleratorType">EnumSet.HardwareAcceleratorType，硬體的類型，預設是 EnumSet.HardwareAcceleratorType.Intel</param>
    /// <param name="deviceNo">數值，GPU 裝置的 ID 值，預設為 0</param>
    /// <param name="isAudioOnly">布林值，是否僅音訊，預設為 false</param>
    /// <returns>IConversion</returns>
    public static IConversion GetConversion(
        IMediaInfo mediaInfo,
        double startSeconds,
        double endSeconds,
        string output,
        bool fixDuration = false,
        bool useCodecCopy = true,
        bool useHardwareAcceleration = false,
        HardwareAcceleratorType hardwareAcceleratorType = HardwareAcceleratorType.Intel,
        int deviceNo = 0,
        bool isAudioOnly = false)
    {
        IConversion conversion = FFmpeg.Conversions.New()
            // 來源：https://trac.ffmpeg.org/wiki/Encode/YouTube
            // 因會產生錯誤，故取消。
            //.AddParameter("-pix_fmt yuv420p")
            .AddParameter("-movflags +faststart")
            .SetOutput(output)
            .SetOverwriteOutput(true);

        if (useHardwareAcceleration)
        {
            SetHardwareAccelerator(
                conversion,
                hardwareAcceleratorType,
                deviceNo);
        }

        if (fixDuration)
        {
            double durationSeconds = startSeconds - endSeconds;

            conversion.AddParameter($"-sseof {durationSeconds}", ParameterPosition.PreInput);

            if (!isAudioOnly)
            {
                conversion.AddStream(mediaInfo.VideoStreams);
            }

            conversion.AddStream(mediaInfo.AudioStreams);
        }
        else
        {
            List<IStream> streams = new();

            if (!isAudioOnly)
            {
                foreach (VideoStream stream in mediaInfo.VideoStreams.Cast<VideoStream>())
                {
                    if (useCodecCopy)
                    {
                        streams.Add(stream.SetCodec(VideoCodec.copy));
                    }
                    else
                    {
                        streams.Add(stream);
                    }
                }
            }

            foreach (AudioStream stream in mediaInfo.AudioStreams.Cast<AudioStream>())
            {
                if (useCodecCopy)
                {
                    streams.Add(stream.SetCodec(AudioCodec.copy));
                }
                else
                {
                    streams.Add(stream);
                }
            }

            conversion.AddStream(streams)
                .SetSeek(TimeSpan.FromSeconds(startSeconds))
                .SetInputTime(TimeSpan.FromSeconds(endSeconds));
        }

        conversion.OnDataReceived += Conversion_OnDataReceived;
        conversion.OnProgress += Conversion_OnProgress;

        return conversion;
    }

    /// <summary>
    /// 取得燒錄字幕的 IConversion
    /// </summary>
    /// <param name="mediaInfo">IMediaInfo，影片的資訊</param>
    /// <param name="subtitlePath">字串，字幕檔案的路徑</param>
    /// <param name="outputPath">字串，輸出檔案的路徑</param>
    /// <param name="subtitleEncode">字串，字幕的文字編碼，預設值為 "UTF-8"</param>
    /// <param name="applyFontSetting">布林值，套用字型設定，預設值為 false</param>
    /// <param name="subtitleStyle">字串，字幕的覆寫風格，預設值為 null</param>
    /// <param name="useHardwareAcceleration">布林值，是否使用硬體加速解編碼，預設值為 false</param>
    /// <param name="hardwareAcceleratorType">EnumSet.HardwareAcceleratorType，硬體的類型，預設是 EnumSet.HardwareAcceleratorType.Intel</param>
    /// <param name="deviceNo">數值，GPU 裝置的 ID 值，預設為 0</param>
    /// <returns>IConversion</returns>
    public static IConversion GetBurnInSubtitleConversion(
        IMediaInfo mediaInfo,
        string subtitlePath,
        string outputPath,
        string subtitleEncode = "UTF-8",
        bool applyFontSetting = false,
        string? subtitleStyle = null,
        bool useHardwareAcceleration = false,
        HardwareAcceleratorType hardwareAcceleratorType = HardwareAcceleratorType.Intel,
        int deviceNo = 0)
    {
        bool isSubtitleAdded = false;

        List<IStream> outputStreams = new();

        // 循例每一個 IStream。
        foreach (IStream stream in mediaInfo.Streams)
        {
            if (stream.StreamType == StreamType.Video)
            {
                // 將 IStream 轉換成 IVideoStream。
                if (stream is IVideoStream videoStream)
                {
                    // 用以避免對於多個 IVideoStream 加入字幕。
                    if (!isSubtitleAdded)
                    {
                        // 判斷是否有套用字型設定。
                        if (!applyFontSetting)
                        {
                            subtitleStyle = null;
                        }

                        // 針對該 IVideoStream 燒錄字幕。
                        videoStream.AddSubtitles(
                            subtitlePath,
                            encode: subtitleEncode,
                            style: subtitleStyle);

                        // 變更開關作用變數的值。
                        isSubtitleAdded = true;
                    }

                    // 將 IVideoStream 加入 outputStreams。
                    outputStreams.Add(videoStream);
                }
            }
            else
            {
                // 將 IStream 加入 outputStreams。
                outputStreams.Add(stream);
            }
        }

        // 2022-12-28 目前發現使用此方式燒錄字幕檔案時，
        // 若使用 mpv 來播放燒錄完成的視訊檔，將會無法正常的顯示字幕。

        // TODO: 2022-12-28 可能需要再調整燒錄字幕檔案的方式。

        IConversion conversion = FFmpeg.Conversions.New()
            // 來源：https://trac.ffmpeg.org/wiki/Encode/YouTube
            // 因會產生錯誤，故取消。
            //.AddParameter("-pix_fmt yuv420p")
            .AddParameter("-movflags +faststart")
            .AddStream(outputStreams)
            .SetOutput(outputPath)
            .SetOverwriteOutput(true);

        if (useHardwareAcceleration)
        {
            SetHardwareAccelerator(
                conversion,
                hardwareAcceleratorType,
                deviceNo);
        }

        conversion.OnDataReceived += Conversion_OnDataReceived;
        conversion.OnProgress += Conversion_OnProgress;

        return conversion;
    }

    /// <summary>
    /// 取得加入 ISubtitleStream 的 IConversion
    /// </summary>
    /// <param name="mediaInfo">IMediaInfo，影片的資訊</param>
    /// <param name="subtitleInfo">IMediaInfo，字幕檔的資訊</param>
    /// <param name="outputPath">字串，輸出檔案的路徑</param>
    /// <param name="useHardwareAcceleration">布林值，是否使用硬體加速解編碼，預設值為 false</param>
    /// <param name="hardwareAcceleratorType">EnumSet.HardwareAcceleratorType，硬體的類型，預設是 EnumSet.HardwareAcceleratorType.Intel</param>
    /// <param name="deviceNo">數值，GPU 裝置的 ID 值，預設為 0</param>
    /// <returns>IConversion</returns>
    public static IConversion GetAddSubtitleStreamConversion(
        IMediaInfo mediaInfo,
        IMediaInfo subtitleInfo,
        string outputPath,
        bool useHardwareAcceleration = false,
        HardwareAcceleratorType hardwareAcceleratorType = HardwareAcceleratorType.Intel,
        int deviceNo = 0)
    {
        // 取得 ISubtitleStream。
        IEnumerable<ISubtitleStream> subtitleStream = subtitleInfo.SubtitleStreams;

        IConversion conversion = FFmpeg.Conversions.New()
            // 來源：https://trac.ffmpeg.org/wiki/Encode/YouTube
            // 因會產生錯誤，故取消。
            //.AddParameter("-pix_fmt yuv420p")
            .AddParameter("-movflags +faststart")
            .AddStream(mediaInfo.Streams)
            .AddStream(subtitleStream)
            .SetOutput(outputPath)
            .SetOverwriteOutput(true);

        if (useHardwareAcceleration)
        {
            SetHardwareAccelerator(
                conversion,
                hardwareAcceleratorType,
                deviceNo);
        }

        conversion.OnDataReceived += Conversion_OnDataReceived;
        conversion.OnProgress += Conversion_OnProgress;

        return conversion;
    }

    /// <summary>
    /// 對 IConversion 設定硬體加速相關參數
    /// </summary>
    /// <param name="conversion">IConversion</param>
    /// <param name="hardwareAcceleratorType">EnumSet.HardwareAcceleratorType，硬體的類型，預設是 EnumSet.HardwareAcceleratorType.Intel</param>
    /// <param name="deviceNo">數值，GPU 裝置的 ID 值，預設為 0</param>
    public static void SetHardwareAccelerator(
        IConversion conversion,
        HardwareAcceleratorType hardwareAcceleratorType = HardwareAcceleratorType.Intel,
        int deviceNo = 0)
    {
        switch (hardwareAcceleratorType)
        {
            case HardwareAcceleratorType.Intel:
                conversion.UseHardwareAcceleration(
                    nameof(HardwareAccelerator.d3d11va),
                    nameof(VideoCodec.h264),
                    "h264_qsv",
                    deviceNo);

                break;
            case HardwareAcceleratorType.NVIDIA:
                conversion.UseHardwareAcceleration(
                    HardwareAccelerator.d3d11va,
                    VideoCodec.h264,
                    VideoCodec.h264_nvenc,
                    deviceNo);

                break;
            case HardwareAcceleratorType.AMD:
                conversion.UseHardwareAcceleration(
                    nameof(HardwareAccelerator.d3d11va),
                    nameof(VideoCodec.h264),
                    "h264_amf",
                deviceNo);

                break;
            default:
                conversion.UseHardwareAcceleration(
                    nameof(HardwareAccelerator.d3d11va),
                    nameof(VideoCodec.h264),
                    "h264_qsv",
                    deviceNo);

                break;
        };
    }

    /// <summary>
    /// 輸出 IConversionResult
    /// </summary>
    /// <param name="conversionResult">IConversionResult</param>
    public static void WriteConversionResult(IConversionResult conversionResult)
    {
        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
        {
            if (_PBProgress != null)
            {
                _PBProgress.Value = 0;
            }

            if (_LOperation != null)
            {
                _LOperation.Content = string.Empty;
            }
        }));

        _WMain?.WriteLog(MsgSet.GetFmtStr(
            MsgSet.TemplateXabeFFmpegConversionResult,
            conversionResult.StartTime.ToString(),
            conversionResult.EndTime.ToString(),
            conversionResult.Duration.ToString(),
            conversionResult.Arguments));
    }

    private static void Conversion_OnDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.TemplateXabeFFmpegOnDataReceived,
                e.Data));
        }
    }

    private static void Conversion_OnProgress(object sender, ConversionProgressEventArgs args)
    {
        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
        {
            int percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);

            if (_PBProgress != null)
            {
                _PBProgress.Value = percent;
            }

            if (_LOperation != null)
            {
                _LOperation.Content = MsgSet.GetFmtStr(
                    MsgSet.TemplateXabeFFmpegOnProgress,
                    args.ProcessId.ToString(),
                    args.Duration.ToString(),
                    args.TotalLength.ToString(),
                    percent.ToString());
            }
        }));
    }
}