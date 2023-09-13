using Control = System.Windows.Controls.Control;
using CustomToolbox.Common.Sets;
using H.NotifyIcon;
using H.NotifyIcon.Core;
using ModernWpf.Controls;
using System.Windows;
using System.Windows.Controls;
using CustomToolbox.Common.Extensions;

namespace CustomToolbox.Common.Utils;

/// <summary>
/// TaskbarIcon 工具
/// </summary>
public class TaskbarIconUtil
{
    /// <summary>
    /// 標題
    /// </summary>
    private static string _Title = string.Empty;

    /// <summary>
    /// TaskbarIcon
    /// </summary>
    private static TaskbarIcon? _TaskbarIcon = null;

    /// <summary>
    /// WMain
    /// </summary>
    private static WMain? _WMain = null;

    /// <summary>
    /// 顯示／隱藏
    /// </summary>
    private static MenuItem MIShowOrHide = new();

    /// <summary>
    /// 靜音／取消靜音
    /// </summary>
    private static MenuItem MIMute = new();

    /// <summary>
    /// 不顯示影片
    /// </summary>
    private static MenuItem MINoVideo = new();

    /// <summary>
    /// 隨機播放短片
    /// </summary>
    private static MenuItem MIRandomPlayClip = new();

    /// <summary>
    /// 播放
    /// </summary>
    private static MenuItem MIPlayClip = new();

    /// <summary>
    /// 暫停
    /// </summary>
    private static MenuItem MIPause = new();

    /// <summary>
    /// 上一個
    /// </summary>
    private static MenuItem MIPrevious = new();

    /// <summary>
    /// 下一個
    /// </summary>
    private static MenuItem MINext = new();

    /// <summary>
    /// 停止
    /// </summary>
    private static MenuItem MIStop = new();

    /// <summary>
    /// 關於選單
    /// </summary>
    private static MenuItem MIAboutMenu = new();

    /// <summary>
    /// 關於
    /// </summary>
    private static MenuItem MIAbout = new();

    /// <summary>
    /// 檢查更新
    /// </summary>
    private static MenuItem MICheckUpdate = new();

    /// <summary>
    /// 結束
    /// </summary>
    private static MenuItem MIExit = new();

    /// <summary>
    /// 右鍵選單
    /// </summary>
    private static ContextMenu CMContextMenu = new();

    /// <summary>
    /// 初始化 TaskbarIcon
    /// </summary>
    /// <param name="wMain">WMain</param>
    /// <param name="taskbarIcon">TaskbarIcon</param>
    public static void Init(WMain wMain, TaskbarIcon taskbarIcon)
    {
        try
        {
            _TaskbarIcon = taskbarIcon;
            _WMain = wMain;
            _Title = _WMain.Title;

            _TaskbarIcon.Icon = Properties.Resources.app_icon;
            _TaskbarIcon.ToolTipText = _Title;

            // 設定 _TaskbarIcon 的滑鼠雙點擊事件。
            _TaskbarIcon.TrayMouseDoubleClick += TaskbarIcon_TrayMouseDoubleClick;

            // 設定 MenuItem。
            MIShowOrHide = new()
            {
                Header = MsgSet.Hide,
                Icon = new SymbolIcon(Symbol.HideBcc)
            };
            MIMute = new()
            {
                Header = _WMain.BtnMute.Label,
                Icon = new SymbolIcon(Symbol.Mute)
            };
            MINoVideo = new()
            {
                Header = MsgSet.MIEnableNoVideo,
                Icon = new SymbolIcon(Symbol.Video)
            };
            MIRandomPlayClip = new()
            {
                Header = _WMain.MIRandomPlayClip.Header,
                Icon = new SymbolIcon(Symbol.Shuffle)
            };
            MIPlayClip = new()
            {
                Header = _WMain.BtnPlay.Label,
                Icon = new SymbolIcon(Symbol.Play)
            };
            MIPause = new()
            {
                Header = _WMain.BtnPause.Label,
                Icon = new SymbolIcon(Symbol.Pause)
            };
            MIPrevious = new()
            {
                Header = _WMain.BtnPrevious.Label,
                Icon = new SymbolIcon(Symbol.Previous)
            };
            MINext = new()
            {
                Header = _WMain.BtnNext.Label,
                Icon = new SymbolIcon(Symbol.Next)
            };
            MIStop = new()
            {
                Header = _WMain.BtnStop.Label,
                Icon = new SymbolIcon(Symbol.Stop)
            };
            MIAboutMenu = new()
            {
                Header = _WMain.MIAbout.Header,
                Icon = new SymbolIcon(Symbol.Help)
            };
            MICheckUpdate = new()
            {
                Header = _WMain.MICheckUpdate.Header,
                Icon = new SymbolIcon(Symbol.Download)
            };
            MIAbout = new()
            {
                Header = _WMain.MIAbout.Header,
                Icon = new SymbolIcon(Symbol.Help)
            };
            MIExit = new()
            {
                Header = _WMain.MIExit.Header,
                Icon = new SymbolIcon(Symbol.Cancel)
            };

            // 設定 MenuItem 的點擊事件。
            MIShowOrHide.Click += MIShowOrHide_Click;
            MIMute.Click += _WMain.BtnMute_Click;
            MINoVideo.Click += MINoVideo_Click;
            MIRandomPlayClip.Click += _WMain.MIRandomPlayClip_Click;
            MIPlayClip.Click += _WMain.BtnPlay_Click;
            MIPause.Click += _WMain.BtnPause_Click;
            MIPrevious.Click += _WMain.BtnPrevious_Click;
            MINext.Click += _WMain.BtnNext_Click;
            MIStop.Click += _WMain.BtnStop_Click;
            MICheckUpdate.Click += _WMain.MICheckUpdate_Click;
            MIAbout.Click += _WMain.MIAbout_Click;
            MIExit.Click += _WMain.MIExit_Click;

            // 建立右鍵選單。
            CMContextMenu = new();

            CMContextMenu.Items.Clear();
            CMContextMenu.Items.Add(MIShowOrHide);
            CMContextMenu.Items.Add(new Separator());
            CMContextMenu.Items.Add(MIMute);
            CMContextMenu.Items.Add(MINoVideo);
            CMContextMenu.Items.Add(MIRandomPlayClip);
            CMContextMenu.Items.Add(new Separator());
            CMContextMenu.Items.Add(MIPlayClip);
            CMContextMenu.Items.Add(MIPause);
            CMContextMenu.Items.Add(MIPrevious);
            CMContextMenu.Items.Add(MINext);
            CMContextMenu.Items.Add(MIStop);
            CMContextMenu.Items.Add(new Separator());

            MIAboutMenu.Items.Clear();
            MIAboutMenu.Items.Add(MICheckUpdate);
            MIAboutMenu.Items.Add(MIAbout);

            CMContextMenu.Items.Add(MIAboutMenu);
            CMContextMenu.Items.Add(MIExit);

            // 設定 _TaskbarIcon 的右鍵選單。
            _TaskbarIcon.ContextMenu = CMContextMenu;

            // 設定 MenuItem 啟用／禁用。
            SetMenuItems();
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }

    private static void TaskbarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
    {
        try
        {
            MIShowOrHide.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }

    /// <summary>
    /// 顯示提示訊息
    /// </summary>
    /// <param name="message">字串，訊息</param>
    /// <param name="notificationIcon">NotificationIcon，預設值為 NotificationIcon.None</param>
    public static void ShowNotify(
        string message,
        NotificationIcon notificationIcon = NotificationIcon.None)
    {
        try
        {
            _TaskbarIcon?.ShowNotification(
                 _Title,
                message,
                notificationIcon);
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }

    }

    /// <summary>
    /// 設定 TaskbarIcon 的工具提示文字
    /// </summary>
    /// <param name="message">字串，訊息</param>
    public static void SetToolTip(string? message)
    {
        try
        {
            if (_TaskbarIcon != null)
            {
                _TaskbarIcon.ToolTipText = string.IsNullOrEmpty(message) ? _Title : message;
            }
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }

    /// <summary>
    /// 拋棄 TaskbarIcon
    /// </summary>
    public static void Dispose()
    {
        try
        {
            if (_WMain != null)
            {
                MIShowOrHide.Click -= MIShowOrHide_Click;
                MIMute.Click -= _WMain.BtnMute_Click;
                MINoVideo.Click -= MINoVideo_Click;
                MIRandomPlayClip.Click -= _WMain.MIRandomPlayClip_Click;
                MIPlayClip.Click -= _WMain.BtnPlay_Click;
                MIPause.Click -= _WMain.BtnPause_Click;
                MIPrevious.Click -= _WMain.BtnPrevious_Click;
                MINext.Click -= _WMain.BtnNext_Click;
                MIStop.Click -= _WMain.BtnStop_Click;
                MICheckUpdate.Click -= _WMain.MICheckUpdate_Click;
                MIAbout.Click -= _WMain.MIAbout_Click;
                MIExit.Click -= _WMain.MIExit_Click;
            }

            if (_TaskbarIcon != null)
            {
                _TaskbarIcon.Dispose();
                _TaskbarIcon = null;
            }
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }

    /// <summary>
    /// 設定 MenuItem 啟用／禁用
    /// </summary>
    /// <param name="enable">布林值，啟用，預設值為 true</param>
    public static void SetMenuItems(bool enable = true)
    {
        try
        {
            Control[] ctrlSet1 =
            {
                MIPlayClip
            };

            Control[] ctrlSet2 =
            {
                MIPrevious,
                MINext,
                MIPause,
                MIStop
            };

            CustomFunction.BatchSetEnabled(ctrlSet1, enable);
            CustomFunction.BatchSetEnabled(ctrlSet2, !enable);
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }

    /// <summary>
    /// 更新 MIMute 的 Header
    /// </summary>
    /// <param name="value">字串，值</param>
    /// <param name="isMuted">布林值，是否靜音，預設值為 false</param>
    public static void UpdateMIMuteHeader(string? value, bool isMuted = false)
    {
        try
        {
            if (!string.IsNullOrEmpty(value))
            {
                MIMute.Header = value;
                MIMute.Icon = isMuted ? new SymbolIcon(Symbol.Volume) : new SymbolIcon(Symbol.Mute);
            }
            else
            {
                MIMute.Header = _WMain?.BtnMute.Label;
                MIMute.Icon = isMuted ? new SymbolIcon(Symbol.Volume) : new SymbolIcon(Symbol.Mute);
            }
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }

    /// <summary>
    /// 更新 MIShowOrHide 的 Header
    /// </summary>
    /// <param name="visibility">Visibility</param>
    public static void UpdateMIShowOrHideHeader(Visibility visibility)
    {
        try
        {
            switch (visibility)
            {
                case Visibility.Visible:
                    MIShowOrHide.Header = MsgSet.Hide;
                    MIShowOrHide.Icon = new SymbolIcon(Symbol.HideBcc);

                    break;
                case Visibility.Collapsed:
                    MIShowOrHide.Header = MsgSet.Show;
                    MIShowOrHide.Icon = new SymbolIcon(Symbol.ShowBcc);

                    break;
                case Visibility.Hidden:
                    MIShowOrHide.Header = MsgSet.Show;
                    MIShowOrHide.Icon = new SymbolIcon(Symbol.ShowBcc);

                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }

    private static void MIShowOrHide_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Visibility visibility = CustomFunction.ShowOrHideWindow(_WMain);

            UpdateMIShowOrHideHeader(visibility);
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }

    private static void MINoVideo_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_WMain != null)
            {
                if (_WMain.CBNoVideo.IsChecked == true)
                {
                    _WMain.CBNoVideo.IsChecked = false;
                }
                else
                {
                    _WMain.CBNoVideo.IsChecked = true;
                }

                string header = _WMain.CBNoVideo.IsChecked == true ?
                    MsgSet.MIDisableNoVideo :
                    MsgSet.MIEnableNoVideo;

                MINoVideo.Header = header;
            }
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }

    /// <summary>
    /// 更新 MINoVideo 的 Header
    /// </summary>
    /// <param name="isChecked">布林值，預設值為 false</param>
    public static void UpdateMINoVideoHeader(bool isChecked = false)
    {
        try
        {
            string header = isChecked == true ?
                MsgSet.MIDisableNoVideo :
                MsgSet.MIEnableNoVideo;
            SymbolIcon icon = isChecked == true ?
                new SymbolIcon(Symbol.Video) :
                new SymbolIcon(Symbol.Placeholder);

            MINoVideo.Header = header;
            MINoVideo.Icon = icon;
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }

    /// <summary>
    /// 更新 MIPause 的 Header
    /// </summary>
    /// <param name="isPaused">布林值，預設值為 false</param>
    public static void UpdateMIPauseHeader(bool isPaused = false)
    {
        try
        {
            string header = isPaused == true ?
                MsgSet.Resume :
                MsgSet.Pause;

            SymbolIcon icon = isPaused == true ?
                new SymbolIcon(Symbol.Play) :
                 new SymbolIcon(Symbol.Pause);

            MIPause.Header = header;
            MIPause.Icon = icon;
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }
}