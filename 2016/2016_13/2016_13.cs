namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/13
/// </summary>
public class _2016_13 : Problem
{
    private int _data;

    public override void Parse()
    {
        _data = int.Parse(Inputs[0]);
    }

    public override object PartOne() => GetHeatMap()[31, 39];

    public override object PartTwo() => GetHeatMap().Count(i => i >= 0 && i <= 50);

    private static bool[,] GetGrid(int width, int height, int key)
    {
        bool[,] result = new bool[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                result[x, y] = GetValue(x, y, key);

        return result;
    }

    private static bool GetValue(int x, int y, int key)
    {
        long value = (long)Math.Pow(x + y, 2) + 3 * x + y + key;
        int cnt = 0;
        while (value > 0)
        {
            if (value % 2 > 0) cnt++;
            value /= 2;
        }
        return cnt % 2 != 0;
    }

    private int[,] GetHeatMap()
    {
        bool[,] walls = GetGrid(100, 100, _data);
        int[,] grid = new int[walls.GetLength(0), walls.GetLength(1)];
        int cnt;
        for (int x = 0; x < walls.GetLength(0); x++)
            for (int y = 0; y < walls.GetLength(1); y++)
                grid[x, y] = -1;
        grid[1, 1] = 0;

        //Console.WriteLine();
        //for (int y = 0; y < walls.GetLength(1); y++)
        //    Console.WriteLine(string.Join("", Enumerable.Range(0, walls.GetLength(0)).Select(x => walls[x, y] ? "#" : ".")));

        do
        {
            cnt = 0;
            for (int x = 0; x < walls.GetLength(0); x++)
            {
                for (int y = 0; y < walls.GetLength(1); y++)
                {
                    if (walls[x, y] || grid[x, y] < 0)
                        continue;

                    foreach (IVector2D dir in IVector2D.DirectionNESW)
                    {
                        int x2 = x + dir.X;
                        int y2 = y + dir.Y;
                        if (!x2.IsInRange(0, 100)
                            || !y2.IsInRange(0, 100)
                            || walls[x2, y2]
                            || grid[x2, y2] >= 0)
                            continue;
                        grid[x2, y2] = grid[x, y] + 1;
                        cnt++;
                    }
                }
            }
        }
        while (cnt > 0);

        return grid;
    }
}