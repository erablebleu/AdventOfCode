namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/01
/// </summary>
public class _2019_01 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => int.Parse(l)).ToArray();
    }

    public override object PartOne() => _data.Sum(e => GetFuel(e));

    public override object PartTwo() => _data.Sum(e => GetRecursiveFuel(e));

    private static int GetFuel(int mass) => Math.Max(mass / 3 - 2, 0);

    private static int GetRecursiveFuel(int mass)
    {
        int fuel = 0;
        while (mass > 0)
            fuel += mass = GetFuel(mass);
        return fuel;
    }
}