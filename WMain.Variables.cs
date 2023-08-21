using CustomToolbox.Common.Models;
using DiscordRPC;
using ModernWpf.Controls;
using Mpv.NET.Player;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace CustomToolbox;

/// <summary>
/// WMain 的變數
/// </summary>
public partial class WMain
{
    /// <summary>
    /// 共用的 ToolTip
    /// </summary>
    public ToolTip TTTips = new();

    /// <summary>
    /// 共用的 MpvPlayer
    /// </summary>
    public MpvPlayer? MPPlayer = null;

    /// <summary>
    /// 共用的 DiscordRpcClient
    /// </summary>
    public DiscordRpcClient? GlobalDRClient = null;

    /// <summary>
    /// 共用的 ClipPlayer
    /// </summary>
    public readonly ClipPlayer CPPlayer = new();

    /// <summary>
    /// 共用的 GlobalCDDialog
    /// </summary>
    public ContentDialog? GlobalCDDialog = null;

    /// <summary>
    /// 共用的 CancellationTokenSource
    /// </summary>
    public CancellationTokenSource? GlobalCTS = null;

    /// <summary>
    /// 共用的 ObservableCollection&lt;ClipData&gt;
    /// </summary>
    private readonly ObservableCollection<ClipData> GlobalDataSet = new();

    /// <summary>
    /// 共用的 IHttpClientFactory
    /// </summary>
    private static IHttpClientFactory? GlobalHCFactory = null;

    /// <summary>
    /// 共用的 WPopupPlayer
    /// </summary>
    private WPopupPlayer? WPPPlayer = null;

    /// <summary>
    /// 共用的 CancellationToken
    /// </summary>
    private CancellationToken? GlobalCT = null;

    /// <summary>
    /// 是否正在初始化中
    /// </summary>
    private bool IsInitializing = false;

    /// <summary>
    /// 是否正在等待中
    /// </summary>
    private bool IsPending = false;
}