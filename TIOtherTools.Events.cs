using Control = System.Windows.Controls.Control;
using CustomToolbox.Common;
using CustomToolbox.Common.Sets;
using CustomToolbox.Common.Utils;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using System.Windows;
using System.Windows.Controls;
using TextBox = System.Windows.Controls.TextBox;
using CustomToolbox.Common.Extensions;

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
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }

    private void BtnYtscToolReset_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                TBYtChannelID.Text = string.Empty;
                NBCustomSubscriberAmount.Value = -1;
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
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
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
                    NBCustomSubscriberAmount,
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
                        Convert.ToInt32(NBCustomSubscriberAmount.Value),
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
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.GetExceptionMessage()));
        }
    }
}