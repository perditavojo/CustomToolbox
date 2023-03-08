# 自定義工具箱更新日誌

- 1.0.9
  1. 更新 H.NotifyIcon.Wpf 函式庫至 2.0.93 版。
  2. 更新 Microsoft.Playwright 函式庫至 1.31.1 版。
  3. 更新 YoutubeDLSharp 函式庫至 1.0.0-beta4 版。
  4. 更新 MANUAL.md。
  5. 更新語系檔案。
  6. 對 " Bilibili 短片清單產生器" 功能，加入"檢查網址"選項。
  7. 更新 BilibiliApi\README.md 的內容。
  8. 更新 BiliBili API 的網址。
  9. 改寫 yt-dlp 的下載方式。
  10. 調整 *.csproj 的設定，在發布時輸出 CHANGELOG.md 以及 MANUAL.md 等檔案。
- 1.0.8
  1. 針對 DataGrid 的排序機制，調整播放時的選取播放項目的策略。
  2. 更新 Microsoft.Playwright 函式庫至 1.31.0 版。
  3. 調整 Client Hints 相關設定方式。
  4. 加入更新應用程式設定值的功能。
  5. 針對隨機重新排序加入依照 DataGrid 的排序機制排序的功能。
- v1.0.7
  1. 修改 mid 的類型，將類型從 int 改為 long，因為目前的 mid 已經會超過 int 的範圍，從而造成 JSON 資料解析失敗。
  2. 更新 Xabe.FFmpeg 至 5.2.5 版。
- v1.0.6
  1. 更新語系檔案，並修正尚未 l10n 化的內容。
  2. 修正在應用程式切換主題時，系統工具列圖示的右鍵選單的主題，不會同步更新的問題。
  3. 將部分 Button 控制項，改為使用 AppBarButton 控制項。
  4. 針對部分按鈕以及選單選項，加入圖示。
  5. 加入時間標記編輯器相關功能。
  6. 更新預設的使用者代理字串。 (Google Chrome)
  7. 更新預設的 Sec-CH-UA 相關標頭資料。 (Google Chrome)
- v1.0.5
  1. 更新函式庫 H.NotifyIcon.Wpf 至 2.0.86 版。
  2. 重新調整 SSeek 控制項的事件 SSeek_KeyUpEvent() 的內部程式判定邏輯。
  3. 修正部分影片透過拖曳時間軸至結束時間點會造成應用程式崩潰的問題。
  4. 更新語系檔案。
- v1.0.4
  1. 修正無法使用方向鍵控制時間軸的問題。
  2. 針對 YouTube 的網址加入判斷，讓 YouTube 的網址可以吃 YouTube 畫質設定。
  3. 修正 libmpv 在 yt-dlp 發生錯誤後接下來的播放會沒有反應的問題。
  4. 更新第三方函示庫 Downloader 至 v3.0.3 版。
  5. 相關文件更新。
- v1.0.3
  - 修正匯出時間標記文字檔案時網址資訊會出錯的問題。 
- v1.0.2
  1. 加入批次下載短片功能。
  2. 調整短片工具頁籤內容的排版，針對 FFmpeg 相關功能的部分加入 GroupBox 控制項以區分。
  3. 更新第三方函式庫 Xabe.FFmpeg 至 v5.2.4 版。
  4. 更新第三方函式庫 Microsoft.Playwright 至 v1.30.0 版。
  5. 針對 yt-dlp 的下載結果補上對應的輸出訊息。
  6. 更新語系檔案。
  7. 更新 README.md。
- v1.0.1
  - 修正呼叫 SetRichPresence() 方法會發生例外的問題。 
- v1.0.0
  - 初始發布。