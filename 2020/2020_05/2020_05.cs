namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/05
/// </summary>
public class _2020_05 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs.Select(line => Convert.ToInt32(line.Replace("F", "0").Replace("B", "1").Replace("L", "0").Replace("R", "1"), 2)).ToArray();
    }

    public override object PartOne() => _data.Max();

    public override object PartTwo() => _data.First(p => _data.Contains(p + 2) && !_data.Contains(p + 1)) + 1;
}