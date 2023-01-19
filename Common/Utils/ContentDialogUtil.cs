using CustomToolbox.Common.Sets;
using ModernWpf.Controls;
using System.Windows;

namespace CustomToolbox.Common.Utils;

/// <summary>
/// ContentDialog 工具
/// </summary>
internal class ContentDialogUtil
{
    /// <summary>
    /// 延遲毫秒數
    /// </summary>
    public static int DelayMilliseconds = 700;

    /// <summary>
    /// 取得 Ok 的 ContentDialog
    /// </summary>
    /// <param name="message">字串，訊息</param>
    /// <param name="title">字串，標題，預設值為空白</param>
    /// <returns>ContentDialog</returns>
    public static ContentDialog GetOkDialog(
        string message,
        string title = "")
    {
        if (string.IsNullOrEmpty(title))
        {
            title = MsgSet.AppName;
        }

        return new ContentDialog()
        {
            Title = title,
            Content = message,
            CloseButtonText = MsgSet.ContentDialogBtnOk
        };
    }

    /// <summary>
    /// 取得 Ok 的 ContentDialog
    /// </summary>
    /// <param name="message">字串，訊息</param>
    /// <param name="title">字串，標題，預設值為空白</param>
    /// <param name="window">Window，預設值為 null</param>
    /// <returns>ContentDialog</returns>
    public static ContentDialog GetOkDialog(
        string message,
        string title = "",
        Window? window = null)
    {
        if (string.IsNullOrEmpty(title))
        {
            title = MsgSet.AppName;
        }

        ContentDialog contentDialog = new()
        {
            Title = title,
            Content = message,
            CloseButtonText = MsgSet.ContentDialogBtnOk
        };

        if (window != null)
        {
            contentDialog.Owner = window;
        }

        return contentDialog;
    }

    /// <summary>
    /// 取得 Confirm 的 ContentDialog
    /// </summary>
    /// <param name="message">字串，訊息</param>
    /// <param name="title">字串，標題，預設值為空白</param>
    /// <param name="window">Window，預設值為 null</param>
    /// <param name="primaryButtonText">字串，主要按鈕文字，預設值為空白</param>
    /// <param name="closeButtonText">字串，關閉按鈕文字，預設值為空白</param>
    /// <param name="secondaryButtonText">字串，第二按鈕文字，預設值為空白</param>
    /// <returns>Task&lt;ContentDialogResult&gt;</returns>
    public static ContentDialog GetConfirmDialog(
        string message,
        string title = "",
        Window? window = null,
        string primaryButtonText = "",
        string closeButtonText = "",
        string secondaryButtonText = "")
    {
        if (string.IsNullOrEmpty(title))
        {
            title = MsgSet.AppName;
        }

        if (string.IsNullOrEmpty(primaryButtonText))
        {
            primaryButtonText = MsgSet.ContentDialogBtnOk;
        }

        if (string.IsNullOrEmpty(primaryButtonText))
        {
            closeButtonText = MsgSet.ContentDialogBtnCancel;
        }

        ContentDialog contentDialog = new()
        {
            Title = title,
            Content = message,
            PrimaryButtonText = primaryButtonText,
            CloseButtonText = closeButtonText
        };

        if (!string.IsNullOrEmpty(secondaryButtonText))
        {
            contentDialog.SecondaryButtonText = secondaryButtonText;
        }

        if (window != null)
        {
            contentDialog.Owner = window;
        }

        return contentDialog;
    }
}