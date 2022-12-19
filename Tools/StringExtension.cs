using System.Text.RegularExpressions;

namespace AdventOfCode.Tools;

public static class StringExtensions
{
    public static string[] ParseExact(this string data, string format) => ParseExact(data, format, false);

    public static string[] ParseExact(this string data, string format, bool ignoreCase)
    {
        if (TryParseExact(data, format, out string[] values, ignoreCase))
            return values;
        else
            throw new ArgumentException("Format not compatible with value.");
    }

    public static bool TryExtract(this string data, string format, out string[] values) => TryParseExact(data, format, out values, false);

    public static bool TryParseExact(this string data, string format, out string[] values, bool ignoreCase)
    {
        format = Regex.Escape(format).Replace("\\{", "{");

        int tokenCount;
        for (tokenCount = 0; ; tokenCount++)
        {
            string token = string.Format("{{{0}}}", tokenCount);
            if (!format.Contains(token)) break;
            format = format.Replace(token,
                string.Format("(?'group{0}'.*)", tokenCount));
        }

        RegexOptions options = ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;

        Match match = new Regex(format, options).Match(data);

        if (tokenCount != match.Groups.Count - 1)
        {
            values = Array.Empty<string>();
            return false;
        }
        else
        {
            values = new string[tokenCount];
            for (int index = 0; index < tokenCount; index++)
                values[index] = match.Groups[string.Format("group{0}", index)].Value;
            return true;
        }
    }
}