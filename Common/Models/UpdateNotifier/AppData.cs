using System.Text.Json.Serialization;

namespace CustomToolbox.Common.Models.UpdateNotifier;

/// <summary>
/// 應用程式資料
/// </summary>
internal class AppData
{
    /// <summary>
    /// 應用程式
    /// </summary>
    [JsonPropertyName("app")]
    public string? App { get; set; }

    /// <summary>
    /// 應用程式名稱
    /// </summary>
    [JsonPropertyName("appName")]
    public string? AppName { get; set; }

    /// <summary>
    /// 應用程式版本號
    /// </summary>
    [JsonPropertyName("appVersion")]
    public string? AppVersion { get; set; }

    /// <summary>
    /// 建置日期
    /// </summary>
    [JsonPropertyName("buildDate")]
    public string? BuildDate { get; set; }

    /// <summary>
    /// 校驗碼
    /// </summary>
    [JsonPropertyName("checksum")]
    public string? Checksum { get; set; }

    /// <summary>
    /// 下載網址
    /// </summary>
    [JsonPropertyName("downloadUrl")]
    public string? DownloadUrl { get; set; }
}