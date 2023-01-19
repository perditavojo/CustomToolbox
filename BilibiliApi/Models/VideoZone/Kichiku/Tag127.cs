using System.Text.Json.Serialization;

namespace CustomToolbox.BilibiliApi.Models.VideoZone.Kichiku;

/// <summary>
/// 鬼畜 -> 教程演示 (course)
/// </summary>
public class Tag127
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}