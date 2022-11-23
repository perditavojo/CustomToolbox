using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Animal;

/// <summary>
/// 動物圈 -> 汪星人 (dog)
/// </summary>
public class Tag219
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}