namespace CustomToolbox.Common.Sets;

/// <summary>
/// 網址組
/// </summary>
internal class UrlSet
{
    /// <summary>
    /// FFmpeg 的壓縮檔名稱
    /// </summary>
    public static readonly string FFmpegArchiveFileName = "ffmpeg-master-latest-win64-gpl.zip";

    /// <summary>
    /// Aria2 的 GitHub 標籤
    /// </summary>
    public static readonly string Aria2RepoTag = "release-1.36.0";

    /// <summary>
    /// Aria2 的壓縮檔名稱
    /// </summary>
    public static readonly string Aria2ArchiveFileName = "aria2-1.36.0-win-64bit-build1.zip";

    /// <summary>
    /// libmpv 的壓縮檔名稱
    /// </summary>
    public static readonly string LibMpvArchiveFileName = "mpv-dev-x86_64-20211212-git-0e76372.7z";

    /// <summary>
    /// FFmpeg 的下載網址
    /// </summary>
    public static readonly string FFmpegUrl = $"https://github.com/yt-dlp/FFmpeg-Builds/releases/download/latest/{FFmpegArchiveFileName}";

    /// <summary>
    /// Aria2 的下載網址
    /// </summary>
    public static readonly string Aria2Url = $"https://github.com/aria2/aria2/releases/download/{Aria2RepoTag}/{Aria2ArchiveFileName}";

    /// <summary>
    /// libmpv 的下載網址
    /// </summary>
    public static readonly string LibMpvUrl = $"https://sourceforge.net/projects/mpv-player-windows/files/libmpv/{LibMpvArchiveFileName}/download";

    /// <summary>
    /// ytdl_jook.lua 的下載網址
    /// </summary>
    public static readonly string YtDlHookLuaUrl = $"https://raw.githubusercontent.com/mpv-player/mpv/master/player/lua/{VariableSet.YtDlHookLuaFileName}";
}