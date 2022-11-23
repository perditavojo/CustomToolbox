using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Anime;

/// <summary>
/// 番劇（主分區）(anime)
/// </summary>
public class Tag13
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}