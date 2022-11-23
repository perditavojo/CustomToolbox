using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
using CheckBox = System.Windows.Controls.CheckBox;
using ComboBox = System.Windows.Controls.ComboBox;
using CustomToolbox.Common;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Utils;
using CustomToolbox.Common.Sets;
using DragCompletedEventArgs = System.Windows.Controls.Primitives.DragCompletedEventArgs;
using DragStartedEventArgs = System.Windows.Controls.Primitives.DragStartedEventArgs;
using H.NotifyIcon.Core;
using Mpv.NET.API;
using Mpv.NET.Player;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CustomToolbox;

public partial class WMain
{
    private void ClipData_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (e.PropertyName == nameof(ClipData.Name))
                {
                    UpdateClipPlayer(
                        CPPlayer.Status,
                        CPPlayer.ClipData);
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

    private void SSeek_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Slider slider = (Slider)sender;

                double newValue = e.NewValue;

                if (MPPlayer != null && MPPlayer.IsMediaLoaded)
                {
                    if (CPPlayer.SeekStatus == EnumSet.SSeekStatus.Drag)
                    {
                        if (newValue >= slider.Minimum &&
                            newValue <= slider.Maximum)
                        {
                            try
                            {
                                MPPlayer.SeekAsync(newValue);
                            }
                            catch (MpvAPIException ex)
                            {
                                WriteLog(MsgSet.GetFmtStr(
                                    MsgSet.MsgErrorOccured,
                                    ex.ToString()));
                            }
                        }
                    }
                }
                else
                {
                    slider.Value = slider.Minimum;
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

    private void SSeek_DragStarted(object sender, DragStartedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CPPlayer.SeekStatus = EnumSet.SSeekStatus.Drag;
            }));
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void SSeek_DragCompleted(object sender, DragCompletedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CPPlayer.SeekStatus = EnumSet.SSeekStatus.Idle;
            }));
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void SVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                int newValue = (int)e.NewValue;

                if (MPPlayer != null)
                {
                    MPPlayer.Volume = newValue;

                    if (Properties.Settings.Default.MpvNetLibVolume != newValue)
                    {
                        Properties.Settings.Default.MpvNetLibVolume = newValue;
                        Properties.Settings.Default.Save();
                    }

                    LVolume.Content = newValue.ToString();
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

    private void CBNoVideo_Checked(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CheckBox control = (CheckBox)sender;

                bool value = control.IsChecked ?? true;

                if (value)
                {
                    if (Properties.Settings.Default.MpvNetLibNoVideo != value)
                    {
                        Properties.Settings.Default.MpvNetLibNoVideo = value;
                        Properties.Settings.Default.Save();
                    }

                    string? vid = MPPlayer?.API.GetPropertyString("vid");

                    if (vid != "no")
                    {
                        MPPlayer?.API.SetPropertyString("vid", "no");

                        string message = MsgSet.MsgNoVideoEnabled;

                        TaskbarIconUtil.ShowNotify(
                            message,
                            NotificationIcon.Info);

                        WriteLog(message);
                    }
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

    private void CBNoVideo_Unchecked(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CheckBox control = (CheckBox)sender;

                bool value = control.IsChecked ?? false;

                if (!value)
                {
                    if (Properties.Settings.Default.MpvNetLibNoVideo != value)
                    {
                        Properties.Settings.Default.MpvNetLibNoVideo = value;
                        Properties.Settings.Default.Save();
                    }

                    string? vid = MPPlayer?.API.GetPropertyString("vid");

                    if (vid != "auto")
                    {
                        MPPlayer?.API.SetPropertyString("vid", "auto");

                        string message = MsgSet.MsgNoVideoDisabled;

                        TaskbarIconUtil.ShowNotify(
                            message,
                            NotificationIcon.Info);

                        WriteLog(message);
                    }
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

    private void CBYTQuality_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ComboBox comboBox = (ComboBox)sender;

                if (comboBox != null)
                {
                    int selectedIndex = comboBox.SelectedIndex;

                    YouTubeDlVideoQuality quality = CustomFunction.GetYTQuality(selectedIndex);

                    if (MPPlayer != null)
                    {
                        if (Properties.Settings.Default.MpvNetLibYTQualityIndex != selectedIndex)
                        {
                            Properties.Settings.Default.MpvNetLibYTQualityIndex = selectedIndex;
                            Properties.Settings.Default.Save();
                        }

                        MPPlayer.YouTubeDlVideoQuality = quality;

                        // 避免在應用程式剛開始執行時就輸出提示訊息。
                        if (!IsInitializing)
                        {
                            WriteLog(MsgSet.MsgYouTubeVideoQualityChanged);
                        }
                    }
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

    private void CBSpeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ComboBox comboBox = (ComboBox)sender;

                if (comboBox != null)
                {
                    int selectedIndex = comboBox.SelectedIndex;

                    double speed = CustomFunction.GetPlaybackSpeed(selectedIndex);

                    if (MPPlayer != null)
                    {
                        if (Properties.Settings.Default.MpvNetLibPlaybackSpeedIndex != selectedIndex)
                        {
                            Properties.Settings.Default.MpvNetLibPlaybackSpeedIndex = selectedIndex;
                            Properties.Settings.Default.Save();
                        }

                        MPPlayer.Speed = speed;
                    }
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

    public void BtnMute_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (MPPlayer != null)
            {
                MuteClip(MPPlayer.Volume > 0);
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    public void BtnPlay_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (GlobalDataSet.Count <= 0)
                {
                    string message = MsgSet.MsgPlayFailedClipListNoData;

                    TaskbarIconUtil.ShowNotify(
                        message,
                        NotificationIcon.Warning);

                    ShowMsgBox(message);

                    return;
                }

                StopClip();

                ClipData clipData = GlobalDataSet[0];

                PlayClip(clipData);
            }));
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    public void BtnPause_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(PauseOrResumeClip));
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    public void BtnPrevious_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                int previousIndex = CPPlayer.PreviousIndex;

                if (previousIndex > -1)
                {
                    ClipData clipData = GlobalDataSet[previousIndex];

                    PlayClip(clipData);
                }
                else
                {
                    BtnStop_Click(
                        nameof(BtnPrevious),
                        new RoutedEventArgs(ButtonBase.ClickEvent));

                    string message = MsgSet.MsgNoPreviousClip;

                    TaskbarIconUtil.ShowNotify(
                        message,
                        NotificationIcon.Info);

                    ShowMsgBox(message);
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

    public void BtnNext_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                int nextIndex = CPPlayer.NextIndex;

                if (nextIndex > -1)
                {
                    ClipData clipData = GlobalDataSet[nextIndex];

                    PlayClip(clipData);
                }
                else
                {
                    BtnStop_Click(
                        nameof(BtnNext_Click),
                        new RoutedEventArgs(ButtonBase.ClickEvent));

                    string message = MsgSet.MsgNoNextClip;

                    TaskbarIconUtil.ShowNotify(
                        message,
                        NotificationIcon.Info);

                    ShowMsgBox(message);
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

    public void BtnStop_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                StopClip();

                // 當停止播放時，關閉彈出視窗。
                if (WPPPlayer != null && WPPPlayer.IsShowing())
                {
                    WPPPlayer.Close();
                    WPPPlayer = null;
                }

                TaskbarIconUtil.ShowNotify(
                    MsgSet.MsgStoppedPlayingClip,
                    NotificationIcon.Info);
                TaskbarIconUtil.SetToolTip(string.Empty);
            }));
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }
}