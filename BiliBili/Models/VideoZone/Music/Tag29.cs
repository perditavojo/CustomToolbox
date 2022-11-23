using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Music;

/// <summary>
/// 音樂 -> 音樂現場 (live)
/// </summary>
public class Tag29
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}