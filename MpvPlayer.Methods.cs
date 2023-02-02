using Control = System.Windows.Controls.Control;
using CustomToolbox.Common;
using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Sets;
using static CustomToolbox.Common.Sets.EnumSet;
using CustomToolbox.Common.Utils;
using H.NotifyIcon.Core;
using System.Security.Cryptography;
using Mpv.NET.API;
using Mpv.NET.Player;

namespace CustomToolbox;

/// <summary>
/// MpvPlayer 的相關方法
/// </summary>
public partial class WMain
{
    /// <summary>
    /// 初始化 MpvPlayer
    /// </summary>
    /// <param name="hwnd">nint</param>
    private void InitMpvPlayer(nint hwnd)
    {
        try
        {
            MPPlayer = new MpvPlayer(hwnd, VariableSet.LibMpvPath)
            {
                Volume = Properties.Settings.Default.MpvNetLibVolume,
                Speed = CustomFunction.GetPlaybackSpeed(Properties.Settings.Default.MpvNetLibPlaybackSpeedIndex),
                YouTubeDlVideoQuality = CustomFunction.GetYTQuality(Properties.Settings.Default.MpvNetLibYTQualityIndex),
                LogLevel = GetMpvLogLeve()
            };

            if (MPPlayer != null)
            {
                MPPlayer.LoadConfig(VariableSet.MpvConfPath);
                MPPlayer.EnableYouTubeDl(VariableSet.YtDlHookLuaPath);

                if (Properties.Settings.Default.MpvNetLibNoVideo)
                {
                    MPPlayer.API.SetPropertyString("vid", "no");
                }
                else
                {
                    MPPlayer.API.SetPropertyString("vid", "auto");
                }

                if (Properties.Settings.Default.MpvNetLibChromaKey)
                {
                    MPPlayer.API.SetPropertyString("vf", VariableSet.VideoFilterColorize);
                }
                else
                {
                    MPPlayer.API.SetPropertyString("vf", VariableSet.VideoFilterNull);
                }

                MPPlayer.MediaLoaded += MediaLoaded;
                MPPlayer.MediaFinished += MediaFinished;
                MPPlayer.MediaPaused += MediaPaused;
                MPPlayer.MediaResumed += MediaResumed;
                MPPlayer.MediaStartedBuffering += MediaStartedBuffering;
                MPPlayer.MediaEndedBuffering += MediaEndedBuffering;
                MPPlayer.MediaStartedSeeking += MediaStartedSeeking;
                MPPlayer.MediaEndedSeeking += MediaEndedSeeking;
                MPPlayer.MediaError += MediaError;
                MPPlayer.MediaUnloaded += MediaUnloaded;
                MPPlayer.PositionChanged += PositionChanged;
                MPPlayer.API.LogMessage += LogMessage;
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 播放短片
    /// </summary>
    /// <param name="ClipData">ClipData，短片資料</param>
    private async void PlayClip(ClipData clipData)
    {
        try
        {
            if (MPPlayer != null)
            {
                // 讓 libmpv 先停止播放，以免在 yt-dlp 發生錯誤後，後續的新播放皆會失效。
                MPPlayer.Stop();

                string? path = clipData.VideoUrlOrID;

                if (path == null)
                {
                    string message = MsgSet.MsgInvalidVideoIDOrUrl;

                    WriteLog(message);

                    TaskbarIconUtil.ShowNotify(
                        message,
                        NotificationIcon.Error);
                    TaskbarIconUtil.SetToolTip(string.Empty);

                    return;
                }

                UpdateClipPlayer(ClipPlayerStatus.Playing, clipData);

                if (!path.StartsWith("http"))
                {
                    path = $"https://www.youtube.com/watch?v={path}";

                    MPPlayer.YouTubeDlVideoQuality = CustomFunction
                        .GetYTQuality(Properties.Settings.Default.MpvNetLibYTQualityIndex);
                }
                else
                {
                    // 非 YouTube 網站的影片套用此設定。 
                    MPPlayer.API.SetPropertyString("ytdl-format", "bv*+ba/b");
                }

                MPPlayer.Load(path);
                MPPlayer.Resume();

                string lyricFilePath = string.Empty;

                // 判斷 clipData.SubtitleFileUrl 是否為空值或 null
                // 且是否有啟用自動歌詞。
                if (string.IsNullOrEmpty(clipData.SubtitleFileUrl) &&
                    Properties.Settings.Default.NetPlaylistAutoLyric)
                {
                    string[] lyricData = await GetLrcFileUrl(
                        clipData.VideoUrlOrID ?? string.Empty,
                        clipData.Name ?? string.Empty,
                        clipData.StartTime.TotalSeconds.ToString());

                    string lyricFileUrl = lyricData[0],
                        offsetSeconds = lyricData[1];

                    // 轉換成正體中文。
                    bool translateToTChinese = Properties.Settings.Default.OpenCCS2TWP;

                    // 取得處理過 *.lrc 檔案的路徑。
                    lyricFilePath = await LyricsUtil.GetProcessedLrcFilePath(
                        lyricFileUrl,
                        translateToTChinese);

                    // *.lrc 檔案的網址。
                    clipData.SubtitleFileUrl = lyricFileUrl;

                    // 設定字幕檔的延遲秒數。
                    MPPlayer?.API.SetPropertyString("sub-delay", offsetSeconds);
                }
                else
                {
                    MPPlayer?.API.SetPropertyString("sub-delay", "0");
                }

                // 延後 3 秒後才執行載入字幕檔。
                await Task.Delay(3000).ContinueWith(t =>
                {
                    try
                    {
                        // 當 clipData.SubtitleFileUrl 不為空值或 null 時才載入字幕檔。
                        if (!string.IsNullOrEmpty(clipData.SubtitleFileUrl))
                        {
                            // 判斷是載入處理過的 *.lrc 檔案還是原始的 *.lrc 檔案。
                            string subtitlePath = !string.IsNullOrEmpty(lyricFilePath) ?
                                lyricFilePath :
                                clipData.SubtitleFileUrl;

                            MPPlayer?.API.Command(new string[]
                            {
                                "sub-add",
                                subtitlePath
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog(MsgSet.GetFmtStr(
                            MsgSet.MsgErrorOccured,
                            ex.ToString()));
                    }
                });
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 暫停或恢復播放短片
    /// </summary>
    private void PauseOrResumeClip()
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                string message = string.Empty;

                switch (CPPlayer.Status)
                {
                    case ClipPlayerStatus.Playing:

                        MPPlayer?.Pause();

                        UpdateClipPlayer(ClipPlayerStatus.Paused, CPPlayer.ClipData);

                        BtnPause.Content = MsgSet.Resume;

                        message = MsgSet.MsgClipPaused;

                        break;
                    case ClipPlayerStatus.Paused:
                        MPPlayer?.Resume();

                        UpdateClipPlayer(ClipPlayerStatus.Playing, CPPlayer.ClipData);

                        BtnPause.Content = MsgSet.Pause;

                        message = MsgSet.MsgClipResumed;

                        break;
                    default:
                        break;
                }

                TaskbarIconUtil.ShowNotify(
                    message,
                    NotificationIcon.Info);
            }));
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 停止播放短片
    /// </summary>
    private void StopClip()
    {
        try
        {
            MPPlayer?.Stop();

            UpdateClipPlayer();
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 靜音短片
    /// </summary>
    /// <param name="isMuted">布林值，靜音短片，預設值為 false</param>
    private void MuteClip(bool isMuted = false)
    {
        try
        {
            if (MPPlayer != null)
            {
                MPPlayer.Volume = isMuted ? 0 : Properties.Settings.Default.MpvNetLibVolume;

                BtnMute.Content = isMuted ? MsgSet.Unmute : MsgSet.Mute;

                // 更新 TaskbarIcon 的 MIMute 的 Header。
                TaskbarIconUtil.UpdateMIMuteHeader(BtnMute.Content.ToString());

                Control[] ctrlSet =
                {
                    SVolume,
                    LVolume
                };

                CustomFunction.BatchSetEnabled(ctrlSet, !isMuted);

                string message = isMuted ? MsgSet.MsgClipMuted : MsgSet.MsgClipMuted;

                TaskbarIconUtil.ShowNotify(
                    message,
                    NotificationIcon.Info);

                WriteLog(message);
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 不顯示影像
    /// </summary>
    /// <param name="enable">布林值，不顯示影像，預設值為 false</param>
    private void ClipNoVideo(bool enable = false)
    {
        try
        {
            Dispatcher.BeginInvoke(() => CBNoVideo.IsChecked = enable);
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 更新 ClipPlayer
    /// </summary>
    /// <param name="clipPlayerStatus">ClipPlayerStatus</param>
    /// <param name="clipData">ClipData</param>
    private void UpdateClipPlayer(
        ClipPlayerStatus clipPlayerStatus = ClipPlayerStatus.Idle,
        ClipData? clipData = null)
    {
        try
        {
            CPPlayer.Status = clipPlayerStatus;

            if (CPPlayer.ClipData != null)
            {
                CPPlayer.ClipData.PropertyChanged -= ClipData_PropertyChanged;
            }

            CPPlayer.ClipData = clipData;

            if (CPPlayer.ClipData != null)
            {
                CPPlayer.ClipData.PropertyChanged += ClipData_PropertyChanged;
            }

            CPPlayer.Index = clipData == null ? -1 : GlobalDataSet.IndexOf(clipData);

            int tempPreIndex = CPPlayer.Index,
                tempNexIndex = CPPlayer.Index,
                previousIndex = --tempPreIndex,
                nextIndex = ++tempNexIndex;

            if (previousIndex < 0)
            {
                previousIndex = -1;
            }

            CPPlayer.PreviousIndex = previousIndex;

            if (nextIndex > GlobalDataSet.Count - 1)
            {
                nextIndex = -1;
            }

            CPPlayer.NextIndex = nextIndex;

            CPPlayer.SeekStatus = SSeekStatus.Idle;

            DGClipList.SelectedIndex = CPPlayer.Index;
            DGClipList.ScrollToSelectedItem();

            if (clipData != null)
            {
                if (CPPlayer.Status != ClipPlayerStatus.Paused)
                {
                    string clipTitle = string.IsNullOrEmpty(clipData.Name) ?
                        MsgSet.ClipTitle :
                        (clipData.Name.Length > 40 ?
                            $"{clipData.Name[..40]}{MsgSet.Ellipses}" :
                            clipData.Name);

                    LClipTitle.Content = clipTitle;
                    LClipTitle.ToolTip = clipTitle != MsgSet.ClipTitle ?
                        clipData.Name :
                        MsgSet.ClipTitleToolTip;

                    int minVal = Convert.ToInt32(clipData.StartTime.TotalSeconds),
                        maxVal = Convert.ToInt32(clipData.EndTime.TotalSeconds);

                    PBDuration.Minimum = minVal;
                    PBDuration.Maximum = maxVal;
                    PBDuration.Value = 0;

                    SSeek.Minimum = minVal;
                    SSeek.Maximum = maxVal;
                    SSeek.Value = 0;

                    ClipNoVideo(clipData.IsAudioOnly);
                }

                if (CPPlayer.Status == ClipPlayerStatus.Playing)
                {
                    string message = string.IsNullOrEmpty(clipData.Name) ?
                        MsgSet.MsgPlayingClip :
                        MsgSet.GetFmtStr(
                            MsgSet.MsgNowPlaying,
                            clipData.Name);

                    TaskbarIconUtil.ShowNotify(
                        message,
                        NotificationIcon.Info);
                    TaskbarIconUtil.SetToolTip(message);

                    if (Properties.Settings.Default.DiscordRichPresence)
                    {
                        string details = string.IsNullOrEmpty(clipData?.Name) ?
                            MsgSet.MsgPlayingClip :
                            clipData.Name;

                        DiscordRichPresenceUtil.SetRichPresence(
                            details: details,
                            state: MsgSet.StatePlaying,
                            timestamps: DiscordRichPresenceUtil.GetTimestamps(),
                            assets: AssetsSet.AssetsPlay);
                    }
                }
            }
            else
            {
                LClipTitle.Content = MsgSet.ClipTitle;
                LClipTitle.ToolTip = MsgSet.ClipTitleToolTip;
                LDuration.Content = "00:00:00 / 00:00:00";

                PBDuration.Minimum = 0;
                PBDuration.Maximum = 100;
                PBDuration.Value = 0;

                SSeek.Minimum = 0;
                SSeek.Maximum = 100;
                SSeek.Value = 0;

                CBPlaybackSpeed.SelectedIndex = 4;

                // 清除 TaskbarIcon 的工具提示文字。
                TaskbarIconUtil.SetToolTip(string.Empty);
            }

            switch (CPPlayer.Status)
            {
                case ClipPlayerStatus.Playing:
                    SetClipPlayerButtons(false);

                    break;
                case ClipPlayerStatus.Paused:
                    if (Properties.Settings.Default.DiscordRichPresence)
                    {
                        DiscordRichPresenceUtil.SetRichPresence(
                            state: MsgSet.StatePause,
                            timestamps: DiscordRichPresenceUtil.GetTimestamps(),
                            assets: AssetsSet.AssetsPause);
                    }

                    break;
                case ClipPlayerStatus.Idle:
                    SetClipPlayerButtons(true);

                    if (Properties.Settings.Default.DiscordRichPresence)
                    {
                        DiscordRichPresenceUtil.SetRichPresence(
                            state: MsgSet.StateStop,
                            timestamps: DiscordRichPresenceUtil.GetTimestamps(),
                            assets: AssetsSet.AssetsStop);
                    }

                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 設定短片播放器的按鈕
    /// </summary>
    /// <param name="enable">布林值，啟用，預設值為 true</param>
    private void SetClipPlayerButtons(bool enable = true)
    {
        try
        {
            Control[] ctrlSet1 =
            {
                BtnPlay
            };

            Control[] ctrlSet2 =
            {
                BtnPrevious,
                BtnNext,
                BtnPause,
                BtnStop
            };

            CustomFunction.BatchSetEnabled(ctrlSet1, enable);
            CustomFunction.BatchSetEnabled(ctrlSet2, !enable);

            TaskbarIconUtil.SetMenuItems(enable);
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 執行隨機播放短片
    /// </summary>
    private void DoRandomPlayClip()
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (GlobalDataSet.Count <= 0)
                {
                    string message = MsgSet.MsgRandomPlayFailedClipListNoData;

                    TaskbarIconUtil.ShowNotify(
                        message,
                        NotificationIcon.Warning);

                    ShowMsgBox(message);

                    return;
                }
                else
                {
                    int randomIndex = RandomNumberGenerator.GetInt32(0, GlobalDataSet.Count - 1);

                    StopClip();

                    ClipData clipData = GlobalDataSet[randomIndex];

                    PlayClip(clipData);
                }
            }));
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 更新 MpvPlayer 的 LogLevel
    /// </summary>
    /// <param name="value">布林值，是否要將 mpv 的 LogLevel 設為 V 預設值是 false</param>
    private void SetMpvPlayerLogLevel(bool value = false)
    {
        try
        {
            if (MPPlayer != null)
            {
                MPPlayer.LogLevel = value ? MpvLogLevel.V : MpvLogLevel.Info;
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 取得設定的 MpvLogLevel
    /// </summary>
    /// <returns>MpvLogLevel</returns>
    private static MpvLogLevel GetMpvLogLeve()
    {
        return Properties.Settings.Default.MpvNetLibLogVerbose ? MpvLogLevel.V : MpvLogLevel.Info;
    }
}