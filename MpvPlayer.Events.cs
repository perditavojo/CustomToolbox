using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
using static CustomToolbox.Common.Sets.EnumSet;
using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Sets;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using System.Windows;
using Mpv.NET.API;
using Mpv.NET.Player;

namespace CustomToolbox;

/// <summary>
/// MpvPlayer 的相關事件
/// </summary>
public partial class WMain
{
    public void PlayerHost_MouseDoubleClick(object? sender, MouseEventArgs? e)
    {
        if (WPPPlayer != null && WPPPlayer.IsShowing())
        {
            WPPPlayer.Close();
            WPPPlayer = null;

            // 雙擊放大到全螢幕。
            /*
            WMain_KeyDown(sender, new KeyEventArgs(
                Keyboard.PrimaryDevice,
                Keyboard.PrimaryDevice.ActiveSource,
                0,
                Key.R));
            */
        }
        else
        {
            WPPPlayer = new WPopupPlayer(this);
            WPPPlayer.Show();
        }
    }

    private void MediaLoaded(object? sender, EventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                WriteLog(MsgSet.MsgMediaLoaded);

                if (MPPlayer != null && CPPlayer.ClipData != null)
                {
                    if (MPPlayer.IsMediaLoaded)
                    {
                        TimeSpan startTime = CPPlayer.ClipData.StartTime,
                            endTime = CPPlayer.ClipData.EndTime;

                        if (endTime == TimeSpan.FromSeconds(0))
                        {
                            endTime = MPPlayer.Duration.StripMilliseconds();

                            CPPlayer.ClipData.EndTime = endTime;

                            if (string.IsNullOrEmpty(CPPlayer.ClipData.Name) &&
                                !string.IsNullOrEmpty(MPPlayer.MediaTitle))
                            {
                                CPPlayer.ClipData.Name = MPPlayer.MediaTitle;
                            }

                            int maxVal = Convert.ToInt32(endTime.TotalSeconds);

                            PBDuration.Maximum = maxVal;
                            PBDuration.Minimum = 0;

                            SSeek.Maximum = maxVal;
                            SSeek.Minimum = 0;
                        }

                        MPPlayer.SeekAsync(startTime.TotalSeconds);
                    }
                }
            }));
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }

    private void MediaFinished(object? sender, EventArgs e)
    {
        WriteLog(MsgSet.MsgMediaFinished);

        BtnNext_Click(
            nameof(MediaFinished),
            new RoutedEventArgs(ButtonBase.ClickEvent));
    }

    private void MediaPaused(object? sender, EventArgs e)
    {
        WriteLog(MsgSet.MsgMediaPaused);
    }

    private void MediaResumed(object? sender, EventArgs e)
    {
        WriteLog(MsgSet.MsgMediaResumed);
    }

    private void MediaStartedBuffering(object? sender, EventArgs e)
    {
        WriteLog(MsgSet.MsgMediaStartedBuffering);
    }

    private void MediaEndedBuffering(object? sender, EventArgs e)
    {
        WriteLog(MsgSet.MsgMediaEndedBuffering);
    }

    private void MediaStartedSeeking(object? sender, EventArgs e)
    {
        WriteLog(MsgSet.MsgMediaStartedSeeking);
    }

    private void MediaEndedSeeking(object? sender, EventArgs e)
    {
        WriteLog(MsgSet.MsgMediaEndedSeeking);
    }

    private void MediaError(object? sender, EventArgs e)
    {
        WriteLog(MsgSet.MsgMediaError);

        BtnNext_Click(
            nameof(MediaError),
            new RoutedEventArgs(ButtonBase.ClickEvent));
    }

    private void MediaUnloaded(object? sender, EventArgs e)
    {
        WriteLog(MsgSet.MsgMediaUnloaded);
    }

    private void PositionChanged(object? sender, MpvPlayerPositionChangedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (CPPlayer.ClipData != null)
                {
                    TimeSpan startTime = CPPlayer.ClipData.StartTime,
                        endTime = CPPlayer.ClipData.EndTime,
                        durationTime = CPPlayer.ClipData.DurationTime,
                        currentTime = e.NewPosition.Subtract(startTime);

                    int currentSeconds = Convert.ToInt32(e.NewPosition.TotalSeconds),
                        endSeconds = Convert.ToInt32(endTime.TotalSeconds),
                        // 僅用於時間標記編輯器模式。
                        durationSeconds = Convert.ToInt32(endTime.TotalSeconds);

                    // 判斷是否為時間標記編輯器模式。
                    if (CPPlayer.Mode == ClipPlayerMode.TimestampEditor)
                    {
                        // 先判斷 MPPlayer 是否已初始化。
                        if (MPPlayer != null)
                        {
                            // 用於避免 libmpv 取不到時間而發生例外。
                            if (currentSeconds >= durationSeconds)
                            {
                                // 先主動讓 mpv 停止播放短片，以免此事件被重複觸發。
                                MPPlayer.Stop();

                                BtnStop_Click(nameof(PositionChanged),
                                    new RoutedEventArgs(ButtonBase.ClickEvent));

                                return;
                            }

                            currentTime = e.NewPosition;
                            durationTime = MPPlayer.Duration.StripMilliseconds();

                            int maxVal = Convert.ToInt32(durationTime.TotalSeconds);

                            PBDuration.Maximum = maxVal;
                            PBDuration.Minimum = 0;

                            SSeek.Maximum = maxVal;
                            SSeek.Minimum = 0;
                        }
                    }
                    else if (CPPlayer.Mode == ClipPlayerMode.ClipPlayer)
                    {
                        // 回歸原本的設計。
                        int maxVal = endSeconds;

                        PBDuration.Maximum = maxVal;
                        PBDuration.Minimum = 0;

                        SSeek.Maximum = maxVal;
                        SSeek.Minimum = 0;
                    }

                    if (currentSeconds >= PBDuration.Minimum &&
                        currentSeconds <= PBDuration.Maximum)
                    {
                        PBDuration.Value = currentSeconds;
                    }

                    // 當 SSeek 的 SeekStatus 為 SSeekStatus.Idle 時才允許更新。
                    if (CPPlayer.SeekStatus == SSeekStatus.Idle)
                    {
                        if (currentSeconds >= SSeek.Minimum &&
                            currentSeconds <= SSeek.Maximum)
                        {
                            SSeek.Value = currentSeconds;
                        }
                    }

                    if (currentTime > durationTime)
                    {
                        double newSeconds = Math.Round(
                            currentTime.TotalSeconds,
                            MidpointRounding.AwayFromZero);

                        // 當模式為多媒體播放器時，才會自動更新 ClipData 的結束時間。
                        if (CPPlayer.Mode == ClipPlayerMode.ClipPlayer)
                        {
                            CPPlayer.ClipData.EndTime = TimeSpan.FromSeconds(newSeconds);
                        }

                        LDuration.Content = $"{currentTime:hh\\:mm\\:ss} / {durationTime:hh\\:mm\\:ss}";
                    }
                    else
                    {
                        LDuration.Content = $"{currentTime:hh\\:mm\\:ss} / {durationTime:hh\\:mm\\:ss}";
                    }

                    // 當模式為多媒體播放器，且只有不是直播的短片，才會到結束時間時自動停止並切換下一個短片。
                    if (CPPlayer.Mode == ClipPlayerMode.ClipPlayer &&
                        !CPPlayer.ClipData.IsLivestream &&
                        currentSeconds >= endSeconds)
                    {
                        // 先主動讓 mpv 停止播放短片，以免此事件被重複觸發。
                        MPPlayer?.Stop();

                        BtnNext_Click(nameof(PositionChanged),
                            new RoutedEventArgs(ButtonBase.ClickEvent));
                    }
                }
            }));
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }

    private void LogMessage(object? sender, MpvLogMessageEventArgs e)
    {
        MpvLogMessage mpvLogMessage = e.Message;

        // 移除尾端的換行字元。
        string rawText = mpvLogMessage.Text.TrimEnd('\r', '\n');

        if (!string.IsNullOrEmpty(rawText))
        {
            WriteLog($"[{mpvLogMessage.Prefix}] ({mpvLogMessage.LogLevel}) {rawText}");
        }
    }
}