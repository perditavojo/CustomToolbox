using Control = System.Windows.Controls.Control;
using CustomToolbox.Common;
using static CustomToolbox.Common.Sets.EnumSet;
using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Sets;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Serilog.Events;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TextBox = System.Windows.Controls.TextBox;

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

    private void TBWhisperBeamSize_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        try
        {
            // 只允許數字鍵、NumPad 的數字鍵、「-」，以及 Crtl + A、X、C、V 等組合案件。
            if (e.Key is >= Key.D0 and <= Key.D9 or
                >= Key.NumPad0 and <= Key.NumPad9 or
                Key.Back or
                Key.Left or
                Key.Right or
                Key.Delete or
                Key.OemMinus or
                Key.Subtract or
                Key.LeftCtrl or
                Key.RightCtrl or
                Key.A or
                Key.X or
                Key.C or
                Key.V)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
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

    private void TBWhisperBeamSize_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            TextBox? textBox = (TextBox)sender;

            if (textBox == null)
            {
                return;
            }

            // 用於避免在只輸入「-」號時就開始解析。
            if (textBox.Text.Contains('-') &&
                textBox.Text.Length == 1)
            {
                return;
            }

            bool canParsed = int.TryParse(textBox.Text, out int parsedResult);

            if (!canParsed)
            {
                textBox.Text = Properties.Settings.Default.WhisperBeamSize.ToString();

                return;
            }

            // 當小於 2 時，改回預設值。
            // 因為等於 1 時，效果等同 Greedy 演算法。
            if (parsedResult < 2)
            {
                textBox.Text = Properties.Settings.Default.WhisperBeamSize.ToString();

                return;
            }
            else
            {
                textBox.Text = parsedResult.ToString();

                return;
            }
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

    private void TBWhisperPatience_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        try
        {
            // 只允許數字鍵、NumPad 的數字鍵、「.」、「-」，以及 Crtl + A、X、C、V 等組合案件。
            if (e.Key is >= Key.D0 and <= Key.D9 or
                >= Key.NumPad0 and <= Key.NumPad9 or
                Key.Back or
                Key.Left or
                Key.Right or
                Key.Delete or
                Key.OemMinus or
                Key.Decimal or
                Key.OemPeriod or
                Key.Subtract or
                Key.LeftCtrl or
                Key.RightCtrl or
                Key.A or
                Key.X or
                Key.C or
                Key.V)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
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

    private void TBWhisperPatience_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            TextBox? textBox = (TextBox)sender;

            if (textBox == null)
            {
                return;
            }

            // 用於避免在只輸入「-」或是「.」時就開始解析。
            if ((textBox.Text.Contains('-') && textBox.Text.Length == 1) ||
                (textBox.Text.Contains('.') && textBox.Text.Length == 3))
            {
                return;
            }

            bool canParsed = float.TryParse(textBox.Text, out float parsedResult);

            if (!canParsed)
            {
                textBox.Text = Properties.Settings.Default.WhisperPatience.ToString();

                return;
            }

            // 2023/10/4 理論上此值預設應為 0.0，且不能大於 0.0。
            // 根據參考資料，最大值應該不可以大於 0.0f，估限制大於 0.0f 時，則設回預設值。
            //
            // Reference: https://github.com/openai/whisper/discussions/154
            // Reference: https://github.com/sandrohanea/whisper.net/blob/main/Whisper.net/BeamSearchSamplingStrategyBuilder.cs#L46
            // 當大於 0.0f 時，改回預設值。
            if (parsedResult > 0.0f)
            {
                textBox.Text = Properties.Settings.Default.WhisperPatience.ToString();

                return;
            }
            else
            {
                textBox.Text = parsedResult.ToString();

                return;
            }
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

    private void TBWhisperBestOf_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        try
        {
            // 只允許數字鍵、NumPad 的數字鍵、「-」，以及 Crtl + A、X、C、V 等組合案件。
            e.Handled = (e.Key < Key.D0 || e.Key > Key.D9) &&
                (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) &&
                e.Key != Key.Back &&
                e.Key != Key.Left &&
                e.Key != Key.Right &&
                e.Key != Key.Delete &&
                e.Key != Key.OemMinus &&
                e.Key != Key.Subtract &&
                e.Key != Key.LeftCtrl &&
                e.Key != Key.RightCtrl &&
                e.Key != Key.A &&
                e.Key != Key.X &&
                e.Key != Key.C &&
                e.Key != Key.V;
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

    private void TBWhisperBestOf_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            // 僅 Greedy 演歲法會使用此參數。

            TextBox? textBox = (TextBox)sender;

            if (textBox == null)
            {
                return;
            }

            // 用於避免在只輸入「-」號時就開始解析。
            if (textBox.Text.Contains('-') &&
                textBox.Text.Length == 1)
            {
                return;
            }

            bool canParsed = int.TryParse(textBox.Text, out int parsedResult);

            if (!canParsed)
            {
                textBox.Text = Properties.Settings.Default.WhisperBestOf.ToString();

                return;
            }

            // 當小於 1 時，改回預設值。
            if (parsedResult < 1)
            {
                textBox.Text = Properties.Settings.Default.WhisperBestOf.ToString();

                return;
            }
            else
            {
                textBox.Text = parsedResult.ToString();

                return;
            }
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