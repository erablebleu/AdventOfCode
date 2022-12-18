using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using AdventOfCode.Tools;

namespace AdventOfCode;

public class _2022_14 : Problem
{
    public static IVector2D[] Directions = new IVector2D[]
    {
        new IVector2D(0, 1),
        new IVector2D(-1, 1),
        new IVector2D(1, 1),
    };
    private static IPoint2D GetPoint(string line)
    {
        string[] el = line.Split(",");
        return new(int.Parse(el[0]), int.Parse(el[1]));
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
    private char[,] GetMap(List<IPoint2D> rocks)
    {
        char[,] map = new char[1000, rocks.Max(p => p.Y) + 2];
        for (int i = 0; i < map.GetLength(0); i++)
            for (int j = 0; j < map.GetLength(1); j++)
                map[i, j] = '.';
        foreach(IPoint2D p in rocks)
        {
            map[p.X, p.Y] = '#';
        }
        return map;

    }
    public override void Solve()
    {
        List<IPoint2D> rocks = new();
        foreach(string line in Inputs)
        {
            string[] el = line.Split(" -> ");
            for(int x = 0; x < el.Length - 1; x++)
            {
                IPoint2D p0 = GetPoint(el[x]);
                IPoint2D p1 = GetPoint(el[x + 1]);
                for (int i = Math.Min(p0.X, p1.X); i <= Math.Max(p0.X, p1.X); i++)
                {
                    for (int j = Math.Min(p0.Y, p1.Y); j <= Math.Max(p0.Y, p1.Y); j++)
                    {
                        IPoint2D p = new(i, j);
                        if (rocks.Contains(p))
                            continue;
                        rocks.Add(p);
                    }
                }
            }
        }

        char[,] map = GetMap(rocks);

        int maxY = rocks.Max(p => p.Y);


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
                foreach(IVector2D direction in Directions)
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
        while(sand.Y < maxY);

        Solutions.Add($"{CountChar(map, 'o')}");

        // infinite floor at y = maxY + 2
        map = GetMap(rocks);
        int moveCount = 0;
        do
        {
            moveCount = 0;
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

        Solutions.Add($"{CountChar(map, 'o') + 1}");
    }
    public void Solve_old()
    {
        List<IPoint2D> rocks = new();
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
                        if (rocks.Contains(p))
                            continue;
                        rocks.Add(p);
                    }
                }
            }
        }

        int maxY = rocks.Max(p => p.Y);

        List<IPoint2D> sands = new();

        bool IsOccupied(IPoint2D position)
        {
            return rocks.Any(p => p.X == position.X && p.Y == position.Y) || sands.Any(p => p.X == position.X && p.Y == position.Y);
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
                sands.Add(sand);
        }
        while (sand.Y < maxY);

        Solutions.Add($"{sands.Count}");

        // infinite floor at y = maxY + 2
        sands = new();
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
            while (move && sand.Y < maxY + 1);

            if (sand.Y > 0)
                sands.Add(sand);
        }
        while (sand.Y > 0);

        Solutions.Add($"{sands.Count}");
    }
}