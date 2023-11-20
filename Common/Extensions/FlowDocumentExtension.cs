using System.Globalization;
using System.Text;
using System.Windows.Documents;
using System.Windows.Media;

namespace CustomToolbox.Common.Extensions;

/// <summary>
/// FlowDocument 的擴充方法
/// <para>Source: https://learn.microsoft.com/en-us/answers/questions/22612/wpf-auto-width-for-flowdocuments-content</para>
/// <para>Author: Alex Li-MSFT</para>
/// </summary>
public static class FlowDocumentExtension
{
    /// <summary>
    /// 取得 Run 以及 Paragraph
    /// </summary>
    /// <param name="flowDocument">FlowDocument</param>
    /// <returns>IEnumerable&lt;TextElement&gt;</returns>
    private static IEnumerable<TextElement> GetRunsAndParagraphs(FlowDocument flowDocument)
    {
        for (TextPointer position = flowDocument.ContentStart;
          position != null && position.CompareTo(flowDocument.ContentEnd) <= 0;
          position = position.GetNextContextPosition(LogicalDirection.Forward))
        {
            if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd)
            {
                if (position.Parent is Run run)
                {
                    yield return run;
                }
                else
                {
                    if (position.Parent is Paragraph paragraph)
                    {
                        yield return paragraph;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 取得 FormattedText
    /// </summary>
    /// <param name="flowDocument">FlowDocument</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static FormattedText GetFormattedText(this FlowDocument flowDocument, double pixelsPerDip)
    {
        ArgumentNullException.ThrowIfNull(flowDocument);

        Typeface typeface = new(
            fontFamily: flowDocument.FontFamily,
            style: flowDocument.FontStyle,
            weight: flowDocument.FontWeight,
            stretch: flowDocument.FontStretch);

        // double pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;

        FormattedText formattedText = new(
            textToFormat: GetText(flowDocument),
            culture: CultureInfo.CurrentCulture,
            flowDirection: flowDocument.FlowDirection,
            typeface: typeface,
            emSize: flowDocument.FontSize,
            foreground: flowDocument.Foreground,
            pixelsPerDip: pixelsPerDip);

        int offset = 0;

        foreach (TextElement textElement in GetRunsAndParagraphs(flowDocument))
        {
            if (textElement is Run run)
            {
                int count = run.Text.Length;

                formattedText.SetFontFamily(run.FontFamily, offset, count);
                formattedText.SetFontStyle(run.FontStyle, offset, count);
                formattedText.SetFontWeight(run.FontWeight, offset, count);
                formattedText.SetFontSize(run.FontSize, offset, count);
                formattedText.SetForegroundBrush(run.Foreground, offset, count);
                formattedText.SetFontStretch(run.FontStretch, offset, count);
                formattedText.SetTextDecorations(run.TextDecorations, offset, count);

                offset += count;
            }
            else
            {
                offset += Environment.NewLine.Length;
            }
        }

        return formattedText;
    }

    /// <summary>
    /// 取得文字
    /// </summary>
    /// <param name="flowDocument">FlowDocument</param>
    /// <returns>字串</returns>
    private static string GetText(FlowDocument flowDocument)
    {
        StringBuilder stringBuilder = new();

        foreach (TextElement textElement in GetRunsAndParagraphs(flowDocument))
        {
            stringBuilder.Append(textElement is not Run run ? Environment.NewLine : run.Text);
        }

        return stringBuilder.ToString();
    }
}
