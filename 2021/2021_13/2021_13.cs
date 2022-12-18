namespace AdventOfCode;

public class _2021_13 : Problem
{
    public override void Solve()
    {
        List<System.Drawing.Point> points = Inputs.Where(l => !string.IsNullOrEmpty(l) && !l.StartsWith("fold")).Select(l => GetPoint(l)).ToList();
        string[] folds = Inputs.Where(l => l.StartsWith("fold")).Select(l => l.Replace("fold along ", "")).ToArray();


        Solutions.Add($"{Fold(points, folds[0]).Count()}");

        foreach(string fold in folds)
            points = Fold(points, fold);

        DisplayCode(points);
    }
    private static List<System.Drawing.Point> Fold(List<System.Drawing.Point> points, string fold)
    {
        List<System.Drawing.Point> result = new();
        int c = int.Parse(fold[2..]);
        if(fold.StartsWith("x="))
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
    private void DisplayCode(List<System.Drawing.Point> points)
    {
        int mX = points.Max(p => p.X) + 1;
        int mY = points.Max(p => p.Y) + 1;
        for(int y = 0; y < mY; y++)
            Solutions.Add(new string(Enumerable.Range(0, mX).Select(x => points.Any(p => p.X == x && p.Y == y) ? '#' : '.').ToArray()));
    }
}