using DiscordRPC;

namespace CustomToolbox.Common.Sets;

/// <summary>
/// 資產組
/// </summary>
internal class AssetsSet
{
    /// <summary>
    /// 資產：正在播放
    /// </summary>
    public static readonly Assets AssetsPlay = new()
    {
        LargeImageKey = "app_icon",
        LargeImageText = MsgSet.AppName,
        SmallImageKey = "play",
        SmallImageText = MsgSet.StatePlaying
    };

    /// <summary>
    /// 資產：暫停播放
    /// </summary>
    public static readonly Assets AssetsPause = new()
    {
        LargeImageKey = "app_icon",
        LargeImageText = MsgSet.AppName,
        SmallImageKey = "pause",
        SmallImageText = MsgSet.StatePause
    };

    /// <summary>
    /// 資產：停止播放
    /// </summary>
    public static readonly Assets AssetsStop = new()
    {
        LargeImageKey = "app_icon",
        LargeImageText = MsgSet.AppName,
        SmallImageKey = "stop",
        SmallImageText = MsgSet.StateStop
    };
}