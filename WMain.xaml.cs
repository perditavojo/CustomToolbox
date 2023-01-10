using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
using CustomToolbox.Common;
using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Sets;
using CustomToolbox.Common.Utils;
using H.NotifyIcon;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ModernWpf.Controls.Primitives;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using TextBox = System.Windows.Controls.TextBox;

namespace CustomToolbox;

/// <summary>
/// Interaction logic for WMain.xaml
/// </summary>
public partial class WMain : Window
{
    public WMain(IHttpClientFactory httpClientFactory)
    {
        InitializeComponent();

        GlobalHCFactory = httpClientFactory;
    }

    private void WMain_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            CustomInit();
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void WMain_Closing(object sender, CancelEventArgs e)
    {
        try
        {
            // 先攔截事件。
            e.Cancel = true;

            // 關閉應用程式。
            ShutdownApp(sender);
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    public void WMain_KeyDown(object? sender, KeyEventArgs e)
    {
        try
        {
            // 排除組合按鍵。
            if (e.Key == Key.LeftCtrl ||
                e.Key == Key.RightCtrl ||
                e.Key == Key.LeftShift ||
                e.Key == Key.RightShift ||
                e.Key == Key.LeftAlt ||
                e.Key == Key.RightAlt)
            {
                return;
            }

            // 避免在非指定的 TextBox 內輸入內容時觸發快速鍵。
            if (e.OriginalSource is TextBox textBox)
            {
                if (textBox != null && textBox != TBLog)
                {
                    return;
                }
            }

            switch (e.Key)
            {
                case Key.Q:
                    // 強制顯示應用程式以顯示對話視窗。
                    WindowExtensions.Show(
                        this,
                        disableEfficiencyMode: true);

                    WindowState = WindowState.Normal;

                    ShutdownApp(sender);

                    break;
                case Key.W:
                    // 重設 PopupPlayer 的大小與位置。
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (WPPPlayer != null && WPPPlayer.IsShowing())
                        {
                            WPPPlayer.Width = 854;
                            WPPPlayer.Height = 480;
                            WPPPlayer.Top = 60;
                            WPPPlayer.Left = 60;
                        }
                    }));
                    break;
                case Key.E:
                    PlayerHost_MouseDoubleClick(
                        sender,
                        new MouseEventArgs(MouseButtons.Left, 2, 0, 0, 0));

                    break;
                case Key.R:
                    // 開關 PopupPlayer 的全螢幕顯示。
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (WPPPlayer != null && WPPPlayer.IsShowing())
                        {
                            if (WPPPlayer.WindowState != WindowState.Maximized)
                            {
                                WPPPlayer.WindowStyle = WindowStyle.None;
                                WPPPlayer.WindowState = WindowState.Normal;
                                WPPPlayer.WindowState = WindowState.Maximized;

                                // 不關閉的話會無法完全的全螢幕化。
                                WindowHelper.SetUseModernWindowStyle(WPPPlayer, false);
                            }
                            else
                            {
                                WPPPlayer.WindowStyle = WindowStyle.SingleBorderWindow;
                                WPPPlayer.WindowState = WindowState.Normal;

                                WindowHelper.SetUseModernWindowStyle(WPPPlayer, true);
                            }
                        }
                    }));

                    break;
                case Key.T:
                    Visibility visibility = CustomFunction.ShowOrHideWindow(this);

                    TaskbarIconUtil.UpdateMIShowOrHideHeader(visibility);

                    break;
                case Key.A:
                    if (BtnPlay.IsEnabled)
                    {
                        BtnPlay.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }

                    break;
                case Key.S:
                    if (BtnPause.IsEnabled)
                    {
                        BtnPause.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }

                    break;
                case Key.D:
                    if (BtnPrevious.IsEnabled)
                    {
                        BtnPrevious.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }

                    break;
                case Key.F:
                    if (BtnNext.IsEnabled)
                    {
                        BtnNext.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }

                    break;
                case Key.G:
                    if (BtnStop.IsEnabled)
                    {
                        BtnStop.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }

                    break;
                case Key.Z:
                    DoRandomPlayClip();

                    break;
                case Key.X:
                    DoReorderClipList();

                    break;
                case Key.C:
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (CBNoVideo.IsEnabled)
                        {
                            CBNoVideo.IsChecked = !CBNoVideo.IsChecked;
                        }
                    }));

                    break;
                case Key.V:
                    if (BtnMute.IsEnabled)
                    {
                        BtnMute.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
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
}