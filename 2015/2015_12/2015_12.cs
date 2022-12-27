using System.Text.RegularExpressions;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/12
/// </summary>
public class _2015_12 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne() => new Regex("-?[0-9]+").Matches(Inputs[0]).Sum(m => int.Parse(m.Value));

    public override object PartTwo()
    {
        string value = Inputs[0];
        while (RemoveSection(value, "red", out value)) ;
        return new Regex("-?[0-9]+").Matches(value).Sum(m => int.Parse(m.Value));
    }

    private static bool RemoveSection(string value, string toRemove, out string result)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] != '{')
                continue;

            int cnt = 0;
            int j;
            bool remove = false;
            for (j = i + 1; j < value.Length; j++)
            {
                if (value[j] == '{' || value[j] == '[')
                    cnt++;
                if (value[j] == '}' || value[j] == ']')
                {
                    if (cnt == 0)
                        break;
                    else
                        cnt--;
                }
                if (j <= value.Length - toRemove.Length && value.Substring(j, toRemove.Length) == toRemove && cnt == 0)
                    remove = true;
            }
            if (remove)
            {
                result = value.Substring(0, i) + value.Substring(j + 1);
                return true;
            }
        }
        result = value;
        return false;
    }
}