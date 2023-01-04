namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/03
/// </summary>
public class _2022_03 : Problem
{
    private int[][] _data;

    public static int GetPriority(char c)
    {
        if (c >= 'a' && c <= 'z') return (int)c - 'a' + 1;
        return c - 'A' + 27;
    }

    public override void Parse()
    {
        _data = Inputs.Select(l => l.Select(c => GetPriority(c)).ToArray()).ToArray();
    }

    public override object PartOne()
    {
        int prioritySum = 0;

        foreach (int[] line in _data)
        {
            int[] c1 = line.Take(line.Length / 2).ToArray();
            int[] c2 = line.Skip(line.Length / 2).ToArray();

            foreach (int c in c1)
            {
                if (!c2.Contains(c))
                    continue;
                prioritySum += c;
                break;
            }
        }

        return prioritySum;
    }

    public override object PartTwo()
    {
        int prioritySum = 0;

        for (int i = 0; i < _data.Length; i += 3)
        {
            foreach (int c in _data[i])
            {
                if (!_data[i + 1].Contains(c)
                    || !_data[i + 2].Contains(c))
                    continue;
                prioritySum += c;
                break;
            }
        }

        return prioritySum;
    }
}