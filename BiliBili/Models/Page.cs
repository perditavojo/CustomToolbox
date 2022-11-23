using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models;

/// <summary>
/// Page
/// </summary>
public class Page
{
    [JsonPropertyName("pn")]
    public int Pn { get; set; }

    [JsonPropertyName("ps")]
    public int Ps { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }
}