namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2025/day/01
/// </summary>
public class _2025_01 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne()
    {
        int pos = 50;
        int result = 0;

        foreach (string line in Inputs)
        {
            int mult = line[0] == 'L' ? -1 : 1;
            int count = int.Parse(line[1..]);

            pos = (pos + mult * count) % 100;

            if (pos == 0)
                result++;
        }

        return result;
    }

    public override object PartTwo()
    {
        int pos = 50;
        int result = 0;

        foreach (string line in Inputs)
        {
            int mult = line[0] == 'L' ? -1 : 1;
            int count = int.Parse(line[1..]);
            int tmpPos = pos + mult * (count % 100);

            result += count / 100;

            if (tmpPos == 0
                || tmpPos >= 100
                || tmpPos < 0 && pos != 0)
                result++;

            pos = (tmpPos + 100) % 100;
        }

        return result;
    }
}