using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Information;

/// <summary>
/// 資訊 -> 社會 (social)
/// </summary>
public class Tag205
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}