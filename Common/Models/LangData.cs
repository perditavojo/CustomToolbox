using CustomToolbox.Common.Sets;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace CustomToolbox.Common.Models;

/// <summary>
/// 類別：語系資料
/// </summary>
public class LangData
{
    [JsonPropertyName("langName")]
    [Description("語系名稱")]
    public string? LangName { get; set; }

    [JsonPropertyName("langCode")]
    [Description("語系代碼")]
    public string? LangCode { get; set; }

    [JsonPropertyName("resDict")]
    [Description("資源字典")]
    public ResourceDictionary? ResDict { get; set; }

    [JsonPropertyName("langFileName")]
    [Description("語系檔案名稱")]
    public string? LangFileName { get; set; }

    /// <summary>
    /// 轉換成字串
    /// </summary>
    /// <returns>字串</returns>
    public override string ToString() => JsonSerializer.Serialize(this, VariableSet.SharedJSOptions);
}