namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2025/day/09
/// </summary>
public class _2025_09 : Problem
{
    private IPoint2D[] _points;
    private record Segment(IPoint2D P0, IPoint2D P1);

    public override void Parse()
    {
        _points = [.. Inputs
            .Select(l => l.Split(","))
            .Select(el => new IPoint2D(int.Parse(el[0]), int.Parse(el[1])))];
    }

    private static long GetSurface(IPoint2D p0, IPoint2D p1)
        => (long)(Math.Abs(p0.X - p1.X) + 1) * (Math.Abs(p0.Y - p1.Y) + 1);

    public override object PartOne()
    {
        long result = 0;

        for (int i = 0; i < _points.Length; i++)
            for (int j = i + 1; j < _points.Length; j++)
            {
                IPoint2D p0 = _points[i];
                IPoint2D p1 = _points[j];
                result = Math.Max(result, GetSurface(p0, p1));
            }

        return result;
    }

    private static bool Cross(Tuple<IPoint2D, IPoint2D> rectangle, Segment segment)
    {
        int minX = Math.Min(rectangle.Item1.X, rectangle.Item2.X);
        int maxX = Math.Max(rectangle.Item1.X, rectangle.Item2.X);
        int minY = Math.Min(rectangle.Item1.Y, rectangle.Item2.Y);
        int maxY = Math.Max(rectangle.Item1.Y, rectangle.Item2.Y);

        if (segment.P0.X >= maxX && segment.P1.X >= maxX
            || segment.P0.X <= minX && segment.P1.X <= minX
            || segment.P0.Y >= maxY && segment.P1.Y >= maxY
            || segment.P0.Y <= minY && segment.P1.Y <= minY)
            return false;

        return true;
    }

    public override object PartTwo()
    {
        List<Segment> segments = [];
        List<Tuple<IPoint2D, IPoint2D>> rectangles = [];

        for (int i = 0; i < _points.Length; i++)
            segments.Add(new(_points[i], _points[(i + 1) % _points.Length]));

        for (int i = 0; i < _points.Length; i++)
            for (int j = i + 1; j < _points.Length; j++)
                rectangles.Add(new(_points[i], _points[j]));

        foreach ((Tuple<IPoint2D, IPoint2D> rectangle, long surface) in rectangles.Select(r => (r, GetSurface(r.Item1, r.Item2))).OrderByDescending(g => g.Item2))
        {
            if (segments.Any(segment => Cross(rectangle, segment)))
                continue;

            return surface;
        }

        return null;
    }
}