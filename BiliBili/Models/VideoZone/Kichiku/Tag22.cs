using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Kichiku;

/// <summary>
/// 鬼畜 -> 鬼畜調教 (guide)
/// </summary>
public class Tag22
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}