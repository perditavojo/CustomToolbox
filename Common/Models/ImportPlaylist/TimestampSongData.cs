using CustomToolbox.Common.Sets;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CustomToolbox.Common.Models.ImportPlaylist;

/// <summary>
/// 類別：時間標記歌曲資料
/// </summary>
public class TimestampSongData
{
    [JsonPropertyName("videoID")]
    [Description("影片 ID")]
    public string? VideoID { get; set; }

    [JsonPropertyName("name")]
    [Description("名稱")]
    public string? Name { get; set; }

    [JsonPropertyName("startTime")]
    [Description("開始時間")]
    public TimeSpan? StartTime { get; set; }

    [JsonPropertyName("endTime")]
    [Description("結束時間")]
    public TimeSpan? EndTime { get; set; }

    [JsonPropertyName("subSrc")]
    [Description("字幕檔案來源")]
    public string? SubSrc { get; set; }

    /// <summary>
    /// 轉換成字串
    /// </summary>
    /// <returns>字串</returns>
    public override string ToString() => JsonSerializer.Serialize(this, VariableSet.SharedJSOptions);
}