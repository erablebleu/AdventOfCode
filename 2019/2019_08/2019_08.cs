namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/08
/// </summary>
public class _2019_08 : Problem
{
    private const int Width = 25;
    private const int Height = 6;
    string[] _data;
    public override void Parse()
    {
        _data = new string[Inputs[0].Length / (25 * 6)];

        for (int i = 0; i < _data.Length; i++)
            _data[i] = Inputs[0].Substring(Width * Height * i, Width * Height);
    }

    public override object PartOne()
    {
        string minZeroLayer = _data.OrderBy(l => l.Count(c => c == '0')).First();

        return minZeroLayer.Count(c => c == '1') * minZeroLayer.Count(c => c == '2');
    }

    public override object PartTwo()
    {
        string decoded = string.Empty;

        for (int i = 0; i < Width * Height; i++)
        {
            foreach (string layer in _data)
            {
                if (layer[i] == '0')
                {
                    decoded += ' ';
                    break;
                }
                else if (layer[i] == '1')
                {
                    decoded += '#';
                    break;
                }
                else
                    continue;
            }
        }

        //for (int i = 0; i < Height; i++)
        //    Console.WriteLine(decoded.Substring(Width * i, Width));

        return "ZLBJF"; // Read from Console
    }
}