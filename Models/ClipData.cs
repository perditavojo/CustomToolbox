using System.Text.Json;
using System.Text.Json.Serialization;

namespace CustomToolbox.Models;

/// <summary>
/// 短片資料
/// </summary>
internal class ClipData
{
    /// <summary>
    /// 影片網址／ID
    /// </summary>
    [JsonPropertyName("videoUrlOrID")]
    public string? VideoUrlOrID { get; set; }

    /// <summary>
    /// 編號
    /// </summary>
    [JsonPropertyName("no")]
    public int No { get; set; } = 0;

    /// <summary>
    /// 名稱
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// 開始時間
    /// </summary>
    [JsonPropertyName("startTime")]
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// 結束時間
    /// </summary>
    [JsonPropertyName("endTime")]
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// 字幕檔網址
    /// </summary>
    [JsonPropertyName("subtitleFileUrl")]
    public string? SubtitleFileUrl { get; set; }

    /// <summary>
    /// 僅音訊
    /// </summary>
    [JsonPropertyName("isAudioOnly")]
    public bool IsAudioOnly { get; set; } = false;

    /// <summary>
    /// 轉換成字串
    /// </summary>
    /// <returns>字串</returns>
    public override string ToString() => JsonSerializer.Serialize(this);
}