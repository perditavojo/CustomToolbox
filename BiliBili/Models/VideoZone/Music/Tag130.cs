using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Music;

/// <summary>
/// 音樂 -> 音樂綜合 (other)
/// </summary>
public class Tag130
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}