using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Life;

/// <summary>
/// 生活 -> 繪畫 (painting)
/// </summary>
public class Tag162
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}