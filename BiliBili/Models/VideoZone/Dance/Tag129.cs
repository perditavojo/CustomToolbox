using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Dance;

/// <summary>
/// 舞蹈（主分區）(dance)
/// </summary>
public class Tag129
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}