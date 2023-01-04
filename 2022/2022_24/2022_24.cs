namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/24
/// </summary>
public class _2022_24 : Problem
{
    private const int Empty = 0x0;
    private const int Wall = 0x10;


    private static int[] DirectionsKeys = new int[]
    {
        0x01,
        0x02,
        0x04,
        0x08,
    };
    private static Dictionary<int, IVector2D> Directions = new()
    {
        { 0x01, new IVector2D(1, 0) },
        { 0x02, new IVector2D(0, 1) },
        { 0x04, new IVector2D(-1, 0) },
        { 0x08, new IVector2D(0, -1) },
    };

    private IPoint2D _end;
    private int[,] _map;
    private IPoint2D _start;

    public static void Log(int[,] _map)
    {
        for (int y = 0; y < _map.GetLength(1); y++)
        {
            char[] line = new char[_map.GetLength(0)];
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                if (_map[x, y] == Wall)
                {
                    line[x] = '#';
                    continue;
                }
                int cnt = DirectionsKeys.Count(k => (_map[x, y] & k) > 0);
                if (cnt > 1)
                {
                    line[x] = cnt.ToString()[0];
                    continue;
                }
                line[x] = DirectionsKeys.FirstOrDefault(k => (_map[x, y] & k) > 0) switch
                {
                    Empty => '.',
                    0x01 => '>',
                    0x02 => 'v',
                    0x04 => '<',
                    0x08 => '^',
                    _ => '?'
                };
            }
            Console.WriteLine(new string(line));
        }
    }

    public override void Parse()
    {
        _map = new int[Inputs[0].Length, Inputs.Length];

        for (int x = 0; x < _map.GetLength(0); x++)
            for (int y = 0; y < _map.GetLength(1); y++)
                _map[x, y] = Inputs[y][x] switch
                {
                    '.' => Empty,
                    '>' => 0x01,
                    'v' => 0x02,
                    '<' => 0x04,
                    '^' => 0x08,
                    _ => Wall
                };

        _start = new(1, 0);
        _end = new(Inputs[0].Length - 2, Inputs.Length - 1);
    }

    public override object PartOne() => Emulate(_map, _start, new IPoint2D[] { _end });

    public override object PartTwo() => Emulate(_map, _start, new IPoint2D[] { _end, _start, _end });

    private static int Emulate(int[,] _map, IPoint2D start, IPoint2D[] checkpoints)
    {
        List<IPoint2D> positions = new() { start };
        int cycleCount = 0;
        int cpIdx = 0;
        while (cpIdx < checkpoints.Length)
        {
            cycleCount++;
            //Log(_map);
            _map = GetNext(_map);
            List<IPoint2D> nextPositions = new();
            foreach (IPoint2D p in positions)
            {
                bool atCP = false;
                if (_map[p.X, p.Y] == Empty)
                    nextPositions.Add(p);

                foreach (int dir in DirectionsKeys)
                {
                    IVector2D direction = Directions[dir];
                    IPoint2D p2 = p + Directions[dir];
                    if (p2.X < 0
                        || p2.Y < 0
                        || p2.X >= _map.GetLength(0)
                        || p2.Y >= _map.GetLength(1)
                        || _map[p2.X, p2.Y] != Empty
                        || nextPositions.Contains(p2))
                        continue;

                    if (p2 == checkpoints[cpIdx])
                    {
                        atCP = true;
                        break;
                    }
                    else
                        nextPositions.Add(p2);
                }

                if (atCP)
                {
                    nextPositions = new List<IPoint2D> { checkpoints[cpIdx] };
                    cpIdx++;
                    break;
                }
            }

            positions = nextPositions;
        }

        return cycleCount;
    }

    private static int[,] GetNext(int[,] _map)
    {
        int[,] result = new int[_map.GetLength(0), _map.GetLength(1)];

        for (int x = 0; x < _map.GetLength(0); x++)
            for (int y = 0; y < _map.GetLength(1); y++)
            {
                if (_map[x, y] == Wall)
                {
                    result[x, y] = _map[x, y];
                    continue;
                }
                foreach (int dir in DirectionsKeys)
                {
                    if ((_map[x, y] & dir) == 0)
                        continue;

                    IVector2D direction = Directions[dir];
                    int nx = x + direction.X;
                    int ny = y + direction.Y;
                    if (_map[nx, ny] == Wall)
                    {
                        if (nx == 0) nx = _map.GetLength(0) - 2;
                        else if (nx == _map.GetLength(0) - 1) nx = 1;
                        else if (ny == 0) ny = _map.GetLength(1) - 2;
                        else if (ny == _map.GetLength(1) - 1) ny = 1;
                    }

                    result[nx, ny] |= dir;
                }
            }

        return result;
    }
}