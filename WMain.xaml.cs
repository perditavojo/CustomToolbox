using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;
using Control = System.Windows.Controls.Control;
using CustomToolbox.Common;
using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Models.ImportPlaylist;
using CustomToolbox.Common.Sets;
using CustomToolbox.Common.Utils;
using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using H.NotifyIcon;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ModernWpf.Controls.Primitives;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Path = System.IO.Path;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TabControl = System.Windows.Controls.TabControl;
using TextBox = System.Windows.Controls.TextBox;

namespace CustomToolbox;

/// <summary>
/// Interaction logic for WMain.xaml
/// </summary>
public partial class WMain : Window
{
    public WMain()
    {
        InitializeComponent();
    }

    private void WMain_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            CustomInit();
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void WMain_Closing(object sender, CancelEventArgs e)
    {
        try
        {
            // 先攔截事件。
            e.Cancel = true;

            // 關閉應用程式。
            ShutdownApp(sender);
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    public void WMain_KeyDown(object? sender, KeyEventArgs e)
    {
        try
        {
            // 排除組合按鍵。
            if (e.Key == Key.LeftCtrl ||
                e.Key == Key.RightCtrl ||
                e.Key == Key.LeftShift ||
                e.Key == Key.RightShift ||
                e.Key == Key.LeftAlt ||
                e.Key == Key.RightAlt)
            {
                return;
            }

            // 避免在非指定的 TextBox 內輸入內容時觸發快速鍵。
            if (e.OriginalSource is TextBox textBox)
            {
                if (textBox != null && textBox != TBLog)
                {
                    return;
                }
            }

            switch (e.Key)
            {
                case Key.Q:
                    // 強制顯示應用程式以顯示對話視窗。
                    WindowExtensions.Show(
                        this,
                        disableEfficiencyMode: true);

                    WindowState = WindowState.Normal;

                    ShutdownApp(sender);

                    break;
                case Key.W:
                    // 重設 PopupPlayer 的大小與位置。
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (WPPPlayer != null && WPPPlayer.IsShowing())
                        {
                            WPPPlayer.Width = 854;
                            WPPPlayer.Height = 480;
                            WPPPlayer.Top = 60;
                            WPPPlayer.Left = 60;
                        }
                    }));
                    break;
                case Key.E:
                    PlayerHost_MouseDoubleClick(
                        sender,
                        new MouseEventArgs(MouseButtons.Left, 2, 0, 0, 0));

                    break;
                case Key.R:
                    // 開關 PopupPlayer 的全螢幕顯示。
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (WPPPlayer != null && WPPPlayer.IsShowing())
                        {
                            if (WPPPlayer.WindowState != WindowState.Maximized)
                            {
                                WPPPlayer.WindowStyle = WindowStyle.None;
                                WPPPlayer.WindowState = WindowState.Normal;
                                WPPPlayer.WindowState = WindowState.Maximized;

                                // 不關閉的話會無法完全的全螢幕化。
                                WindowHelper.SetUseModernWindowStyle(WPPPlayer, false);
                            }
                            else
                            {
                                WPPPlayer.WindowStyle = WindowStyle.SingleBorderWindow;
                                WPPPlayer.WindowState = WindowState.Normal;

                                WindowHelper.SetUseModernWindowStyle(WPPPlayer, true);
                            }
                        }
                    }));

                    break;
                case Key.T:
                    Visibility visibility = CustomFunction.ShowOrHideWindow(this);

                    TaskbarIconUtil.UpdateMIShowOrHideHeader(visibility);

                    break;
                case Key.A:
                    if (BtnPlay.IsEnabled)
                    {
                        BtnPlay.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }

                    break;
                case Key.S:
                    if (BtnPause.IsEnabled)
                    {
                        BtnPause.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }

                    break;
                case Key.D:
                    if (BtnPrevious.IsEnabled)
                    {
                        BtnPrevious.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }

                    break;
                case Key.F:
                    if (BtnNext.IsEnabled)
                    {
                        BtnNext.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }

                    break;
                case Key.G:
                    if (BtnStop.IsEnabled)
                    {
                        BtnStop.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }

                    break;
                case Key.Z:
                    DoRandomPlayClip();

                    break;
                case Key.X:
                    DoReorderClipList();

                    break;
                case Key.C:
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (CBNoVideo.IsEnabled)
                        {
                            CBNoVideo.IsChecked = !CBNoVideo.IsChecked;
                        }
                    }));

                    break;
                case Key.V:
                    if (BtnMute.IsEnabled)
                    {
                        BtnMute.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }

                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void MILoadClipListFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = MsgSet.LoadClipListSelectFile,
                Filter = MsgSet.LoadClipListFilter,
                FilterIndex = 1,
                InitialDirectory = VariableSet.ClipListsFolderPath
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = openFileDialog.FileName,
                    rawJson = File.ReadAllText(filePath);

                switch (openFileDialog.FilterIndex)
                {
                    case 1:
                        List<ClipData>? dataSource1 = JsonSerializer
                            .Deserialize<List<ClipData>>(rawJson, VariableSet.SharedJSOptions);

                        List<TimestampSongData>? dataSource2 = JsonSerializer
                            .Deserialize<List<TimestampSongData>>(rawJson, VariableSet.SharedJSOptions);

                        List<SecondsSongData>? dataSource3 = JsonSerializer
                            .Deserialize<List<SecondsSongData>>(rawJson, VariableSet.SharedJSOptions);

                        ClipData? data1 = dataSource1?.FirstOrDefault();
                        TimestampSongData? data2 = dataSource2?.FirstOrDefault();
                        SecondsSongData? data3 = dataSource3?.FirstOrDefault();

                        // 判斷讀取到的檔案是否為短片清單檔案。
                        if (!string.IsNullOrEmpty(data1?.VideoUrlOrID) &&
                            (data2?.VideoID == null || data3?.VideoID == null))
                        {
                            DGClipList.ImportData(dataSource1);
                        }
                        else
                        {
                            TimeSpan? timeSpan = data2?.StartTime;

                            double? seconds = data3?.StartSeconds;

                            // 當 timeSpan 不為 null 時，則表示讀取到的是時間標記播放清單檔案。
                            if (timeSpan != null)
                            {
                                DGClipList.ImportData(dataSource2);
                            }
                            else
                            {
                                // 當 seconds 為 null 或大於等於 0 時，則表示讀取到的是秒數播放清單檔案。
                                if (seconds == null || seconds >= 0)
                                {
                                    DGClipList.ImportData(dataSource3);
                                }
                            }
                        }

                        break;
                    case 2:
                        // 支援來自：https://github.com/YoutubeClipPlaylist/Playlists 的 *.jsonc 檔案。
                        List<List<object>>? dataSource4 = JsonSerializer
                            .Deserialize<List<List<object>>>(rawJson, VariableSet.SharedJSOptions);

                        DGClipList.ImportData(dataSource4);

                        break;
                    case 3:
                        DGClipList.ImportData(filePath);

                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private async void MISaveClipListFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ObservableCollection<ClipData>? dataSource = DGClipList.GetDataSource();

            if (dataSource == null || dataSource.Count <= 0)
            {
                WriteLog(MsgSet.MsgCanNotSaveClipList);

                return;
            }

            string dateString = $"{DateTime.Now:yyyyMMdd}";
            string defaultFileName = $"Playlist_{dateString}";

            SaveFileDialog saveFileDialog = new()
            {
                Title = MsgSet.SaveClipListSelectFile,
                FileName = defaultFileName,
                Filter = MsgSet.SaveClipListFilter,
                FilterIndex = 1,
                AddExtension = true,
                InitialDirectory = VariableSet.ClipListsFolderPath
            };

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string originFilePath = saveFileDialog.FileName;
                string originFileName = Path.GetFileNameWithoutExtension(originFilePath);

                // 當檔案名稱為預設名稱的時候，才會變更儲存的檔案名稱。
                if (originFilePath.Contains(defaultFileName))
                {
                    // 變更檔案名稱。
                    saveFileDialog.FileName = saveFileDialog.FilterIndex switch
                    {
                        1 => originFilePath.Replace(originFileName, $"ClipList_{dateString}"),
                        2 => originFilePath.Replace(originFileName, $"Playlist_Timestamps_{dateString}"),
                        3 => originFilePath.Replace(originFileName, $"Playlist_Seconds_{dateString}"),
                        4 => originFilePath.Replace(originFileName, $"SongList_{dateString}"),
                        5 => originFilePath.Replace(originFileName, $"Exported_timestamps_{dateString}"),
                        _ => saveFileDialog.FileName
                    };
                }

                using FileStream fileStream = (FileStream)saveFileDialog.OpenFile();

                string outputContent = string.Empty;

                switch (saveFileDialog.FilterIndex)
                {
                    case 1:
                        await JsonSerializer.SerializeAsync(
                            fileStream,
                            dataSource,
                            VariableSet.SharedJSOptions);

                        break;
                    case 2:
                        List<TimestampSongData> outputDataSource1 = new();

                        foreach (ClipData clipData in dataSource)
                        {
                            outputDataSource1.Add(new TimestampSongData()
                            {
                                VideoID = clipData.VideoUrlOrID,
                                Name = clipData.Name,
                                StartTime = clipData.StartTime,
                                EndTime = clipData.EndTime,
                                SubSrc = clipData.SubtitleFileUrl
                            });
                        }

                        await JsonSerializer.SerializeAsync(
                            fileStream,
                            outputDataSource1,
                            VariableSet.SharedJSOptions);

                        break;
                    case 3:
                        List<SecondsSongData> outputDataSource2 = new();

                        foreach (ClipData clipData in dataSource)
                        {
                            outputDataSource2.Add(new SecondsSongData()
                            {
                                VideoID = clipData.VideoUrlOrID,
                                Name = clipData.Name,
                                StartSeconds = clipData.StartTime.TotalSeconds,
                                EndSeconds = clipData.EndTime.TotalSeconds,
                                SubSrc = clipData.SubtitleFileUrl
                            });
                        }

                        await JsonSerializer.SerializeAsync(
                            fileStream,
                            outputDataSource2,
                            VariableSet.SharedJSOptions);

                        break;
                    case 4:
                        List<List<object>> outputDataSource3 = new();

                        foreach (ClipData clipData in dataSource)
                        {
                            outputDataSource3.Add(new List<object>
                            {
                                clipData.VideoUrlOrID ?? string.Empty,
                                clipData.StartTime.TotalSeconds,
                                clipData.EndTime.TotalSeconds,
                                clipData.Name ?? string.Empty,
                                clipData.SubtitleFileUrl ?? string.Empty
                            });
                        }

                        await JsonSerializer.SerializeAsync(
                            fileStream,
                            outputDataSource3,
                            VariableSet.SharedJSOptions);

                        break;
                    case 5:
                        // ※輸出的備註內容需保持正體中文。 



                        var grpClipDataSet = dataSource.GroupBy(n => n.VideoUrlOrID);

                        foreach (var grpClipData in grpClipDataSet)
                        {
                            int countIndex2 = 0;

                            foreach (ClipData clipData in grpClipData)
                            {
                                if (countIndex2 == 0)
                                {
                                    outputContent += VariableSet.TimestampHeaderTemplate.Replace("{VideoID}", grpClipData.Key);
                                    outputContent += $"{VariableSet.Timestamp.StartReadingToken}{Environment.NewLine}";
                                }

                                // 不輸出字幕檔來源。
                                outputContent += $"{VariableSet.Timestamp.CommentPrefixToken} " +
                                    $"{clipData.Name}{VariableSet.Timestamp.CommentSuffixToken1}" +
                                    $"{Environment.NewLine}{(clipData.StartTime.ToTimestampString())}" +
                                    $"{Environment.NewLine}{VariableSet.Timestamp.CommentPrefixToken} " +
                                    $"{clipData.Name}{VariableSet.Timestamp.CommentSuffixToken2}" +
                                    $"{Environment.NewLine}{(clipData.EndTime.ToTimestampString())}" +
                                    $"{Environment.NewLine}{Environment.NewLine}";

                                countIndex2++;
                            }

                            if (countIndex2 > 0 && countIndex2 < grpClipDataSet.Count() - 1)
                            {
                                // 分隔用行。
                                outputContent += $"{VariableSet.Timestamp.BlockSeparator}" +
                                    $"{Environment.NewLine}{Environment.NewLine}";
                            }
                        }

                        break;
                    default:
                        break;
                }

                switch (saveFileDialog.FilterIndex)
                {
                    case 4:
                        {
                            StringBuilder stringBuilder = new();

                            using StreamReader streamReader = new(fileStream);

                            stringBuilder.Append(VariableSet.JsoncHeader);

                            streamReader.BaseStream.Seek(0, SeekOrigin.Begin);

                            while (!streamReader.EndOfStream)
                            {
                                stringBuilder.AppendLine(streamReader.ReadLine());
                            }

                            await fileStream.DisposeAsync();

                            using StreamWriter streamWriter = new(saveFileDialog.FileName);

                            streamWriter.Write(stringBuilder);

                            await streamWriter.DisposeAsync();
                        }

                        break;
                    case 5:
                        {
                            using StreamWriter streamWriter = new(fileStream);

                            streamWriter.Write(outputContent);

                            await streamWriter.DisposeAsync();
                            await fileStream.DisposeAsync();
                        }

                        break;
                    default:
                        await fileStream.DisposeAsync();

                        break;
                }
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    public void MIExit_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // 強制顯示應用程式以顯示對話視窗。
            WindowExtensions.Show(
                this,
                disableEfficiencyMode: true);

            WindowState = WindowState.Normal;

            ShutdownApp(sender);
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void MICancel_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (GlobalCTS != null &&
                    !GlobalCTS.IsCancellationRequested)
                {
                    GlobalCTS.Cancel();

                    // 清除變數。
                    GlobalCTS = null;
                    GlobalCT = null;

                    // 重設控制項。
                    PBProgress.Value = 0;
                    LOperation.Content = string.Empty;
                }
            }));
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    public void MIAbout_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Version? version = Assembly.GetExecutingAssembly().GetName().Version;

            if (version != null)
            {
                // 強制顯示應用程式以顯示對話視窗。
                WindowExtensions.Show(
                    this,
                    disableEfficiencyMode: true);

                WindowState = WindowState.Normal;

                string appAbout = MsgSet.GetFmtStr(
                    MsgSet.AppAbout,
                    MsgSet.AppName,
                    version.ToString(),
                    MsgSet.AppDescription);

                ShowMsgBox(appAbout, MsgSet.About);
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void TCTabBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            // TODO: 2022-12-12 暫時先不利用。

            TabControl control = (TabControl)sender;
            TabItem tabItem = (TabItem)control.SelectedItem;

            string value = tabItem.Name;

            switch (value)
            {
                case nameof(TIClipPlayer):
                    break;
                case nameof(TINetResource):
                    break;
                case nameof(TIClipTool):
                    break;
                case nameof(TIOtherTool):
                    break;
                case nameof(TIMisc):
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void GlobalDataSet_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        try
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        // 產生新的 No。
                        int newInsertNo = e.NewStartingIndex;

                        newInsertNo++;

                        // 理論上 e.NewItems 只會有一筆資料。
                        foreach (ClipData newClipData in e.NewItems)
                        {
                            // 當新加入資料的 No 為 0 時，才更新 No。
                            if (newClipData.No == 0)
                            {
                                newClipData.No = newInsertNo;

                                newInsertNo++;
                            }
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    DoUpdateNoInGlobalDataSet();

                    break;
                case NotifyCollectionChangedAction.Replace:
                    DoUpdateNoInGlobalDataSet();

                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void DGClipList_DragEnter(object sender, DragEventArgs e)
    {
        try
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void DGClipList_Drop(object sender, DragEventArgs e)
    {
        try
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetData(DataFormats.FileDrop) != null)
                {
                    List<string>? files = ((string[]?)e.Data.GetData(DataFormats.FileDrop))
                        ?.Where(n => VariableSet.AllowedExts.Contains(Path.GetExtension(n)))
                        .ToList();

                    if (files != null)
                    {
                        if (files.Count == 0)
                        {
                            ShowMsgBox(MsgSet.MsgSelectAValidClipListFile);

                            return;
                        }

                        DoLoadClipLists(files);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void DGClipList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            if (sender is DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem != null)
                {
                    if (dataGrid.CurrentCell.Item is ClipData clipData &&
                        !string.IsNullOrEmpty(clipData.VideoUrlOrID))
                    {
                        PlayClip(clipData);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void MIPlayClip_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ClipData? clipData = GetDGClipListSelectedItem();

            if (clipData != null)
            {
                if (string.IsNullOrEmpty(clipData.VideoUrlOrID) ||
                    !CustomFunction.IsSupportDomain(clipData.VideoUrlOrID))
                {
                    ShowMsgBox(MsgSet.MsgUnSupportedSite);

                    return;
                }

                PlayClip(clipData);
            }
            else
            {
                ShowMsgBox(MsgSet.MsgSelectTheClipToPlay);
            }
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private async void MIFetchClip_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Control[] ctrlSet1 =
            {
                MIFetchClip,
                MIDLClip,
                BtnGenerateB23ClipList,
                BtnBurnInSubtitle,
            };

            Control[] ctrlSet2 =
            {
                MICancel
            };

            CustomFunction.BatchSetEnabled(ctrlSet1, false);
            CustomFunction.BatchSetEnabled(ctrlSet2, true);

            // 先清除日誌紀錄。
            MIClearLog_Click(sender, e);

            ClipData? clipData = GetDGClipListSelectedItem();

            if (clipData != null && !string.IsNullOrEmpty(clipData.VideoUrlOrID))
            {
                await OperationSet.FetchClipInfo(clipData.VideoUrlOrID, GetGlobalCT());
            }
            else
            {
                ShowMsgBox(MsgSet.MsgSelectTheClipToFetchInfo);
            }

            CustomFunction.BatchSetEnabled(ctrlSet1, true);
            CustomFunction.BatchSetEnabled(ctrlSet2, false);
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void MIDLClip_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Control[] ctrlSet1 =
{
                MIFetchClip,
                MIDLClip,
                BtnGenerateB23ClipList,
                BtnBurnInSubtitle,
            };

            Control[] ctrlSet2 =
            {
                MICancel
            };

            CustomFunction.BatchSetEnabled(ctrlSet1, false);
            CustomFunction.BatchSetEnabled(ctrlSet2, true);

            ClipData? clipData = GetDGClipListSelectedItem();

            if (clipData != null)
            {
                // TODO: 2022-11-25 待完成相關功能。
            }
            else
            {
                ShowMsgBox(MsgSet.MsgSelectTheClipToDownload);
            }

            CustomFunction.BatchSetEnabled(ctrlSet1, true);
            CustomFunction.BatchSetEnabled(ctrlSet2, false);
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    public void MIRandomPlayClip_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            DoRandomPlayClip();
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void MIReorderClipList_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            DoReorderClipList();
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void MIClearClipList_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            GlobalDataSet.Clear();
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void MIClearLog_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                TBLog.Clear();

                WriteLog(MsgSet.MsgLogCleared);
            }));
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void MIExportLog_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                SaveFileDialog saveFileDialog = new()
                {
                    FileName = $"Log_{DateTime.Now:yyyyMMddHHmmss}",
                    Filter = MsgSet.ExportLogFilter,
                    FilterIndex = 1,
                    InitialDirectory = VariableSet.LogsFolderPath
                };

                bool? result = saveFileDialog.ShowDialog();

                if (result == true)
                {
                    File.WriteAllText(
                        saveFileDialog.FileName,
                        TBLog.Text);

                    WriteLog(MsgSet.GetFmtStr(
                        MsgSet.MsgExportLogTo,
                        saveFileDialog.FileName));
                }
            }));
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    /// <summary>
    /// 寫紀錄
    /// </summary>
    /// <param name="message">字串，訊息</param>
    [SuppressMessage("Performance", "CA1822:將成員標記為靜態", Justification = "<暫止>")]
    public void WriteLog(string message) => CustomFunction.WriteLog(message);
}