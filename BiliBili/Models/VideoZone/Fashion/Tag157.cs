using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Fashion;

/// <summary>
/// 時尚 -> 美妝護膚 (makeup)
/// </summary>
public class Tag157
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}