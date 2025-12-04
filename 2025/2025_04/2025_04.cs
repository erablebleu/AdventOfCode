namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2025/day/04
/// </summary>
public class _2025_04 : Problem
{
    private static readonly IVector2D[] Directions = [
        new IVector2D(1, -1),
        new IVector2D(1, 0),
        new IVector2D(1, 1),
        new IVector2D(0, 1),
        new IVector2D(0, -1),
        new IVector2D(-1, 1),
        new IVector2D(-1, 0),
        new IVector2D(-1, -1),
    ];
    
    private char[,] _data;
    private int _xMax;
    private int _yMax;

    public override void Parse()
    {
        _data = Inputs.To2DArray();
        _xMax = Inputs[0].Length;
        _yMax = Inputs.Length;
    }

    private bool CanRemove(char[,] data, IPoint2D p)
    {
        if (data[p.Y, p.X] != '@')
            return false;

        int count = Directions
            .Select(d => p + d)
            .Where(p => p.X >= 0 && p.Y >= 0 && p.X < _xMax && p.Y < _yMax)
            .Where(p => data[p.Y, p.X] == '@')
            .Count();
        
        return count < 4;
    }

    public override object PartOne()
    {
        long result = 0;

        for (int y = 0; y < _yMax; y++)
        {
            for (int x = 0; x < _xMax; x++)
            {
                IPoint2D p = new(x, y);

                if (CanRemove(_data, p))
                    result++;
            }
        }

        return result;
    }

    public override object PartTwo()
    {
        long result = 0;
        long removed;
        char[,] data = _data;
        char[,] next = new char[_yMax, _xMax];

        do
        {
            removed = 0;

            for (int y = 0; y < _yMax; y++)
            {
                for (int x = 0; x < _xMax; x++)
                {
                    IPoint2D p = new(x, y);

                    if (CanRemove(data, p))
                    {
                        removed++;
                        next[y, x] = '.';
                    }
                    else
                    {
                        next[y, x] = data[y, x];                        
                    }
                }
            }

            result += removed;
            (data, next) = (next, data);
        }
        while(removed > 0);

        return result;
    }
}