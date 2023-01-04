namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/06
/// </summary>
public class _2022_06 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne()
    {
        for (int i = 0; i < Inputs[0].Length; i++)
        {
            string marker = Inputs[0].Substring(i, 4);
            if (marker.Distinct().Count() == marker.Length)
                return i + 4;
        }

        return null;
    }

    public override object PartTwo()
    {
        for (int i = 0; i < Inputs[0].Length; i++)
        {
            string marker = Inputs[0].Substring(i, 14);
            if (marker.Distinct().Count() == marker.Length)
                return i + 14;
        }

        return null;
    }
}