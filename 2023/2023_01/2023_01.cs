namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2023/day/01
/// </summary>
public class _2023_01 : Problem
{
    private static readonly Dictionary<string, char> Digits = new() {
        { "one", '1' },
        { "two", '2' },
        { "three", '3' },
        { "four", '4' },
        { "five", '5' },
        { "six", '6' },
        { "seven", '7' },
        { "eight", '8' },
        { "nine", '9' },
    };

    public override void Parse()
    { }

    public override object PartOne()
        => Inputs.Sum(l => GetValue(GetDigits(l, false)));

    public override object PartTwo()
        => Inputs.Sum(l => GetValue(GetDigits(l, true)));

    private static string GetDigits(string s, bool text)
    {
        string result = string.Empty;

        for(int i = 0; i < s.Length; i++)
        {
            if (s[i] >= '0' && s[i] <= '9')
                result += s[i];

            if (!text)
                continue;

            foreach(KeyValuePair<string, char> kv in Digits)
            {
                if (kv.Key.Length > s.Length - i
                    || s.Substring(i, kv.Key.Length) != kv.Key)
                    continue;

                result += kv.Value;
            }
        }

        return result;
    }

    private static int GetValue(string s)
        => int.Parse($"{s.First()}{s.Last()}");

}