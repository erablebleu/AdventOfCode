namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/22
/// </summary>
public class _2022_22 : Problem
{
    private static IVector2D[] Directions = new IVector2D[]
    {
        new IVector2D(1, 0),
        new IVector2D(0, 1),
        new IVector2D(-1, 0),
        new IVector2D(0, -1),
    };

    private List<string> _instructions;
    private CubeMap _map;
    private int _sideLength;
    private int _yMax;

    private delegate IPoint2D MoveDelegate(IPoint2D p, ref IVector2D dir, out bool move);

    public override void Parse()
    {
        _yMax = Array.IndexOf(Inputs, Inputs.First(l => string.IsNullOrWhiteSpace(l)));
        _sideLength = Math.Max(_yMax, Inputs.Take(_yMax).Max(l => l.Length)) / 4;
        _map = new CubeMap(_sideLength, Enumerable.Range(0, _yMax / _sideLength).Select(y => Enumerable.Range(0, 3 + (_yMax / _sideLength == 3 ? 1 : 0)).Select(x => GetValue(x * _sideLength, y * _sideLength) != ' ')).To2DArray());
        _instructions = ParseInstruction(Inputs[_yMax + 1]).ToList();
    }

    public override object PartOne() => Emulate(_instructions, GetNext);

    public override object PartTwo() => Emulate(_instructions, GetNext3D);

    private static int GetPassword(IPoint2D p, IVector2D dir) => 1000 * (p.Y + 1) + 4 * (p.X + 1) + Array.IndexOf(Directions, dir);

    private static IEnumerable<string> ParseInstruction(string line)
    {
        int sIdx = 0;
        int eIdx = 0;
        while (sIdx < line.Length)
        {
            while (eIdx < line.Length && line[eIdx] >= 48 && line[eIdx] < 58) eIdx++;
            if (eIdx > sIdx)
            {
                yield return new string(line.Skip(sIdx).Take(eIdx - sIdx).ToArray());
                eIdx--;
            }
            else
                yield return line[sIdx].ToString();
            sIdx = eIdx + 1;
            eIdx = sIdx;
        }
        yield break;
    }

    private static IVector2D Rotate(IVector2D direction, string rot)
    {
        int dirIdx = Array.IndexOf(Directions, direction);
        switch (rot)
        {
            case "R":
                dirIdx++;
                break;

            case "L":
                dirIdx--;
                break;
        }
        return Directions[(dirIdx + 4) % 4];
    }

    private int Emulate(List<string> instructions, MoveDelegate getNext)
    {
        IVector2D direction = Directions[0];
        IPoint2D position = new();

        // Look for start position
        while (GetValue(position) == ' ') position += direction;

        foreach (string instruction in instructions)
        {
            if (int.TryParse(instruction, out int count))
            {
                bool move = true;
                for (int i = 0; i < count && move; i++)
                {
                    position = getNext(position, ref direction, out move);
                }
            }
            else
            {
                direction = Rotate(direction, instruction);
            }
        }

        return GetPassword(position, direction);
    }

    private IPoint2D GetNext(IPoint2D p, ref IVector2D dir, out bool move)
    {
        IPoint2D p2 = p + dir;
        move = false;

        if (GetValue(p2) == ' ')
        {
            // U turn
            IVector2D dir2 = Directions[(Array.IndexOf(Directions, dir) + 2) % 4];
            p2 = p;
            while (GetValue(p2 + dir2) != ' ')
                p2 += dir2;
        }
        if (GetValue(p2) == '#')
            return p;
        move = true;
        return p2;
    }

    private IPoint2D GetNext3D(IPoint2D p, ref IVector2D dir, out bool move)
    {
        IPoint2D p2 = p + dir;
        IVector2D dir2 = dir;
        move = false;

        if (GetValue(p2) == ' ')
            p2 = _map.GetNext2(p, ref dir2);

        if (GetValue(p2) == '#')
            return p;

        move = true;
        dir = dir2;
        return p2;
    }

    private char GetValue(IPoint2D p) => GetValue(p.X, p.Y);

    private char GetValue(int x, int y)
    {
        if (y < 0 || y >= _yMax) return ' ';
        if (x < 0 || x >= Inputs[y].Length) return ' ';
        return Inputs[y][x];
    }

    private class CubeMap
    {
        private bool[,] _map;
        private int _sideLength;

        private Dictionary<int, Tuple<IPoint2D, int>>[,] _sideTransition = new Dictionary<int, Tuple<IPoint2D, int>>[,]
                {
            {
                null,
                new Dictionary<int, Tuple<IPoint2D, int>>
                {
                    { 2, new Tuple<IPoint2D, int>(new IPoint2D(0, 2), 2) },
                    { 3, new Tuple<IPoint2D, int>(new IPoint2D(0, 3), 1) },
                },
                new Dictionary<int, Tuple<IPoint2D, int>>
                {
                    { 0, new Tuple<IPoint2D, int>(new IPoint2D(1, 2), 2) },
                    { 1, new Tuple<IPoint2D, int>(new IPoint2D(1, 1), 1) },
                    { 3, new Tuple<IPoint2D, int>(new IPoint2D(0, 3), 0) },
                }
            },
            {
                null,
                new Dictionary<int, Tuple<IPoint2D, int>>
                {
                    { 0, new Tuple<IPoint2D, int>(new IPoint2D(2, 0), 3) },
                    { 2, new Tuple<IPoint2D, int>(new IPoint2D(0, 2), 3) },
                }, null
            },
            {
                new Dictionary<int, Tuple<IPoint2D, int>>
                {
                    { 2, new Tuple<IPoint2D, int>(new IPoint2D(1, 0), 2) },
                    { 3, new Tuple<IPoint2D, int>(new IPoint2D(1, 1), 1) },
                },
                new Dictionary<int, Tuple<IPoint2D, int>>
                {
                    { 0, new Tuple<IPoint2D, int>(new IPoint2D(2, 0), 2) },
                    { 1, new Tuple<IPoint2D, int>(new IPoint2D(0, 3), 1) },
                }, null
            },
            {
                new Dictionary<int, Tuple<IPoint2D, int>>
                {
                    { 0, new Tuple<IPoint2D, int>(new IPoint2D(1, 2), 3) },
                    { 1, new Tuple<IPoint2D, int>(new IPoint2D(2, 0), 0) },
                    { 2, new Tuple<IPoint2D, int>(new IPoint2D(1, 0), 3) },
                }, null, null
            }
                };

        // Couldn't find a generic solution, so harcoded cube side transitions
        private Dictionary<int, Tuple<IPoint2D, int>>[,] _sideTransitionExample = new Dictionary<int, Tuple<IPoint2D, int>>[,]
        {
            {
                null, null,
                new Dictionary<int, Tuple<IPoint2D, int>>
                {
                    { 0, new Tuple<IPoint2D, int>(new IPoint2D(3, 2), 2) },
                    { 2, new Tuple<IPoint2D, int>(new IPoint2D(1, 1), 3) },
                    { 3, new Tuple<IPoint2D, int>(new IPoint2D(0, 1), 2) },
                }, null
            },
            {
                new Dictionary<int, Tuple<IPoint2D, int>>
                {
                    { 1, new Tuple<IPoint2D, int>(new IPoint2D(2, 2), 2) },
                    { 2, new Tuple<IPoint2D, int>(new IPoint2D(3, 2), 1) },
                    { 3, new Tuple<IPoint2D, int>(new IPoint2D(2, 0), 2) },
                },
                new Dictionary<int, Tuple<IPoint2D, int>>
                {
                    { 1, new Tuple<IPoint2D, int>(new IPoint2D(2, 2), 3) },
                    { 3, new Tuple<IPoint2D, int>(new IPoint2D(2, 0), 1) },
                },
                new Dictionary<int, Tuple<IPoint2D, int>>
                {
                    { 0, new Tuple<IPoint2D, int>(new IPoint2D(3, 2), 1) },
                }, null
            },
            {
                null, null,
                new Dictionary<int, Tuple<IPoint2D, int>>
                {
                    { 1, new Tuple<IPoint2D, int>(new IPoint2D(0, 1), 2) },
                    { 2, new Tuple<IPoint2D, int>(new IPoint2D(1, 1), 1) },
                },
                new Dictionary<int, Tuple<IPoint2D, int>>
                {
                    { 0, new Tuple<IPoint2D, int>(new IPoint2D(2, 0), 2) },
                    { 1, new Tuple<IPoint2D, int>(new IPoint2D(0, 1), 3) },
                    { 3, new Tuple<IPoint2D, int>(new IPoint2D(2, 1), 3) },
                },
            }
        };

        public CubeMap(int sideLength, bool[,] map)
        {
            _sideLength = sideLength;
            _map = map;
        }

        public IPoint2D GetNext2(IPoint2D p, ref IVector2D dir)
        {
            IPoint2D p0 = p / _sideLength;
            int dirIdx = Array.IndexOf(_2022_22.Directions, dir);
            Tuple<IPoint2D, int> data = _sideTransition[p0.Y, p0.X][dirIdx];

            IPoint2D result = new(p.X - p0.X * _sideLength, p.Y - p0.Y * _sideLength);
            int length = _sideLength - 1;

            switch (data.Item2)
            {
                case 0:
                    if (dirIdx % 2 == 0)
                        result = new IPoint2D(length - result.X, result.Y);
                    else
                        result = new IPoint2D(result.X, length - result.Y);
                    break;

                case 1:
                    if (dirIdx % 2 == 0)
                        result = new IPoint2D(length - result.Y, length - result.X);
                    else
                        result = new IPoint2D(result.Y, result.X);
                    break;

                case 2:
                    if (dirIdx % 2 == 0)
                        result = new IPoint2D(result.X, length - result.Y);
                    else
                        result = new IPoint2D(length - result.X, result.Y);
                    break;

                default:
                    if (dirIdx % 2 == 0)
                        result = new IPoint2D(result.Y, result.X);
                    else
                        result = new IPoint2D(length - result.Y, length - result.X);
                    break;
            }
            for (int i = 0; i < data.Item2; i++)
                dir = _2022_22.Rotate(dir, "R");

            return new IPoint2D(result.X + data.Item1.X * _sideLength, result.Y + data.Item1.Y * _sideLength);
        }
    }
}