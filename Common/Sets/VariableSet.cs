using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;

namespace CustomToolbox.Common.Sets;

/// <summary>
/// 變數組
/// </summary>
public class VariableSet
{
    /// <summary>
    /// 根路徑
    /// </summary>
    public static readonly string RootPath = AppContext.BaseDirectory;

    /// <summary>
    /// Langs 資料夾的路徑
    /// </summary>
    public static readonly string LangsFolderPath = Path.Combine(RootPath, "Langs");

    /// <summary>
    /// Assets 資料夾的路徑
    /// </summary>
    public static readonly string AssetsFolderPath = Path.Combine(RootPath, "Assets");

    /// <summary>
    /// Bins 資料夾的路徑
    /// </summary>
    public static readonly string BinsFolderPath = Path.Combine(RootPath, "Bins");

    /// <summary>
    /// Downloads 資料夾的路徑
    /// </summary>
    public static readonly string DownloadsFolderPath = Path.Combine(RootPath, "Downloads");

    /// <summary>
    /// ClipLists 資料夾的路徑
    /// </summary>
    public static readonly string ClipListsFolderPath = Path.Combine(RootPath, "ClipLists");

    /// <summary>
    /// 暫存的短片清單檔案路徑
    /// </summary>
    public static readonly string TempClipListFilePath = Path.Combine(ClipListsFolderPath, "ClipList_Temp.json");

    /// <summary>
    /// Logs 資料夾的路徑
    /// </summary>
    public static readonly string LogsFolderPath = Path.Combine(RootPath, "Logs");

    /// <summary>
    /// Lyrics 資料夾的路徑
    /// </summary>
    public static readonly string LyricsFolderPath = Path.Combine(RootPath, "Lyrics");

    /// <summary>
    /// Temp 資料夾的路徑
    /// </summary>
    public static readonly string TempFolderPath = Path.Combine(RootPath, "Temp");

    /// <summary>
    /// Models 資料夾的路徑
    /// </summary>
    public static readonly string ModelsFolderPath = Path.Combine(RootPath, "Models");

    /// <summary>
    /// yt-dlp.exe 的執行檔名稱
    /// </summary>
    public static readonly string YtDlpExecName = "yt-dlp.exe";

    /// <summary>
    /// yt-dlp.exe 的路徑
    /// </summary>
    public static readonly string YtDlpPath = Path.Combine(BinsFolderPath, YtDlpExecName);

    /// <summary>
    /// yt-dlp.conf 的檔案名稱
    /// </summary>
    public static readonly string YtDlpConfName = "yt-dlp.conf";

    /// <summary>
    /// yt-dlp.conf 的路徑
    /// </summary>
    public static readonly string YtDlpConfPath = Path.Combine(BinsFolderPath, YtDlpConfName);

    /// <summary>
    /// yt-dlp 預設的 Output
    /// </summary>
    public static readonly string YtDlpDefaultOutput = Path.Combine(DownloadsFolderPath, "%(uploader)s_%(title)s-%(id)s.%(ext)s");

    /// <summary>
    /// yt-dlp 使用 --split-chapters 時的 Output
    /// </summary>
    public static readonly string YtDlpSplitChaptersOutput = $"chapter:{Path.Combine(DownloadsFolderPath, @"%(uploader)s_%(title)s-%(id)s\%(section_number)03d.%(section_title)s.%(ext)s")}";

    /// <summary>
    /// ffmpeg.exe 的執行檔名稱
    /// </summary>
    public static readonly string FFmpegExecName = "ffmpeg.exe";

    /// <summary>
    /// ffmpeg.exe 的路徑
    /// </summary>
    public static readonly string FFmpegPath = Path.Combine(BinsFolderPath, FFmpegExecName);

    /// <summary>
    /// ffprobe.exe 的執行檔名稱
    /// </summary>
    public static readonly string FFprobeExecName = "ffprobe.exe";

    /// <summary>
    /// ffprobe.exe 的路徑
    /// </summary>
    public static readonly string FFprobePath = Path.Combine(BinsFolderPath, FFprobeExecName);

    /// <summary>
    /// sub_charenc_parameters.txt 的檔案名稱
    /// </summary>
    public static readonly string SubCharencParametersTxtFileName = "sub_charenc_parameters.txt";

    /// <summary>
    /// sub_charenc_parameters.txt 的路徑
    /// </summary>
    public static readonly string SubCharencParametersTxtPath = Path.Combine(BinsFolderPath, SubCharencParametersTxtFileName);

    /// <summary>
    /// aria2c.exe 的執行檔名稱
    /// </summary>
    public static readonly string Aria2ExecName = "aria2c.exe";

    /// <summary>
    /// aria2c.exe 的路徑
    /// </summary>
    public static readonly string Aria2Path = Path.Combine(BinsFolderPath, Aria2ExecName);

    /// <summary>
    /// mpv-1.dll 的檔案名稱
    /// </summary>
    public static readonly string LibMpvDllFileName = "mpv-1.dll";

    /// <summary>
    /// mpv-1.dll 的路徑
    /// </summary>
    public static readonly string LibMpvPath = Path.Combine(BinsFolderPath, LibMpvDllFileName);

    /// <summary>
    /// mpv.conf 的檔案名稱
    /// </summary>
    public static readonly string MpvConfFileName = "mpv.conf";

    /// <summary>
    /// mpv.conf 的路徑
    /// </summary>
    public static readonly string MpvConfPath = Path.Combine(BinsFolderPath, MpvConfFileName);

    /// <summary>
    /// ytdl_hook.lua 的檔案名稱
    /// </summary>
    public static readonly string YtDlHookLuaFileName = "ytdl_hook.lua";

    /// <summary>
    /// ytdl_hook.lua 的路徑
    /// </summary>
    public static readonly string YtDlHookLuaPath = Path.Combine(BinsFolderPath, YtDlHookLuaFileName);

    /// <summary>
    /// mpv.conf 的內容模板
    /// </summary
    public static readonly string MpvConfTemplate =
        $"# 設定使用的設定檔。{Environment.NewLine}" +
        $"#profile=\"gpu-hq\"{Environment.NewLine}" +
        Environment.NewLine +
        $"# 設定使用的影像輸出後端。{Environment.NewLine}" +
        $"#vo=\"gpu\"{Environment.NewLine}" +
        Environment.NewLine +
        $"# 問題參考：https://github.com/hudec117/Mpv.NET-lib-/issues/27{Environment.NewLine}" +
        $"# 2022-10-22 因此取消預設的 hwdec 的設定。{Environment.NewLine}" +
        $"# 設定使用的影像硬體解碼 API。{Environment.NewLine}" +
        $"#hwdec=\"auto-safe\"{Environment.NewLine}" +
        Environment.NewLine +
        $"# 設定使用的字幕語言。{Environment.NewLine}" +
        $"slang=\"zh-TW,zh,chi\"{Environment.NewLine}" +
        Environment.NewLine +
        $"# 設定字幕使用的字型。{Environment.NewLine}" +
        $"sub-font=\"微軟正黑體\"{Environment.NewLine}" +
        Environment.NewLine +
        $"# 取消使用內置的 ytdl_hook.lua。{Environment.NewLine}" +
        $"# 注意！此為必須之設定請勿變更或是移除。{Environment.NewLine}" +
        "no-ytdl";

    /// <summary>
    /// *.jsonc 的標頭
    /// <para>來源：https://github.com/YoutubeClipPlaylist/Playlists/blob/BasePlaylist/Template/TemplateSongList.jsonc </para>
    /// </summary>
    public static readonly string JsoncHeader = $"/**{Environment.NewLine}" +
        $" * 歌單格式為JSON with Comments{Environment.NewLine}" +
        $" * [\"VideoID\", StartTime, EndTime, \"Title\", \"SubSrc\"]{Environment.NewLine}" +
        $" * VideoID: 必須用引號包住，為字串型態。{Environment.NewLine}" +
        $" * StartTime: 只能是非負數。如果要從頭播放，輸入0{Environment.NewLine}" +
        $" * EndTime: 只能是非負數。如果要播放至尾，輸入0{Environment.NewLine}" +
        $" * Title?: 必須用引號包住，為字串型態{Environment.NewLine}" +
        $" * SubSrc?: 必須用雙引號包住，為字串型態，可選{Environment.NewLine}" +
        $" */{Environment.NewLine}";

    /// <summary>
    /// 時間標記的標頭
    /// </summary>
    public static readonly string TimestampHeaderTemplate = $"網址：https://www.youtube.com/watch?v={{VideoID}}{Environment.NewLine}{Environment.NewLine}" +
        $"格式：{{FFmpeg 時間格式}}｜{{YouTube 留言}}｜{{YouTube 秒數}}｜{{Twitch 時間格式}}{Environment.NewLine}" +
        $"格式說明：{Environment.NewLine}" +
        $"1. 給予在 FFmpeg 的參數 -ss 或 -t 帶入值使用。{Environment.NewLine}" +
        $"> e.g. .\\ffmpeg.exe -ss {{時間標記}} -i input.mp4 -vcodec copy -acodec copy -t {{時間標記}} -o output.mp4{Environment.NewLine}" +
        $"2. 給予在 YouTube 留言內標記時間點使用。{Environment.NewLine}" +
        $"3. 給予在 YouTube 網址參數 t 帶入值使用。{Environment.NewLine}" +
        $"> e.g. https://www.youtube.com/watch?v={{影片 ID}}&t={{時間標記}}s{Environment.NewLine}" +
        $"4. 給予在 Twitch 網址參數 t 帶入值使用。{Environment.NewLine}" +
        $"> e.g. https://www.twitch.tv/videos/{{影片 ID}}?t={{時間標記}}{Environment.NewLine}{Environment.NewLine}" +
        $"※時間標記，請以 YouTube / Twitch 網站上的影片為準。{Environment.NewLine}{Environment.NewLine}";

    /// <summary>
    /// 等待刪除毫秒數
    /// </summary>
    public static readonly int WaitForDeleteMilliseconds = 5000;

    /// <summary>
    /// 預設的附加秒數（300 秒）
    /// </summary>
    public static readonly double DefaultAppendSeconds = 300;

    /// <summary>
    /// 共用的 JsonSerializerOptions
    /// </summary>
    public static JsonSerializerOptions SharedJSOptions = new()
    {
        // 忽略掉註解。
        ReadCommentHandling = JsonCommentHandling.Skip,
        // 來源：https://stackoverflow.com/a/59260196
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true
    };

    /// <summary>
    /// 允許的附檔名（播放清單檔案）
    /// </summary>
    public static readonly string[] AllowedExts =
    {
        ".txt",
        ".json",
        ".jsonc"
    };

    /// <summary>
    /// 時間標記資訊
    /// </summary>
    public static class Timestamp
    {
        /// <summary>
        /// 影片的網址
        /// </summary>
        public static readonly string VideoUrl = "網址：";

        /// <summary>
        /// 開啟讀取標記
        /// </summary>
        public static readonly string StartReadingToken = "時間標記：";

        /// <summary>
        /// 註解行前綴
        /// </summary>
        public static readonly string CommentPrefixToken = "#";

        /// <summary>
        /// 註解行後綴一
        /// </summary>
        public static readonly string CommentSuffixToken1 = "（開始）";

        /// <summary>
        /// 註解行後綴二
        /// </summary>
        public static readonly string CommentSuffixToken2 = "（結束）";

        /// <summary>
        /// 分隔用行
        /// </summary>
        public static readonly string BlockSeparator = "-------";
    }

    /// <summary>
    /// Regex 判斷 ASCII 換行字元
    /// <para>來源：https://social.msdn.microsoft.com/Forums/en-US/34ddba13-d352-4a55-b144-1cf75c2f954d/form-view-c-how-to-replace-carriage-return-with-ltbrgt?forum=aspwebformsdata </para>
    /// </summary>
    [SuppressMessage("GeneratedRegex", "SYSLIB1045:轉換為 'GeneratedRegexAttribute'。", Justification = "<暫止>")]
    public static readonly Regex RegexAscii = new(@"(\r\n|\r|\n)+");

    /// <summary>
    /// 截圖的 Timeout
    /// </summary>
    public static readonly int ScreenshotTimeout = Properties.Settings.Default.PlaywrightScreenshotTimeout;

    /// <summary>
    /// 暫停執行毫秒
    /// </summary>
    public static readonly int SleepMs = Properties.Settings.Default.PlaywrightSleepMs;

    /// <summary>
    /// 開發模式的暫停執行毫秒
    /// </summary>
    public static readonly int DevSleepMs = Properties.Settings.Default.PlaywrightDevSleepMs;

    /// <summary>
    /// 頻道名稱字串分割長度
    /// </summary>
    public static readonly int SplitLength = Properties.Settings.Default.PlaywrightSplitLength;

    /// <summary>
    /// 分割後的頻道名稱列數限制
    /// </summary>
    public static readonly int ChannelNameRowLimit = Properties.Settings.Default.PlaywrightChannelNameRowLimit;

    /// <summary>
    /// 影像濾鏡（綠幕）
    /// <para>全黑："colorize:120:1:0:0"</para>
    /// </summary>
    public static readonly string VideoFilterColorize = "colorize:120:1:0.5:0";

    /// <summary>
    /// 影像濾鏡（無）
    /// </summary>
    public static readonly string VideoFilterNull = "null";
}