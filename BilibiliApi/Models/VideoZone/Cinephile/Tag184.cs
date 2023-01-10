using System.Text.Json.Serialization;

namespace CustomToolbox.BilibiliApi.Models.VideoZone.Cinephile;

/// <summary>
/// 影視 -> 預告、資訊 (trailer_info)
/// </summary>
public class Tag184
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}