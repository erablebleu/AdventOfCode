namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/09
/// </summary>
public class _2019_09 : Problem
{
    private long[] _data;
    private long[] _intcode;
    private long _relativeBase;

    public override void Parse()
    {
        _data = Inputs[0].Split(',').Select(s => long.Parse(s)).ToArray();
    }

    public override object PartOne()
    {
        _intcode = _data.ToArray();
        long output = 0;
        Exec(1, ref output);
        return output;
    }

    public override object PartTwo()
    {
        _intcode = _data.ToArray();
        _relativeBase = 0;
        long output = 0;
        Exec(2, ref output);
        return output;
    }

    private bool Exec(int input, ref long output)
    {
        for (long i = 0; i < _intcode.Length; i++)
        {
            long opcode = GetValue(i);
            //Console.WriteLine($"# i:{i.ToString("D3")} - {opcode}");
            int[] digits = opcode.GetDigits();
            switch (opcode % 100)
            {
                case 1: // +
                    SetValue(digits[4], i + 3, GetValue(digits[2], i + 1) + GetValue(digits[3], i + 2));
                    i += 3;
                    break;

                case 2: // *
                    SetValue(digits[4], i + 3, GetValue(digits[2], i + 1) * GetValue(digits[3], i + 2));
                    i += 3;
                    break;

                case 3: // input
                    SetValue(digits[2], i + 1, input);
                    i += 1;
                    break;

                case 4: // output
                    output = GetValue(GetAddress(digits[2], i + 1));
                    //Console.WriteLine($"####### OUTPUT:{output}");
                    i += 1;
                    break;
                    return true;

                case 5: // jump-if-true
                    if (GetValue(digits[2], i + 1) != 0)
                        i = GetValue(digits[3], i + 2) - 1;
                    else
                        i += 2;
                    break;

                case 6: // jump-if-false
                    if (GetValue(digits[2], i + 1) == 0)
                        i = GetValue(digits[3], i + 2) - 1;
                    else
                        i += 2;
                    break;

                case 7: // less than
                    SetValue(digits[4], i + 3, GetValue(digits[2], i + 1) < GetValue(digits[3], i + 2) ? 1 : 0);
                    i += 3;
                    break;

                case 8: // equals
                    SetValue(digits[4], i + 3, GetValue(digits[2], i + 1) == GetValue(digits[3], i + 2) ? 1 : 0);
                    i += 3;
                    break;

                case 9: // adjusts the relative base
                    _relativeBase += GetValue(digits[2], i + 1);
                    i += 1;
                    break;

                case 99:
                default:
                    return false;
            }
        }
        return false;
    }

    private long GetAddress(int paramMode, long idx)
    {
        switch (paramMode)
        {
            case 0: return GetValue(idx); // position mode
            case 1: return idx; //value
            case 2: return _relativeBase + GetValue(idx); // relative mode
        }
        return 0;
    }

    private long GetValue(int paramMode, long idx)
    {
        return GetValue(GetAddress(paramMode, idx));
    }

    private long GetValue(long addr)
    {
        ValidateAddress(addr);
        return _intcode[addr];
    }

    private void SetValue(int paramMode, long idx, long value)
    {
        long addr = GetAddress(paramMode, idx);
        ValidateAddress(addr);
        _intcode[addr] = value;
    }

    private void ValidateAddress(long addr)
    {
        if (addr >= _intcode.Length)
            Array.Resize(ref _intcode, (int)addr + 1);
    }
}