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

/// <summary>
/// https://adventofcode.com/2021/day/09
/// </summary>
public class _2021_09 : Problem
{
    private int[][] _grid;
    private List<System.Drawing.Point> _lowPoints;

    public override void Parse()
    {
        _grid = Inputs.Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
        _lowPoints = new();

        for (int i = 0; i < _grid.Length; i++)
        {
            for (int j = 0; j < _grid[i].Length; j++)
            {
                if (i > 0 && _grid[i - 1][j] <= _grid[i][j]) continue;
                if (j > 0 && _grid[i][j - 1] <= _grid[i][j]) continue;
                if (i < _grid.Length - 1 && _grid[i + 1][j] <= _grid[i][j]) continue;
                if (j < _grid[i].Length - 1 && _grid[i][j + 1] <= _grid[i][j]) continue;
                _lowPoints.Add(new System.Drawing.Point(i, j));
            }
        }
    }

    public override object PartOne() => _lowPoints.Sum(p => _grid[p.X][p.Y] + 1);

    public override object PartTwo() => _lowPoints.Select(p => GetBasinSize(_grid, p)).OrderByDescending(v => v).Take(3).Product();

    private static int GetBasinSize(int[][] _grid, System.Drawing.Point lowPoint)
    {
        List<System.Drawing.Point> points = new() { lowPoint };
        List<System.Drawing.Point> pointsToCheck = new() { lowPoint };

        while (pointsToCheck.Any())
        {
            List<System.Drawing.Point> next = new();
            foreach (System.Drawing.Point p in pointsToCheck)
            {
                int val = _grid[p.X][p.Y];
                if (p.X > 0 && _grid[p.X - 1][p.Y] > val) next.Add(new System.Drawing.Point(p.X - 1, p.Y));
                if (p.Y > 0 && _grid[p.X][p.Y - 1] > val) next.Add(new System.Drawing.Point(p.X, p.Y - 1));
                if (p.X < _grid.Length - 1 && _grid[p.X + 1][p.Y] > val) next.Add(new System.Drawing.Point(p.X + 1, p.Y));
                if (p.Y < _grid[p.X].Length - 1 && _grid[p.X][p.Y + 1] > val) next.Add(new System.Drawing.Point(p.X, p.Y + 1));
            }
            next = next.Where(p => !points.Contains(p) && _grid[p.X][p.Y] < 9).Distinct().ToList();
            points.AddRange(next);
            pointsToCheck = next;
        }

        return points.Count;
    }
}