namespace CustomToolbox.Common.Extensions;

/// <summary>
/// String 的擴充方法
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// 依據數值陣列分割字串至字串陣列
    /// <para>Source: https://stackoverflow.com/a/28950853</para>
    /// <para>Author: Igor Sheva</para>
    /// <para>Licence: CC BY-SA 3.0</para>
    /// <para>CC BY-SA 3.0: https://creativecommons.org/licenses/by-sa/3.0/</para>
    /// </summary>
    /// <param name="source">string</param>
    /// <param name="sizes">數值陣列</param>
    /// <returns></returns>
    public static string[]? Split(this string source, params int[] sizes)
    {
        int length = sizes.Sum();

        if (length > source.Length)
        {
            return null;
        }

        int resultSize = sizes.Length;

        if (length < source.Length)
        {
            resultSize++;
        }

        string[] result = new string[resultSize];

        int start = 0;

        for (int i = 0; i < resultSize; i++)
        {
            if (i + 1 == resultSize)
            {
                result[i] = source[start..];

                break;
            }

            result[i] = source.Substring(start, sizes[i]);

            start += sizes[i];
        }

        return result;
    }
}