using AdventOfCode.Tools;

namespace AdventOfCode;

public class _2022_12 : Problem
{
    private static readonly IVector2D[] Directions = new IVector2D[]
        {
            new IVector2D(-1, 0),
            new IVector2D(1, 0),
            new IVector2D(0, -1),
            new IVector2D(0, 1),
        };

    public override void Solve()
    {
        int[,] data = Inputs.Select(l => l.Select(c => c - 'a')).To2DArray();
        int[,] map = new int[data.GetLength(0), data.GetLength(1)];
        IPoint2D s = new();
        IPoint2D e = new();
        int step = 0;
        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                map[i, j] = int.MaxValue;
                if (data[i, j] == 'S' - 'a')
                {
                    s = new IPoint2D(i, j);
                    data[i, j] = 0;
                }
                if (data[i, j] == 'E' - 'a')
                {
                    e = new IPoint2D(i, j);
                    data[i, j] = 25;
                    map[i, j] = 0;
                }
            }
        }

        int GetVal(IPoint2D p) => data[p.X, p.Y];
        bool IsInclude(IPoint2D p) => p.X >= 0 && p.Y >= 0 && p.X < data.GetLength(0) && p.Y < data.GetLength(1);

        int newCnt;
        do
        {
            newCnt = 0;
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    IPoint2D p = new(i, j);
                    if (p == e) continue;
                    if (map[p.X, p.Y] < int.MaxValue) continue;

                    int min = int.MaxValue;

                    foreach (IVector2D direction in Directions)
                    {
                        IPoint2D p2 = p + direction;
                        if (!IsInclude(p2)) continue;
                        if (map[p2.X, p2.Y] >= min) continue;
                        if (GetVal(p2) > GetVal(p) + 1) continue;
                        min = map[p2.X, p2.Y];
                    }
                    if (min == int.MaxValue)
                        continue;
                    map[p.X, p.Y] = min + 1;
                    newCnt++;
                }
            }
        }
        while (newCnt > 0);

        Solutions.Add($"{map[s.X, s.Y]}");

        int minASteps = map[s.X, s.Y];
        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                if (data[i, j] == 0 && map[i, j] < minASteps)
                    minASteps = map[i, j];
            }
        }
        Solutions.Add($"{minASteps}");
    }
}