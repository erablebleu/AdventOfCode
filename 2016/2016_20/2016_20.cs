namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/20
/// </summary>
public class _2016_20 : Problem
{
    private long[][] _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => l.Split('-').Select(e => long.Parse(e)).ToArray()).ToArray();
    }

    public override object PartOne()
    {
        LMultiRange mr = new();

        foreach (long[] range in _data)
            mr.Add(range[0], range[1]);

        return mr.GetFirstOut();
    }

    public override object PartTwo()
    {
        LMultiRange mr = new();

        foreach (long[] range in _data)
            mr.Add(range[0], range[1]);

        return mr.CountOut(0, 4294967295);
    }
}