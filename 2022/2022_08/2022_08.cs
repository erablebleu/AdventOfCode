using AdventOfCode.Tools;
using IPoint = System.Drawing.Point;

namespace AdventOfCode;

public class _2022_08 : Problem
{
    private static readonly IPoint[] Directions = new IPoint[]
        {
            new IPoint(-1, 0),
            new IPoint(1, 0),
            new IPoint(0, -1),
            new IPoint(0, 1),
        };

    public static bool IsVisible(int[,] data, int i, int j)
    {
        int val = data[i, j];

        foreach (IPoint v in Directions)
        {
            bool result = true;

            int x = i + v.X;
            int y = j + v.Y;

            while (x >= 0 && x < data.GetLength(0)
                && y >= 0 && y < data.GetLength(1)
                && result)
            {
                if (data[x, y] >= val)
                    result = false;
                x += v.X;
                y += v.Y;
            }

            if (result)
                return true;
        }

        return false;
    }
    public static int ScenicScore(int[,] data, int i, int j)
    {
        int val = data[i, j];
        int result = 1;

        foreach (IPoint v in Directions)
        {
            int x = i + v.X;
            int y = j + v.Y;
            int view = 0;

            while (x >= 0 && x < data.GetLength(0)
                && y >= 0 && y < data.GetLength(1))
            {
                view++;
                if (data[x, y] >= val) 
                    break;
                x += v.X;
                y += v.Y;
            }

            result *= view;
        }

        return result;
    }

    public override void Solve()
    {
        int[,] data = Inputs.Select(l => l.Select(c => int.Parse(c.ToString()))).To2DArray();

        Solutions.Add($"{data.Count(IsVisible)}");

        Solutions.Add($"{data.Max(ScenicScore)}");
    }
}