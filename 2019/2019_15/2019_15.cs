namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/15
/// </summary>
public class _2019_15 : Problem
{
    private static Dictionary<int, Point> Directions = new Dictionary<int, Point>()
      {
         { 0, new Point() { X = -1, Type = 3 } }, // north (1), south (2), west (3), and east (4)
         { 1, new Point() { Y = -1, Type = 1 } },
         { 2, new Point() { X = 1, Type = 4 } },
         { 3, new Point() { Y = 1, Type = 2 } },
      };

    private IntCode _computer;
    private int _currentDir = 0;
    private Point _currentPoint;
    private List<Point> _map;
    private int _moveCnt = 0;
    private Point _target;

    public override void Parse()
    {
        _computer = new IntCode(Inputs[0].Split(',').Select(s => long.Parse(s)).ToArray());
        _map = new List<Point>();
        _currentPoint = new Point
        {
            Type = 1
        };
        _map.Add(_currentPoint);
        _computer.End += OnIntCodeEnd;
        _computer.NewOutput += OnNewOutput;
        _computer.Input = Directions[_currentDir].Type;
        _computer.Exec();
    }

    public override object PartOne() => _moveCnt;

    public override object PartTwo() => GetSteps();

    private static string GetChar(int? type)
    {
        if (type is null) return "?";
        switch (type)
        {
            case 0: return "#";
            case 1: return " ";
            case 2: return "o";
        }
        return ".";
    }

    private bool AddPosition(Point p, bool takeCnt)
    {
        var point = _map.FirstOrDefault(mp => mp == p);
        if (point != null)
        {
            point.Type = p.Type;
            if (takeCnt && _target == null)
                _moveCnt = point.Cnt;
            return false;
        }
        if (takeCnt && _target == null)
        {
            _moveCnt++;
            p.Cnt = _moveCnt;
        }
        _map.Add(p);
        return true;
    }

    private void ChangeDirection()
    {
        Point tmp;
        do
        {
            _currentDir = Turn(-1);
            tmp = _map.FirstOrDefault(p => p == _currentPoint + Directions[_currentDir]);
        }
        while (tmp != null && tmp.Type == 0);
    }

    private void Draw()
    {
        Console.Clear();
        int x = _map.Min(p => p.X);
        int y = _map.Min(p => p.Y);
        int width = _map.Max(p => p.X) + 1;
        int height = _map.Max(p => p.Y) + 1;

        for (int j = y; j < height; j++)
        {
            string line = string.Empty;
            for (int i = x; i < width; i++)
                if (i == _currentPoint.X && j == _currentPoint.Y)
                    line += "*";
                else
                    line += GetChar(_map.FirstOrDefault(p => p.X == i && p.Y == j)?.Type);
            Console.WriteLine(line);
        }
        if (_target != null)
        {
            Console.WriteLine();
            Console.WriteLine($"target found at:{_moveCnt}");
        }
    }

    private int GetSteps()
    {
        int step = 0;
        int x = _map.Min(p => p.X);
        int y = _map.Min(p => p.Y);
        int width = _map.Max(p => p.X) + 1;
        int height = _map.Max(p => p.Y) + 1;
        List<Point> map = new List<Point>();

        foreach (var p in _map)
            map.Add(p.Copy());

        // 2 == oxygen

        while (map.Any(p => p.Type == 1))
        {
            foreach (var ox in map.Where(p => p.Type == 2).ToList())
                foreach (var an in map.Where(p => p.Type == 1
                                                  && ((p.X == ox.X && (p.Y == ox.Y - 1 || p.Y == ox.Y + 1))
                                                      || (p.Y == ox.Y && (p.X == ox.X - 1 || p.X == ox.X + 1)))))
                    an.Type = 2;
            step++;
        }
        return step;
    }

    private void OnIntCodeEnd(object sender, EventArgs e)
    {
    }

    private void OnNewOutput(object sender, IntCodeOutputEventArgs e)
    {
        if (e.Idx > 14000)
        {
            //Console.WriteLine($"steps to fill:{GetSteps()}");
            _computer.Input = 0;
            return;
        }

        /*
         * 0: The repair droid hit a wall. Its position has not changed.
         * 1: The repair droid has moved one step in the requested direction.
         * 2: The repair droid has moved one step in the requested direction; its new position is the location of the oxygen system.
         */
        Point newPosition = _currentPoint.Move(_currentDir);
        newPosition.Type = (int)e.Value;
        switch (e.Value)
        {
            case 0:
                AddPosition(newPosition, false);
                ChangeDirection();
                break;

            case 1:
                AddPosition(newPosition, true);
                _currentPoint = newPosition;
                _currentDir = Turn(1);
                break;

            case 2:
                AddPosition(newPosition, true);
                _currentPoint = newPosition;
                _currentDir = Turn(1);
                _target = newPosition;
                break;
        }
        //if (e.Idx % 1000 == 0)
        //    Draw();
        _computer.Input = Directions[_currentDir].Type;
    }

    private int Turn(int a)
    {
        int dir = _currentDir + a;
        if (dir > 3) dir = 0;
        else if (dir < 0) dir = 3;
        return dir;
    }

    private class Point
    {
        public int Cnt { get; set; }
        public int Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public static bool operator !=(Point a, Point b)
        {
            if (a is null && b is null) return false;
            if ((!(a is null) && b is null) || (!(b is null) && a is null)) return true;
            return a.X != b.X || a.Y != b.Y;
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point() { X = a.X + b.X, Y = a.Y + b.Y };
        }

        public static bool operator ==(Point a, Point b)
        {
            if (a is null && b is null) return true;
            if ((!(a is null) && b is null) || (!(b is null) && a is null)) return false;
            return a.X == b.X && a.Y == b.Y;
        }

        public Point Copy()
        {
            return new Point() { X = X, Y = Y, Type = Type };
        }

        public Point Move(int direction)
        {
            var point = this + Directions[direction];
            point.Type = Type;
            return point;
        }

        public override string ToString()
        {
            return $"X:{X} Y:{Y} Type:{Type}";
        }
    }
}