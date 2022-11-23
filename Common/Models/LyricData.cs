using CustomToolbox.Common.Sets;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CustomToolbox.Common.Models;

/// <summary>
/// 類別：歌詞資料
/// </summary>
internal class LyricData
{
    [JsonPropertyName("time")]
    [Description("時間")]
    public TimeSpan Time { get; set; }

    [JsonPropertyName("text")]
    [Description("歌詞")]
    public string? Text { get; set; }

    /// <summary>
    /// 轉換成字串
    /// </summary>
    /// <returns>字串</returns>
    public override string ToString() => JsonSerializer.Serialize(this, VariableSet.SharedJSOptions);
}