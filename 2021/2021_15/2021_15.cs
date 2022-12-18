using AdventOfCode.Tools;

namespace AdventOfCode;

public class _2021_15 : Problem
{
    public override void Solve()
    {
        int[,] grid = Inputs.Select(l => l.Select(c => int.Parse(c.ToString()))).To2DArray();

        Solutions.Add($"{GetLast(GetRiskGrid(grid))}");
        Solutions.Add($"{GetLast(GetRiskGrid2(grid))}");
        Solutions.Add($"{GetLast(GetRiskGrid(RepeatGrid(grid, 5)))}");
        Solutions.Add($"{GetLast(GetRiskGrid2(RepeatGrid(grid, 5)))}");
    }

    private static List<System.Drawing.Point> GetDiag(int[,] grid, int x)
    {
        List<System.Drawing.Point> result = new();
        int y = x >= (grid.GetLength(0) - 1) ? x - (grid.GetLength(0) - 1) : 0;
        x = Math.Min(x, grid.GetLength(0) - 1);

        while (x >= 0 && y < grid.GetLength(0))
        {
            result.Add(new System.Drawing.Point(x, y));
            x -= 1;
            y += 1;
        }
        return result;
    }

    private static int GetLast(int[,] grid) => grid[grid.GetLength(0) - 1, grid.GetLength(1) - 1];


    private static IEnumerable<int> GetPrevPoint(int[,] risk, System.Drawing.Point p)
    {
        if (p.X > 0)
            yield return risk[p.X - 1, p.Y];
        if (p.Y > 0)
            yield return risk[p.X, p.Y - 1];
        yield break;
    }
    private static IEnumerable<System.Drawing.Point> GetAdjPoint(int[,] grid, System.Drawing.Point p) => GetAdjPoint(grid, p.X, p.Y);
    private static IEnumerable<System.Drawing.Point> GetAdjPoint(int[,] grid, int x, int y)
    {
        if (x > 0) yield return new System.Drawing.Point(x - 1, y);
        if (y > 0) yield return new System.Drawing.Point(x, y - 1);
        if (x < grid.GetLength(0) - 1) yield return new System.Drawing.Point(x + 1, y);
        if (y < grid.GetLength(1) - 1) yield return new System.Drawing.Point(x, y + 1);
        yield break;
    }

    private static int[,] GetRiskGrid(int[,] grid)
    {
        int[,] risk = new int[grid.GetLength(0), grid.GetLength(1)];
        List<System.Drawing.Point> diag;
        int cnt = 1;
        do
        {
            diag = GetDiag(grid, cnt);
            cnt++;

            foreach (System.Drawing.Point p in diag)
            {
                risk[p.X, p.Y] = grid[p.X, p.Y] + GetPrevPoint(risk, p).Min();
            }
        }
        while (diag.Count > 1);

        return risk;
    }

    private static int[,] GetRiskGrid2(int[,] grid) // using Dijkstra
    {
        int[,] risk = new int[grid.GetLength(0), grid.GetLength(1)];
        System.Drawing.Point target = new(grid.GetLength(0) - 1, grid.GetLength(1) - 1);
        List<System.Drawing.Point> interrest = new();

        for (int x = 0; x < risk.GetLength(0); x++)
            for (int y = 0; y < risk.GetLength(1); y++)
                risk[x, y] = int.MaxValue;

        risk[0, 0] = 0;
        interrest.Add(new System.Drawing.Point(0, 0));

        while (interrest.Any())
        {
            System.Drawing.Point p = interrest.Distinct().OrderBy(p => risk[p.X, p.Y]).First();
            interrest.Remove(p);

            foreach(var np in GetAdjPoint(grid, p))
            {
                if (risk[np.X, np.Y] <= grid[np.X, np.Y] + risk[p.X, p.Y])
                    continue;

                risk[np.X, np.Y] = grid[np.X, np.Y] + risk[p.X, p.Y];
                interrest.Add(np);
            }
        }

        return risk;
    }

    private static int[,] RepeatGrid(int[,] grid, int cnt)
    {
        int lx = grid.GetLength(0);
        int ly = grid.GetLength(1);
        int[,] result = new int[lx * cnt, ly * cnt];
        for (int i = 0; i < cnt; i++)
            for (int j = 0; j < cnt; j++)
                for (int x = 0; x < lx; x++)
                    for (int y = 0; y < ly; y++)
                    {
                        result[i * lx + x, j * ly + y] = (grid[x, y] + i + j - 1) % 9 + 1;
                    }

        return result;
    }
}