namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/05
/// </summary>
public class _2019_05 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs[0].Split(',').Select(s => int.Parse(s)).ToArray();
    }

    public override object PartOne() => ExecCode(_data.ToArray(), 1);

    public override object PartTwo() => ExecCode(_data.ToArray(), 5);

    private static int ExecCode(int[] intcode, int input)
    {
        int result = 0;
        for (int i = 0; i < intcode.Length; i++)
        {
            switch (intcode[i] % 100)
            {
                case 1: // +
                    intcode[intcode[i + 3]] = GetValue(intcode, GetDigit(intcode[i], 2), intcode[i + 1])
                                              + GetValue(intcode, GetDigit(intcode[i], 3), intcode[i + 2]);
                    i += 3;
                    break;

                case 2: // *
                    intcode[intcode[i + 3]] = GetValue(intcode, GetDigit(intcode[i], 2), intcode[i + 1])
                                              * GetValue(intcode, GetDigit(intcode[i], 3), intcode[i + 2]);
                    i += 3;
                    break;

                case 3: // input
                    intcode[intcode[i + 1]] = input;
                    i += 1;
                    break;

                case 4: // output
                        //Console.WriteLine($"{intcode[i - 4]},{intcode[i - 3]},{intcode[i - 2]},{intcode[i - 1]}  ->  {intcode[intcode[i + 1]]}");
                    result = intcode[intcode[i + 1]];
                    i += 1;
                    break;

                case 5: // jump-if-true
                    if (GetValue(intcode, GetDigit(intcode[i], 2), intcode[i + 1]) != 0)
                        i = GetValue(intcode, GetDigit(intcode[i], 3), intcode[i + 2]) - 1;
                    else
                        i += 2;
                    break;

                case 6: // jump-if-false
                    if (GetValue(intcode, GetDigit(intcode[i], 2), intcode[i + 1]) == 0)
                        i = GetValue(intcode, GetDigit(intcode[i], 3), intcode[i + 2]) - 1;
                    else
                        i += 2;
                    break;

                case 7: // less than
                    intcode[intcode[i + 3]] = GetValue(intcode, GetDigit(intcode[i], 2), intcode[i + 1])
                                                < GetValue(intcode, GetDigit(intcode[i], 3), intcode[i + 2])
                                                ? 1 : 0;
                    i += 3;
                    break;

                case 8: // equals
                    intcode[intcode[i + 3]] = GetValue(intcode, GetDigit(intcode[i], 2), intcode[i + 1])
                                                == GetValue(intcode, GetDigit(intcode[i], 3), intcode[i + 2])
                                                ? 1 : 0;
                    i += 3;
                    break;

                case 99:
                default:
                    return result;
            }
        }
        return result;
    }

    private static int GetDigit(int val, int digit) => val / (int)Math.Pow(10, digit) % 10;

    private static int GetValue(int[] intcode, int paramMode, int addr) => paramMode switch
    {
        0 => intcode[addr],
        1 => addr,
        _ => 0,
    };
}