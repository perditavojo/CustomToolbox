using CustomToolbox.Common.Sets;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CustomToolbox.Common.Models;

/// <summary>
/// 類別：排序狀態
/// </summary>
public class SortState
{
    [JsonPropertyName("columnIndex")]
    [Description("欄位的索引值")]
    public int ColumnIndex { get; set; } = -1;

    [JsonPropertyName("sortDirection")]
    [Description("排序方向")]
    public ListSortDirection? SortDirection { get; set; }

    /// <summary>
    /// 轉換成字串
    /// </summary>
    /// <returns>字串</returns>
    public override string ToString() => JsonSerializer.Serialize(this, VariableSet.SharedJSOptions);
}