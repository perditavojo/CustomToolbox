using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Game;

/// <summary>
/// 遊戲 -> 單機遊戲 (stand_alone)
/// </summary>
public class Tag17
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}