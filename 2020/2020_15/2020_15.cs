namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/15
/// </summary>
public class _2020_15 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs[0].Split(",").Select(i => int.Parse(i)).ToArray();
    }

    public override object PartOne() => PlayN(2020, _data);

    public override object PartTwo() => PlayN(30000000, _data);

    private static int PlayN(int roundCount, int[] input)
    {
        Dictionary<int, int> data = new Dictionary<int, int>();
        int last = 0;

        for (int i = 0; i < roundCount; i++)
        {
            int n;

            if (i < input.Length)
                n = input[i];
            else if (data.ContainsKey(last))
                n = i - data[last];
            else
                n = 0;

            if (i > 0) data[last] = i;
            last = n;
        }
        return last;
    }
}