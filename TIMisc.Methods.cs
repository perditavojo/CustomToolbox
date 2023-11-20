using CustomToolbox.Common;
using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Sets;
using CustomToolbox.Common.Utils;
using Serilog.Events;
using System.Drawing.Text;
using System.IO;
using System.Windows.Controls;
using YoutubeDLSharp.Options;

namespace CustomToolbox;

/// <summary>
/// TIMisc 的方法
/// </summary>
public partial class WMain
{
    /// <summary>
    /// 載入 yt-dlp.conf
    /// </summary>
    private void LoadYtDlpConf()
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                OptionSet optionSet = ExternalProgram.GetOptionSet();

                #region 判斷格式的設定值

                TBFormat.Text = optionSet.Format;

                #endregion

                #region 判斷外部下載器的設定值

                if (optionSet.Downloader != null)
                {
                    CBUseAria2.IsChecked = optionSet.Downloader
                        .Values.Any(n => n.Contains("aria2c"));
                }
                else
                {
                    CBUseAria2.IsChecked = false;
                }

                #endregion

                #region 判斷嵌入後設資料的設定值

                if (optionSet.EmbedMetadata ||
                    optionSet.EmbedThumbnail ||
                    optionSet.EmbedSubs)
                {
                    CBEmbedMetadata.IsChecked = true;
                }
                else
                {
                    CBEmbedMetadata.IsChecked = false;
                }

                #endregion

                #region 判斷使用 --live-from-start 的設定值

                if (optionSet.LiveFromStart == true)
                {
                    CBLiveFromStart.IsChecked = true;
                }
                else
                {
                    CBLiveFromStart.IsChecked = false;
                }

                #endregion

                #region 判斷使用 --wait-for-video 的設定值

                if (!string.IsNullOrEmpty(optionSet.WaitForVideo))
                {
                    CBWaitForVideo.IsChecked = true;
                }
                else
                {
                    CBWaitForVideo.IsChecked = false;
                }

                #endregion

                #region 判斷使用 --split-chapters 的設定值

                if (optionSet.SplitChapters)
                {
                    CBSplitChapters.IsChecked = true;
                }
                else
                {
                    CBSplitChapters.IsChecked = false;
                }

                #endregion

                #region 判斷 Cookies 的設定值

                if (optionSet.NoCookiesFromBrowser)
                {
                    CBCookieFromBrowser.IsChecked = false;
                }

                if (!string.IsNullOrEmpty(optionSet.CookiesFromBrowser))
                {
                    char[] separators = [':'];

                    // 格式：BROWSER[+KEYRING][:PROFILE][::CONTAINER]
                    // 0：網頁瀏覽器、1：磁碟機代號、2：網頁瀏覽器設定檔路徑、4：容器。
                    string[] parameters = optionSet.CookiesFromBrowser
                        .Split(
                            separators,
                            StringSplitOptions.RemoveEmptyEntries);

                    CBBrowserName.Text = parameters[0];

                    if (parameters.Length >= 3)
                    {
                        TBBrowserProfilePath.Text = $"{parameters[1]}:{parameters[2]}";
                    }

                    CBCookieFromBrowser.IsChecked = true;
                }
                else
                {
                    CBCookieFromBrowser.IsChecked = false;
                }

                // 當最終的判斷結果為 false 時。
                if (CBCookieFromBrowser.IsChecked == false)
                {
                    CBBrowserName.Text = Properties.Settings.Default.YtDlpBrowserName;
                    TBBrowserProfilePath.Text = Properties.Settings.Default.YtDlpBrowserProfilePath;
                }

                #endregion
            }));
        }
        catch (Exception ex)
        {
            WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 更新 yt-dlp.conf
    /// </summary>
    private void UpdateYtDlpConf()
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                OptionSet optionSet = ExternalProgram.GetOptionSet();

                #region 判斷格式的設定值

                if (!string.IsNullOrEmpty(TBFormat.Text))
                {
                    optionSet.Format = TBFormat.Text;
                }
                else
                {
                    // 預設不會是最高畫質。
                    optionSet.Format = Properties.Settings.Default.YtDlpDefaultFormat;
                }

                #endregion

                #region 判斷外部下載器的設定值

                if (CBUseAria2.IsChecked == true)
                {
                    optionSet.Downloader = new MultiValue<string>(VariableSet.Aria2Path);
                    optionSet.DownloaderArgs = "aria2c:--allow-overwrite=true";
                }
                else
                {
                    optionSet.Downloader = null;
                    optionSet.DownloaderArgs = null;
                }

                #endregion

                #region 判斷嵌入後設資料的設定值

                if (CBEmbedMetadata.IsChecked == true)
                {
                    optionSet.EmbedMetadata = true;
                    optionSet.EmbedThumbnail = true;

                    if (Properties.Settings.Default.YtDlpEmbedSubs)
                    {
                        optionSet.EmbedSubs = true;
                    }
                }
                else
                {
                    optionSet.EmbedMetadata = false;
                    optionSet.EmbedThumbnail = false;
                    optionSet.EmbedSubs = false;
                }

                #endregion

                #region 判斷使用 --live-from-start 的設定值

                if (CBLiveFromStart.IsChecked == true)
                {
                    // --live-from-start 跟 --downloader 不能同時使用，
                    // 因為 --live-from-start 是透過 FFmpeg 進行下載的。
                    optionSet.LiveFromStart = true;
                    optionSet.Downloader = null;
                    optionSet.DownloaderArgs = null;
                }
                else
                {
                    optionSet.LiveFromStart = false;

                    // 額外再判斷是否有使用外部下載器。
                    if (CBUseAria2.IsChecked == true)
                    {
                        optionSet.Downloader = new MultiValue<string>(VariableSet.Aria2Path);
                        optionSet.DownloaderArgs = "aria2c:--allow-overwrite=true";
                    }
                }

                #endregion

                #region 判斷使用 --wait-for-video 的設定值

                if (CBWaitForVideo.IsChecked == true)
                {
                    optionSet.WaitForVideo = Properties.Settings.Default
                        .YtDlpWaitForVideoSeconds.ToString();
                }
                else
                {
                    optionSet.WaitForVideo = null;
                }

                #endregion

                #region 判斷使用 --split-chapters 的設定值

                if (CBSplitChapters.IsChecked == true)
                {
                    optionSet.ForceKeyframesAtCuts = true;
                    optionSet.SplitChapters = true;
                    optionSet.Output = VariableSet.YtDlpSplitChaptersOutput;
                }
                else
                {
                    optionSet.ForceKeyframesAtCuts = false;
                    optionSet.SplitChapters = false;
                    optionSet.Output = VariableSet.YtDlpDefaultOutput;
                }

                #endregion

                #region 判斷 Cookies 的設定值

                if (CBCookieFromBrowser.IsChecked == true)
                {
                    string browserName = CBBrowserName.Text,
                        browserProfilePath = TBBrowserProfilePath.Text,
                        cookiesFromBrowser = string.Empty;

                    cookiesFromBrowser = browserName;

                    if (!string.IsNullOrEmpty(cookiesFromBrowser))
                    {
                        if (!string.IsNullOrEmpty(browserProfilePath))
                        {
                            cookiesFromBrowser += $":{browserProfilePath}";
                        }

                        optionSet.CookiesFromBrowser = cookiesFromBrowser;
                    }
                    else
                    {
                        optionSet.CookiesFromBrowser = null;
                    }
                }
                else
                {
                    optionSet.CookiesFromBrowser = null;
                }

                #endregion

                optionSet.WriteConfigFile(VariableSet.YtDlpConfPath);

                WriteLog(message: MsgSet.GetFmtStr(
                    MsgSet.MsgUpdated,
                    VariableSet.YtDlpConfName));
            }));
        }
        catch (Exception ex)
        {
            WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 初始化 CBLanguages
    /// </summary>
    private void InitCBLanguages()
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                List<LangData>? listLang = App.GetLangData();

                CBLanguages.ItemsSource = listLang;
                CBLanguages.DisplayMemberPath = nameof(LangData.LangName);
                CBLanguages.SelectedValuePath = nameof(LangData.LangCode);
                CBLanguages.SelectedValue = Properties.Settings.Default.AppLangCode;
            }));
        }
        catch (Exception ex)
        {
            WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 初始化不支援的域名
    /// </summary>
    private void InitUnsupportedDomains()
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                char[] separators = [';'];

                string[] tempValue = Properties.Settings.Default
                    .NetPlaylistUnsupportedDomains.Split(
                        separators,
                        StringSplitOptions.RemoveEmptyEntries);

                string value = string.Join(Environment.NewLine, tempValue);

                TBUnsupportedDomains.Text = value;
            }));
        }
        catch (Exception ex)
        {
            WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 初始化 GPU 裝置
    /// </summary>
    private void InitGpuDevice()
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CBGpuDevice.ItemsSource = VideoCardUtil.GetDeviceList();
                CBGpuDevice.DisplayMemberPath = nameof(VideoCardData.DeviceName);
                CBGpuDevice.SelectedValuePath = nameof(VideoCardData.DeviceNo);
                CBGpuDevice.SelectedIndex = Properties.Settings.Default.FFmpegGpuDeviceIndex;

                if (CBGpuDevice.SelectedItem is VideoCardData videoCardData)
                {
                    if (string.IsNullOrEmpty(Properties.Settings.Default.FFmpegHardwareAccelerationType))
                    {
                        foreach (ComboBoxItem comboBoxItem in CBHardwareAccelerationType.Items)
                        {
                            string content = comboBoxItem.Content.ToString() ?? string.Empty;

                            if (!string.IsNullOrEmpty(videoCardData.DeviceName) &&
                                !string.IsNullOrEmpty(content) &&
                                videoCardData.DeviceName.Contains(content))
                            {
                                CBHardwareAccelerationType.SelectedItem = comboBoxItem;

                                break;
                            }
                        }
                    }
                    else
                    {
                        CBHardwareAccelerationType.Text = Properties.Settings.Default
                            .FFmpegHardwareAccelerationType;
                    }
                }
                else
                {
                    CBHardwareAccelerationType.Text = Properties.Settings.Default
                        .FFmpegDefaultHardwareAccelerationType;
                }
            }));
        }
        catch (Exception ex)
        {
            WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 初始化字型列表
    /// </summary>
    private void InitFontList()
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                using InstalledFontCollection installedFontCollection = new();

                List<string> dataSource = [];

                for (int i = 0; i < installedFontCollection.Families.Length; i++)
                {
                    dataSource.Add(installedFontCollection.Families[i].Name);
                }

                CBFontList.ItemsSource = dataSource;
                CBFontList.Text = AppLangUtil
                    .GetDefaultFont(AppLangUtil.GetCurrentLangResDict());
            }));
        }
        catch (Exception ex)
        {
            WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 初始化文字編碼列表
    /// </summary>
    private void InitEncodingList()
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!File.Exists(VariableSet.SubCharencParametersTxtPath))
                {
                    return;
                }

                List<string> dataSource =
                [
                    .. File.ReadAllLines(VariableSet.SubCharencParametersTxtPath)
                ];

                CBEncodingList.ItemsSource = dataSource;
                CBEncodingList.Text = AppLangUtil
                    .GetDefaultEncoding(AppLangUtil.GetCurrentLangResDict());
            }));
        }
        catch (Exception ex)
        {
            WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }
}