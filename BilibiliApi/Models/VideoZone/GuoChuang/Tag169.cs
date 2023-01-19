using System.Text.Json.Serialization;

namespace CustomToolbox.BilibiliApi.Models.VideoZone.GuoChuang;

/// <summary>
/// 國創 -> 布袋戲 (puppetry)
/// </summary>
public class Tag169
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}