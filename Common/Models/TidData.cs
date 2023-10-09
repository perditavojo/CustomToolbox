using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using VariableSet = CustomToolbox.Common.Sets.VariableSet;

namespace CustomToolbox.Common.Models;

/// <summary>
/// 類別：tid 資料
/// </summary>
public class TidData
{
    [JsonPropertyName("tid")]
    [Description("tid")]
    public int TID { get; set; }

    [JsonPropertyName("name")]
    [Description("名稱")]
    public string? Name { get; set; }

    /// <summary>
    /// 轉換成字串
    /// </summary>
    /// <returns>字串</returns>
    public override string ToString() => JsonSerializer.Serialize(this, VariableSet.SharedJSOptions);
}