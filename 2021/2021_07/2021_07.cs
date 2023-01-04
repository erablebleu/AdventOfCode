namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2021/day/07
/// </summary>
public class _2021_07 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs.First().Split(",").Select(v => int.Parse(v)).ToArray();
    }

    public override object PartOne() => Minimize(_data, (v, i) => Math.Abs(v - i));

    public override object PartTwo() => Minimize(_data, (v, i) => Math.Abs(v - i) * (Math.Abs(v - i) + 1) / 2);

    private static int Minimize(int[] values, Func<int, int, int> predicate)
    {
        int[] ordered = values.OrderBy(v => v).ToArray();

        int value = 0;
        int result = int.MaxValue;

        for (int i = ordered.First(); i < ordered.Last(); i++)
        {
            int sum = values.Sum(v => predicate.Invoke(v, i));

            if (sum >= result)
                continue;

            result = sum;
            value = i;
        }

        return values.Sum(v => predicate.Invoke(v, value));
    }
}