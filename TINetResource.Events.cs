﻿using CheckBox = System.Windows.Controls.CheckBox;
using Control = System.Windows.Controls.Control;
using CustomToolbox.Common;
using CustomToolbox.Common.Extensions;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Sets;
using CustomToolbox.Common.Utils;
using System.Windows;
using System.Windows.Controls;
using TextBox = System.Windows.Controls.TextBox;

namespace CustomToolbox;

/// <summary>
/// TINetResource 的控制項事件
/// </summary>
public partial class WMain
{
    private void CBAutoLyric_Checked(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CheckBox control = (CheckBox)sender;

                bool value = control.IsChecked ?? true;

                if (value)
                {
                    if (Properties.Settings.Default.NetPlaylistAutoLyric != value)
                    {
                        Properties.Settings.Default.NetPlaylistAutoLyric = value;
                        Properties.Settings.Default.Save();
                    }
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

    private void CBAutoLyric_Unchecked(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CheckBox control = (CheckBox)sender;

                bool value = control.IsChecked ?? false;

                if (!value)
                {
                    if (Properties.Settings.Default.NetPlaylistAutoLyric != value)
                    {
                        Properties.Settings.Default.NetPlaylistAutoLyric = value;
                        Properties.Settings.Default.Save();
                    }
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

    private void BtnRefreshNetResurce_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            InitNetResurce();
        }
        catch (Exception ex)
        {
            WriteLog(MsgSet.GetFmtStr(
                MsgSet.MsgErrorOccured,
                ex.ToString()));
        }
    }

    private void BtnLoadClipList_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Dispatcher.BeginInvoke(new Action(async () =>
            {
                ClipListData netData = (ClipListData)CBNetPlaylists.SelectedItem;
                ClipListData localData = (ClipListData)CBLocalClipLists.SelectedItem;

                if (netData.Name != MsgSet.SelectPlease &&
                    localData.Name != MsgSet.SelectPlease)
                {
                    ShowMsgBox(MsgSet.MsgSelectAClipListAtSameTime);

                    return;
                }
                else if (netData.Name == MsgSet.SelectPlease &&
                    localData.Name == MsgSet.SelectPlease)
                {
                    ShowMsgBox(MsgSet.MsgSelectAValidClipList);

                    return;
                }
                else
                {
                    if (netData.Name != MsgSet.SelectPlease)
                    {
                        List<List<object>> dataSource = await ClipListUtil.GetNetPlaylist(netData.Path);

                        DGClipList.ImportData(dataSource);

                        if (CBNetPlaylists.Items.Count > 0)
                        {
                            CBNetPlaylists.SelectedIndex = 0;
                        }
                    }
                    else if (localData.Name != MsgSet.SelectPlease)
                    {
                        DoLoadClipLists(new List<string>()
                        {
                            localData.Path
                        });

                        if (CBLocalClipLists.Items.Count > 0)
                        {
                            CBLocalClipLists.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        return;
                    }
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

    private void TBB23ClipListExcludedPhrases_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (!IsInitializing)
            {
                TextBox control = (TextBox)sender;

                string[] tempValue = control.Text.Split(
                    Environment.NewLine.ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries);

                string value = string.Join(";", tempValue);

                if (Properties.Settings.Default.B23ClipListExcludedPhrases != value)
                {
                    Properties.Settings.Default.B23ClipListExcludedPhrases = value;
                    Properties.Settings.Default.Save();

                    WriteLog(MsgSet.MsgUpdateLB23ClipListExcludedPhrases);

                    Task.Delay(1500).ContinueWith(t => 
                    {
                        IsInitializing = true;

                        InitB23ClipListExcludedPhrases();

                        IsInitializing = false;
                    });
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

    private void BtnGenerateB23ClipList_Click(object sender, RoutedEventArgs e)
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
                    BtnBurnInSubtitle
                };

                Control[] ctrlSet2 =
                {
                    MICancel
                };

                CustomFunction.BatchSetEnabled(ctrlSet1, false);
                CustomFunction.BatchSetEnabled(ctrlSet2, true);

                if (string.IsNullOrEmpty(TBB23UserMID.Text))
                {
                    ShowMsgBox(MsgSet.MsgB23UserMidCantBeEmpty);

                    // 重設控制項。
                    await Dispatcher.BeginInvoke(new Action(() =>
                    {
                        TBB23UserMID.Text = string.Empty;
                        CBB23ClipListExportJsonc.IsChecked = false;
                    }));

                    CustomFunction.BatchSetEnabled(ctrlSet1, true);
                    CustomFunction.BatchSetEnabled(ctrlSet2, false);

                    return;
                }
                await OperationSet.DoGenerateB23ClipList(
                    TBB23UserMID.Text,
                    CBB23ClipListExportJsonc.IsChecked ?? false,
                    GetGlobalCT());

                // 重設控制項。
                await Task.Delay(1500).ContinueWith(t =>
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        TBB23UserMID.Text = string.Empty;
                        CBB23ClipListExportJsonc.IsChecked = false;
                    }));
                });

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