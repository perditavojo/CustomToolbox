using Control = System.Windows.Controls.Control;
using CustomToolbox.Common;
using CustomToolbox.Common.Sets;
using System.Windows;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace CustomToolbox;

public partial class WMain
{
    private void BtnResetYtSct_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                TBYtChannelID.Text = string.Empty;
                NBCustomSubscriberAmount.Value = -1;
                DPCustomDate.SelectedDate = DateTime.Now;
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
                ex.ToString()));
        }
    }

    private void BtnYtSctTakeScreensshot_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(async () =>
            {
                Control[] ctrlSet1 =
                {
                    MIFetchClip,
                    MIDLClip,
                    BtnGenerateB23ClipList,
                    BtnBurnInSubtitle,
                    BtnYtSctTakeScreensshot
                };

                Control[] ctrlSet2 =
                {
                    MICancel
                };

                CustomFunction.BatchSetEnabled(ctrlSet1, false);
                CustomFunction.BatchSetEnabled(ctrlSet2, true);

                if (string.IsNullOrEmpty(TBYtChannelID.Text))
                {
                    return;
                }

                SaveFileDialog saveFileDialog = new()
                {
                    AddExtension = true,
                    DefaultExt = ".png",
                    FileName = $"Screenshot_{DateTime.Now:yyyyMMddHHmmss}",
                    Filter = "PNG 影像檔 (*.png)|*.png|JPEG 影像檔 (*.jpg)|*.jpg",
                    FilterIndex = 1
                };

                bool? dialogResult = saveFileDialog.ShowDialog();

                if (dialogResult == true)
                {
                    await OperationSet.DoTakeYtChSubsCntScrnshot(
                        TBYtChannelID.Text,
                        saveFileDialog.FileName,
                        Convert.ToInt32(NBCustomSubscriberAmount.Value),
                        CBUseTranslate.IsChecked ?? false,
                        CBUseClip.IsChecked ?? false,
                        CBAddTimestamp.IsChecked ?? false,
                        DPCustomDate.SelectedDate?.ToShortDateString() ?? string.Empty,
                        CBBlurBackground.IsChecked ?? false,
                        CBForceChromium.IsChecked ?? false,
                        CBIsDevelopmentMode.IsChecked ?? false,
                        ct: GetGlobalCT());
                }

                CustomFunction.BatchSetEnabled(ctrlSet1, true);
                CustomFunction.BatchSetEnabled(ctrlSet2, false);
            }));
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }
}