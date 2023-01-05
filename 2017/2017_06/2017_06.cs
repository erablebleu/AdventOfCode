namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2017/day/06
/// </summary>
public class _2017_06 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs[0].Split('\t').Select(e => int.Parse(e)).ToArray();
    }

    public override object PartOne() => Emulate(_data.ToArray(), true);

    public override object PartTwo() => Emulate(_data.ToArray(), false);

    private static int Emulate(int[] data, bool cycleCount)
    {
        HashSet<string> set = new() { GetHash(data) };
        int count = 0;

        while (true)
        {
            count++;
            int val = data.Max();
            int idx = Array.IndexOf(data, val);
            data[idx] = 0;
            for (int i = 0; i < val; i++)
                data[(idx + i + 1).Loop(0, data.Length)]++;

            string hash = GetHash(data);
            if (set.Contains(hash))
                return cycleCount ? count : count - Array.IndexOf(set.ToArray(), hash);

            set.Add(hash);
        }
    }

    private static string GetHash(int[] data) => string.Join('-', data);
}