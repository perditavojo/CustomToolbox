using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Knowledge;

/// <summary>
/// 知識 -> 人文歷史 (humanity_history)
/// </summary>
public class Tag228
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}