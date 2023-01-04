namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/06
/// </summary>
public class _2020_06 : Problem
{
    public override void Parse()
    {
        for (int i = 0; i < Inputs.Length; i++)
            if (Inputs[i] == string.Empty)
                Inputs[i] = "#";
    }

    public override object PartOne() => string.Join("", Inputs).Split("#").Sum(l => l.GroupBy(c => c).Count());

    public override object PartTwo() => string.Join("/", Inputs).Split("#").Sum(l => l.Replace("/", "").GroupBy(c => c).Count(g => g.Count() == l.Split("/", System.StringSplitOptions.RemoveEmptyEntries).Length));
}