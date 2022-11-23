using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Music;

/// <summary>
/// 音樂 -> MV (mv)
/// </summary>
public class Tag193
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}