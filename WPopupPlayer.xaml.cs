using AirspaceFixer;
using CustomToolbox.Common.Sets;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms.Integration;
using TextBox = System.Windows.Controls.TextBox;

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

    /// <summary>
    /// TBLog
    /// </summary>
    private readonly TextBox _TBLog;

    /// <summary>
    /// AAPanel
    /// </summary>
    private readonly AirspacePanel _AAPanel;

    public WPopupPlayer(WMain window)
    {
        InitializeComponent();

        _WMain = window;
        _WFHContainer = _WMain.WFHContainer;
        _PlayerHost = _WMain.PlayerHost;
        _TTTips = _WMain.TTTips;
        _TBLog = _WMain.TBLog;
        _AAPanel = _WMain.APPanel;

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
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
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
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
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
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }
}