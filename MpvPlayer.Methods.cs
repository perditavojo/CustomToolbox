using CustomToolbox.Common.Sets;
using Mpv.NET.API;
using Mpv.NET.Player;

namespace CustomToolbox;

public partial class WMain
{
    /// <summary>
    /// 初始化 MpvPlayer
    /// </summary>
    /// <param name="hwnd">nint</param>
    public void InitMpvPlayer(nint hwnd)
    {
        try
        {
            MPlayer = new MpvPlayer(hwnd, VariableSet.LibMpvPath)
            {
                Volume = 100,
                YouTubeDlVideoQuality = YouTubeDlVideoQuality.Highest,
                LogLevel = MpvLogLevel.Error
            };

            MPlayer?.LoadConfig(VariableSet.MpvConfPath);
            MPlayer?.EnableYouTubeDl(VariableSet.YtDlHookLuaPath);

            if (MPlayer != null)
            {
                MPlayer.MediaLoaded += MediaLoaded;
                MPlayer.MediaFinished += MediaFinished;
                MPlayer.MediaPaused += MediaPaused;
                MPlayer.MediaResumed += MediaResumed;
                MPlayer.MediaStartedBuffering += MediaStartedBuffering;
                MPlayer.MediaEndedBuffering += MediaEndedBuffering;
                MPlayer.MediaStartedSeeking += MediaStartedSeeking;
                MPlayer.MediaEndedSeeking += MediaEndedSeeking;
                MPlayer.MediaError += MediaError;
                MPlayer.MediaUnloaded += MediaUnloaded;
                MPlayer.PositionChanged += PositionChanged;
                MPlayer.API.LogMessage += LogMessage;
            }
        }
        catch (Exception ex)
        {
            WriteLog($"發生錯誤：{ex}");
        }
    }

    /// <summary>
    /// 播放多媒體
    /// </summary>
    /// <param name="path">字串，檔案路徑、影片的網址或影片的 ID</param>
    private void PlayMedia(string path)
    {
        if (!path.StartsWith("http"))
        {
            path = $"https://www.youtube.com/watch?v={path}";
        }

        // TODO: 2022-11-23 針對非 YouTube 的網站需要調整 mpv 的設定。

        MPlayer?.Load(path);
        MPlayer?.Resume();
    }
}