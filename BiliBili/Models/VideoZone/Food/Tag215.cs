using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models.VideoZone.Food;

/// <summary>
/// 美食 -> 美食紀錄 (record)
/// </summary>
public class Tag215
{
    [JsonPropertyName("tid")]
    public int Tid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}