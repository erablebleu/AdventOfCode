namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/17
/// </summary>
public class _2015_17 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => int.Parse(l)).ToArray();
    }

    public override object PartOne() => CombinatoryHelper.GetCombination(_data, Enumerable.Sum, 150).Count();

    public override object PartTwo()
    {
        int[][] combs = CombinatoryHelper.GetCombination(_data, Enumerable.Sum, 150).ToArray();
        int cnt = combs.Select(c => c.Length).Min();
        return combs.Count(c => c.Length == cnt);
    }
}