namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/16
/// </summary>
public class _2016_16 : Problem
{
    private bool[] _data;

    public override void Parse()
    {
        _data = Inputs[0].Select(c => c == '1').ToArray();
    }

    public override object PartOne() => GetChecksum(_data, 272);

    public override object PartTwo() => GetChecksum(_data, 35651584);

    private static bool[] EnlargeYourData(bool[] data)
    {
        bool[] result = new bool[2 * data.Length + 1];
        for (int i = 0; i < data.Length; i++)
        {
            result[i] = data[i];
            result[result.Length - i - 1] = !data[i];
        }
        return result;
    }

    private static string GetChecksum(bool[] data, int size)
    {
        while (data.Length < size)
            data = EnlargeYourData(data);

        return new string(GetChecksum(data.Take(size).ToArray()).Select(b => b ? '1' : '0').ToArray());
    }

    private static bool[] GetChecksum(bool[] data)
    {
        bool[] result;

        do
        {
            result = new bool[data.Length / 2];
            for (int i = 0; i < result.Length; i++)
                result[i] = data[2 * i] == data[2 * i + 1];
            data = result;
        }
        while (result.Length % 2 == 0);

        return result;
    }
}