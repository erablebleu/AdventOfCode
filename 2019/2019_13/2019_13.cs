namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/13
/// </summary>
public class _2019_13 : Problem
{
    private Point _ball;
    private bool _bypass;
    private IntCode _computer;
    private Point _currentPoint;
    private long _height;
    private Point _paddle;
    private List<Point> _points;
    private int _score;
    private char[][] _screen;
    private List<long> _solutions = new();
    private Dictionary<IPoint2D, int> _tiles = new();
    private long _width;

    public override void Parse()
    {
        _points = new List<Point>();
        _computer = new IntCode(Inputs[0].Split(',').Select(s => long.Parse(s)).ToArray());
        _ball = new Point();
        _paddle = new Point();
    }

    public override object PartOne()
    {
        _computer.Data[0] = 1;
        _computer.End += OnIntCodeEnd;
        _computer.NewOutput += OnNewOutput;
        _computer.Exec();
        return _tiles.Values.Count(i => i == 2);
    }

    public override object PartTwo()
    {
        _computer.Data[0] = 2;
        _computer.End += OnIntCodeEnd;
        _computer.NewOutput += OnNewOutput;
        _computer.Exec();
        return _score;
    }

    private void Draw()
    {
        if (_bypass)
        {
            foreach (var point in _points.ToList())
            {
                if (point.X < 0)
                    continue;
                _screen[point.Y][point.X] = GetChar(point.Type);
                _points.Remove(point);
            }
        }
        else
        {
            _width = _points.Max(p => p.X) + 1;
            _height = _points.Max(p => p.Y) + 1;
            _screen = new char[_height][];
            for (int i = 0; i < _height; i++)
            {
                _screen[i] = new char[_width];
                for (int j = 0; j < _width; j++)
                {
                    var point = _points.FirstOrDefault(p => p.X == j && p.Y == i);
                    _screen[i][j] = GetChar(point?.Type ?? 0);
                    if (point != null)
                        _points.Remove(point);
                }
            }
            _bypass = true;
        }

        _points.Clear();
        _computer.Input = GetInput();
    }

    private char GetChar(long type)
    {
        switch (type)
        {
            case 1: return '#';
            case 2: return '+';
            case 3: return '_';
            case 4: return 'o';
            case 0:
            default:
                return ' ';
        }
    }

    private long GetInput()
    {
        if (_ball.X > _paddle.X)
            return 1;
        else if (_ball.X < _paddle.X)
            return -1;
        else
            return 0;
    }

    private void OnIntCodeEnd(object sender, EventArgs e)
    {
        _solutions.Add(_points.Where(p => p.Type == 2).Count());
    }

    private void OnNewOutput(object sender, IntCodeOutputEventArgs e)
    {
        switch (e.Idx % 3)
        {
            case 0:
                _currentPoint = new Point();
                _currentPoint.X = e.Value;
                break;

            case 1:
                _currentPoint.Y = e.Value;
                break;

            case 2:
                _currentPoint.Type = e.Value;
                _tiles[new IPoint2D((int)_currentPoint.X, (int)_currentPoint.Y)] = (int)e.Value;
                if (_currentPoint.X == -1)
                    _score = (int)e.Value;
                _points.Add(_currentPoint);
                switch (_currentPoint.Type)
                {
                    case 3:
                        _paddle.X = _currentPoint.X;
                        _paddle.Y = _currentPoint.Y;
                        break;

                    case 4:
                        _ball.X = _currentPoint.X;
                        _ball.Y = _currentPoint.Y;
                        break;
                }
                if (_points.Count == 26 * 46 || _bypass)
                    Draw();
                break;
        }
    }

    private class Point
    {
        public long Type { get; set; }
        public long X { get; set; }
        public long Y { get; set; }
    }
}