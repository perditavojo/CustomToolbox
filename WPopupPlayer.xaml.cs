using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Sets;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using Serilog.Events;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms.Integration;

namespace CustomToolbox;

/// <summary>
/// WPopupPlayer.xaml 的互動邏輯
/// </summary>
public partial class WPopupPlayer : Window
{
    /// <summary>
    /// WMain
    /// </summary>
    private readonly WMain _WMain;

    /// <summary>
    /// WFHContainer
    /// </summary>
    private readonly WindowsFormsHost _WFHContainer;

    /// <summary>
    /// PlayerHost
    /// </summary>
    private readonly Panel _PlayerHost;

    /// <summary>
    /// TTTips
    /// </summary>
    private readonly ToolTip _TTTips;

    public WPopupPlayer(WMain window)
    {
        InitializeComponent();

        _WMain = window;
        _WFHContainer = _WMain.WFHContainer;
        _PlayerHost = _WMain.PlayerHost;
        _TTTips = _WMain.TTTips;

        KeyDown += _WMain.WMain_KeyDown;
    }

    private void WPopupPlayer_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Title = _WMain.Title;
                Icon = _WMain.Icon;

                WFHContainer.Child = _PlayerHost;
                _WFHContainer.Child = null;

                Panel PPlaceholder = new()
                {
                    BackColor = Color.Gray,
                    Dock = DockStyle.Fill
                };

                _TTTips.SetToolTip(PPlaceholder, MsgSet.MsgDoubleClickToTogglePopupWindow);

                PPlaceholder.MouseDoubleClick += PPlaceholder_MouseDoubleClick;

                _WFHContainer.Child = PPlaceholder;
            }));
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    private void WPopupPlayer_Closing(object sender, CancelEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                KeyDown -= _WMain.WMain_KeyDown;

                _WFHContainer.Child = null;
                _WFHContainer.Child = WFHContainer.Child;
                WFHContainer.Child = null;
            }));
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    private void PPlaceholder_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                _WMain.PlayerHost_MouseDoubleClick(sender, e);
            }));
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }
}