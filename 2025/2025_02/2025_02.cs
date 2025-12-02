using System.Globalization;
using System.Text.Json;
using System.Windows.Markup;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2025/day/02
/// </summary>
public class _2025_02 : Problem
{
    private Range[] _ranges = [];

    public override void Parse()
    {
        _ranges = [.. Inputs[0].Split(',').Select(l => new Range(l))];
    }

    public override object PartOne()
    {
        long result = 0;

        void Test(long value)
        {
            if (IsValid(value.ToString()))
                return;

            result += value;
        }

        foreach (Range range in _ranges)
        {
            for (long l = range.First; l <= range.Last; l++)
                Test(l);
        }

        return result;
    }

    public override object PartTwo()
    {
        long result = 0;

        void Test(long value)
        {
            if (IsValid2(value.ToString()))
                return;

            result += value;
        }

        foreach (Range range in _ranges)
        {
            for (long l = range.First; l <= range.Last; l++)
                Test(l);
        }

        return result;
    }

    private static bool IsValid(string str)
    {
        if (str.Length % 2 != 0)
            return true;

        int l = str.Length / 2;

        for (int j = 1; j < str.Length / l; j++)
        {
            for (int k = 0; k < l; k++)
            {
                if (str[k] != str[j * l + k])
                    return true;
            }
        }

        return false;
    }

    private static bool IsValid2(string str)
    {
        for (int i = 0; i < str.Length / 2; i++)
        {
            int l = i + 1;

            if (str.Length % l != 0)
                continue;

            bool same = true;

            for (int j = 1; j < str.Length / l && same; j++)
            {
                for (int k = 0; k < l && same; k++)
                {
                    if (str[k] != str[j * l + k])
                        same = false;
                }
            }

            if (same)
                return false;
        }

        return true;
    }

    private class Range
    {
        public readonly long First;
        public readonly long Last;

        public Range(string line)
        {
            long[] el = [.. line.Split('-').Select(long.Parse)];
            First = el[0];
            Last = el[1];
        }
    }
}