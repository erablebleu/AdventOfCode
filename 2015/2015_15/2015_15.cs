namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/15
/// </summary>
public class _2015_15 : Problem
{
    private int[,] _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => l.ParseExact("{0}: capacity {1}, durability {2}, flavor {3}, texture {4}, calories {5}").Skip(1).Select(e => int.Parse(e))).To2DArray();
    }

    public override object PartOne() => GetScore();

    public override object PartTwo() => GetScore(500);

    /// <summary>
    /// General solution : a bit slower than imbricated loops but works for any number of ingredients
    /// </summary>
    /// <param name="cal"></param>
    /// <returns></returns>
    private long GetScore(int? cal = null)
    {
        List<int[]> q = new() { Array.Empty<int>() };
        List<int[]> nextQ;

        for (int i = 0; i < _data.GetLength(0) - 1; i++)
        {
            nextQ = new();
            foreach (int[] item in q)
            {
                int sum = item.Sum();
                for (int j = 0; j <= 100 - sum; j++)
                {
                    int[] n = new int[item.Length + 1];
                    Array.Copy(item, n, item.Length);
                    n[^1] = j;
                    nextQ.Add(n);
                }
            }

            q = nextQ;
        }

        nextQ = new();
        foreach (int[] item in q)
        {
            int[] n = new int[item.Length + 1];
            Array.Copy(item, n, item.Length);
            n[n.Length - 1] = 100 - item.Sum();
            if (cal.HasValue && cal.Value != Enumerable.Range(0, n.Length).Sum(i => n[i] * _data[i, 4]))
                continue;
            nextQ.Add(n);
        }

        return nextQ.Max(e => GetScore(e));
    }

    private long GetScore(int[] q)
    {
        long result = 1;
        for (int i = 0; i < _data.GetLength(1) - 1; i++) // -1 to exclude calories
        {
            int sum = 0;
            for (int j = 0; j < _data.GetLength(0); j++)
                sum += q[j] * _data[j, i];

            if (sum <= 0)
                return 0;

            result *= sum;
        }
        return result;
    }
}