namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2021/day/25
/// </summary>
public class _2021_25 : Problem
{
    private static readonly Dictionary<char, IPoint2D> Directions = new Dictionary<char, IPoint2D>()
    {
        { '>', new IPoint2D(1, 0) },
        { 'v', new IPoint2D(0, 1) },
    };

    private char[,] _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => l.ToArray()).To2DArray();
    }

    public override object PartOne()
    {
        int moveCount = 0;
        int stepCount = 0;

        do
        {
            moveCount = 0;
            foreach (KeyValuePair<char, IPoint2D> direction in Directions)
            {
                char[,] nmap = _data.ToArray();
                for (int x = 0; x < _data.GetLength(1); x++)
                {
                    for (int y = 0; y < _data.GetLength(0); y++)
                    {
                        if (_data[y, x] != direction.Key)
                            continue;

                        int x2 = x + direction.Value.X;
                        int y2 = y + direction.Value.Y;
                        if (x2 >= _data.GetLength(1)) x2 = 0;
                        if (y2 >= _data.GetLength(0)) y2 = 0;

                        if (_data[y2, x2] != '.')
                            continue;

                        nmap[y2, x2] = direction.Key;
                        nmap[y, x] = '.';
                        moveCount++;
                    }
                }
                _data = nmap;
            }
            stepCount++;
        }
        while (moveCount > 0);

        return stepCount;
    }

    public override object PartTwo() => "Merry Christmas!";
}