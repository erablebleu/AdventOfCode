namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2017/day/02
/// </summary>
public class _2017_02 : Problem
{
    private int[][] _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => l.Split('\t', StringSplitOptions.RemoveEmptyEntries).Select(e => int.Parse(e)).ToArray()).ToArray();
    }

    public override object PartOne() => _data.Sum(l => l.Max() - l.Min());

    public override object PartTwo() => _data.Sum(l => GetLineCheck(l));

    private static int GetLineCheck(int[] l)
    {
        for (int i = 0; i < l.Length; i++)
            for (int j = 0; j < l.Length; j++)
                if (i != j && l[i] % l[j] == 0)
                    return l[i] / l[j];
        return 0;
    }
}