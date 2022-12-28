namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/18
/// </summary>
public class _2015_18 : Problem
{
    private bool[,] _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => l.Select(c => c == '#')).To2DArray();
    }

    public override object PartOne() => Emulate(_data, 100).Count(true);

    public override object PartTwo() => Emulate(_data, 100, true).Count(true);

    private static bool[,] Emulate(bool[,] data, int count, bool step2 = false)
    {
        int w = data.GetLength(0);
        int h = data.GetLength(1);

        void SetAngle(bool[,] d, bool value = true)
        {
            d[0, 0] = value;
            d[w - 1, 0] = value;
            d[0, h - 1] = value;
            d[w - 1, h - 1] = value;
        }
        if (step2)
            SetAngle(data);

        for (int i = 0; i < count; i++)
        {
            bool[,] result = new bool[w, h];

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int cnt = 0;
                    for (int x2 = Math.Max(0, x - 1); x2 < Math.Min(w, x + 2); x2++)
                        for (int y2 = Math.Max(0, y - 1); y2 < Math.Min(h, y + 2); y2++)
                            if ((x != x2 || y != y2) && data[x2, y2])
                                cnt++;

                    result[x, y] = data[x, y] && cnt >= 2 && cnt <= 3
                        || !data[x, y] && cnt == 3;
                }
            }

            if (step2)
                SetAngle(result);

            data = result;
        }

        return data;
    }
}