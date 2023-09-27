﻿using Control = System.Windows.Controls.Control;
using CustomToolbox.Common;
using static CustomToolbox.Common.Sets.EnumSet;
using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Sets;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Serilog.Events;
using System.IO;
using System.Windows;

namespace CustomToolbox;

/// <summary>
/// TIClipTools 的控制項事件
/// </summary>
public partial class WMain
{
    private void BtnSubtitleCreator_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string path = Path.Combine(
                VariableSet.AssetsFolderPath,
                "SubtitleCreator.html");

            CustomFunction.OpenUrl(path);
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

    private void BtnYTSecConverter_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string path = Path.Combine(
                VariableSet.AssetsFolderPath,
                "YTSecConverter.html");

            CustomFunction.OpenUrl(path);
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

    private void BtnBurnInSubtitle_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(async () =>
            {
                Control[] ctrlSet1 =
                {
                    MIFetchClip,
                    MIDLClip,
                    MIDLClipsByTheSameUrl,
                    MIBatchDLClips,
                    BtnGenerateB23ClipList,
                    BtnBurnInSubtitle,
                    BtnSplitVideo
                };

                Control[] ctrlSet2 =
                {
                    MICancel
                };

                CustomFunction.BatchSetEnabled(ctrlSet1, false);
                CustomFunction.BatchSetEnabled(ctrlSet2, true);

                string? videoFilePath = string.Empty,
                    subtitleFilePath = string.Empty,
                    subtitleStyle = string.Empty,
                    subtitleEncoding = string.Empty;

                bool applyFontSetting = false,
                    useHardwareAcceleration = false;

                OpenFileDialog openFileDialog1 = new()
                {
                    Title = MsgSet.SelectVideoFile,
                    Filter = MsgSet.SelectVideoFileFilter,
                    FilterIndex = 1,
                    InitialDirectory = VariableSet.DownloadsFolderPath
                };

                bool? result1 = openFileDialog1.ShowDialog();

                if (result1 == true)
                {
                    // 設定視訊檔案的路徑。
                    videoFilePath = openFileDialog1.FileName;

                    OpenFileDialog openFileDialog2 = new()
                    {
                        Title = MsgSet.SelectSubtitleFile,
                        Filter = MsgSet.SelectSubtitleFileFilter,
                        FilterIndex = 1
                    };

                    bool? result2 = openFileDialog2.ShowDialog();

                    if (result2 == true)
                    {
                        #region 前處理變數

                        // 設定字幕檔案的路徑。
                        subtitleFilePath = openFileDialog2.FileName;

                        applyFontSetting = Properties.Settings.Default
                            .FFmpegApplyFontSetting;

                        subtitleStyle = CBFontList.Text;

                        if (!string.IsNullOrEmpty(TBCustomFont.Text))
                        {
                            subtitleStyle = TBCustomFont.Text;
                        }

                        subtitleEncoding = CBEncodingList.Text;

                        if (!string.IsNullOrEmpty(TBCustomEncoding.Text))
                        {
                            subtitleEncoding = TBCustomEncoding.Text;
                        }

                        useHardwareAcceleration = Properties.Settings.Default
                            .FFmpegEnableHardwareAcceleration;

                        HardwareAcceleratorType hardwareAcceleratorType = HardwareAcceleratorType.Intel;

                        if (!string.IsNullOrEmpty(CBHardwareAccelerationType.Text))
                        {
                            hardwareAcceleratorType = CBHardwareAccelerationType.Text switch
                            {
                                nameof(HardwareAcceleratorType.AMD) => HardwareAcceleratorType.AMD,
                                nameof(HardwareAcceleratorType.Intel) => HardwareAcceleratorType.Intel,
                                nameof(HardwareAcceleratorType.NVIDIA) => HardwareAcceleratorType.NVIDIA,
                                _ => HardwareAcceleratorType.Intel
                            };
                        }

                        VideoCardData videoCardData = (VideoCardData)CBGpuDevice.SelectedItem;

                        int deviceNo = videoCardData.DeviceNo;

                        string videoExtName = Path.GetExtension(videoFilePath);
                        string videoFileName = Path.GetFileName(videoFilePath);
                        string videoFileNameNoExt = Path.GetFileNameWithoutExtension(videoFilePath);
                        string videoNewFileName = $"{videoFileNameNoExt}_Subtitled{videoExtName}";
                        string outputPath = Path.GetFullPath(videoFilePath).Replace(videoFileName, videoNewFileName);

                        // 判斷選擇的影片的副檔名。
                        if (videoExtName != ".mp4" && videoExtName != ".mkv")
                        {
                            MICancel_Click(sender, e);

                            ShowMsgBox(MsgSet.MsgSelectedVideoNonSupported);

                            return;
                        }

                        #endregion

                        // 先清除日誌紀錄。
                        MIClearLog_Click(sender, e);

                        await OperationSet.DoBurnInSubtitle(
                            videoFilePath,
                            subtitleFilePath,
                            outputPath,
                            subtitleEncoding,
                            applyFontSetting,
                            subtitleStyle,
                            useHardwareAcceleration,
                            hardwareAcceleratorType,
                            deviceNo,
                            GetGlobalCT());
                    }
                }

                CustomFunction.BatchSetEnabled(ctrlSet1, true);
                CustomFunction.BatchSetEnabled(ctrlSet2, false);
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

    private void BtnSplitVideo_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(async () =>
            {
                Control[] ctrlSet1 =
                {
                    DGClipList,
                    MIFetchClip,
                    MIDLClip,
                    MIDLClipsByTheSameUrl,
                    MIBatchDLClips,
                    MIReorderClipList,
                    MIClearClipList,
                    BtnGenerateB23ClipList,
                    BtnBurnInSubtitle,
                    BtnSplitVideo
                };

                Control[] ctrlSet2 =
                {
                    MICancel
                };

                CustomFunction.BatchSetEnabled(ctrlSet1, false);
                CustomFunction.BatchSetEnabled(ctrlSet2, true);

                string? videoFilePath = string.Empty;

                bool useHardwareAcceleration = false;

                List<ClipData> clipDatas = GlobalDataSet.ToList();

                if (!clipDatas.Any())
                {
                    ShowMsgBox(MsgSet.MsgClipListNoData);

                    CustomFunction.BatchSetEnabled(ctrlSet1, true);
                    CustomFunction.BatchSetEnabled(ctrlSet2, false);

                    return;
                }

                OpenFileDialog openFileDialog = new()
                {
                    Title = MsgSet.SelectVideoFile,
                    Filter = MsgSet.SelectVideoFileFilter,
                    FilterIndex = 1,
                    InitialDirectory = VariableSet.DownloadsFolderPath
                };

                bool? result1 = openFileDialog.ShowDialog();

                if (result1 == true)
                {
                    // 設定視訊檔案的路徑。
                    videoFilePath = openFileDialog.FileName;

                    #region 前處理變數

                    useHardwareAcceleration = Properties.Settings.Default
                        .FFmpegEnableHardwareAcceleration;

                    HardwareAcceleratorType hardwareAcceleratorType = HardwareAcceleratorType.Intel;

                    if (!string.IsNullOrEmpty(CBHardwareAccelerationType.Text))
                    {
                        hardwareAcceleratorType = CBHardwareAccelerationType.Text switch
                        {
                            nameof(HardwareAcceleratorType.AMD) => HardwareAcceleratorType.AMD,
                            nameof(HardwareAcceleratorType.Intel) => HardwareAcceleratorType.Intel,
                            nameof(HardwareAcceleratorType.NVIDIA) => HardwareAcceleratorType.NVIDIA,
                            _ => HardwareAcceleratorType.Intel
                        };
                    }

                    VideoCardData videoCardData = (VideoCardData)CBGpuDevice.SelectedItem;

                    int deviceNo = videoCardData.DeviceNo;

                    string videoExtName = Path.GetExtension(videoFilePath);

                    // 判斷選擇的影片的副檔名。
                    if (videoExtName != ".mp4" && videoExtName != ".mkv")
                    {
                        MICancel_Click(sender, e);

                        ShowMsgBox(MsgSet.MsgSelectedVideoNonSupported);

                        return;
                    }

                    #endregion

                    // 先清除日誌紀錄。
                    MIClearLog_Click(sender, e);

                    await OperationSet.DoSplitVideo(
                        DGClipList,
                        clipDatas,
                        videoFilePath,
                        useHardwareAcceleration,
                        hardwareAcceleratorType,
                        deviceNo,
                        GetGlobalCT());
                }

                CustomFunction.BatchSetEnabled(ctrlSet1, true);
                CustomFunction.BatchSetEnabled(ctrlSet2, false);
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