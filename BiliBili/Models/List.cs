using System.Text.Json.Serialization;

namespace CustomToolbox.Bilibili.Models;

/// <summary>
/// List
/// </summary>
public class List
{
    [JsonPropertyName("tlist")]
    public TList? TList { get; set; }

    [JsonPropertyName("vlist")]
    public List<VList>? VList { get; set; }
}