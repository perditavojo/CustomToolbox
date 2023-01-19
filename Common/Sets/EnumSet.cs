﻿namespace CustomToolbox.Common.Sets;

/// <summary>
/// 列舉組
/// </summary>
public class EnumSet
{
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
}