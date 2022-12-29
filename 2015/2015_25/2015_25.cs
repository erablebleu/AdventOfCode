namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/25
/// </summary>
public class _2015_25 : Problem
{
    private int[] _coord;

    public override void Parse()
    {
        _coord = Inputs[0].ParseExact("To continue, please consult the code grid in the manual.  Enter the code at row {0}, column {1}.")
            .Select(e => int.Parse(e)).ToArray();
    }

    public override object PartOne() => GetCodeAt(_coord[1], _coord[0]);

    public override object PartTwo() => "Happy Christmas !";

    private long GetCodeAt(int x, int y)
    {
        long number = 0;
        for (int i = 1; i <= x + y - 1; i++)
            number += i;
        number += -y + 1;

        long result = 20151125;
        for (int i = 1; i < number; i++)
            result = result * 252533 % 33554393;
        return result;
    }
}