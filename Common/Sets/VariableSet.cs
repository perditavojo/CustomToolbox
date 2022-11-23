using System.IO;

namespace CustomToolbox.Common.Sets;

/// <summary>
/// 變數組
/// </summary>
internal class VariableSet
{
    /// <summary>
    /// 根路徑
    /// </summary>
    public static readonly string RootPath = AppContext.BaseDirectory;

    /// <summary>
    /// Deps 資料夾的路徑
    /// </summary>
    public static readonly string DepsFolderPath = Path.Combine(AppContext.BaseDirectory, "Deps");

    /// <summary>
    /// Downloads 資料夾的路徑
    /// </summary>
    public static readonly string DownloadsFolderPath = Path.Combine(AppContext.BaseDirectory, "Downloads");

    /// <summary>
    /// Playlists 資料夾的路徑
    /// </summary>
    public static readonly string PlaylistsFolderPath = Path.Combine(AppContext.BaseDirectory, "Playlists");

    /// <summary>
    /// yt-dlp.exe 的執行檔名稱
    /// </summary>
    public static readonly string YtDlpExecName = "yt-dlp.exe";

    /// <summary>
    /// yt-dlp.exe 的路徑
    /// </summary>
    public static readonly string YtDlpPath = Path.Combine(DepsFolderPath, YtDlpExecName);

    /// <summary>
    /// ffmpeg.exe 的執行檔名稱
    /// </summary>
    public static readonly string FFmpegExecName = "ffmpeg.exe";

    /// <summary>
    /// ffmpeg.exe 的路徑
    /// </summary>
    public static readonly string FFmpegPath = Path.Combine(DepsFolderPath, FFmpegExecName);

    /// <summary>
    /// ffprobe.exe 的執行檔名稱
    /// </summary>
    public static readonly string FFprobeExecName = "ffprobe.exe";

    /// <summary>
    /// ffprobe.exe 的路徑
    /// </summary>
    public static readonly string FFprobePath = Path.Combine(DepsFolderPath, FFprobeExecName);

    /// <summary>
    /// aria2c.exe 的執行檔名稱
    /// </summary>
    public static readonly string Aria2ExecName = "aria2c.exe";

    /// <summary>
    /// aria2c.exe 的路徑
    /// </summary>
    public static readonly string Aria2Path = Path.Combine(DepsFolderPath, Aria2ExecName);

    /// <summary>
    /// mpv-1.dll 的檔案名稱
    /// </summary>
    public static readonly string LibMpvDllFileName = "mpv-1.dll";

    /// <summary>
    /// mpv-1.dll 的路徑
    /// </summary>
    public static readonly string LibMpvPath = Path.Combine(DepsFolderPath, LibMpvDllFileName);

    /// <summary>
    /// mpv.conf 的檔案名稱
    /// </summary>
    public static readonly string MpvConfFileName = "mpv.conf";

    /// <summary>
    /// mpv.conf 的路徑
    /// </summary>
    public static readonly string MpvConfPath = Path.Combine(DepsFolderPath, MpvConfFileName);

    /// <summary>
    /// ytdl_hook.lua 的檔案名稱
    /// </summary>
    public static readonly string YtDlHookLuaFileName = "ytdl_hook.lua";

    /// <summary>
    /// ytdl_hook.lua 的路徑
    /// </summary>
    public static readonly string YtDlHookLuaPath = Path.Combine(DepsFolderPath, YtDlHookLuaFileName);

    /// <summary>
    /// 等待刪除毫秒數
    /// </summary>
    public static readonly int WaitForDeleteMilliseconds = 5000;
}