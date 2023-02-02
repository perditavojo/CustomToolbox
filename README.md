# 自定義工具箱

自定義工具箱是一款整合數個功能的整合型應用程式。

## 一、功能

- [短片播放器](MANUAL.md#一短片播放器)
  - 透過 libmpv 以及 yt-dlp 來支援數個影音網站的影片播放。
- [網路資源](MANUAL.md#二網路資源)
- [下載短片](MANUAL.md#三下載短片)
  - 透過 yt-dlp 以及 FFmpeg 或 aria2 來下載影片。
- [陽春字幕產生器](MANUAL.md#四陽春字幕產生器)
  - 製作 SubRip Text 或 WebVTT 格式的字幕檔案。
- [YouTube 影片秒數轉換器](MANUAL.md#五youtube-影片秒數轉換器)
  - 將秒數與時間標記互相轉換的轉換器。
- [燒錄字幕檔](MANUAL.md#六燒錄字幕檔)
  - 透過 FFmpeg 將字幕檔案燒錄至視訊檔案。
- [分割影片](MANUAL.md#七分割影片)
  - 透過 FFmpeg 將指定的視訊檔案分割成數個短片。
- [YouTube 訂閱者計數器工具](MANUAL.md#八youtube-訂閱者計數器工具)
  - 透過 Playwright 來操作模擬人工瀏覽 [YouTube Subscriber Counter](https://subscribercounter.com) 網站，並將網站內容拍攝成截圖。

## 二、文件

- [使用手冊](MANUAL.md)

## 三、注意事項

1. 在發佈時請勿勾選`產生單一檔案`選項，這會造成 H.NotifyIcon.Wpf 函式庫運作不正常。
2. 在發佈時請勿勾選`啟用 ReadyToRun 編譯`選項，這會造成應用程式在啟動時崩潰。