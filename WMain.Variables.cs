using CustomToolbox.Common.Models;
using DiscordRPC;
using ModernWpf.Controls;
using Mpv.NET.Player;
using System.Collections.ObjectModel;

namespace CustomToolbox;

public partial class WMain
{
    public ToolTip TTTips = new();

    public MpvPlayer? MPPlayer = null;

    public DiscordRpcClient? GlobalDRClient = null;

    public readonly ClipPlayer CPPlayer = new();

    public ContentDialog? GlobalCDDialog = null;

    private readonly ObservableCollection<ClipData> GlobalDataSet = new();

    private WPopupPlayer? WPPPlayer = null;

    private CancellationTokenSource? GlobalCTS = null;

    private CancellationToken? GlobalCT = null;

    private bool IsInitializing = false;
}   