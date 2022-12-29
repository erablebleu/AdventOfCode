namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/20
/// </summary>
public class _2015_20 : Problem
{
    private int _data;

    public override void Parse()
    {
        _data = int.Parse(Inputs[0]);
    }

    public override object PartOne()
    {
        for (int i = 1; i < _data / 10; i++)
        {
            int sum = 0;
            for (int j = 1; j < Math.Sqrt(i); j++)
                if (i % j == 0)
                    sum += (j + i / j) * 10;
            if (Math.Pow((int)Math.Sqrt(i), 2) == i)
                sum -= 10 * (int)Math.Sqrt(i);
            if (sum >= _data)
                return i;
        }
        return null;
    }

    public override object PartTwo()
    {
        Dictionary<int, int> map = new();
        int max = int.MaxValue;
        for (int i = 1; i < max; i++)
        {
            int a = 11 * i;
            for (int j = i, cnt = 0; cnt < 50 && j < max; j += i, cnt++) // house
            {
                if (map.ContainsKey(j))
                    map[j] += a;
                else
                    map[j] = a;

                if (map[j] > _data)
                    max = j;
            }
        }
        return max;
    }
}