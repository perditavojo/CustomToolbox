using CustomToolbox.Common.Sets;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Media;

namespace CustomToolbox;

/// <summary>
/// WPopupPlayer.xaml 的互動邏輯
/// </summary>
public partial class WPopupPlayer : Window
{
    private readonly WMain oWMain;
    private readonly WindowsFormsHost oWFHContainer;
    private readonly Panel oPlayerHost;
 
    public WPopupPlayer(WMain window)
    {
        InitializeComponent();

        oWMain = window;
        oWFHContainer = oWMain.WFHContainer;
        oPlayerHost = oWMain.PlayerHost;
    }

    private void WPopupPlayer_Loaded(object sender, RoutedEventArgs e)
    {
        WFHContainer.Child = oPlayerHost;

        oWFHContainer.Child = null;

        Panel PPlaceholder = new()
        {
            BackColor = System.Drawing.Color.Gray,
            Dock = DockStyle.Fill
        };

        PPlaceholder.MouseDoubleClick += PPlaceholder_MouseDoubleClick;

        oWFHContainer.Child = PPlaceholder;

        // TODO: 2022-11-23 待處裡 Window 的標題。
    }

    private void WPopupPlayer_Closing(object sender, CancelEventArgs e)
    {
        oWFHContainer.Child = null;
        oWFHContainer.Child = WFHContainer.Child;
        WFHContainer.Child = null;
    }

    private void PPlaceholder_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        oWMain.PlayerHost_MouseDoubleClick(sender, e);
    }
}