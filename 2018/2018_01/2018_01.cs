namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2018/day/01
/// </summary>
public class _2018_01 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs.Select(e => int.Parse(e)).ToArray();
    }

    public override object PartOne()
        => _data.Sum();

    public override object PartTwo()
    {
        List<int> reached = new();
        int value = 0;
        int idx = 0;

        while (!reached.Contains(value)) 
        {
            reached.Add(value);
            value += _data[idx];
            idx = (idx+1) % _data.Length;
        }
        return value;
    }
}