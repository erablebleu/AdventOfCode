namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/09
/// </summary>
public class _2020_09 : Problem
{
    private long[] _data;
    private long _invalidNumber;

    public override void Parse()
    {
        _data = Inputs.Select(v => long.Parse(v)).ToArray();
    }

    public override object PartOne()
    {
        _invalidNumber = 0;

        for (int i = 25; i < _data.Length; i++)
            if (!IsSumOfTwo(_data.Skip(i - 25).Take(25).ToArray(), _data[i]))
            {
                _invalidNumber = _data[i];
                return _invalidNumber;
            }

        return null;
    }

    public override object PartTwo()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            int j = 0;
            long sum = _data[i];
            for (j = i + 1; j < _data.Length && sum < _invalidNumber; j++)
                sum += _data[j];

            if (sum != _invalidNumber)
                continue;

            var arr = _data.Skip(i).Take(j - i).ToArray();

            return arr.Min() + arr.Max();
        }

        return null;
    }

    private static bool IsSumOfTwo(long[] _data, long value)
    {
        for (int i = 0; i < _data.Length; i++)
            for (int j = i + 1; j < _data.Length; j++)
                if (_data[i] + _data[j] == value)
                    return true;
        return false;
    }
}