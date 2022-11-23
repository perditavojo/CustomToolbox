using CustomToolbox.Extensions;
using Mpv.NET.API;
using Mpv.NET.Player;

namespace CustomToolbox;

public partial class WMain
{
    public void PlayerHost_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        if (PopupPlayer != null && PopupPlayer.IsShowing())
        {
            PopupPlayer.Close();
            PopupPlayer = null;
        }
        else
        {
            PopupPlayer = new WPopupPlayer(this);
            PopupPlayer.Show();
        }
    }

    private void MediaLoaded(object? sender, EventArgs e)
    {
        WriteLog("已載入多媒體。");
    }

    private void MediaFinished(object? sender, EventArgs e)
    {
        WriteLog("已結束播放多媒體。");
    }

    private void MediaPaused(object? sender, EventArgs e)
    {
        WriteLog("已暫停播放多媒體。");
    }

    private void MediaResumed(object? sender, EventArgs e)
    {
        WriteLog("已恢復播放多媒體。");
    }

    private void MediaStartedBuffering(object? sender, EventArgs e)
    {
        WriteLog("已開始緩衝多媒體……");
    }

    private void MediaEndedBuffering(object? sender, EventArgs e)
    {
        WriteLog("已結束緩衝多媒體。");
    }

    private void MediaStartedSeeking(object? sender, EventArgs e)
    {
        WriteLog("已開始搜尋多媒體……");
    }

    private void MediaEndedSeeking(object? sender, EventArgs e)
    {
        WriteLog("已結束搜尋多媒體。");
    }

    private void MediaError(object? sender, EventArgs e)
    {
        WriteLog("發生錯誤，多媒體播放失敗。");
    }

    private void MediaUnloaded(object? sender, EventArgs e)
    {
        WriteLog("已卸載多媒體。");
    }

    private void PositionChanged(object? sender, MpvPlayerPositionChangedEventArgs e)
    {
        // TODO: 2022-11-23 待完成相關功能。
    }

    private void LogMessage(object? sender, MpvLogMessageEventArgs e)
    {
        MpvLogMessage mpvLogMessage = e.Message;

        WriteLog($"[{mpvLogMessage.Prefix}] ({mpvLogMessage.LogLevel}) {mpvLogMessage.Text}");
    }
}