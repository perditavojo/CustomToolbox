namespace CustomToolbox.Common.Sets;

/// <summary>
/// 列舉組
/// </summary>
public class EnumSet
{
    /// <summary>
    /// 列舉：短片播放器的模式
    /// </summary>
    public enum ClipPlayerMode
    {
        /// <summary>
        /// 短片播放器
        /// </summary>
        ClipPlayer,
        /// <summary>
        /// 時間標記編輯器
        /// </summary>
        TimestampEditor
    }

    /// <summary>
    /// 列舉：短片播放器的狀態
    /// </summary>
    public enum ClipPlayerStatus
    {
        /// <summary>
        /// 閒置
        /// </summary>
        Idle,
        /// <summary>
        /// 播放中
        /// </summary>
        Playing,
        /// <summary>
        /// 已暫停
        /// </summary>
        Paused
    }

    /// <summary>
    /// 列舉：SSeek 的狀態
    /// </summary>
    public enum SSeekStatus
    {
        /// <summary>
        /// 閒置
        /// </summary>
        Idle,
        /// <summary>
        /// 拖曳
        /// </summary>
        Drag
    }

    /// <summary>
    /// 列舉：硬體加速類型
    /// </summary>
    public enum HardwareAcceleratorType
    {
        Intel,
        NVIDIA,
        AMD
    };

    /// <summary>
    /// 抽樣策略類型
    /// </summary>
    public enum SamplingStrategyType
    {
        /// <summary>
        /// 預設
        /// </summary>
        Default,
        /// <summary>
        /// 貪婪
        /// </summary>
        Greedy,
        /// <summary>
        /// 集束搜尋
        /// </summary>
        BeamSearch
    }

    /// <summary>
    /// yt-dlp 更新頻道類型
    /// </summary>
    public enum YtDlpUpdateChannelType
    {
        /// <summary>
        /// 穩定版（stable）
        /// </summary>
        Stable,
        /// <summary>
        /// 每夜版（nightly）
        /// </summary>
        Nightly,
        /// <summary>
        /// Git 倉庫 master 分支版（master）
        /// </summary>
        Master,
        /// <summary>
        /// 自定義（需要自行輸入標籤）
        /// <para>範例：stable@2023.07.06</para>
        /// </summary>
        Custom
    }
}