# 自定義工具箱

自定義工具箱是一款整合數個功能的整合型應用程式。

## 注意事項

1. 在發佈時請勿勾選`啟用 ReadyToRun 編譯`選項，這會造成應用程式在啟動時崩潰。
2. 在發佈時若有勾選`產生單一檔案`選項時，請在發佈完成後，手動將檔案 `Microsoft.Playwright.dll.bak` 重新命名成 `Microsoft.Playwright.dll`，以免 Playwright 會不能正常運作。 