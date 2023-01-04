namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2017/day/01
/// </summary>
public class _2017_01 : Problem
{
    private string _data;

    public override void Parse()
    {
        _data = Inputs[0];
    }

    public override object PartOne() => _data.Select((c, i) => _data[i] == _data[(i + 1).Loop(0, _data.Length)] ? _data[i] - '0' : 0).Sum();

    public override object PartTwo() => _data.Select((c, i) => _data[i] == _data[(i + _data.Length / 2).Loop(0, _data.Length)] ? _data[i] - '0' : 0).Sum();
}