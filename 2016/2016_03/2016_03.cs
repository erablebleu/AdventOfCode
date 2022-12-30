namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/03
/// </summary>
public class _2016_03 : Problem
{
    private int[][] _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => l.ExtractIntegers().ToArray()).ToArray();
    }

    public override object PartOne() => _data.Count(t => CanBeTriangle(t));

    public override object PartTwo() => GetColumns(_data).Count(t => CanBeTriangle(t));

    private static bool CanBeTriangle(int[] sides)
        => sides[0] + sides[1] > sides[2]
        && sides[1] + sides[2] > sides[0]
        && sides[0] + sides[2] > sides[1];

    private static IEnumerable<int[]> GetColumns(int[][] data)
    {
        for (int i = 0; i < data.Length - 2; i += 3)
        {
            yield return new int[] { data[i][0], data[i + 1][0], data[i + 2][0] };
            yield return new int[] { data[i][1], data[i + 1][1], data[i + 2][1] };
            yield return new int[] { data[i][2], data[i + 1][2], data[i + 2][2] };
        }
        yield break;
    }
}