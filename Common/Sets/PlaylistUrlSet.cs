namespace CustomToolbox.Common.Sets;

/// <summary>
/// 播放清單網址組
/// </summary>
internal class PlaylistUrlSet
{
    /// <summary>
    /// GitHub 基礎網址
    /// </summary>
    public static readonly string GitHubBaseUrl = "https://raw.githubusercontent.com/";

    /// <summary>
    /// YoutubeClipPlaylist/Playlists 的基礎網址
    /// </summary>
    public static readonly string YCPBaseUrl = $"{GitHubBaseUrl}YoutubeClipPlaylist/Playlists/minify/";

    /// <summary>
    /// YoutubeClipPlaylist/Playlists 的 Playlists.jsonc 的網址
    /// </summary>
    public static readonly string YCPPlaylistsJsonUrl = $"{YCPBaseUrl}Playlists.jsonc";

    /// <summary>
    /// YoutubeClipPlaylist/Lyrics 的基礎網址
    /// </summary>
    public static readonly string YCPLyricsBaseUrl = $"{GitHubBaseUrl}YoutubeClipPlaylist/Lyrics/minify/";

    /// <summary>
    /// YoutubeClipPlaylist/Lyrics 的 Lyrics.json 的網址
    /// </summary>
    public static readonly string YCPLyricsJsonUrl = $"{YCPLyricsBaseUrl}Lyrics.json";

    /// <summary>
    /// YoutubeClipPlaylist/Lyrics 的 *.lrc 的模版網址
    /// </summary>
    public static readonly string YCPLrcFileTemplateUrl = $"{YCPLyricsBaseUrl}Lyrics/[SongId].lrc";

    /// <summary>
    /// rubujo/CustomPlaylist 的基礎網址
    /// </summary>
    public static readonly string FCPBaseUrl = $"{GitHubBaseUrl}rubujo/CustomPlaylist/main/";

    /// <summary>
    /// rubujo/CustomPlaylist 的 Playlists.jsonc 的網址
    /// </summary>
    public static readonly string FCPPlaylistsJsonUrl = $"{FCPBaseUrl}Playlists.jsonc";

    /// <summary>
    /// rubujo/CustomPlaylist 的 B23Playlists.jsonc 的網址
    /// </summary>
    public static readonly string FCPB23PlaylistsJsonUrl = $"{FCPBaseUrl}B23Playlists.jsonc";
}