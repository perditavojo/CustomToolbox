using Application = System.Windows.Application;

namespace CustomToolbox.Common.Sets;

/// <summary>
/// 訊息組
/// </summary>
internal class MsgSet
{
    #region 應用程式

    /// <summary>
    /// 應用程式：應用程式名稱
    /// </summary>
    public static readonly string AppName = Application.Current
        .FindResource("AppName").ToString() ??
        string.Empty;

    /// <summary>
    /// 應用程式：應用程式關於
    /// </summary>
    public static readonly string AppAbout = Application.Current
        .FindResource("AppAbout").ToString() ??
        string.Empty;

    /// <summary>
    /// 應用程式：應用程式簡述
    /// </summary>
    public static readonly string AppDescription = Application.Current
        .FindResource("AppDescription").ToString() ??
        string.Empty;

    #endregion

    #region 字詞

    /// <summary>
    /// 字詞：省略號
    /// </summary>
    public static readonly string Ellipses = Application.Current
        .FindResource("Ellipses").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：關於
    /// </summary>
    public static readonly string About = Application.Current
        .FindResource("About").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：（無）
    /// </summary>
    public static readonly string ClipTitle = Application.Current
        .FindResource("ClipTitle").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：短片名稱
    /// </summary>
    public static readonly string ClipTitleToolTip = Application.Current
        .FindResource("ClipTitleToolTip").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：隱藏
    /// </summary>
    public static readonly string Hide = Application.Current
        .FindResource("Hide").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：顯示
    /// </summary>
    public static readonly string Show = Application.Current
        .FindResource("Show").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：靜音
    /// </summary>
    public static readonly string Mute = Application.Current
        .FindResource("Mute").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：取消靜音
    /// </summary>
    public static readonly string Unmute = Application.Current
        .FindResource("Unmute").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：啟用
    /// </summary>
    public static readonly string Enable = Application.Current
        .FindResource("Enable").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：停用
    /// </summary>
    public static readonly string Disable = Application.Current
        .FindResource("Disable").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：啟用
    /// </summary>
    public static readonly string Enabled = Application.Current
        .FindResource("Enabled").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：停用
    /// </summary>
    public static readonly string Disabled = Application.Current
        .FindResource("Disabled").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：暫停
    /// </summary>
    public static readonly string Pause = Application.Current
        .FindResource("Pause").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：恢復播放
    /// </summary>
    public static readonly string Resume = Application.Current
        .FindResource("Resume").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：請選擇
    /// </summary>
    public static readonly string SelectPlease = Application.Current
        .FindResource("SelectPlease").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：正在播放
    /// </summary>
    public static readonly string StatePlaying = Application.Current
        .FindResource("StatePlaying").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：暫停播放
    /// </summary>
    public static readonly string StatePause = Application.Current
        .FindResource("StatePause").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：停止播放
    /// </summary>
    public static readonly string StateStop = Application.Current
        .FindResource("StateStop").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：儲存並結束
    /// </summary>
    public static readonly string SaveAndExit = Application.Current
        .FindResource("SaveAndExit").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：直接結束
    /// </summary>
    public static readonly string ExitDirectly = Application.Current
        .FindResource("ExitDirectly").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：取消
    /// </summary>
    public static readonly string Cancel = Application.Current
        .FindResource("Cancel").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：頻道：
    /// </summary>
    public static readonly string VideoDataChannel = Application.Current
        .FindResource("VideoDataChannel").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：上傳者：
    /// </summary>
    public static readonly string VideoDataUploader = Application.Current
        .FindResource("VideoDataUploader").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：上傳時間：
    /// </summary>
    public static readonly string VideoDataUploadDate = Application.Current
        .FindResource("VideoDataUploadDate").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：影片 ID 值：
    /// </summary>
    public static readonly string VideoDataID = Application.Current
        .FindResource("VideoDataID").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：影片標題：
    /// </summary>
    public static readonly string VideoDataTitle = Application.Current
        .FindResource("VideoDataTitle").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：觀看次數：
    /// </summary>
    public static readonly string VideoDataViewCount = Application.Current
        .FindResource("VideoDataViewCount").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：描述：
    /// </summary>
    public static readonly string VideoDataDescription = Application.Current
        .FindResource("VideoDataDescription").ToString() ??
        string.Empty;

    /// <summary>
    /// 字詞：格式：
    /// </summary>
    public static readonly string VideoDataFormats = Application.Current
        .FindResource("VideoDataFormats").ToString() ??
        string.Empty;

    #endregion

    #region 字串模板

    /// <summary>
    /// 字串模板：[維護者：？]
    /// </summary>
    public static readonly string TemplateMaintainer = Application.Current
        .FindResource("TemplateMaintainer").ToString() ??
        string.Empty;

    /// <summary>
    /// 字串模板：[？][？（？）]：（？）？
    /// </summary>
    public static readonly string TemplateDiscordRichPresenceOnPresenceUpdate = Application.Current
        .FindResource("TemplateDiscordRichPresenceOnPresenceUpdate").ToString() ??
        string.Empty;

    /// <summary>
    /// 字串模板：[？][？]：？
    /// </summary>
    public static readonly string TemplateDiscordRichPresenceOnError = Application.Current
        .FindResource("TemplateDiscordRichPresenceOnError").ToString() ??
        string.Empty;

    /// <summary>
    /// 字串模板：[？][？]：？
    /// </summary>
    public static readonly string TemplateDiscordRichPresenceOnClose = Application.Current
        .FindResource("TemplateDiscordRichPresenceOnClose").ToString() ??
        string.Empty;

    /// <summary>
    /// 字串模板：[Xabe.FFmpeg][資料接收]：？
    /// </summary>
    public static readonly string TemplateXabeFFmpegOnDataReceived = Application.Current
        .FindResource("TemplateXabeFFmpegOnDataReceived").ToString() ??
        string.Empty;

    /// <summary>
    /// 字串模板：[Xabe.FFmpeg][進度]：處理識別：？ [？/？] ？%
    /// </summary>
    public static readonly string TemplateXabeFFmpegOnProgress = Application.Current
        .FindResource("TemplateXabeFFmpegOnProgress").ToString() ??
        string.Empty;

    /// <summary>
    /// 字串模板：[Xabe.FFmpeg] 作業完成。&#x0a;開始時間：？&#x0a;結束時間：？&#x0a;耗時：？&#x0a;使用的參數：？
    /// </summary>
    public static readonly string TemplateXabeFFmpegConversionResult = Application.Current
        .FindResource("TemplateXabeFFmpegConversionResult").ToString() ??
        string.Empty;

    #endregion

    #region 控制項

    /// <summary>
    /// 控制項：啟用不顯示影像
    /// </summary>
    public static readonly string MIEnableNoVideo = Application.Current
        .FindResource("MIEnableNoVideo").ToString() ??
        string.Empty;

    /// <summary>
    /// 控制項：停用不顯示影像
    /// </summary>
    public static readonly string MIDisableNoVideo = Application.Current
        .FindResource("MIDisableNoVideo").ToString() ??
        string.Empty;

    #endregion

    #region 對話視窗

    /// <summary>
    /// 對話視窗：選擇檔案
    /// </summary>
    public static readonly string LoadClipListSelectFile = Application.Current
        .FindResource("LoadClipListSelectFile").ToString() ??
        string.Empty;

    /// <summary>
    /// 對話視窗：JSON 檔案（*.json）|*.json|JSON 檔案（含備註）|*.jsonc|時間標記文字檔案（*.txt）|*.txt
    /// </summary>
    public static readonly string LoadClipListFilter = Application.Current
        .FindResource("LoadClipListFilter").ToString() ??
        string.Empty;

    /// <summary>
    /// 對話視窗：儲存檔案
    /// </summary>
    public static readonly string SaveClipListSelectFile = Application.Current
        .FindResource("SaveClipListSelectFile").ToString() ??
        string.Empty;

    /// <summary>
    /// 對話視窗：短片清單檔案（*.json）|*.json|時間標記播放清單檔案（*.json）|*.json|秒數播放清單檔案（*.json）|*.json|JSON 檔案（含備註）|*.jsonc|時間標記文字檔案（*.txt）|*.txt
    /// </summary>
    public static readonly string SaveClipListFilter = Application.Current
        .FindResource("SaveClipListFilter").ToString() ??
        string.Empty;

    /// <summary>
    /// 對話視窗：選擇路徑
    /// </summary>
    public static readonly string SelectProfilePath = Application.Current
        .FindResource("SelectProfilePath").ToString() ??
        string.Empty;

    /// <summary>
    /// 對話視窗：文字檔（*.txt）|.txt
    /// </summary>
    public static readonly string ExportLogFilter = Application.Current
        .FindResource("ExportLogFilter").ToString() ??
        string.Empty;

    /// <summary>
    /// 對話視窗：選擇視訊檔案
    /// </summary>
    public static readonly string SelectVideoFile = Application.Current
        .FindResource("SelectVideoFile").ToString() ??
        string.Empty;

    /// <summary>
    /// 對話視窗：MPEG-4 Part 14|*.mp4|Matroska|*.mkv
    /// </summary>
    public static readonly string SelectVideoFileFilter = Application.Current
        .FindResource("SelectVideoFileFilter").ToString() ??
        string.Empty;

    /// <summary>
    /// 對話視窗：選擇字幕檔案
    /// </summary>
    public static readonly string SelectSubtitleFile = Application.Current
        .FindResource("SelectSubtitleFile").ToString() ??
        string.Empty;

    /// <summary>
    /// 對話視窗：SupRip Text|*.srt|WebVTT|*.vtt|SubStation Alpha|*.ssa|Advanced SubStation Alpha|*.ass
    /// </summary>
    public static readonly string SelectSubtitleFileFilter = Application.Current
        .FindResource("SelectSubtitleFileFilter").ToString() ??
        string.Empty;

    /// <summary>
    /// 對話視窗：確認按鈕
    /// </summary>
    public static readonly string ContentDialogBtnOk = Application.Current
        .FindResource("ContentDialogBtnOk").ToString() ??
        string.Empty;

    /// <summary>
    /// 對話視窗：取消按鈕
    /// </summary>
    public static readonly string ContentDialogBtnCancel = Application.Current
        .FindResource("ContentDialogBtnCancel").ToString() ??
        string.Empty;

    #endregion

    #region 訊息

    /// <summary>
    /// 訊息：發生錯誤
    /// </summary>
    public static readonly string MsgErrorOccured = Application.Current
        .FindResource("MsgErrorOccured").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：開始下載
    /// </summary>
    public static readonly string MsgStartDownloading = Application.Current
        .FindResource("MsgStartDownloading").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已下載
    /// </summary>
    public static readonly string? MsgDownloaded = Application.Current
        .FindResource("MsgDownloaded").ToString();

    /// <summary>
    /// 訊息：已解壓縮
    /// </summary>
    public static readonly string MsgDecompressed = Application.Current
        .FindResource("MsgDecompressed").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已刪除
    /// </summary>
    public static readonly string MsgDeleted = Application.Current
        .FindResource("MsgDeleted").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已建立
    /// </summary>
    public static readonly string MsgCreated = Application.Current
        .FindResource("MsgCreated").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已編輯
    /// </summary>
    public static readonly string MsgEdited = Application.Current
        .FindResource("MsgEdited").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已更新
    /// </summary>
    public static readonly string MsgUpdated = Application.Current
        .FindResource("MsgUpdated").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已找到
    /// </summary>
    public static readonly string MsgFound = Application.Current
        .FindResource("MsgFound").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：雙擊以開關彈出視窗
    /// </summary>
    public static readonly string MsgDoubleClickToTogglePopupWindow = Application.Current
        .FindResource("MsgDoubleClickToTogglePopupWindow").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已取消下載檔案
    /// </summary>
    public static readonly string MsgDownloadCanceled = Application.Current
        .FindResource("MsgDownloadCanceled").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已取消下載
    /// </summary>
    public static readonly string MsgCancelFileDownload = Application.Current
        .FindResource("MsgCancelFileDownload").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：下載時發生例外，錯誤訊息
    /// </summary>
    public static readonly string DownloadError = Application.Current
        .FindResource("MsgDownloadError").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：檔案下載完成
    /// </summary>
    public static readonly string MsgDownloadFinished = Application.Current
        .FindResource("MsgDownloadFinished").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：您確定要強制重新下載相依性檔案嗎
    /// </summary>
    public static readonly string MsgReDownloadDeps = Application.Current
        .FindResource("MsgReDownloadDeps").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：無法儲存短片清單檔案，短片清單沒有任何資料
    /// </summary>
    public static readonly string MsgCanNotSaveClipList = Application.Current
        .FindResource("MsgCanNotSaveClipList").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：請選擇有效的短片清單檔案
    /// </summary>
    public static readonly string MsgSelectAValidClipListFile = Application.Current
        .FindResource("MsgSelectAValidClipListFile").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：不支援的網站
    /// </summary>
    public static readonly string MsgUnSupportedSite = Application.Current
        .FindResource("MsgUnSupportedSite").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：請先選擇要播放的短片
    /// </summary>
    public static readonly string MsgSelectTheClipToPlay = Application.Current
        .FindResource("MsgSelectTheClipToPlay").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：短片資料獲取成功
    /// </summary>
    public static readonly string MsgClipInfoFetchSuceed = Application.Current
        .FindResource("MsgClipInfoFetchSuceed").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：短片資料獲取失敗
    /// </summary>
    public static readonly string MsgClipInfoFetchFailed = Application.Current
        .FindResource("MsgClipInfoFetchFailed").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：請先選擇要獲取資訊的短片
    /// </summary>
    public static readonly string MsgSelectTheClipToFetchInfo = Application.Current
        .FindResource("MsgSelectTheClipToFetchInfo").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：請先選擇要下載的短片
    /// </summary>
    public static readonly string MsgSelectTheClipToDownload = Application.Current
        .FindResource("MsgSelectTheClipToDownload").ToString() ??
        string.Empty;

    /// <summary>
    /// 已清除日誌紀錄
    /// </summary>
    public static readonly string MsgLogCleared = Application.Current
        .FindResource("MsgLogCleared").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已匯出日誌紀錄至
    /// </summary>
    public static readonly string MsgExportLogTo = Application.Current
        .FindResource("MsgExportLogTo").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已於？檢查 yt-dlp 是否有新版本，故今日不會再自動檢查；如有需要，請手動執行更新
    /// </summary>
    public static readonly string MsgYtDlpNotice = Application.Current
        .FindResource("MsgYtDlpNotice").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：請手動點選 yt-dlp/FFmpeg-Builds 按鈕以更新 FFmpeg
    /// </summary>
    public static readonly string MsgUpdateFFmpeg = Application.Current
        .FindResource("MsgUpdateFFmpeg").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：10 秒後自動重新啟動應用程式
    /// </summary>
    public static readonly string MsgAutoRestartApplicationAfterTenSeconds = Application.Current
        .FindResource("MsgAutoRestartApplicationAfterTenSeconds").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：請重新啟動應用程式
    /// </summary>
    public static readonly string MsgRestartApplication = Application.Current
        .FindResource("MsgRestartApplication").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：播放失敗，無效的短片 ID 或是網址
    /// </summary>
    public static readonly string MsgInvalidVideoIDOrUrl = Application.Current
        .FindResource("MsgInvalidVideoIDOrUrl").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已暫停播放短片
    /// </summary>
    public static readonly string MsgClipPaused = Application.Current
        .FindResource("MsgClipPaused").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已恢復播放短片
    /// </summary>
    public static readonly string MsgClipResumed = Application.Current
        .FindResource("MsgClipResumed").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已靜音短片
    /// </summary>
    public static readonly string MsgClipMuted = Application.Current
        .FindResource("MsgClipMuted").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已取消靜音短片
    /// </summary>
    public static readonly string MsgClipUnmuted = Application.Current
        .FindResource("MsgClipUnmuted").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：正在播放短片
    /// </summary>
    public static readonly string MsgPlayingClip = Application.Current
        .FindResource("MsgPlayingClip").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：正在播放：？
    /// </summary>
    public static readonly string MsgNowPlaying = Application.Current
        .FindResource("MsgNowPlaying").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：隨機播放失敗，短片清單無資料
    /// </summary>
    public static readonly string MsgRandomPlayFailedClipListNoData = Application.Current
        .FindResource("MsgRandomPlayFailedClipListNoData").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已載入多媒體
    /// </summary>
    public static readonly string MsgMediaLoaded = Application.Current
        .FindResource("MsgMediaLoaded").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已結束播放多媒體
    /// </summary>
    public static readonly string MsgMediaFinished = Application.Current
        .FindResource("MsgMediaFinished").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已暫停播放多媒體
    /// </summary>
    public static readonly string MsgMediaPaused = Application.Current
        .FindResource("MsgMediaPaused").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已恢復播放多媒體
    /// </summary>
    public static readonly string MsgMediaResumed = Application.Current
        .FindResource("MsgMediaResumed").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已開始緩衝多媒體
    /// </summary>
    public static readonly string MsgMediaStartedBuffering = Application.Current
        .FindResource("MsgMediaStartedBuffering").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已結束緩衝多媒體
    /// </summary>
    public static readonly string MsgMediaEndedBuffering = Application.Current
        .FindResource("MsgMediaEndedBuffering").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已開始搜尋多媒體
    /// </summary>
    public static readonly string MsgMediaStartedSeeking = Application.Current
        .FindResource("MsgMediaStartedSeeking").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已結束搜尋多媒體
    /// </summary>
    public static readonly string MsgMediaEndedSeeking = Application.Current
        .FindResource("MsgMediaEndedSeeking").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：發生錯誤，多媒體播放失敗
    /// </summary>
    public static readonly string MsgMediaError = Application.Current
        .FindResource("MsgMediaError").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已卸載多媒體
    /// </summary>
    public static readonly string MsgMediaUnloaded = Application.Current
        .FindResource("MsgMediaUnloaded").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已啟用不顯示影像
    /// </summary>
    public static readonly string MsgNoVideoEnabled = Application.Current
        .FindResource("MsgNoVideoEnabled").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已停用不顯示影像
    /// </summary>
    public static readonly string MsgNoVideoDisabled = Application.Current
        .FindResource("MsgNoVideoDisabled").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已設定 YouTube 影片畫質，但在播放下一個短片時才會生效
    /// </summary>
    public static readonly string MsgYouTubeVideoQualityChanged = Application.Current
        .FindResource("MsgYouTubeVideoQualityChanged").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：播放失敗，短片清單無資料
    /// </summary>
    public static readonly string MsgPlayFailedClipListNoData = Application.Current
        .FindResource("MsgPlayFailedClipListNoData").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：無上一個短片，短片清單已播畢
    /// </summary>
    public static readonly string MsgNoPreviousClip = Application.Current
        .FindResource("MsgNoPreviousClip").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：無下一個短片，短片清單已播畢
    /// </summary>
    public static readonly string MsgNoNextClip = Application.Current
        .FindResource("MsgNoNextClip").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已停止播放短片
    /// </summary>
    public static readonly string MsgStoppedPlayingClip = Application.Current
        .FindResource("MsgStoppedPlayingClip").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：您確定要將應用程式切換使用此（？）語系嗎？變更語系會需要重新啟動應用程式，請確認您目前的作業皆已完成。如要繼續，請按「確定」按鈕
    /// </summary>
    public static readonly string MsgChangeLanguage = Application.Current
        .FindResource("MsgChangeLanguage").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：您一次只能選擇一個短片清單
    /// </summary>
    public static readonly string MsgSelectAClipListAtSameTime = Application.Current
        .FindResource("MsgSelectAClipListAtSameTime").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：請選擇有效的短片清單
    /// </summary>
    public static readonly string MsgSelectAValidClipList = Application.Current
        .FindResource("MsgSelectAValidClipList").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：SongId：？
    /// </summary>
    public static readonly string MsgSongID = Application.Current
        .FindResource("MsgSongID").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已找到 [？]「？」可用的 *.lrc 檔案，網址：？
    /// </summary>
    public static readonly string MsgFoundAvailableLrcFile = Application.Current
        .FindResource("MsgFoundAvailableLrcFile").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：開始秒數：？
    /// </summary>
    public static readonly string MsgStartSeconds = Application.Current
        .FindResource("MsgStartSeconds").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：*.lrc 檔案的偏移秒數：？
    /// </summary>
    public static readonly string MsgLrcFileOffsetSeconds = Application.Current
        .FindResource("MsgLrcFileOffsetSeconds").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：*.lrc 檔案的實際偏移秒數：？
    /// </summary>
    public static readonly string MsgLrcFileActualOffsetSeconds = Application.Current
        .FindResource("MsgLrcFileActualOffsetSeconds").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：[？]「？」已被手動禁用歌詞搜尋功能
    /// </summary>
    public static readonly string MsgManualDisabledLyricSearch = Application.Current
        .FindResource("MsgManualDisabledLyricSearch").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：[？]「？」歌曲搜尋失敗
    /// </summary>
    public static readonly string MsgSongFindingFailed = Application.Current
        .FindResource("MsgSongFindingFailed").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：[？]「？」有找到歌曲但歌詞搜尋失敗，記錄為 -SongId（？）
    /// </summary>
    public static readonly string MsgFoundSongButNoLyric = Application.Current
        .FindResource("MsgFoundSongButNoLyric").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：[？]「？」找不到對應的 *.lrc 資訊
    /// </summary>
    public static readonly string MsgCanNotFindLrcFileInfo = Application.Current
        .FindResource("MsgCanNotFindLrcFileInfo").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已更新不支援的域名
    /// </summary>
    public static readonly string MsgUpdateUnsupportedDomains = Application.Current
        .FindResource("MsgUpdateUnsupportedDomains").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已更新附加秒數
    /// </summary>
    public static readonly string MsgUpdateAppendSeconds = Application.Current
        .FindResource("MsgUpdateAppendSeconds").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：請輸入有效的數值
    /// </summary>
    public static readonly string MsgInputAValidNumber = Application.Current
        .FindResource("MsgInputAValidNumber").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：目標資料夾不存在
    /// </summary>
    public static readonly string MsgPathIsNotExists = Application.Current
        .FindResource("MsgPathIsNotExists").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：變更主題至？
    /// </summary>
    public static readonly string MsgSwitchTheme = Application.Current
        .FindResource("MsgSwitchTheme").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已更新使用者代理字串
    /// </summary>
    public static readonly string MsgUpdateUserAgent = Application.Current
        .FindResource("MsgUpdateUserAgent").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：您想將目前的短片清單，儲存至短片清單暫存檔案，並結束本應用程式嗎
    /// </summary>
    public static readonly string MsgConfirmExitApp = Application.Current
        .FindResource("MsgConfirmExitApp").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：[？]：已連接的管道：？
    /// </summary>
    public static readonly string MsgDiscordRichPresenceConnectedPipe = Application.Current
        .FindResource("MsgDiscordRichPresenceConnectedPipe").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：[？]：已失敗的管道：？
    /// </summary>
    public static readonly string MsgDiscordRichPresenceFailedPipe = Application.Current
        .FindResource("MsgDiscordRichPresenceFailedPipe").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：Discord 豐富狀態，連線失敗已達 ？ 次，已暫時關閉此功能
    /// </summary>
    public static readonly string MsgDiscordRichPresenceConnectionFailed = Application.Current
        .FindResource("MsgDiscordRichPresenceConnectionFailed").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：[？]：已接收到使用者 {？} 的準備完成
    /// </summary>
    public static readonly string MsgDiscordRichPresenceOnReady = Application.Current
        .FindResource("MsgDiscordRichPresenceOnReady").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已啟用 Discord 豐富狀態
    /// </summary>
    public static readonly string MsgEnableDiscordRichPresence = Application.Current
        .FindResource("MsgEnableDiscordRichPresence").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已停用 Discord 豐富狀態
    /// </summary>
    public static readonly string MsgDisableDiscordRichPresence = Application.Current
        .FindResource("MsgDisableDiscordRichPresence").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：您選擇的視訊檔案為不支援的格式
    /// </summary>
    public static readonly string MsgSelectedVideoNonSupported = Application.Current
        .FindResource("MsgSelectedVideoNonSupported").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：請輸入 Bilibili 使用者 mid
    /// </summary>
    public static readonly string MsgB23UserMidCantBeEmpty = Application.Current
        .FindResource("MsgB23UserMidCantBeEmpty").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：作業失敗，發生非預期的錯誤
    /// </summary>
    public static readonly string MsgJobFailedAndErrorOccurred = Application.Current
        .FindResource("MsgJobFailedAndErrorOccurred").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：資料解析失敗，沒有取到有效的標籤資訊。已取消產製 Bilibili 使用者（？）的短片清單檔案
    /// </summary>
    public static readonly string MsgDataParsingFailedAndCanceled = Application.Current
        .FindResource("MsgDataParsingFailedAndCanceled").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：正在準備產製 Bilibili 使用者（？）的短片清單檔案
    /// </summary>
    public static readonly string MsgPrepToProduceClipListFile = Application.Current
        .FindResource("MsgPrepToProduceClipListFile").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：正在處裡標籤「？（？）」的資料
    /// </summary>
    public static readonly string MsgProcessingDataForTag = Application.Current
        .FindResource("MsgProcessingDataForTag").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：正在處裡第 ？/？ 頁的資料
    /// </summary>
    public static readonly string MsgProcessingDataForPage = Application.Current
        .FindResource("MsgProcessingDataForPage").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：正在處裡第 ？/？ 部影片的資料
    /// </summary>
    public static readonly string MsgProcessingDataForVideo = Application.Current
        .FindResource("MsgProcessingDataForVideo").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：Bilibili 使用者（？）在此 tid（？）下共有 ？ 部影片，已成功處理 ？ 部影片的資料
    /// </summary>
    public static readonly string MsgProcessResult = Application.Current
        .FindResource("MsgProcessResult").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：資料解析失敗，無法產生 Bilibili 使用者（？）的短片清單檔案
    /// </summary>
    public static readonly string MsgDataParsingFailedAndCantCreateClipListFile = Application.Current
        .FindResource("MsgDataParsingFailedAndCantCreateClipListFile").ToString() ??
        string.Empty;

    /// <summary>
    /// 訊息：已產生 Bilibili 使用者（？）的短片清單檔案：？
    /// </summary>
    public static readonly string MsgClipListFileGeneratedFor = Application.Current
        .FindResource("MsgClipListFileGeneratedFor").ToString() ??
        string.Empty;

    #endregion

    /// <summary>
    /// 取得格式化的字串
    /// </summary>
    /// <param name="format">字串，格式</param>
    /// <param name="value">字串陣列，值</param>
    /// <returns>字串</returns>
    public static string GetFmtStr(
        string? format,
        params string[] value)
    {
        if (string.IsNullOrEmpty(format))
        {
            return string.Join(string.Empty, value);
        }

        return string.Format(format, value);
    }
}