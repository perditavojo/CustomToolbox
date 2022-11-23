using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.TV;

/// <summary>
/// 電視劇 -> 國產劇 (mainland)
/// </summary>
public class Tag185
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}