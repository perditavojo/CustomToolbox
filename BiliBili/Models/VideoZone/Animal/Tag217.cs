using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Animal;

/// <summary>
/// 動物圈（主分區）(animal)
/// </summary>
public class Tag217
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}