using Control = System.Windows.Controls.Control;
using CustomToolbox.Common.Sets;
using H.NotifyIcon;
using H.NotifyIcon.Core;
using ModernWpf.Controls;
using System.Windows;
using System.Windows.Controls;

namespace CustomToolbox.Common.Utils;

/// <summary>
/// TaskbarIcon 工具
/// </summary>
internal class TaskbarIconUtil
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
    private static readonly MenuItem MIShowOrHide = new();

    /// <summary>
    /// 靜音／取消靜音
    /// </summary>
    private static readonly MenuItem MIMute = new();

    /// <summary>
    /// 不顯示影片
    /// </summary>
    private static readonly MenuItem MINoVideo = new();

    /// <summary>
    /// 隨機播放短片
    /// </summary>
    private static readonly MenuItem MIRandomPlayClip = new();

    /// <summary>
    /// 播放
    /// </summary>
    private static readonly MenuItem MIPlayClip = new();

    /// <summary>
    /// 暫停
    /// </summary>
    private static readonly MenuItem MIPause = new();

    /// <summary>
    /// 上一個
    /// </summary>
    private static readonly MenuItem MIPrevious = new();

    /// <summary>
    /// 下一個
    /// </summary>
    private static readonly MenuItem MINext = new();

    /// <summary>
    /// 停止
    /// </summary>
    private static readonly MenuItem MIStop = new();

    /// <summary>
    /// 關於選單
    /// </summary>
    private static readonly MenuItem MIAboutMenu = new();

    /// <summary>
    /// 關於
    /// </summary>
    private static readonly MenuItem MIAbout = new();

    /// <summary>
    /// 檢查更新
    /// </summary>
    private static readonly MenuItem MICheckUpdate = new();

    /// <summary>
    /// 結束
    /// </summary>
    private static readonly MenuItem MIExit = new();

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
            MIShowOrHide.Header = MsgSet.Hide;
            MIShowOrHide.Icon = new SymbolIcon(Symbol.HideBcc);
            MIMute.Header = _WMain.BtnMute.Label;
            MIMute.Icon = new SymbolIcon(Symbol.Mute);
            MINoVideo.Header = MsgSet.MIEnableNoVideo;
            MINoVideo.Icon = new SymbolIcon(Symbol.Video);
            MIRandomPlayClip.Header = _WMain.MIRandomPlayClip.Header;
            MIRandomPlayClip.Icon = new SymbolIcon(Symbol.Shuffle);
            MIPlayClip.Header = _WMain.BtnPlay.Label;
            MIPlayClip.Icon = new SymbolIcon(Symbol.Play);
            MIPause.Header = _WMain.BtnPause.Label;
            MIPause.Icon = new SymbolIcon(Symbol.Pause);
            MIPrevious.Header = _WMain.BtnPrevious.Label;
            MIPrevious.Icon = new SymbolIcon(Symbol.Previous);
            MINext.Header = _WMain.BtnNext.Label;
            MINext.Icon = new SymbolIcon(Symbol.Next);
            MIStop.Header = _WMain.BtnStop.Label;
            MIStop.Icon = new SymbolIcon(Symbol.Stop);
            MIAboutMenu.Header = _WMain.MIAbout.Header;
            MIAboutMenu.Icon = new SymbolIcon(Symbol.Help);
            MICheckUpdate.Header = _WMain.MICheckUpdate.Header;
            MICheckUpdate.Icon = new SymbolIcon(Symbol.Download);
            MIAbout.Header = _WMain.MIAbout.Header;
            MIAbout.Icon = new SymbolIcon(Symbol.Help);
            MIExit.Header = _WMain.MIExit.Header;
            MIExit.Icon = new SymbolIcon(Symbol.Cancel);

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
            ContextMenu contextMenu = new();

            contextMenu.Items.Add(MIShowOrHide);
            contextMenu.Items.Add(new Separator());
            contextMenu.Items.Add(MIMute);
            contextMenu.Items.Add(MINoVideo);
            contextMenu.Items.Add(MIRandomPlayClip);
            contextMenu.Items.Add(new Separator());
            contextMenu.Items.Add(MIPlayClip);
            contextMenu.Items.Add(MIPause);
            contextMenu.Items.Add(MIPrevious);
            contextMenu.Items.Add(MINext);
            contextMenu.Items.Add(MIStop);
            contextMenu.Items.Add(new Separator());

            MIAboutMenu.Items.Add(MICheckUpdate);
            MIAboutMenu.Items.Add(MIAbout);

            contextMenu.Items.Add(MIAboutMenu);
            contextMenu.Items.Add(MIExit);

            // 設定 _TaskbarIcon 的右鍵選單。 
            _TaskbarIcon.ContextMenu = contextMenu;

            // 設定 MenuItem 啟用／禁用。
            SetMenuItems();
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
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
                ex.ToString()));
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
                ex.ToString()));
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
                ex.ToString()));
        }
    }

    /// <summary>
    /// 拋棄 TaskbarIcon
    /// </summary>
    public static void Dispose()
    {
        try
        {
            _TaskbarIcon?.Dispose();
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
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
                ex.ToString()));
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
                ex.ToString()));
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
                ex.ToString()));
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
                ex.ToString()));
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
                ex.ToString()));
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

            MINoVideo.Header = header;
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }
}