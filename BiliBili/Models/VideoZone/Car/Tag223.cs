using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Car;

/// <summary>
/// 汽車（主分區）(car)
/// </summary>
public class Tag223
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}