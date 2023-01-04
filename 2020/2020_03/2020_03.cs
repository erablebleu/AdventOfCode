namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/03
/// </summary>
public class _2020_03 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne() => CountChar('#', 3, 1);

    public override object PartTwo() => CountChar('#', 1, 1) * CountChar('#', 3, 1) * CountChar('#', 5, 1) * CountChar('#', 7, 1) * CountChar('#', 1, 2);


    private long CountChar(char searched, int right, int bot)
    {
        int col = 0;
        long result = 0;
        int width = Inputs.FirstOrDefault()?.Length ?? 0;
        for (int line = bot; line < Inputs.Length; line += bot)
        {
            col += right;
            result += Inputs[line][col % width] == searched ? 1 : 0;
        }

        return result;
    }
}