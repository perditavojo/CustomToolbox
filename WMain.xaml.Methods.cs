using CustomToolbox.Common.Sets;
using Downloader;
using SevenZipExtractor;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using YoutubeDLSharp;

namespace CustomToolbox;

public partial class WMain
{
    /// <summary>
    /// 自定義初始化
    /// </summary>
    private void CustomInit()
    {
        try
        {
            InitControls();
            CheckDependencyFiles();
        }
        catch (Exception ex)
        {
            WriteLog($"發生錯誤：{ex}");
        }
    }

    /// <summary>
    /// 初始化控制項
    /// </summary>
    private void InitControls()
    {
        try
        {
            Title = "自定義工具箱";

            DGClipList.ItemsSource = ClipDatas;

            Version? version = Assembly.GetExecutingAssembly().GetName().Version;

            if (version != null)
            {
                LVersion.Content = version.ToString();
            }
        }
        catch (Exception ex)
        {
            WriteLog($"發生錯誤：{ex}");
        }
    }

    /// <summary>
    /// 檢查相依性檔案
    /// </summary>
    private async void CheckDependencyFiles()
    {
        try
        {
            #region 檢查資料夾

            string[] folders =
            {
                VariableSet.DepsFolderPath,
                VariableSet.DownloadsFolderPath,
                VariableSet.PlaylistsFolderPath
            };

            foreach (string folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);

                    WriteLog($"已建立 {folder}");
                }
            }

            #endregion

            #region 檢查 yt-dlp

            if (!File.Exists(VariableSet.YtDlpPath))
            {
                WriteLog($"開始下載 {VariableSet.YtDlpExecName}。");

                await YoutubeDL.DownloadYtDlpBinary(VariableSet.DepsFolderPath)
                    .ContinueWith(task => WriteLog($"已下載 {VariableSet.YtDlpExecName}。"));
            }
            else
            {
                WriteLog($"已找到 {VariableSet.YtDlpExecName}");
            }

            await Dispatcher.BeginInvoke(new Action(() =>
            {
                // TODO: 2022-11-22 待修改。
                YoutubeDL ytdl = new()
                {
                    FFmpegPath = VariableSet.FFmpegPath,
                    YoutubeDLPath = VariableSet.YtDlpPath
                };

                LYtDlpVersion.Content = ytdl.Version;
            }));

            #endregion

            #region 檢查 FFmpeg

            if (!File.Exists(VariableSet.FFmpegPath) ||
                !File.Exists(VariableSet.FFprobePath))
            {
                string path = Path.Combine(
                    VariableSet.DepsFolderPath,
                    UrlSet.FFmpegArchiveFileName);

                DownloadService downloadService = GetDownloadService(new Action(() =>
                {
                    if (File.Exists(path))
                    {
                        using ArchiveFile archiveFile = new(path);

                        IList<Entry> entries = archiveFile.Entries
                            .Where(n => !n.IsFolder &&
                                (n.FileName.Contains(VariableSet.FFmpegExecName) ||
                                n.FileName.Contains(VariableSet.FFprobeExecName))).ToList();

                        foreach (Entry entry in entries)
                        {
                            string fileName = Path.GetFileName(entry.FileName);
                            string targetPath = Path.Combine(VariableSet.DepsFolderPath, fileName);

                            entry.Extract(targetPath);

                            WriteLog($"已解壓縮 {fileName}。");
                        }

                        Task.Delay(VariableSet.WaitForDeleteMilliseconds)
                            .ContinueWith(task => File.Delete(path))
                            .ContinueWith(task => WriteLog($"已刪除 {path}"));
                    }
                }));

                await downloadService.DownloadFileTaskAsync(UrlSet.FFmpegUrl, path);
            }
            else
            {
                WriteLog($"已找到 {VariableSet.FFmpegExecName}、{VariableSet.FFprobeExecName}。");
            }

            #endregion

            #region 檢查 libmpv

            if (!File.Exists(VariableSet.LibMpvPath))
            {
                string path = Path.Combine(
                    VariableSet.DepsFolderPath,
                    UrlSet.LibMpvArchiveFileName);

                DownloadService downloadService = GetDownloadService(new Action(() =>
                {
                    if (File.Exists(path))
                    {
                        using ArchiveFile archiveFile = new(path);

                        IList<Entry> entries = archiveFile.Entries
                            .Where(n => !n.IsFolder &&
                                n.FileName.Contains(VariableSet.LibMpvDllFileName)).ToList();

                        foreach (Entry entry in entries)
                        {
                            string fileName = Path.GetFileName(entry.FileName);
                            string targetPath = Path.Combine(VariableSet.DepsFolderPath, fileName);

                            entry.Extract(targetPath);

                            WriteLog($"已解壓縮 {fileName}。");
                        }

                        Task.Delay(VariableSet.WaitForDeleteMilliseconds)
                            .ContinueWith(task => File.Delete(path))
                            .ContinueWith(task => WriteLog($"已刪除 {path}"));
                    }
                }));

                await downloadService.DownloadFileTaskAsync(UrlSet.LibMpvUrl, path);
            }
            else
            {
                WriteLog($"已找到 {VariableSet.LibMpvDllFileName}。");
            }

            #endregion

            #region 檢查 ytdl_hook.lua

            if (!File.Exists(VariableSet.YtDlHookLuaPath))
            {
                DownloadService downloadService = GetDownloadService(new Action(() =>
                {
                    if (File.Exists(VariableSet.YtDlHookLuaPath))
                    {
                        // 修改「ytdl_path = "",」。
                        string[] lines = File.ReadAllLines(VariableSet.YtDlHookLuaPath);

                        for (int i = 0; i < lines.Length; i++)
                        {
                            string line = lines[i];

                            // 判斷是否為目標行。
                            if (line.StartsWith("    ytdl_path = \"\","))
                            {
                                // 替換內容。
                                line = $"    ytdl_path = \"Deps\\\\yt-dlp\",";

                                // 回寫至陣列。
                                lines[i] = line;
                            }
                        }

                        // 將陣列輸出至指定檔案內。
                        File.WriteAllText(
                            VariableSet.YtDlHookLuaPath,
                            string.Join(Environment.NewLine, lines));

                        WriteLog($"已修改 {VariableSet.YtDlHookLuaFileName}。");
                    }
                }));

                await downloadService.DownloadFileTaskAsync(
                    UrlSet.YtDlHookLuaUrl,
                    VariableSet.YtDlHookLuaPath);
            }
            else
            {
                WriteLog($"已找到 {VariableSet.YtDlHookLuaFileName}。");
            }

            #endregion

            // 初始化 MpvPlayer。
            InitMpvPlayer(PlayerHost.Handle);
        }
        catch (Exception ex)
        {
            WriteLog($"發生錯誤：{ex}");
        }
    }

    /// <summary>
    /// 取得 DownloadService
    /// </summary>
    /// <param name="action">Action</param>
    /// <returns>DownloadService</returns>
    private DownloadService GetDownloadService(Action action)
    {
        // 供手動減速使用。
        int previousPercentageValue = 0;

        string fileName = string.Empty;

        DownloadService downloadService = new(new DownloadConfiguration());

        downloadService.DownloadStarted += (object? sender, DownloadStartedEventArgs e) =>
        {
            fileName = Path.GetFileName(e.FileName);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                PBProgress.Maximum = 100;
            }));

            WriteLog($"開始下載 {fileName}。");
        };

        downloadService.DownloadProgressChanged += (object? sender, DownloadProgressChangedEventArgs e) =>
        {
            int currentPercentageValue = (int)e.ProgressPercentage;

            // 減速更新 UI 的頻率，以免 UI 卡死。
            if (currentPercentageValue > previousPercentageValue)
            {
                // 更新 UI。
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    PBProgress.Value = currentPercentageValue;

                    LOperation.Content = $"[{e.ProgressId}] " +
                        $"{e.ReceivedBytesSize}/{e.TotalBytesToReceive} bytes " +
                        $"({currentPercentageValue}%) {(int)e.BytesPerSecondSpeed} bytes/s";
                }));
            }

            previousPercentageValue = currentPercentageValue;
        };

        downloadService.DownloadFileCompleted += (object? sender, AsyncCompletedEventArgs e) =>
        {
            // 重設 previousPercentageValue。
            previousPercentageValue = 0;

            // 重設 UI。
            Dispatcher.BeginInvoke(new Action(() =>
            {
                PBProgress.Value = 0;

                LOperation.Content = string.Empty;
            }));

            if (e.Cancelled)
            {

                if (string.IsNullOrEmpty(fileName))
                {
                    WriteLog("已取消下載檔案。");
                }
                else
                {
                    WriteLog($"已取消下載 {fileName}。");
                }
            }
            else if (e.Error != null)
            {
                WriteLog($"下載時發生例外，錯誤訊息：{e.Error.Message}");
            }
            else
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    WriteLog("檔案下載完成。");
                }
                else
                {
                    WriteLog($"已下載 {fileName}。");
                }

                action();
            }

            // 重設 fileName。
            fileName = string.Empty;
        };

        return downloadService;
    }

    /// <summary>
    /// 寫紀錄
    /// </summary>
    /// <param name="message">字串，訊息</param>
    private void WriteLog(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        Dispatcher.BeginInvoke(new Action(() =>
        {
            try
            {
                string newMessage = $"[{DateTime.Now}] {message}{Environment.NewLine}";

                TBLog.AppendText(newMessage);
                TBLog.CaretIndex = TBLog.Text.Length;
                TBLog.ScrollToEnd();
                TBLog.Focus();
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show(
                    Title,
                    ex.ToString(),
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }));
    }
}