namespace CustomToolbox.Common.Models.UpdateNotifier;

/// <summary>
/// 檢查結果
/// </summary>
internal class CheckResult
{
    /// <summary>
    /// 訊息文字
    /// </summary>
    public string? MessageText { get; set; }

    /// <summary>
    /// 版本號文字
    /// </summary>
    public string? VersionText { get; set; }

    /// <summary>
    /// 下載網址
    /// </summary>
    public string? DownloadUrl { get; set; }

    /// <summary>
    /// 是否有新版本
    /// </summary>
    public bool HasNewVersion { get; set; }

    /// <summary>
    /// 是否為例外
    /// </summary>
    public bool IsException { get; set; }

    /// <summary>
    /// 網路版本是否比本基版本還要舊
    /// </summary>
    public bool NetVersionIsOdler { get; set; }

    /// <summary>
    /// 校驗碼
    /// </summary>
    public string? Checksum { get; set; }
}