using CustomToolbox.Common.Sets;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CustomToolbox.Common.Models;

/// <summary>
/// 類別：短片資料
/// </summary>
public class ClipData : INotifyPropertyChanged
{
    [JsonIgnore]
    private TimeSpan? _DurationTime;

    [JsonIgnore]
    private string? _VideoUrlOrID;

    [JsonIgnore]
    private int _No = 0;

    [JsonIgnore]
    private string? _Name;

    [JsonIgnore]
    private TimeSpan _StartTime;

    [JsonIgnore]
    private TimeSpan _EndTime;

    [JsonIgnore]
    private string? _SubtitleFileUrl;

    [JsonIgnore]
    private bool _IsAudioOnly = false;

    [JsonIgnore]
    private bool _IsLivestream = false;

    [JsonPropertyName("videoUrlOrID")]
    [Description("影片網址／ID")]
    public string? VideoUrlOrID
    {
        get { return _VideoUrlOrID; }
        set
        {
            if (_VideoUrlOrID != value)
            {
                _VideoUrlOrID = value;

                OnPropertyChanged(nameof(VideoUrlOrID));
            }
        }
    }

    [JsonPropertyName("no")]
    [Description("編號")]
    public int No
    {
        get { return _No; }
        set
        {
            if (_No != value)
            {
                _No = value;

                OnPropertyChanged(nameof(No));
            }
        }
    }

    [JsonPropertyName("name")]
    [Description("名稱")]
    public string? Name
    {
        get { return _Name; }
        set
        {
            if (_Name != value)
            {
                _Name = value;

                OnPropertyChanged(nameof(Name));
            }
        }
    }

    [JsonPropertyName("startTime")]
    [Description("開始時間")]
    public TimeSpan StartTime
    {
        get { return _StartTime; }
        set
        {
            if (_StartTime != value)
            {
                _StartTime = value;

                OnPropertyChanged(nameof(StartTime));
            }
        }
    }

    [JsonPropertyName("endTime")]
    [Description("結束時間")]
    public TimeSpan EndTime
    {
        get { return _EndTime; }
        set
        {
            if (_EndTime != value)
            {
                _EndTime = value;

                OnPropertyChanged(nameof(EndTime));
            }
        }
    }

    [JsonIgnore]
    [JsonPropertyName("durationTime")]
    [Description("時長")]
    public TimeSpan DurationTime
    {
        get
        {
            if (_DurationTime != null)
            {
                return _DurationTime.Value;
            }

            return EndTime.Subtract(StartTime);
        }
        set { _DurationTime = value; }
    }

    [JsonPropertyName("subtitleFileUrl")]
    [Description("字幕檔網址")]
    public string? SubtitleFileUrl
    {
        get { return _SubtitleFileUrl; }
        set
        {
            if (_SubtitleFileUrl != value)
            {
                _SubtitleFileUrl = value;

                OnPropertyChanged(nameof(SubtitleFileUrl));
            }
        }
    }

    [JsonPropertyName("isAudioOnly")]
    [Description("僅音訊")]
    public bool IsAudioOnly
    {
        get { return _IsAudioOnly; }
        set
        {
            if (_IsAudioOnly != value)
            {
                _IsAudioOnly = value;

                OnPropertyChanged(nameof(IsAudioOnly));
            }
        }
    }

    [JsonIgnore]
    [JsonPropertyName("isLivestream")]
    [Description("直播")]
    public bool IsLivestream
    {
        get { return _IsLivestream; }
        set
        {
            if (_IsLivestream != value)
            {
                _IsLivestream = value;

                OnPropertyChanged(nameof(IsLivestream));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// 轉換成字串
    /// </summary>
    /// <returns>字串</returns>
    public override string ToString() => JsonSerializer.Serialize(this, VariableSet.SharedJSOptions);

    /// <summary>
    /// 取得 VideoUrlOrID 的名稱
    /// </summary>
    /// <returns>字串</returns>
    public static string GetVideoUrlOrIDName()
    {
        return nameof(VideoUrlOrID);
    }

    /// <summary>
    /// 取得 No 的名稱
    /// </summary>
    /// <returns>字串</returns>
    public static string GetNoName()
    {
        return nameof(No);
    }

    /// <summary>
    /// 取得 Name 的名稱
    /// </summary>
    /// <returns>字串</returns>
    public static string GetNameName()
    {
        return nameof(Name);
    }

    /// <summary>
    /// 取得 StartTime 的名稱
    /// </summary>
    /// <returns>字串</returns>
    public static string GetStartTimeName()
    {
        return nameof(StartTime);
    }

    /// <summary>
    /// 取得 EndTime 的名稱
    /// </summary>
    /// <returns>字串</returns>
    public static string GetEndTimeName()
    {
        return nameof(EndTime);
    }

    /// <summary>
    /// 取得 SubtitleFileUrl 的名稱
    /// </summary>
    /// <returns>字串</returns>
    public static string GetSubtitleFileUrlName()
    {
        return nameof(SubtitleFileUrl);
    }

    /// <summary>
    /// 取得 IsAudioOnly 的名稱
    /// </summary>
    /// <returns>字串</returns>
    public static string GetIsAudioOnlyName()
    {
        return nameof(IsAudioOnly);
    }

    /// <summary>
    /// 取得 IsLivestream 的名稱
    /// </summary>
    /// <returns>字串</returns>
    public static string GetIsLivestreamName()
    {
        return nameof(IsLivestream);
    }
}