namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/01
/// </summary>
public class _2020_01 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs.Select(i => int.Parse(i)).ToArray();
    }

    public override object PartOne()
    {
        for (int i = 0; i < _data.Length; i++)
            for (int j = i + 1; j < _data.Length; j++)
                if (_data[i] + _data[j] == 2020)
                    return _data[i] * _data[j];

        return null;
    }

    public override object PartTwo()
    {
        for (int i = 0; i < _data.Length; i++)
            for (int j = i + 1; j < _data.Length; j++)
                for (int k = j + 1; k < _data.Length; k++)
                    if (_data[i] + _data[j] + _data[k] == 2020)
                        return _data[i] * _data[j] * _data[k];

        return null;
    }
}