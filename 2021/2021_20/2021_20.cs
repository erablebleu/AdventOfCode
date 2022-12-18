using AdventOfCode.Tools;

namespace AdventOfCode;
public class _2021_20 : Problem
{
    public override void Solve()
    {
        string key = Inputs[0];
        char[,] img = Inputs.Skip(2).To2DArray();
        Dictionary<System.Drawing.Point, bool> data = new ();

        for (int i = 0; i < img.GetLength(0); i++)
            for (int j = 0; j < img.GetLength(1); j++)
                data.Add(new System.Drawing.Point(i, j), img[i, j] == '#');

        for (int i = 0; i < 2; i++)
            data = EnhanceImage(data, key, i);

        Solutions.Add($"{data.Values.Count(v => v)}");

        for (int i = 0; i < 48; i++)
            data = EnhanceImage(data, key, i);

        Solutions.Add($"{data.Values.Count(v => v)}");
    }
    private static int GetVal(Dictionary<System.Drawing.Point, bool> data, System.Drawing.Point p, int defValue)
    {
        int value = 0;
        for (int dx = -1; dx < 2; dx++)
        {
            for (int dy = -1; dy < 2; dy++)
            {
                System.Drawing.Point np = new System.Drawing.Point(p.X + dx, p.Y + dy);
                if (data.ContainsKey(np))
                    value += data[np] ? 1 : 0;
                else
                    value += defValue;
                value *= 2;
            }
        }
        return value / 2;
    }
    private Dictionary<System.Drawing.Point, bool> EnhanceImage(Dictionary<System.Drawing.Point, bool> data, string key, int count)
    {
        Dictionary<System.Drawing.Point, bool> result = new ();

        foreach (KeyValuePair<System.Drawing.Point, bool> kv in data)
        {
            for (int dx = -1; dx < 2; dx++)
            {
                for (int dy = -1; dy < 2; dy++)
                {
                    System.Drawing.Point p = new System.Drawing.Point(kv.Key.X + dx, kv.Key.Y + dy);
                    if (result.ContainsKey(p))
                        continue;

                    int val = GetVal(data, p, count % 2);
                    result.Add(p, key[val] == '#');
                }
            }
        }

        return result;
    }

    private char[,] EnhanceImage(char[,] img, string key, int count)
    {
        char[,] result = new char[img.GetLength(0) + 2, img.GetLength(1) + 2];

        for(int i = 0; i < result.GetLength(0); i++)
        {
            for(int j = 0; j < result.GetLength(1); j++)
            {
                int value = 0;
                for (int dx = -1; dx < 2; dx++)
                {
                    for (int dy = -1; dy < 2; dy++)
                    {
                        int x = i - 1 + dx;
                        int y = j - 1 + dy;
                        if (x < 0 || x >= img.GetLength(0)
                            || y < 0 || y >= img.GetLength(1))
                            value += count % 2;
                        else
                            value += img[x, y] == '#' ? 1 - count % 2 : count% 2;

                        value *= 2;
                    }
                }
                value /= 2;
                result[i, j] = key[value];
                Console.Write(key[value]);

            }
            Console.WriteLine();
        }
        Console.WriteLine();
        Console.WriteLine();

        return result;
    }
}