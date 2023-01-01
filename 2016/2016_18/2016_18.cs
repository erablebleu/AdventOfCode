namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/18
/// </summary>
public class _2016_18 : Problem
{
    private bool[] _data;

    public override void Parse()
    {
        _data = Inputs[0].Select(c => c == '^').ToArray();
    }

    public override object PartOne() => CountSafeTiles(_data, 40);

    public override object PartTwo() => CountSafeTiles(_data, 400000);

    private static int CountSafeTiles(bool[] data, int count)
    {
        bool[] next = new bool[data.Length];
        bool Get(int x) => x.IsInRange(0, data.Length) && data[x];
        int cnt = data.Count(b => !b);
        for (int y = 1; y < count; y++)
        {
            for (int x = 0; x < data.Length; x++)
            {
                bool state = Get(x - 1) ^ Get(x + 1);
                next[x] = state;
                if (!state)
                    cnt++;
            }
            bool[] tmp = data;
            data = next;
            next = tmp;
        }

        return cnt;
    }
}