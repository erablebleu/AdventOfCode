namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/11
/// </summary>
public class _2019_11 : Problem
{
    private IntCode _computer;
    private int _dir;
    private Map _map;
    private IPoint2D _pos;

    public override void Parse()
    {
        _computer = new IntCode(Inputs[0].Split(',').Select(s => long.Parse(s)).ToArray());
        _pos = new IPoint2D();
        _map = new Map();
        _dir = 1;

        _computer.NewOutput += OnNewOutput;
    }

    public override object PartOne()
    {
        _map.Color = false;
        _computer.Input = 0;
        _computer.Exec();
        return _map.Points.Count();
    }

    public override object PartTwo()
    {
        //_computer.End += (s, e) => _map.Log(); // uncomment for PartOne
        _map.Color = false;
        _computer.Input = 1;
        _computer.Exec();
        return "BJRKLJUP";
    }

    private void OnNewOutput(object sender, IntCodeOutputEventArgs e)
    {
        if (e.Idx % 2 == 0) // paint
        {
            _map.Color = e.Value == 0;
            _map.Paint(_pos);
        }
        else // move
        {
            _dir = (_dir + (e.Value == 0 ? - 1 : 1)).Loop(0, 4);
            _pos += IVector2D.DirectionWNES[_dir];
            _computer.Input = _map.Points.ContainsKey(_pos) ? (_map.Points[_pos] ? 0 : 1) : 0;
        }
    }

    private class Map
    {
        public Dictionary<IPoint2D, bool> Points = new();
        public bool Color;

        public void Paint(IPoint2D point)
        {
            Points[point] = Color;
        }

        public void Log()
        {
            int x = Points.Keys.Min(p => p.X);
            int y = Points.Keys.Min(p => p.Y);
            int width = Points.Keys.Max(p => p.X) - x + 1;
            int height = Points.Keys.Max(p => p.Y) - y + 1;

            Console.WriteLine();
            for (int j = y; j < height; j++)
            {
                char[] line = new char[width];

                for (int i = 0; i < width; i++)
                    line[i] = ' ';

                foreach (KeyValuePair<IPoint2D, bool> p in Points.Where(kv => kv.Key.Y == j))
                    line[p.Key.X - x] = p.Value ? ' ' : 'X';

                Console.WriteLine(new string(line));
            }
        }
    }
}