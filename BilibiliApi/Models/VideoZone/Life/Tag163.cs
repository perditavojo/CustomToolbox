using System.Text.Json.Serialization;

namespace CustomToolbox.BilibiliApi.Models.VideoZone.Life;

/// <summary>
/// 生活 -> 運動 (sports)
/// </summary>
public class Tag163
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}