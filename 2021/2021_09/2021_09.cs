using AdventOfCode.Tools;

namespace AdventOfCode;

public static class EnumerableExtension
{
    public static void Product(this Dictionary<int, long> dic, int key, long value)
    {
        if (dic.ContainsKey(key))
            dic[key] += value;
        else
            dic.Add(key, value);
    }
}

public class _2021_09 : Problem
{
    public override void Solve()
    {
        int[][] grid = Inputs.Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
        List<System.Drawing.Point> lowPoints = new();

        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[i].Length; j++)
            {
                if (i > 0 && grid[i - 1][j] <= grid[i][j]) continue;
                if (j > 0 && grid[i][j - 1] <= grid[i][j]) continue;
                if (i < grid.Length - 1 && grid[i + 1][j] <= grid[i][j]) continue;
                if (j < grid[i].Length - 1 && grid[i][j + 1] <= grid[i][j]) continue;
                lowPoints.Add(new System.Drawing.Point(i, j));
            }
        }

        Solutions.Add($"{lowPoints.Sum(p => grid[p.X][p.Y] + 1)}");
        Solutions.Add($"{lowPoints.Select(p => GetBasinSize(grid, p)).OrderByDescending(v => v).Take(3).Product()}");
        var toto = lowPoints.Select(p => GetBasinSize(grid, p)).ToList();
    }

    private static int GetBasinSize(int[][] grid, System.Drawing.Point lowPoint)
    {
        List<System.Drawing.Point> points = new() { lowPoint };
        List<System.Drawing.Point> pointsToCheck = new() { lowPoint };

        while(pointsToCheck.Any())
        {
            List<System.Drawing.Point> next = new();
            foreach (System.Drawing.Point p in pointsToCheck)
            {
                int val = grid[p.X][p.Y];
                if (p.X > 0 && grid[p.X - 1][p.Y] > val) next.Add(new System.Drawing.Point(p.X - 1, p.Y));
                if (p.Y > 0 && grid[p.X][p.Y - 1] > val) next.Add(new System.Drawing.Point(p.X, p.Y - 1));
                if (p.X < grid.Length - 1 && grid[p.X + 1][p.Y] > val) next.Add(new System.Drawing.Point(p.X+ 1, p.Y));
                if (p.Y < grid[p.X].Length - 1 && grid[p.X][p.Y + 1] > val) next.Add(new System.Drawing.Point(p.X, p.Y + 1));
            }
            next = next.Where(p => !points.Contains(p) && grid[p.X][p.Y] < 9).Distinct().ToList();
            points.AddRange(next);
            pointsToCheck = next;
        }

        return points.Count;
    }
}