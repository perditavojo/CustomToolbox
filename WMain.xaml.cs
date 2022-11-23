using CustomToolbox.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomToolbox;

/// <summary>
/// Interaction logic for WMain.xaml
/// </summary>
public partial class WMain : Window
{
    public WMain()
    {
        InitializeComponent();
    }

    private void WMain_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            CustomInit();
        }
        catch (Exception ex)
        {
            WriteLog($"發生錯誤：{ex}");
        }
    }

    private void WMain_Closing(object sender, CancelEventArgs e)
    {
        try
        {
            MPlayer?.Dispose();
        }
        catch (Exception ex)
        {
            WriteLog($"發生錯誤：{ex}");
        }
    }

    private void MILoadPlaylistFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // TODO: 2022-11-23 待完成相關功能。
        }
        catch (Exception ex)
        {
            WriteLog($"發生錯誤：{ex}");
        }
    }

    private void MISavePlaylistFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // TODO: 2022-11-23 待完成相關功能。
        }
        catch (Exception ex)
        {
            WriteLog($"發生錯誤：{ex}");
        }
    }

    private void MIAbuot_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // TODO: 2022-11-23 待完成相關功能。
        }
        catch (Exception ex)
        {
            WriteLog($"發生錯誤：{ex}");
        }
    }

    private void DGClipList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            if (sender is DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem != null)
                {
                    if (dataGrid.CurrentCell.Item is ClipData clipData &&
                        !string.IsNullOrEmpty(clipData.VideoUrlOrID))
                    {
                        PlayMedia(clipData.VideoUrlOrID);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            WriteLog($"發生錯誤：{ex}");
        }
    }
}