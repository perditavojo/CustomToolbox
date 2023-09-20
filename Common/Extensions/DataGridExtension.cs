using Application = System.Windows.Application;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Models.ImportPlaylist;
using CustomToolbox.Common.Sets;
using Serilog.Events;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Controls;

namespace CustomToolbox.Common.Extensions;

/// <summary>
/// DataGrid 的擴充方法
/// </summary>
public static class DataGridExtension
{
    /// <summary>
    /// WMain
    /// </summary>
    private static WMain? _WMain = null;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="wMain">WMain</param>
    public static void Init(WMain wMain)
    {
        _WMain = wMain;
    }

    /// <summary>
    /// 取得資料來源
    /// </summary>
    /// <param name="dataGrid">DataGrid</param>
    /// <returns>ObservableCollection&lt;ClipData&gt;?</returns>
    public static ObservableCollection<ClipData>? GetDataSource(this DataGrid dataGrid)
    {
        return dataGrid.ItemsSource as ObservableCollection<ClipData>;
    }

    /// <summary>
    /// 捲動至已選擇的項目
    /// </summary>
    /// <param name="dataGrid">DataGrid</param>
    public static void ScrollToSelectedItem(this DataGrid dataGrid)
    {
        try
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    ObservableCollection<ClipData>? dataSource = dataGrid.GetDataSource();

                    if (dataSource == null)
                    {
                        dataGrid.ScrollIntoView(dataGrid.Items.GetItemAt(0));
                    }

                    int selectedIndex = dataGrid.SelectedIndex;

                    if (selectedIndex >= 0 && selectedIndex <= dataSource!.Count - 1)
                    {
                        dataGrid.ScrollIntoView(dataGrid.Items.GetItemAt(dataGrid.SelectedIndex));
                    }
                    else
                    {
                        dataGrid.ScrollIntoView(dataGrid.Items.GetItemAt(0));
                    }
                }
                catch (Exception ex)
                {
                    _WMain?.WriteLog(
                        message: MsgSet.GetFmtStr(
                            MsgSet.MsgErrorOccured,
                            ex.GetExceptionMessage()),
                        logEventLevel: LogEventLevel.Error);
                }

            }));
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 匯入資料（短片資料）
    /// </summary>
    /// <param name="dataGrid">DataGrid</param>
    /// <param name="dataSource">List&lt;ClipData&gt;?</param>
    public static void ImportData(this DataGrid dataGrid, List<ClipData>? dataSource)
    {
        try
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (dataSource == null ||
                        dataGrid.ItemsSource is not ObservableCollection<ClipData> itemsSource)
                    {
                        return;
                    }

                    List<ClipData> filteredDataSource = dataSource
                        .Where(n => !itemsSource
                            .Any(m => CustomFunction.IsSupportDomain(n.VideoUrlOrID) &&
                                m.VideoUrlOrID == n.VideoUrlOrID &&
                                m.Name == n.Name))
                        .ToList();

                    int no = itemsSource.Count + 1;

                    foreach (ClipData item in filteredDataSource)
                    {
                        if (item.No == 0)
                        {
                            item.No = no;
                        }

                        itemsSource.Add(item);

                        no++;
                    }

                    dataGrid.Refresh();
                }
                catch (Exception ex)
                {
                    _WMain?.WriteLog(
                        message: MsgSet.GetFmtStr(
                            MsgSet.MsgErrorOccured,
                            ex.GetExceptionMessage()),
                        logEventLevel: LogEventLevel.Error);
                }
            }));
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 匯入資料（時間標記歌曲資料）
    /// </summary>
    /// <param name="dataGrid">DataGrid</param>
    /// <param name="dataSource">List&lt;TimestampSongData&gt;?</param>
    public static void ImportData(this DataGrid dataGrid, List<TimestampSongData>? dataSource)
    {
        try
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (dataSource == null ||
                        dataGrid.ItemsSource is not ObservableCollection<ClipData> itemsSource)
                    {
                        return;
                    }

                    List<TimestampSongData> filteredDataSource = dataSource
                        .Where(n => !itemsSource
                            .Any(m => CustomFunction.IsSupportDomain(n.VideoID) &&
                                m.VideoUrlOrID == n.VideoID &&
                                m.Name == n.Name))
                        .ToList();

                    int no = itemsSource.Count + 1;

                    foreach (TimestampSongData item in filteredDataSource)
                    {
                        itemsSource.Add(new ClipData()
                        {
                            VideoUrlOrID = item.VideoID,
                            No = no,
                            Name = item.Name,
                            StartTime = item.StartTime ?? TimeSpan.FromSeconds(0),
                            EndTime = item.EndTime ?? TimeSpan.FromSeconds(0),
                            SubtitleFileUrl = item.SubSrc,
                            IsAudioOnly = false,
                            IsLivestream = false
                        });

                        no++;
                    }

                    dataGrid.Refresh();
                }
                catch (Exception ex)
                {
                    _WMain?.WriteLog(
                        message: MsgSet.GetFmtStr(
                            MsgSet.MsgErrorOccured,
                            ex.GetExceptionMessage()),
                        logEventLevel: LogEventLevel.Error);
                }
            }));
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 匯入資料（秒數歌曲資料）
    /// </summary>
    /// <param name="dataGrid">DataGrid</param>
    /// <param name="dataSource">List&lt;SecondsSongData&gt;?</param>
    public static void ImportData(this DataGrid dataGrid, List<SecondsSongData>? dataSource)
    {
        try
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (dataSource == null ||
                        dataGrid.ItemsSource is not ObservableCollection<ClipData> itemsSource)
                    {
                        return;
                    }

                    List<SecondsSongData> filteredDataSource = dataSource
                        .Where(n => !itemsSource
                            .Any(m => CustomFunction.IsSupportDomain(n.VideoID) &&
                                m.VideoUrlOrID == n.VideoID &&
                                m.Name == n.Name))
                        .ToList();

                    int no = itemsSource.Count + 1;

                    foreach (SecondsSongData item in filteredDataSource)
                    {
                        itemsSource.Add(new ClipData()
                        {
                            VideoUrlOrID = item.VideoID,
                            No = no,
                            Name = item.Name,
                            StartTime = TimeSpan.FromSeconds(item.StartSeconds ?? 0),
                            EndTime = TimeSpan.FromSeconds(item.EndSeconds ?? 0),
                            SubtitleFileUrl = item.SubSrc,
                            IsAudioOnly = false,
                            IsLivestream = false
                        });

                        no++;
                    }

                    dataGrid.Refresh();
                }
                catch (Exception ex)
                {
                    _WMain?.WriteLog(
                        message: MsgSet.GetFmtStr(
                            MsgSet.MsgErrorOccured,
                            ex.GetExceptionMessage()),
                        logEventLevel: LogEventLevel.Error);
                }
            }));
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 匯入資料（*.jsonc）
    /// </summary>
    /// <param name="dataGrid">DataGrid</param>
    /// <param name="dataSource">List&lt;List&lt;object&gt;&gt;?</param>
    public static void ImportData(this DataGrid dataGrid, List<List<object>>? dataSource)
    {
        try
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (dataSource == null ||
                        dataGrid.ItemsSource is not ObservableCollection<ClipData> itemsSource)
                    {
                        return;
                    }

                    List<List<object>> filteredDataSource = dataSource
                        .Where(n => !itemsSource
                            .Any(m => CustomFunction.IsSupportDomain(n[0].ToString()) &&
                                m.VideoUrlOrID == n[0].ToString() &&
                                m.Name == n[3].ToString()))
                        .ToList();

                    int no = itemsSource.Count + 1;

                    foreach (List<object> item in filteredDataSource)
                    {
                        itemsSource.Add(new ClipData()
                        {
                            VideoUrlOrID = item[0].ToString(),
                            No = no,
                            Name = item[3].ToString(),
                            StartTime = TimeSpan.FromSeconds(Convert.ToInt32(item[1].ToString())),
                            EndTime = TimeSpan.FromSeconds(Convert.ToInt32(item[2].ToString())),
                            SubtitleFileUrl = item.Count > 4 ? item[4].ToString() : string.Empty,
                            IsAudioOnly = false,
                            IsLivestream = false
                        });

                        no++;
                    }

                    dataGrid.Refresh();
                }
                catch (Exception ex)
                {
                    _WMain?.WriteLog(
                        message: MsgSet.GetFmtStr(
                            MsgSet.MsgErrorOccured,
                            ex.GetExceptionMessage()),
                        logEventLevel: LogEventLevel.Error);
                }
            }));
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 匯入資料（*.txt 時間標記）
    /// </summary>
    /// <param name="dataGrid">DataGrid</param>
    /// <param name="path">字串，檔案的路徑</param>
    public static void ImportData(this DataGrid dataGrid, string path)
    {
        try
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(path) ||
                        dataGrid.ItemsSource is not ObservableCollection<ClipData> itemsSource)
                    {
                        return;
                    }

                    string[] lines = File.ReadAllLines(path);

                    bool canProcess = false;

                    List<TimestampSongData> dataSource = new();

                    TimestampSongData? tempTimestampSongData = null;

                    int timestampCount = 0, currentIndex = 0;

                    string videoID = string.Empty;

                    foreach (string currentLine in lines)
                    {
                        // 分隔用行。
                        if (currentLine == VariableSet.Timestamp.BlockSeparator)
                        {
                            canProcess = false;

                            continue;
                        }

                        // 從網址取得影片的 ID。
                        if (currentLine.Contains(VariableSet.Timestamp.VideoUrl))
                        {
                            canProcess = false;

                            videoID = CustomFunction.GetYTVideoID(
                                currentLine.Replace(VariableSet.Timestamp.VideoUrl,
                                string.Empty));

                            continue;
                        }

                        if (currentLine == VariableSet.Timestamp.StartReadingToken)
                        {
                            canProcess = true;

                            continue;
                        }

                        if (canProcess && !string.IsNullOrEmpty(currentLine))
                        {
                            // 判斷是否為備註列。
                            if (currentLine.Contains(VariableSet.Timestamp.CommentPrefixToken))
                            {
                                string songName = string.Empty;

                                // 判斷是否為開始的點。
                                if (currentLine.Contains(VariableSet.Timestamp.CommentSuffixToken1))
                                {
                                    // 當遇到連續的 "（開始）" 時。
                                    if (tempTimestampSongData != null)
                                    {
                                        // 設定 EndTime。
                                        tempTimestampSongData.EndTime = CustomFunction
                                            .GetAutoEndTime(tempTimestampSongData.StartTime);

                                        // 重置 timestampCount 為 0，以供下一個流程使用。
                                        timestampCount = 0;

                                        // 將 tempTimestampSongData 加入 dataSource;
                                        dataSource.Add(tempTimestampSongData);

                                        // 在加入 dataSource 後清空 tempTimestampSongData。
                                        tempTimestampSongData = null;
                                    }

                                    tempTimestampSongData = new();

                                    // 去除不必要的內容。
                                    songName = currentLine.Replace(
                                        VariableSet.Timestamp.CommentPrefixToken,
                                            string.Empty)
                                        .Replace(
                                            VariableSet.Timestamp.CommentSuffixToken1,
                                            string.Empty)
                                        .TrimStart();
                                }

                                if (!string.IsNullOrEmpty(songName))
                                {
                                    if (tempTimestampSongData != null)
                                    {
                                        tempTimestampSongData.VideoID = videoID;
                                        tempTimestampSongData.Name = songName;
                                        tempTimestampSongData.StartTime = null;
                                        tempTimestampSongData.EndTime = null;
                                    }
                                }
                            }
                            else
                            {
                                string[] timestampSet = currentLine.Split(
                                    new char[] { '｜' },
                                    StringSplitOptions.RemoveEmptyEntries);

                                if (timestampSet.Length > 0 &&
                                    !string.IsNullOrEmpty(timestampSet[2]))
                                {
                                    if (tempTimestampSongData != null)
                                    {
                                        // timestampCount 為 0 時，設定 StartTime。
                                        if (timestampCount == 0)
                                        {
                                            if (double.TryParse(timestampSet[2], out double seconds))
                                            {
                                                tempTimestampSongData.StartTime = TimeSpan.FromSeconds(seconds);
                                            }
                                            else
                                            {
                                                tempTimestampSongData.StartTime = TimeSpan
                                                    .FromSeconds(CustomFunction.GetSeconds(timestampSet[0]));
                                            }

                                            tempTimestampSongData.StartTime = TimeSpan.FromSeconds(seconds);

                                            timestampCount++;
                                        }
                                        else if (timestampCount == 1)
                                        {
                                            // timestampCount 為 1 時，設定 EndTime。
                                            if (double.TryParse(timestampSet[2], out double seconds))
                                            {
                                                tempTimestampSongData.EndTime = TimeSpan.FromSeconds(seconds);
                                            }
                                            else
                                            {
                                                tempTimestampSongData.EndTime = TimeSpan
                                                    .FromSeconds(CustomFunction.GetSeconds(timestampSet[0]));
                                            }

                                            // 重置 timestampCount 為 0，以供下一個流程使用。
                                            timestampCount = 0;

                                            // 將 tempTimestampSongData 加入 dataSource;
                                            dataSource.Add(tempTimestampSongData);

                                            // 在加入 dataSource 後清空。
                                            tempTimestampSongData = null;
                                        }
                                    }
                                }
                            }

                            // 當剛好是最後一個 "（開始）" 組合，且後面沒有其他有效資料行時。
                            if (currentIndex >= lines.Length - 2)
                            {
                                // 判斷 tempTimestampSongData 是否為 null。
                                if (tempTimestampSongData != null)
                                {
                                    // 判斷 EndTime 是否為 0.0 的 TimeSpan。
                                    if (tempTimestampSongData.EndTime == TimeSpan.FromSeconds(0.0))
                                    {
                                        // 設定 EndTime。
                                        tempTimestampSongData.EndTime = CustomFunction
                                            .GetAutoEndTime(tempTimestampSongData.StartTime);

                                        // 重置 timestampCount 為 0，以供下一個流程使用。
                                        timestampCount = 0;

                                        // 將 tempTimestampSongData 加入 dataSource;
                                        dataSource.Add(tempTimestampSongData);

                                        // 在加入 dataSource 後清空 tempTimestampSongData。
                                        tempTimestampSongData = null;
                                    }
                                }
                            }
                        }

                        currentIndex++;
                    }

                    List<TimestampSongData> filteredDataSource = dataSource
                        .Where(n => !itemsSource.Any(m =>
                            CustomFunction.IsSupportDomain(n.VideoID) &&
                            m.VideoUrlOrID == n.VideoID &&
                            m.Name == n.Name))
                        .ToList();

                    int no = itemsSource.Count + 1;

                    foreach (TimestampSongData item in filteredDataSource)
                    {
                        itemsSource.Add(new ClipData()
                        {
                            VideoUrlOrID = item.VideoID,
                            No = no,
                            Name = item.Name,
                            StartTime = item.StartTime ?? TimeSpan.FromSeconds(0),
                            EndTime = item.EndTime ?? TimeSpan.FromSeconds(0),
                            SubtitleFileUrl = item.SubSrc,
                            IsAudioOnly = false,
                            IsLivestream = false
                        });

                        no++;
                    }

                    dataGrid.Refresh();
                }
                catch (Exception ex)
                {
                    _WMain?.WriteLog(
                        message: MsgSet.GetFmtStr(
                            MsgSet.MsgErrorOccured,
                            ex.GetExceptionMessage()),
                        logEventLevel: LogEventLevel.Error);
                }
            }));
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 排序
    /// <para>來源：https://stackoverflow.com/a/19952233 </para>
    /// </summary>
    /// <param name="dataGrid">DataGrid</param>
    /// <param name="columnIndex">數值，欄位的索引值，預設值為 0</param>
    /// <param name="listSortDirection">ListSortDirection，預設值為 ListSortDirection.Ascending</param>
    public static void Sort(
        this DataGrid dataGrid,
        int columnIndex = 0,
        ListSortDirection? listSortDirection = ListSortDirection.Ascending)
    {
        try
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (columnIndex <= 0)
                    {
                        columnIndex = 0;
                    }

                    listSortDirection ??= ListSortDirection.Ascending;

                    DataGridColumn targetDataGridColumn = dataGrid.Columns[columnIndex];

                    dataGrid.Items.SortDescriptions.Clear();
                    dataGrid.Items.SortDescriptions.Add(new SortDescription(
                        targetDataGridColumn.SortMemberPath,
                        listSortDirection!.Value));

                    foreach (DataGridColumn dataGridColumn in dataGrid.Columns)
                    {
                        dataGridColumn.SortDirection = null;
                    }

                    targetDataGridColumn.SortDirection = listSortDirection;

                    dataGrid.Refresh();
                }
                catch (Exception ex)
                {
                    _WMain?.WriteLog(
                        message: MsgSet.GetFmtStr(
                            MsgSet.MsgErrorOccured,
                            ex.GetExceptionMessage()),
                        logEventLevel: LogEventLevel.Error);
                }
            }));
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }

    /// <summary>
    /// 重新整理
    /// </summary>
    /// <param name="dataGrid">DataGrid</param>
    public static void Refresh(this DataGrid dataGrid)
    {
        try
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    dataGrid.IsReadOnly = true;

                    // 重新整理 DataGrid。
                    dataGrid.Items.Refresh();
                    // 移動到 DataGrid 的最後一列。
                    dataGrid.ScrollIntoView(dataGrid.Items.GetItemAt(dataGrid.Items.Count - 1));

                    dataGrid.IsReadOnly = false;
                }
                catch (Exception ex)
                {
                    _WMain?.WriteLog(
                        message: MsgSet.GetFmtStr(
                            MsgSet.MsgErrorOccured,
                            ex.GetExceptionMessage()),
                        logEventLevel: LogEventLevel.Error);
                }
            }));
        }
        catch (Exception ex)
        {
            _WMain?.WriteLog(
                message: MsgSet.GetFmtStr(
                    MsgSet.MsgErrorOccured,
                    ex.GetExceptionMessage()),
                logEventLevel: LogEventLevel.Error);
        }
    }
}