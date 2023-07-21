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

- [Bilibili API](BilibiliApi/README.md)
- [更新日誌](CHANGELOG.md)
- [使用手冊](MANUAL.md)

## 三、相依性檔案

- [aria2/aria2](https://github.com/aria2/aria2)
- [libmpv](https://sourceforge.net/projects/mpv-player-windows/files/libmpv/)
  - 限定：mpv-dev-x86_64-20211212-git-0e76372.7z
- [sub_charenc_parameters.txt](https://trac.ffmpeg.org/attachment/ticket/2431/sub_charenc_parameters.txt)
- [yt-dlp/FFmpeg-Builds](https://github.com/yt-dlp/FFmpeg-Builds)
- [yt-dlp/yt-dlp](https://github.com/yt-dlp/yt-dlp)
- [ytdl_hook.lua](https://github.com/mpv-player/mpv/blob/master/player/lua/ytdl_hook.lua)

## 四、網路資源

- [rubujo/CustomPlaylist](https://github.com/rubujo/CustomPlaylist)
- [YoutubeClipPlaylist/Lyrics](https://github.com/YoutubeClipPlaylist/Lyrics)
- [YoutubeClipPlaylist/Playlists](https://github.com/YoutubeClipPlaylist/Playlists)

## 五、授權資訊

- [adoconnection/SevenZipExtractor](https://github.com/adoconnection/SevenZipExtractor)
   - Copyright (c) 2017 Alexander Selishchev
   - [MIT 授權條款](https://github.com/adoconnection/SevenZipExtractor/blob/master/LICENSE)
- [bezzad/Downloader](https://github.com/bezzad/Downloader)
   - Copyright (c) 2021 Behzad Khosravifar
   - [MIT 授權條款](https://github.com/bezzad/Downloader/blob/master/LICENSE)
- [Bluegrams/YoutubeDLSharp](https://github.com/Bluegrams/YoutubeDLSharp)
   - Copyright (c) 2020-2022, Bluegrams. All rights reserved.
   - [BSD 三句版授權條款](https://github.com/Bluegrams/YoutubeDLSharp/blob/master/LICENSE.txt)
- [chris84948/AirspaceFixer](https://github.com/chris84948/AirspaceFixer)
   - Copyright (c) 2017 chris84948
   - [MIT 授權條款](https://github.com/chris84948/AirspaceFixer/blob/master/LICENSE)
- [CosineG/OpenCC.NET](https://github.com/CosineG/OpenCC.NET)
   - [Apache 授權條款版本 2](https://github.com/CosineG/OpenCC.NET/blob/master/LICENSE)
- [HavenDV/H.NotifyIcon](https://github.com/HavenDV/H.NotifyIcon)
   - Copyright (c) 2020 havendv
   - [MIT 授權條款](https://github.com/HavenDV/H.NotifyIcon/blob/master/LICENSE.md)
- [hudec117/Mpv.NET-lib-](https://github.com/hudec117/Mpv.NET-lib-)
   - Copyright (c) 2019 Aurel Hudec Jr
   - [MIT 授權條款](https://github.com/hudec117/Mpv.NET-lib-/blob/master/LICENSE)
- [Humanizr/Humanizer](https://github.com/Humanizr/Humanizer)
   - Copyright (c) .NET Foundation and Contributors
   - [MIT 授權條款](https://github.com/Humanizr/Humanizer/blob/main/LICENSE)
- [Kinnara/ModernWpf](https://github.com/Kinnara/ModernWpf)
   - Copyright (c) 2019 Yimeng Wu
   - [MIT 授權條款](https://github.com/Kinnara/ModernWpf/blob/master/LICENSE)
- [Lachee/discord-rpc-csharp](https://github.com/Lachee/discord-rpc-csharp)
   - Copyright (c) 2021 Lachee
   - [MIT 授權條款](https://github.com/Lachee/discord-rpc-csharp/blob/master/LICENSE)
- [microsoft/playwright-dotnet](https://github.com/microsoft/playwright-dotnet)
   - Copyright (c) 2020 Darío Kondratiuk
   - [MIT 授權條款](https://github.com/microsoft/playwright-dotnet/blob/main/LICENSE)
- [minhhungit/ConsoleTableExt](https://github.com/minhhungit/ConsoleTableExt)
   - Copyright (c) 2017 Jin
   - [MIT 授權條款](https://github.com/minhhungit/ConsoleTableExt/blob/master/LICENSE)
- [tomaszzmuda/Xabe.FFmpeg](https://github.com/tomaszzmuda/Xabe.FFmpeg)
   - [授權合約](https://ffmpeg.xabe.net/license.html)
- [zzzprojects/html-agility-pack](https://github.com/zzzprojects/html-agility-pack)
   - [MIT 授權條款](https://github.com/zzzprojects/html-agility-pack/blob/master/LICENSE)
- [FFmpeg](https://ffmpeg.org/)
   - [FFmpeg 授權條款和法律方面的注意事項](https://ffmpeg.org/legal.html)
- [ggerganov/whisper.cpp](https://github.com/ggerganov/whisper.cpp)
   - Copyright (c) 2023  Georgi Gerganov
   - [MIT 授權條款](https://github.com/ggerganov/whisper.cpp/blob/master/LICENSE)
- [sandrohanea/whisper.net](https://github.com/sandrohanea/whisper.net)
   - Copyright (c) 2023 sandrohanea
   - [MIT 授權條款](https://github.com/sandrohanea/whisper.net/blob/main/LICENSE)

因 [Xabe.FFmpeg](https://github.com/tomaszzmuda/Xabe.FFmpeg) 函式庫[授權合約](https://ffmpeg.xabe.net/license.html)的限制，此 GitHub 倉庫內，`沒有標註來源`的內容，皆採用 [CC BY-NC-SA 3.0](https://creativecommons.org/licenses/by-nc-sa/3.0/) 授權條款釋出，反之皆以其來源之授權條款為準。

## 六、注意事項

1. 在發佈時請勿勾選`啟用 ReadyToRun 編譯`選項，這會造成應用程式在啟動時崩潰。
2. 請在發布後將 `Microsoft.Playwright.dll.bak` 重新命名成 `Microsoft.Playwright.dll`。