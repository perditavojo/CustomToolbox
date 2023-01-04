using Application = System.Windows.Application;
using Contorl = System.Windows.Controls.Control;
using CustomToolbox.Common.Sets;
using H.NotifyIcon;
using Mpv.NET.Player;
using System.Diagnostics;
using System.IO;
using System.IO.Enumeration;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using TextBox = System.Windows.Controls.TextBox;

namespace CustomToolbox.Common;

/// <summary>
/// 自定義函式
/// </summary>
internal class CustomFunction
{
    /// <summary>
    /// WMain
    /// </summary>
    private static WMain? _WMain = null;

    /// <summary>
    /// TBLog
    /// </summary>
    private static TextBox? _TBLog = null;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="wMain">WMain</param>
    public static void Init(WMain wMain)
    {
        _WMain = wMain;
        _TBLog = _WMain.TBLog;
    }

    /// <summary>
    /// 取得 YouTube 影片網址的影片 ID
    /// <para>來源：https://gist.github.com/takien/4077195 </para>
    /// </summary>
    /// <param name="url">字串，網址</param>
    /// <returns>字串，影片 ID</returns>
    public static string GetYTVideoID(string url)
    {
        string videoID = string.Empty;
        string pattern = @"(vi\/|v%3D|v=|\/v\/|youtu\.be\/|\/embed\/)";

        // 0：網址；1：v=；2：影片 ID。 
        string[] splittedArray = Regex.Split(url, pattern);

        if (splittedArray.Length == 3)
        {
            videoID = splittedArray[2];
        }

        return videoID;
    }

    /// <summary>
    /// 批次設定啟用
    /// </summary>
    /// <param name="controls">Control 的陣列</param>
    /// <param name="enabled">布林值，啟用，預設值為 true</param>
    public static void BatchSetEnabled(
        Contorl[] controls,
        bool enabled = true)
    {
        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
        {
            foreach (Contorl control in controls)
            {
                if (control is DataGrid dataGrid)
                {
                    dataGrid.IsReadOnly = !enabled;
                    dataGrid.CanUserAddRows = enabled;
                    dataGrid.CanUserDeleteRows = enabled;
                    dataGrid.AllowDrop = enabled;
                }
                else
                {
                    control.IsEnabled = enabled;
                }
            }
        }));
    }

    /// <summary>
    /// 開啟網址
    /// </summary>
    /// <param name="url">字串，網址</param>
    /// <returns>Process</returns>
    public static Process? OpenUrl(string url)
    {
        return Process.Start(new ProcessStartInfo()
        {
            FileName = url,
            UseShellExecute = true
        });
    }

    /// <summary>
    /// EnumerateFiles
    /// <para>來源：https://stackoverflow.com/a/72291652 </para>
    /// </summary>
    /// <param name="path">字串，路徑</param>
    /// <param name="searchPatterns">字串，搜尋模式</param>
    /// <param name="searchOption">SearchOption，預設值是 SearchOption.TopDirectoryOnly</param>
    /// <returns>IEnumerable&lt;string&gt;</returns>
    public static IEnumerable<string> EnumerateFiles(
        string path,
        string[] searchPatterns,
        SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        return Directory.EnumerateFiles(path, "*", searchOption)
            .Where(fileName => searchPatterns
            .Any(pattern =>
            {
                if (!pattern.StartsWith("*"))
                {
                    pattern = $"*{pattern}";
                }

                return FileSystemName.MatchesSimpleExpression(pattern, fileName);
            }));
    }

    /// <summary>
    /// 是不是支援的域名
    /// </summary>
    /// <param name="url">字串，網址</param>
    /// <returns>布林值</returns>
    public static bool IsSupportDomain(string? url)
    {
        bool result = true;

        string value = Properties.Settings.Default.NetPlaylistUnsupportedDomains;

        if (string.IsNullOrEmpty(url) ||
            string.IsNullOrEmpty(value))
        {
            return result;
        }

        string[] domains = value!.Split(
            new char[] { ';' },
            StringSplitOptions.RemoveEmptyEntries);

        foreach (string domain in domains)
        {
            if (url.Contains(domain))
            {
                result = false;

                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 寫紀錄
    /// </summary>
    /// <param name="message">字串，訊息</param>
    /// <param name="title">字串，應用程式的名稱，預設值為空白</param>
    public static void WriteLog(string message, string title = "")
    {
        if (_TBLog == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
        {
            try
            {
                string newMessage = $"[{DateTime.Now}] {message}{Environment.NewLine}";

                _TBLog.AppendText(newMessage);
                _TBLog.CaretIndex = _TBLog.Text.Length;
                _TBLog.ScrollToEnd();
            }
            catch (Exception ex)
            {
                _WMain?.ShowMsgBox(ex.ToString(), title);
            }
        }));
    }

    /// <summary>
    /// 取得播放速度
    /// </summary>
    /// <param name="index">數值，索引值</param>
    /// <returns>數值</returns>
    public static double GetPlaybackSpeed(int index)
    {
        return index switch
        {
            0 => 2,
            1 => 1.75,
            2 => 1.5,
            3 => 1.25,
            4 => 1,
            5 => 0.75,
            6 => 0.5,
            7 => 0.25,
            _ => 1
        };
    }

    /// <summary>
    /// 取得 YouTubeDlVideoQuality
    /// </summary>
    /// <param name="index">數值，索引值</param>
    /// <returns>數值</returns>
    public static YouTubeDlVideoQuality GetYTQuality(int index)
    {
        return index switch
        {
            0 => YouTubeDlVideoQuality.Highest,
            1 => YouTubeDlVideoQuality.High,
            2 => YouTubeDlVideoQuality.MediumHigh,
            3 => YouTubeDlVideoQuality.Medium,
            4 => YouTubeDlVideoQuality.LowMedium,
            5 => YouTubeDlVideoQuality.Low,
            6 => YouTubeDlVideoQuality.Lowest,
            _ => YouTubeDlVideoQuality.Highest
        };
    }

    /// <summary>
    /// 取得秒數
    /// </summary>
    /// <param name="value">字串</param>
    /// <returns>數值</returns>
    public static double GetSeconds(string value)
    {
        double seconds = 0.0;

        if (TimeSpan.TryParse(value, out TimeSpan timeSpan))
        {
            seconds = timeSpan.TotalSeconds;
        }

        return seconds;
    }

    /// <summary>
    /// 取得自動的 EndTime
    /// </summary>
    /// <param name="startTime">TimeSpan?</param>
    /// <returns>TimeSpan</returns>
    public static TimeSpan? GetAutoEndTime(TimeSpan? startTime)
    {
        double seconds = startTime?.TotalSeconds ?? 0.0d;

        seconds += Properties.Settings.Default.PlaylistAppendSeconds;

        return TimeSpan.FromSeconds(seconds);
    }

    /// <summary>
    /// 重新啟動應用程式
    /// </summary>
    /// <param name="isImmediately">布林值，是否立即執行，預設值是 false</param>
    public static async void RestartApplication(bool isImmediately = false)
    {
        try
        {
            // 重新啟動用程式。
            string? processPath = Environment.ProcessPath;

            if (!string.IsNullOrEmpty(processPath))
            {
                if (!isImmediately)
                {
                    _WMain?.WriteLog(MsgSet.MsgAutoRestartApplicationAfterTenSeconds);

                    await Task.Delay(10000);
                }

                Process.Start(processPath);

                Application.Current.Shutdown();
            }
            else
            {
                _WMain?.WriteLog(MsgSet.MsgRestartApplication);

                Application.Current.Shutdown();
            }
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 顯示或隱藏 Window
    /// </summary>
    /// <param name="window">Window</param>
    /// <returns>Visibility</returns>
    public static Visibility ShowOrHideWindow(Window? window)
    {
        try
        {
            if (window != null)
            {
                switch (window.Visibility)
                {
                    case Visibility.Visible:
                        WindowExtensions.Hide(
                            window,
                            enableEfficiencyMode: true);

                        break;
                    case Visibility.Collapsed:
                        WindowExtensions.Show(
                            window,
                            disableEfficiencyMode: true);

                        break;
                    case Visibility.Hidden:
                        WindowExtensions.Show(
                            window,
                            disableEfficiencyMode: true);

                        break;
                    default:
                        break;
                }

                return window.Visibility;
            }
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }

        return Visibility.Visible;
    }

    /// <summary>
    /// 開啟資料夾
    /// </summary>
    /// <param name="path">字串，路徑</param>
    public static void OpenFolder(string path)
    {
        try
        {
            if (!Directory.Exists(path))
            {
                _WMain?.WriteLog(MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    MsgSet.MsgPathIsNotExists));

                return;
            }

            // 來源：https://stackoverflow.com/a/1132559
            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true,
                Verb = "Open"
            });
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 取得使用者代理字串
    /// </summary>
    /// <returns>字串</returns>
    public static string GetUserAgent()
    {
        // TODO: 2023-01-04 Bilibili 的 API 會依據使用者代理字串來封鎖連線。
        return $"{Properties.Settings.Default.UserAgent} {DateTime.Now:yyyyMMddHHmm}";
    }
}