using System.Text.Json.Serialization;

namespace CustomToolbox.BilibiliApi.Models.VideoZone.Life;

/// <summary>
/// 生活 -> 親子 (parenting)
/// </summary>
public class Tag254
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}