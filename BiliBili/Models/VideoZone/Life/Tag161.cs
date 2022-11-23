using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Life;

/// <summary>
/// 生活 -> 手工 (handmake)
/// </summary>
public class Tag161
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}