namespace CustomToolbox.Common.Sets;

/// <summary>
/// 網址組
/// </summary>
public class UrlSet
{
    /// <summary>
    /// 查詢使用者代理字串的網址
    /// </summary>
    public static readonly string QueryUserAgentUrl = "https://www.google.com/search?q=My+User+Agent";

    /// <summary>
    /// AppVersion.json 檔案的網址
    /// </summary>
    public static readonly string AppVersionJsonUrl = "https://drive.google.com/uc?id=1cCrvyBTvtDKqK4rqq1wjizqnpTogRN70";

    /// <summary>
    /// FFmpeg 的壓縮檔名稱
    /// </summary>
    public static readonly string FFmpegArchiveFileName = "ffmpeg-master-latest-win64-gpl.zip";

    /// <summary>
    /// FFmpeg 的下載網址
    /// </summary>
    public static readonly string FFmpegUrl = $"https://github.com/yt-dlp/FFmpeg-Builds/releases/download/latest/{FFmpegArchiveFileName}";

    /// <summary>
    /// yt-dlp 的下載網址
    /// </summary>
    public static readonly string YtDlpUrl = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe";

    /// <summary>
    /// aria2 的版本號
    /// </summary>
    public static readonly string Aria2Version = Properties.Settings.Default.Aria2Version;

    /// <summary>
    /// aria2 的壓縮檔名稱
    /// </summary>
    public static readonly string Aria2ArchiveFileName = $"aria2-{Aria2Version}-win-64bit-build1.zip";

    /// <summary>
    /// libmpv 的壓縮檔名稱
    /// </summary>
    public static readonly string LibMpvArchiveFileName = Properties.Settings.Default.LibMpvArchiveFileName;

    /// <summary>
    /// aria2 的下載網址
    /// </summary>
    public static readonly string Aria2Url = $"https://github.com/aria2/aria2/releases/latest/download/{Aria2ArchiveFileName}";

    /// <summary>
    /// libmpv 的下載網址
    /// </summary>
    public static readonly string LibMpvUrl = $"https://sourceforge.net/projects/mpv-player-windows/files/libmpv/{LibMpvArchiveFileName}/download";

    /// <summary>
    /// ytdl_jook.lua 的下載網址
    /// </summary>
    public static readonly string YtDlHookLuaUrl = $"https://raw.githubusercontent.com/mpv-player/mpv/master/player/lua/{VariableSet.YtDlHookLuaFileName}";

    /// <summary>
    /// sub_charenc_parameters.txt 的下載網址
    /// <para>來源：https://trac.ffmpeg.org/attachment/ticket/2431/sub_charenc_parameters.txt </para>
    /// </summary>
    public static readonly string SubCharencParametersTxtUrl = "https://trac.ffmpeg.org/raw-attachment/ticket/2431/sub_charenc_parameters.txt";

    /// <summary>
    /// YouTube 網址
    /// </summary>
    public static readonly string YTUrl = "https://www.youtube.com/";

    /// <summary>
    /// YouTube 頻道網址
    /// </summary>
    public static readonly string YTChannelUrl = "https://www.youtube.com/channel/";

    /// <summary>
    /// YouTube 自定義頻道網址
    /// </summary>
    public static readonly string YTCustomChannelUrl = "https://www.youtube.com/c/";

    /// <summary>
    /// YouTube Subscriber Counter 網站網址
    /// </summary>
    public static readonly string YTSubscriberCounterUrl = "https://subscribercounter.com/fullscreen/";
}