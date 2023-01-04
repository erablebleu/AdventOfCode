namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2021/day/01
/// </summary>
public class _2021_01 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs.Select(v => int.Parse(v)).ToArray();
    }

    public override object PartOne() => _data.Zip(_data.Skip(1)).Where(v => v.Second > v.First).Count();

    public override object PartTwo()
    {
        var slidingWin = _data.Skip(2).Select((v, i) => _data[i] + _data[i + 1] + _data[i + 2]).ToList();
        return slidingWin.Zip(slidingWin.Skip(1)).Where(v => v.Second > v.First).Count();
    }
}