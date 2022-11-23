using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Cinephile;

/// <summary>
/// 影視 -> 小劇場 (shortfilm)
/// </summary>
public class Tag85
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}