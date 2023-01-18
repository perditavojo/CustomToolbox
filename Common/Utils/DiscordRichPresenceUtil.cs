using CustomToolbox.Common.Models;
using CustomToolbox.Common.Sets;
using DiscordRPC;
using DiscordRPC.Logging;
using DiscordRPC.Message;

namespace CustomToolbox.Common.Utils;

/// <summary>
/// Discord 豐富狀態工具
/// </summary>
internal class DiscordRichPresenceUtil
{
    /// <summary>
    /// WMain
    /// </summary>
    private static WMain? _WMain = null;

    /// <summary>
    /// GlobalDRClient
    /// </summary>
    private static DiscordRpcClient? _GlobalDRClient = null;

    /// <summary>
    /// Discord 豐富狀態連線失敗次數
    /// </summary>
    private static int ConnectionFailedCount = 0;

    /// <summary>
    /// Discord 豐富狀態連線最大失敗次數
    /// </summary>
    private static readonly int MaxConnectionFailedCount = Properties.Settings.
        Default.DiscrodMaxConnectionFailedCount;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="wMain">WMain</param>
    public static void Init(WMain wMain)
    {
        _WMain = wMain;
        _GlobalDRClient = _WMain.GlobalDRClient;
    }

    /// <summary>
    /// 初始化 Discord 豐富狀態
    /// </summary>
    public static void InitRichPresence()
    {
        try
        {
            // 共用 MpvLogVerbose 的設定值。
            bool logVerbose = Properties.Settings.Default.MpvNetLibLogVerbose;

            string dicordAppID = Properties.Settings.Default.DiscrodApplicationID;

            if (string.IsNullOrEmpty(dicordAppID))
            {
                return;
            }

            _GlobalDRClient ??= new(dicordAppID)
            {
                Logger = new ConsoleLogger { Level = logVerbose ? LogLevel.Trace : LogLevel.Warning }
            };

            _GlobalDRClient.OnConnectionEstablished += (object sender, ConnectionEstablishedMessage e) =>
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgDiscordRichPresenceConnectedPipe,
                    e.Type.ToString(),
                    e.ConnectedPipe.ToString()));
            };

            _GlobalDRClient.OnConnectionFailed += (object sender, ConnectionFailedMessage e) =>
            {
                ConnectionFailedCount++;

                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgDiscordRichPresenceFailedPipe,
                    e.Type.ToString(),
                    e.FailedPipe.ToString()));

                if (ConnectionFailedCount == MaxConnectionFailedCount)
                {
                    _WMain?.WriteLog(MsgSet.GetFmtStr(
                        MsgSet.MsgDiscordRichPresenceConnectionFailed,
                        ConnectionFailedCount.ToString()));

                    Dispose();

                    if (_WMain != null)
                    {
                        _WMain.GlobalDRClient = null;
                    }
                }
            };

            _GlobalDRClient.OnReady += (object sender, ReadyMessage e) =>
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgDiscordRichPresenceOnReady,
                    e.Type.ToString(),
                    e.User.Username));
            };

            _GlobalDRClient.OnPresenceUpdate += (object sender, PresenceMessage e) =>
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.TemplateDiscordRichPresenceOnPresenceUpdate,
                    e.Type.ToString(),
                    e.Name,
                    e.ApplicationID,
                    e.Presence.State,
                    e.Presence.Details));
            };

            _GlobalDRClient.OnError += (object sender, ErrorMessage e) =>
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.TemplateDiscordRichPresenceOnError,
                    e.Type.ToString(),
                    e.Code.ToString(),
                    e.Message));
            };

            _GlobalDRClient.OnClose += (object sender, CloseMessage e) =>
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.TemplateDiscordRichPresenceOnClose,
                    e.Type.ToString(),
                    e.Code.ToString(),
                    e.Reason));
            };

            _GlobalDRClient.Initialize();

            _WMain?.WriteLog(MsgSet.MsgEnableDiscordRichPresence);

            SetFirstRichPresence();
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 清除 DiscordRpcClient
    /// </summary>
    public static void Dispose()
    {
        if (_GlobalDRClient != null &&
            !_GlobalDRClient.IsDisposed) 
        {
            _GlobalDRClient?.ClearPresence();
            _GlobalDRClient?.Dispose();

            // 重設 ConnectionFailedCount。
            ConnectionFailedCount = 0;

            _WMain?.WriteLog(MsgSet.MsgDisableDiscordRichPresence);
        }
    }

    /// <summary>
    /// 設定 Discord 的豐富狀態
    /// </summary>
    /// <param name="details">字串，細節，預設值為空白</param>
    /// <param name="state">字串，狀態，預設值為空白</param>
    /// <param name="timestamps">Timestamps，預設值為 null</param>
    /// <param name="assets">Assets，預設值為 null</param>
    public static void SetRichPresence(
        string details = "",
        string state = "",
        Timestamps? timestamps = null,
        Assets? assets = null)
    {
        try
        {
            if (!string.IsNullOrEmpty(details) ||
                !string.IsNullOrEmpty(state) ||
                assets != null)
            {
                _GlobalDRClient?.SetPresence(new()
                {
                    Details = details,
                    State = state,
                    Timestamps = timestamps,
                    Assets = assets
                });
            }
            else
            {
                _GlobalDRClient?.SetPresence(null);
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
    /// 取得 Timestamps
    /// </summary>
    /// <returns>Timestamps</returns>
    public static Timestamps GetTimestamps()
    {
        return _GlobalDRClient?.CurrentPresence?.Timestamps ?? Timestamps.Now;
    }

    /// <summary>
    /// 設定第一個 Discord 豐富狀態
    /// </summary>
    private static void SetFirstRichPresence()
    {
        EnumSet.ClipPlayerStatus? clipPlayerStatus = _WMain?.CPPlayer.Status;

        if (clipPlayerStatus != null)
        {
            Timestamps timestamps = GetTimestamps();

            switch (clipPlayerStatus)
            {
                case EnumSet.ClipPlayerStatus.Playing:
                    ClipData? clipData = _WMain?.CPPlayer.ClipData;

                    string details = string.Empty;

                    if (clipData != null && !string.IsNullOrEmpty(clipData.Name))
                    {
                        details = clipData.Name;
                    }

                    SetRichPresence(
                        details: details,
                        state: MsgSet.StatePlaying,
                        timestamps: timestamps,
                        assets: AssetsSet.AssetsPlay);

                    break;
                case EnumSet.ClipPlayerStatus.Paused:
                    SetRichPresence(
                        state: MsgSet.StatePause,
                        timestamps: timestamps,
                        assets: AssetsSet.AssetsPause);

                    break;
                case EnumSet.ClipPlayerStatus.Idle:
                    SetRichPresence(
                        state: MsgSet.StateStop,
                        timestamps: timestamps,
                        assets: AssetsSet.AssetsStop);

                    break;
                default:
                    SetRichPresence(
                        state: MsgSet.StateStop,
                        timestamps: timestamps,
                        assets: AssetsSet.AssetsStop);

                    break;
            };
        }
        else
        {
            SetRichPresence(
                state: MsgSet.StateStop,
                assets: AssetsSet.AssetsStop);
        }
    }
}