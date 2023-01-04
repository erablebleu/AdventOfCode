namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/12
/// </summary>
public class _2022_12 : Problem
{
    private static readonly IVector2D[] Directions = new IVector2D[]
        {
            new IVector2D(-1, 0),
            new IVector2D(1, 0),
            new IVector2D(0, -1),
            new IVector2D(0, 1),
        };

    private int[,] _data;
    private IPoint2D _end;
    private int[,] _map;
    private IPoint2D _start;

    public override void Parse()
    {
        _data = Inputs.Select(l => l.Select(c => c - 'a')).To2DArray();
        _map = new int[_data.GetLength(0), _data.GetLength(1)];
        for (int i = 0; i < _data.GetLength(0); i++)
        {
            for (int j = 0; j < _data.GetLength(1); j++)
            {
                _map[i, j] = int.MaxValue;
                if (_data[i, j] == 'S' - 'a')
                {
                    _start = new IPoint2D(i, j);
                    _data[i, j] = 0;
                }
                if (_data[i, j] == 'E' - 'a')
                {
                    _end = new IPoint2D(i, j);
                    _data[i, j] = 25;
                    _map[i, j] = 0;
                }
            }
        }

        int GetVal(IPoint2D p) => _data[p.X, p.Y];
        bool IsInclude(IPoint2D p) => p.X >= 0 && p.Y >= 0 && p.X < _data.GetLength(0) && p.Y < _data.GetLength(1);

        int newCnt;
        do
        {
            newCnt = 0;
            for (int i = 0; i < _data.GetLength(0); i++)
            {
                for (int j = 0; j < _data.GetLength(1); j++)
                {
                    IPoint2D p = new(i, j);
                    if (p == _end) continue;
                    if (_map[p.X, p.Y] < int.MaxValue) continue;

                    int min = int.MaxValue;

                    foreach (IVector2D direction in Directions)
                    {
                        IPoint2D p2 = p + direction;
                        if (!IsInclude(p2)) continue;
                        if (_map[p2.X, p2.Y] >= min) continue;
                        if (GetVal(p2) > GetVal(p) + 1) continue;
                        min = _map[p2.X, p2.Y];
                    }
                    if (min == int.MaxValue)
                        continue;
                    _map[p.X, p.Y] = min + 1;
                    newCnt++;
                }
            }
        }
        while (newCnt > 0);
    }

    public override object PartOne() => _map[_start.X, _start.Y];

    public override object PartTwo()
    {
        int minASteps = _map[_start.X, _start.Y];
        for (int i = 0; i < _data.GetLength(0); i++)
        {
            for (int j = 0; j < _data.GetLength(1); j++)
            {
                if (_data[i, j] == 0 && _map[i, j] < minASteps)
                    minASteps = _map[i, j];
            }
        }
        return minASteps;
    }
}