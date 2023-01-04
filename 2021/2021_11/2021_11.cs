namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2021/day/11
/// </summary>
public class _2021_11 : Problem
{
    private int[,] _data;

    public static int NextStep(ref int[,] grid)
    {
        int cnt = 0;
        int[,] next = new int[grid.GetLength(0), grid.GetLength(1)];
        bool[,] flash = new bool[grid.GetLength(0), grid.GetLength(1)];

        for (int i = 0; i < grid.GetLength(0); i++)
            for (int j = 0; j < grid.GetLength(0); j++)
                grid[i, j]++;

        int nextCnt;
        do
        {
            nextCnt = 0;
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(0); j++)
                {
                    if (flash[i, j]
                        || grid[i, j] < 10)
                        continue;

                    flash[i, j] = true;
                    nextCnt++;

                    for (int di = -1; di < 2; di++)
                        for (int dj = -1; dj < 2; dj++)
                        {
                            int x = i + di;
                            int y = j + dj;
                            if (di == 0 && dj == 0
                                || x < 0 || x >= grid.GetLength(0)
                                || y < 0 || y >= grid.GetLength(1))
                                continue;
                            grid[x, y]++;
                        }
                }

            cnt += nextCnt;
        }
        while (nextCnt > 0);

        for (int i = 0; i < grid.GetLength(0); i++)
            for (int j = 0; j < grid.GetLength(0); j++)
                next[i, j] = flash[i, j] ? 0 : grid[i, j];

        grid = next;
        return cnt;
    }

    public override void Parse()
    {
        _data = Inputs.Select(l => l.Select(c => int.Parse(c.ToString()))).To2DArray();
    }

    public override object PartOne()
    {
        int cnt = 0;
        for (int i = 0; i < 100; i++)
            cnt += NextStep(ref _data);

        return cnt;
    }

    public override object PartTwo()
    {
        int cnt = 100;
        while (NextStep(ref _data) < 100)
            cnt++;

        return cnt + 1;
    }
}