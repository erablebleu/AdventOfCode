using AdventOfCode.Tools;

namespace AdventOfCode;
/// <summary>
/// https://adventofcode.com/2021/day/20
/// </summary>
public class _2021_20 : Problem
{
    private Dictionary<System.Drawing.Point, bool> _data;

    public override void Parse()
    {
        char[,] img = Inputs.Skip(2).To2DArray();
        _data = new Dictionary<System.Drawing.Point, bool>();

        for (int i = 0; i < img.GetLength(0); i++)
            for (int j = 0; j < img.GetLength(1); j++)
                _data.Add(new System.Drawing.Point(i, j), img[i, j] == '#');
    }

    public override object PartOne()
    {
        string key = Inputs[0];
        for (int i = 0; i < 2; i++)
            _data = EnhanceImage(_data, key, i);

        return _data.Values.Count(v => v);
    }

    public override object PartTwo()
    {
        string key = Inputs[0];

        for (int i = 0; i < 48; i++)
            _data = EnhanceImage(_data, key, i);

        return _data.Values.Count(v => v);
    }

    private static int GetVal(Dictionary<System.Drawing.Point, bool> _data, System.Drawing.Point p, int defValue)
    {
        int value = 0;
        for (int dx = -1; dx < 2; dx++)
        {
            for (int dy = -1; dy < 2; dy++)
            {
                System.Drawing.Point np = new System.Drawing.Point(p.X + dx, p.Y + dy);
                if (_data.ContainsKey(np))
                    value += _data[np] ? 1 : 0;
                else
                    value += defValue;
                value *= 2;
            }
        }
        return value / 2;
    }
    private Dictionary<System.Drawing.Point, bool> EnhanceImage(Dictionary<System.Drawing.Point, bool> _data, string key, int count)
    {
        Dictionary<System.Drawing.Point, bool> result = new ();

        foreach (KeyValuePair<System.Drawing.Point, bool> kv in _data)
        {
            for (int dx = -1; dx < 2; dx++)
            {
                for (int dy = -1; dy < 2; dy++)
                {
                    System.Drawing.Point p = new System.Drawing.Point(kv.Key.X + dx, kv.Key.Y + dy);
                    if (result.ContainsKey(p))
                        continue;

                    int val = GetVal(_data, p, count % 2);
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

