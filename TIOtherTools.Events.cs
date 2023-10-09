using Control = System.Windows.Controls.Control;
using CustomToolbox.Common;
using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Sets;
using CustomToolbox.Common.Utils;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using Serilog.Events;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TextBox = System.Windows.Controls.TextBox;

namespace CustomToolbox;

/// <summary>
/// TIOtherTools 的控制項事件
/// </summary>
public partial class WMain
{
    private void TBYtChannelID_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                TextBox control = (TextBox)sender;

                string value = control.Text;

                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                if (value.StartsWith("@"))
                {
                    control.Text = PlaywrightUtil.GetYTChannelID($"{UrlSet.YTUrl}{value}");

                    return;
                }
                else if (value.Contains('@'))
                {
                    control.Text = PlaywrightUtil.GetYTChannelID(value);

                    return;
                }
                else if (value.Contains(UrlSet.YTChannelUrl))
                {
                    control.Text = value.Contains(UrlSet.YTCustomChannelUrl) ?
                        PlaywrightUtil.GetYTChannelID(value) :
                        value.Replace(UrlSet.YTChannelUrl, string.Empty);
                }
                else
                {
                    return;
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

    private void TBCustomSubscriberAmount_PreviewKeyDown(object sender, KeyEventArgs e)
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

    private void TBCustomSubscriberAmount_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            // 用於避免在只輸入「-」號時就開始解析。
            if (TBCustomSubscriberAmount.Text.Contains('-') &&
                TBCustomSubscriberAmount.Text.Length == 1)
            {
                return;
            }

            bool canParsed = int.TryParse(TBCustomSubscriberAmount.Text, out int parsedResult);

            if (!canParsed)
            {
                TBCustomSubscriberAmount.Text = "-1";

                return;
            }

            if (parsedResult < -1)
            {
                TBCustomSubscriberAmount.Text = "-1";

                return;
            }
            else
            {
                TBCustomSubscriberAmount.Text = parsedResult.ToString();

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

    private void BtnYtscToolReset_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                TBYtChannelID.Text = string.Empty;
                TBCustomSubscriberAmount.Text = "-1";
                DPCustomDate.SelectedDate = null;
                CBAddTimestamp.IsChecked = false;
                CBUseClip.IsChecked = false;
                CBBlurBackground.IsChecked = false;
                CBUseTranslate.IsChecked = false;
                CBForceChromium.IsChecked = false;
                CBIsDevelopmentMode.IsChecked = false;
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

    private void BtnYtscToolTakeScreenshot_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(async () =>
            {
                Control[] ctrlSet1 =
                {
                    TBYtChannelID,
                    TBCustomSubscriberAmount,
                    DPCustomDate,
                    CBAddTimestamp,
                    CBUseClip,
                    CBBlurBackground,
                    CBForceChromium,
                    CBIsDevelopmentMode,
                    BtnYtscToolReset,
                    BtnYtscToolTakeScreenshot
                };

                if (CBUseTranslate.IsEnabled)
                {
                    List<Control> tempList = ctrlSet1.ToList();

                    tempList.Add(CBUseTranslate);

                    ctrlSet1 = tempList.ToArray();
                }

                CustomFunction.BatchSetEnabled(ctrlSet1, false);

                if (string.IsNullOrEmpty(TBYtChannelID.Text))
                {
                    ShowMsgBox(MsgSet.MsgYtChannelIDCCantNoBeEmpty);

                    CustomFunction.BatchSetEnabled(ctrlSet1, true);

                    return;
                }

                SaveFileDialog saveFileDialog = new()
                {
                    AddExtension = true,
                    DefaultExt = ".png",
                    FileName = $"{TBYtChannelID.Text}_Screenshot_{DateTime.Now:yyyyMMddHHmmss}",
                    Filter = MsgSet.SaveImageFileFilter,
                    FilterIndex = 1,
                    Title = MsgSet.SaveImageFile
                };

                bool? dialogResult = saveFileDialog.ShowDialog();

                if (dialogResult == true)
                {
                    await OperationSet.DoTakeYtscScrnshot(
                        TBYtChannelID.Text,
                        saveFileDialog.FileName,
                        Convert.ToInt32(TBCustomSubscriberAmount.Text),
                        CBUseTranslate.IsChecked ?? false,
                        CBUseClip.IsChecked ?? false,
                        CBAddTimestamp.IsChecked ?? false,
                        DPCustomDate.SelectedDate?.ToShortDateString() ?? string.Empty,
                        CBBlurBackground.IsChecked ?? false,
                        CBForceChromium.IsChecked ?? false,
                        CBIsDevelopmentMode.IsChecked ?? false);
                }

                CustomFunction.BatchSetEnabled(ctrlSet1, true);
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