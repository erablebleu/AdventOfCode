namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/14
/// </summary>
public class _2022_14 : Problem
{
    public static IVector2D[] Directions = new IVector2D[]
    {
        new IVector2D(0, 1),
        new IVector2D(-1, 1),
        new IVector2D(1, 1),
    };

    private List<IPoint2D> _rocks;

    public override void Parse()
    {
        _rocks = new();
        foreach (string line in Inputs)
        {
            string[] el = line.Split(" -> ");
            for (int x = 0; x < el.Length - 1; x++)
            {
                IPoint2D p0 = GetPoint(el[x]);
                IPoint2D p1 = GetPoint(el[x + 1]);
                for (int i = Math.Min(p0.X, p1.X); i <= Math.Max(p0.X, p1.X); i++)
                {
                    for (int j = Math.Min(p0.Y, p1.Y); j <= Math.Max(p0.Y, p1.Y); j++)
                    {
                        IPoint2D p = new(i, j);
                        if (_rocks.Contains(p))
                            continue;
                        _rocks.Add(p);
                    }
                }
            }
        }
    }

    public override object PartOne()
    {
        char[,] map = GetMap(_rocks);
        int maxY = _rocks.Max(p => p.Y);

        bool IsOccupied(IPoint2D position)
        {
            return map[position.X, position.Y] != '.';
        }

        IPoint2D sand;
        do
        {
            bool move = false;
            sand = new IPoint2D(500, 0);
            do
            {
                move = false;
                foreach (IVector2D direction in Directions)
                {
                    IPoint2D newPos = sand + direction;
                    if (IsOccupied(newPos))
                        continue;

                    move = true;
                    sand = newPos;
                    break;
                }
            }
            while (move && sand.Y < maxY);

            if (sand.Y < maxY)
            {
                map[sand.X, sand.Y] = 'o';
            }
        }
        while (sand.Y < maxY);

        return CountChar(map, 'o');
    }

    public override object PartTwo()
    {
        char[,] map = GetMap(_rocks);
        int maxY = _rocks.Max(p => p.Y);
        int moveCount = 0;

        bool IsOccupied(IPoint2D position)
        {
            return map[position.X, position.Y] != '.';
        }

        do
        {
            moveCount = 0;
            bool move = false;
            IPoint2D sand = new(500, 0);
            do
            {
                move = false;
                foreach (IVector2D direction in Directions)
                {
                    IPoint2D newPos = sand + direction;
                    if (IsOccupied(newPos))
                        continue;

                    moveCount++;
                    move = true;
                    sand = newPos;
                    break;
                }
            }
            while (move && sand.Y < maxY + 1);

            if (moveCount > 0)
            {
                map[sand.X, sand.Y] = 'o';
            }
        }
        while (moveCount > 0);

        return CountChar(map, 'o') + 1;
    }

    private static int CountChar(char[,] map, char c)
    {
        int count = 0;
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
                if (map[i, j] == c)
                    count++;
        return count;
    }

    private static IPoint2D GetPoint(string line)
    {
        string[] el = line.Split(",");
        return new(int.Parse(el[0]), int.Parse(el[1]));
    }

    private char[,] GetMap(List<IPoint2D> _rocks)
    {
        char[,] map = new char[1000, _rocks.Max(p => p.Y) + 2];
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
                map[i, j] = '.';
        foreach (IPoint2D p in _rocks)
        {
            map[p.X, p.Y] = '#';
        }
        return map;
    }
}