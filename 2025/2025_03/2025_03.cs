using System.Dynamic;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2025/day/03
/// </summary>
public class _2025_03 : Problem
{
    public override void Parse()
    { }

    private static long GetMaxJoltage(string line, int batteryCount)
    {
        long result = 0;

        for(int i = 0; i < batteryCount; i++)
        {
            char d = line[..^(batteryCount - i - 1)].OrderDescending().First();
            int idx = line.IndexOf(d);
            result += (d - 48) * (long)Math.Pow(10, batteryCount - i - 1);
            line = line[(idx + 1)..];            
        }

        return result;
    }

    public override object PartOne()
    {
        long result = 0;

        foreach(string line in Inputs)
            result += GetMaxJoltage(line, 2);

        return result;
    }

    public override object PartTwo()
    {
        long result = 0;

        foreach(string line in Inputs)
            result += GetMaxJoltage(line, 12);

        return result;
    }
}