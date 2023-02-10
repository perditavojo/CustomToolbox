using CustomToolbox.Common.Sets;
using static CustomToolbox.Common.Sets.EnumSet;
using Mpv.NET.Player;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CustomToolbox.Common.Models;

/// <summary>
/// 類別：短片播放器
/// </summary>
public class ClipPlayer
{
    [JsonPropertyName("status")]
    [Description("狀態")]
    public ClipPlayerStatus Status { get; set; } = ClipPlayerStatus.Idle;

    [JsonPropertyName("mode")]
    [Description("模式")]
    public ClipPlayerMode Mode { get; set; } = ClipPlayerMode.ClipPlayer;

    [JsonPropertyName("clipData")]
    [Description("短片資料")]
    public ClipData? ClipData { get; set; }

    [JsonPropertyName("index")]
    [Description("索引值")]
    public int Index { get; set; } = -1;

    [JsonPropertyName("previousIndex")]
    [Description("上一個索引值")]
    public int PreviousIndex { get; set; } = -1;

    [JsonPropertyName("nextIndex")]
    [Description("下一個索引值")]
    public int NextIndex { get; set; } = -1;

    [JsonPropertyName("seekStatus")]
    [Description("SSeek 的狀態")]
    public SSeekStatus SeekStatus { get; set; } = SSeekStatus.Idle;

    [JsonPropertyName("noVideo")]
    [Description("不顯示影像")]
    public readonly bool NoVideo = Properties.Settings.Default.MpvNetLibNoVideo;

    [JsonPropertyName("playbackSpeed")]
    [Description("播放速度")]
    public readonly double PlaybackSpeed = CustomFunction
        .GetPlaybackSpeed(Properties.Settings.Default.MpvNetLibPlaybackSpeedIndex);

    [JsonPropertyName("ytQuality")]
    [Description("YouTube 影片畫質")]
    public readonly YouTubeDlVideoQuality YTQuality = CustomFunction
        .GetYTQuality(Properties.Settings.Default.MpvNetLibYTQualityIndex);

    [JsonPropertyName("volume")]
    [Description("音量")]
    public readonly double Volume = Properties.Settings.Default.MpvNetLibVolume;

    /// <summary>
    /// 更新索引值
    /// </summary>
    /// <param name="control">DataGrid，DGClipList</param>
    /// <param name="dataSource">ObservableCollection&lt;ClipData&gt;</param>
    /// <param name="newIndex">數值，新的索引值</param>
    public void UpdateIndex(
        DataGrid control,
        ObservableCollection<ClipData> dataSource,
        int newIndex)
    {
        // 當 newIndex 等於 -1 時，即表示目前播放的短片已從短篇清單中刪除。
        if (newIndex == -1)
        {
            // TODO: 2022-12-03 待考慮是否要調整程式的行為。

            // 當 Index 的值還在 dataSource 的範圍內時，則繼續使用 Index 的值。
            // 注意！這行為有可能會造成播放項目會有跳躍的情況。
            if (Index <= dataSource.Count - 1)
            {
                newIndex = Index;
            }
        }

        int tempPreIndex = newIndex,
            tempNexIndex = newIndex,
            previousIndex = --tempPreIndex,
            nextIndex = ++tempNexIndex;

        if (previousIndex < 0)
        {
            previousIndex = -1;
        }

        PreviousIndex = previousIndex;

        if (nextIndex > dataSource.Count - 1)
        {
            nextIndex = -1;
        }

        NextIndex = nextIndex;

        Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
        {
            control.SelectedIndex = newIndex;
        }));
    }

    /// <summary>
    /// 轉換成字串
    /// </summary>
    /// <returns>字串</returns>
    public override string ToString() => JsonSerializer.Serialize(this, VariableSet.SharedJSOptions);
}