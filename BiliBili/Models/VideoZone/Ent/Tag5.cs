using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Ent;

/// <summary>
/// 娛樂（主分區）(ent)
/// </summary>
public class Tag5
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}