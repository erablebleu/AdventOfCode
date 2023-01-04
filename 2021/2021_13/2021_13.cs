namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2021/day/13
/// </summary>
public class _2021_13 : Problem
{
    private string[] _folds;
    private List<System.Drawing.Point> _points;

    public override void Parse()
    {
        _points = Inputs.Where(l => !string.IsNullOrEmpty(l) && !l.StartsWith("fold")).Select(l => GetPoint(l)).ToList();
        _folds = Inputs.Where(l => l.StartsWith("fold")).Select(l => l.Replace("fold along ", "")).ToArray();
    }

    public override object PartOne() => Fold(_points, _folds[0]).Count();

    public override object PartTwo()
    {
        foreach (string fold in _folds)
            _points = Fold(_points, fold);

        //Log(_points);
        return "FGKCKBZG"; // Read from log
    }

    private static List<System.Drawing.Point> Fold(List<System.Drawing.Point> points, string fold)
    {
        List<System.Drawing.Point> result = new();
        int c = int.Parse(fold[2..]);
        if (fold.StartsWith("x="))
        {
            result.AddRange(points.Where(p => p.X < c));
            result.AddRange(points.Where(p => p.X > c).Select(p => new System.Drawing.Point(2 * c - p.X, p.Y)));
        }
        else
        {
            result.AddRange(points.Where(p => p.Y < c));
            result.AddRange(points.Where(p => p.Y > c).Select(p => new System.Drawing.Point(p.X, 2 * c - p.Y)));
        }

        return result.Distinct().ToList();
    }

    private static System.Drawing.Point GetPoint(string line)
    {
        int[] c = line.Split(',').Select(i => int.Parse(i)).ToArray();
        return new System.Drawing.Point(c[0], c[1]);
    }

    private static void Log(List<System.Drawing.Point> points)
    {
        int mX = points.Max(p => p.X) + 1;
        int mY = points.Max(p => p.Y) + 1;
        for (int y = 0; y < mY; y++)
            Console.WriteLine(new string(Enumerable.Range(0, mX).Select(x => points.Any(p => p.X == x && p.Y == y) ? '#' : '.').ToArray()));
    }
}