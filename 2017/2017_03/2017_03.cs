namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2017/day/03
/// </summary>
public class _2017_03 : Problem
{
    private int _data;

    public override void Parse()
    {
        _data = int.Parse(Inputs[0]);
    }

    public override object PartOne() => GetSteps(_data);

    public override object PartTwo()
    {
        int[,] grid = new int[999, 999];
        IPoint2D p = new(499, 499);
        IVector2D v = new(499, 499);
        int idx = 1;
        grid[p.X, p.Y] = 1;
        while (grid[p.X, p.Y] <= _data)
        {
            idx++;
            p = GetPosition(idx) + v;
            grid[p.X, p.Y] = IVector2D.Direction8.Select(d => p + d).Sum(p2 => grid[p2.X, p2.Y]);
        }

        return grid[p.X, p.Y];
    }

    private static int GetDistance(IPoint2D p) => Math.Abs(p.X) + Math.Abs(p.Y);

    private static IPoint2D GetPosition(int target)
    {
        int d = (int)Math.Sqrt(target);
        if (d % 2 == 0) d--;
        int r = target - d * d;
        IPoint2D p = new(d / 2 + (r > 0 ? 1 : 0), d / 2);
        int off = 1;
        foreach (IVector2D dir in IVector2D.DirectionNWSE)
        {
            if (r <= 0) return p;
            p += dir * Math.Min(r - off, d + 1 - off);
            r -= d + 1;
            off = 0;
        }
        return p;
    }

    private static int GetSteps(int target) => GetDistance(GetPosition(target));
}