namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/02
/// </summary>
public class _2019_02 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs[0].Split(',').Select(s => int.Parse(s)).ToArray();
    }

    public override object PartOne()
    {
        int[] intcode = _data.ToArray();
        intcode[1] = 12; // noun
        intcode[2] = 2; // verb
        ExecCode(intcode);
        return intcode[0];
    }

    public override object PartTwo()
    {
        int[] intcode = _data.ToArray();
        intcode[1] = 12; // noun
        intcode[2] = 2; // verb

        for (int i = 0; i < 10000; i++)
        {
            intcode = Inputs[0].Split(',').Select(s => int.Parse(s)).ToArray();
            intcode[1] = i / 100; // 0-99
            intcode[2] = i % 100; // 0-99
            ExecCode(intcode);
            if (intcode[0] == 19690720)
                break;
        }

        return intcode[1] * 100 + intcode[2];
    }

    private static void ExecCode(int[] intcode)
    {
        for (int i = 0; i < intcode.Length; i++)
        {
            switch (intcode[i])
            {
                case 1: // +
                    intcode[intcode[i + 3]] = intcode[intcode[i + 1]] + intcode[intcode[i + 2]];
                    i += 3;
                    break;

                case 2: // *
                    intcode[intcode[i + 3]] = intcode[intcode[i + 1]] * intcode[intcode[i + 2]];
                    i += 3;
                    break;

                case 99:
                default:
                    return;
            }
        }
    }
}