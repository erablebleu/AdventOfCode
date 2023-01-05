namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2017/day/05
/// </summary>
public class _2017_05 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => int.Parse(l)).ToArray();
    }

    public override object PartOne() => CountSteps(_data.ToArray(), i => i + 1);

    public override object PartTwo() => CountSteps(_data.ToArray(), i => i + (i >= 3 ? -1 : 1));

    private static int CountSteps(int[] data, Func<int, int> step)
    {
        int count = 0;
        int idx = 0;

        while (idx.IsInRange(0, data.Length))
        {
            int i2 = idx + data[idx];
            data[idx] = step(data[idx]);
            idx = i2;
            count++;
        }

        return count;
    }
}