namespace AdventOfCode;

public class _2015_02 : Problem
{
    private int[][] _data;
    public override void Parse()
    {
        _data = Inputs.Select(l => l.Split("x").Select(d => int.Parse(d)).ToArray()).ToArray();
    }

    public override object PartOne()
    {
        return _data.Sum(p => 2 * (p[0] * p[1] + p[0] * p[2] + p[1] * p[2]) + Math.Min(p[0] * p[1], Math.Min(p[0] * p[2], p[1] * p[2])));
    }

    public override object PartTwo()
    {
        return _data.Sum(p => 2 * Math.Min(p[0] + p[1], Math.Min(p[0] + p[2], p[1] + p[2])) + p[0] * p[1] * p[2]);
    }
}