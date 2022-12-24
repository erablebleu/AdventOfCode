namespace AdventOfCode;

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
        { DirectionsKeys[0], new IVector2D(1, 0) },
        { DirectionsKeys[1], new IVector2D(0, 1) },
        { DirectionsKeys[2], new IVector2D(-1, 0) },
        { DirectionsKeys[3], new IVector2D(0, -1) },
    };


    public static void Log(int[,] map)
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            char[] line = new char[map.GetLength(0)];
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x, y] == Wall)
                {
                    line[x] = '#';
                    continue;
                }
                int cnt = DirectionsKeys.Count(k => (map[x, y] & k) > 0);
                if (cnt > 1)
                {
                    line[x] = cnt.ToString()[0];
                    continue;
                }
                line[x] = DirectionsKeys.FirstOrDefault(k => (map[x, y] & k) > 0) switch
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

    public override void Solve()
    {
        int[,] map = new int[Inputs[0].Length, Inputs.Length];

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
                map[x, y] = Inputs[y][x] switch
                {
                    '.' => Empty,
                    '>' => 0x01,
                    'v' => 0x02,
                    '<' => 0x04,
                    '^' => 0x08,
                    _ => Wall
                };

        IPoint2D start = new(1, 0);
        IPoint2D end = new(Inputs[0].Length - 2, Inputs.Length - 1);

        AddSolution(Emulate(map, start, new IPoint2D[] { end }));
        AddSolution(Emulate(map, start, new IPoint2D[] { end, start, end }));
    }

    private static int Emulate(int[,] map, IPoint2D start, IPoint2D[] checkpoints)
    {
        List<IPoint2D> positions = new() { start };
        int cycleCount = 0;
        int cpIdx = 0;
        while (cpIdx < checkpoints.Length)
        {
            cycleCount++;
            //Log(map);
            map = GetNext(map);
            List<IPoint2D> nextPositions = new();
            foreach (IPoint2D p in positions)
            {
                bool atCP = false;
                if (map[p.X, p.Y] == Empty)
                    nextPositions.Add(p);

                foreach (int dir in DirectionsKeys)
                {
                    IVector2D direction = Directions[dir];
                    IPoint2D p2 = p + Directions[dir];
                    if (p2.X < 0
                        || p2.Y < 0
                        || p2.X >= map.GetLength(0)
                        || p2.Y >= map.GetLength(1)
                        || map[p2.X, p2.Y] != Empty
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

    private static int[,] GetNext(int[,] map)
    {
        int[,] result = new int[map.GetLength(0), map.GetLength(1)];

        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] == Wall)
                {
                    result[x, y] = map[x, y];
                    continue;
                }
                foreach (int dir in DirectionsKeys)
                {
                    if ((map[x, y] & dir) == 0)
                        continue;

                    IVector2D direction = Directions[dir];
                    int nx = x + direction.X;
                    int ny = y + direction.Y;
                    if (map[nx, ny] == Wall)
                    {
                        if (nx == 0) nx = map.GetLength(0) - 2;
                        else if (nx == map.GetLength(0) - 1) nx = 1;
                        else if (ny == 0) ny = map.GetLength(1) - 2;
                        else if (ny == map.GetLength(1) - 1) ny = 1;
                    }

                    result[nx, ny] |= dir;
                }
            }

        return result;
    }
}