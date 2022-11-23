using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Game;

/// <summary>
/// 遊戲 -> Mugen (mugen)
/// </summary>
public class Tag19
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}