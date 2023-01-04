namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/03
/// </summary>
public class _2019_03 : Problem
{
    private List<Point> _points;

    public override void Parse()
    {
        _points = new List<Point>();
        string[][] moves = Inputs.Select(s => s.Split(',')).ToArray();

        var a1 = new Point();
        foreach (string moveA in moves[0])
        {
            Point a2 = a1.Copy().Move(moveA);
            var b1 = new Point();
            foreach (string moveB in moves[1])
            {
                Point b2 = b1.Copy().Move(moveB);
                if (Point.Cross(a1, a2, b1, b2, out var point))
                    _points.Add(point);
                b1 = b2;
            }
            a1 = a2;
        }
    }

    public override object PartOne() => _points.Select(p => Math.Abs(p.X) + Math.Abs(p.Y)).Min();

    public override object PartTwo() => _points.Select(p => p.Length).Min();

    private class Point
    {
        public bool Dir { get; set; }
        public int Length { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public static bool Cross(Point a1, Point a2, Point b1, Point b2, out Point p)
        {
            p = new Point()
            {
                X = a2.Dir ? a1.X : b1.X,
                Y = b2.Dir ? a1.Y : b1.Y,
            };
            p.Length = a1.Length + b1.Length
                     + (b2.Dir
                        ? (Math.Abs(p.X - a1.X) + Math.Abs(p.Y - b1.Y))
                        : (Math.Abs(p.X - b1.X) + Math.Abs(p.Y - a1.Y)));
            if (a2.Dir == b2.Dir)
                return false;

            if ((a2.Dir
                && Math.Min(b1.X, b2.X) < p.X
                && Math.Max(b1.X, b2.X) >= p.X
                && Math.Min(a1.Y, a2.Y) < p.Y
                && Math.Max(a1.Y, a2.Y) >= p.Y)
               || (b2.Dir
                && Math.Min(a1.X, a2.X) < p.X
                && Math.Max(a1.X, a2.X) >= p.X
                && Math.Min(b1.Y, b2.Y) < p.Y
                && Math.Max(b1.Y, b2.Y) >= p.Y))
                return true;
            return false;
        }

        public Point Copy()
        {
            return new Point() { X = X, Y = Y, Dir = Dir, Length = Length };
        }

        public Point Move(string move)
        {
            int val = int.Parse(move.Substring(1));
            Length += val;
            Dir = move[0] == 'D' || move[0] == 'U';
            switch (move[0])
            {
                case 'L': X -= val; break;
                case 'U': Y += val; break;
                case 'R': X += val; break;
                case 'D': Y -= val; break;
            }
            return this;
        }

        public override string ToString()
        {
            return $"X:{X} Y:{Y} Length:{Length}";
        }
    }
}