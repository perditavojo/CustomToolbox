using CustomToolbox.Common.Sets;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CustomToolbox.Common.Models.ImportPlaylist;

/// <summary>
/// 類別：秒數歌曲資料
/// </summary>
public class SecondsSongData
{
    [JsonPropertyName("videoID")]
    [Description("影片 ID")]
    public string? VideoID { get; set; }

    [JsonPropertyName("name")]
    [Description("名稱")]
    public string? Name { get; set; }

    [JsonPropertyName("startSeconds")]
    [Description("開始秒數")]
    public double? StartSeconds { get; set; } = 0;

    [JsonPropertyName("endSeconds")]
    [Description("結束秒數")]
    public double? EndSeconds { get; set; } = 0;

    [JsonPropertyName("subSrc")]
    [Description("字幕檔案來源")]
    public string? SubSrc { get; set; }

    /// <summary>
    /// 轉換成字串
    /// </summary>
    /// <returns>字串</returns>
    public override string ToString() => JsonSerializer.Serialize(this, VariableSet.SharedJSOptions);
}