using CustomToolbox.Common.Sets;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CustomToolbox.Common.Models;

/// <summary>
/// 類別：應用程式語系資料
/// </summary>
public class AppLangData
{
    [JsonPropertyName("langDatas")]
    [Description("語系資料")]
    public List<LangData>? LangDatas { get; set; }

    [JsonPropertyName("errMsg")]
    [Description("錯誤訊息")]
    public string? ErrMsg { get; set; }

    /// <summary>
    /// 轉換成字串
    /// </summary>
    /// <returns>字串</returns>
    public override string ToString() => JsonSerializer.Serialize(this, VariableSet.SharedJSOptions);
}