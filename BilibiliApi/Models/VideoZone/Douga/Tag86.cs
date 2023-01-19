using System.Text.Json.Serialization;

namespace CustomToolbox.BilibiliApi.Models.VideoZone.Douga;

/// <summary>
/// 動畫 -> 特攝 (tokusatsu)
/// </summary>
public class Tag86
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}