namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/16
/// </summary>
public class _2015_16 : Problem
{
    private static readonly Dictionary<string, int> Target = new Dictionary<string, int>()
    {
        { "children", 3 },
        { "cats", 7 },
        { "samoyeds", 2 },
        { "pomeranians", 3 },
        { "akitas", 0 },
        { "vizslas", 0 },
        { "goldfish", 5 },
        { "trees", 3 },
        { "cars", 2 },
        { "perfumes", 1 },
    };

    public override void Parse()
    {
    }

    public override object PartOne()
    {
        foreach (string line in Inputs)
            if (IsSolution(line, out int number))
                return number;

        return null;
    }

    public override object PartTwo()
    {
        foreach (string line in Inputs)
            if (IsSolution(line, out int number, new string[] { "cats", "trees" }, new string[] { "pomeranians", "goldfish" }))
                return number;

        return null;
    }

    private bool IsSolution(string line, out int number, string[] greater = null, string[] fewer = null)
    {
        int idx = line.IndexOf(':');
        number = int.Parse(line.Substring(0, idx).Replace("Sue ", ""));
        greater = greater ?? new string[0];
        fewer = fewer ?? new string[0];

        foreach (string[] items in line.Substring(idx + 2).Split(", ").Select(l => l.Split(": ")))
        {
            int cnt = int.Parse(items[1]);
            if (greater.Contains(items[0]) && cnt <= Target[items[0]]
                || fewer.Contains(items[0]) && cnt >= Target[items[0]]
                || !greater.Contains(items[0]) && !fewer.Contains(items[0]) && Target[items[0]] != cnt)
                return false;
        }

        return true;
    }
}