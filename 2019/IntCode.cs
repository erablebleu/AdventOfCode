namespace AdventOfCode;

public delegate long IntCodeInputEvent(object sender, IntCodeInputEventArgs e);

public delegate void IntCodeOutputEvent(object sender, IntCodeOutputEventArgs e);

public class IntCode
{
    public long[] _intcode;
    private long _relativeBase;

    public IntCode(long[] intcode)
    {
        _intcode = intcode;
    }

    public event EventHandler End;

    public event IntCodeInputEvent NewInput;

    public event IntCodeOutputEvent NewOutput;

    public long[] Data { get => _intcode; }
    public long Input { get; set; }

    public void Exec()
    {
        int outputIdx = 0;
        int inputIdx = 0;
        _relativeBase = 0;
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
                    if (NewInput != null)
                        SetValue(digits[2], i + 1, NewInput.Invoke(this, new IntCodeInputEventArgs() { Idx = inputIdx }));
                    else
                        SetValue(digits[2], i + 1, Input);
                    inputIdx++;
                    i += 1;
                    break;

                case 4: // output
                    long output = GetValue(GetAddress(digits[2], i + 1));
                    //Console.WriteLine($"####### OUTPUT:{output}");
                    i += 1;
                    NewOutput?.Invoke(this, new IntCodeOutputEventArgs() { Value = output, Idx = outputIdx });
                    outputIdx++;
                    break;

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
                    End?.Invoke(this, null);
                    return;

                default:
                    Console.WriteLine($"Intcode unknown opcode:{opcode}");
                    return;
            }
        }
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

public class IntCodeInputEventArgs : EventArgs
{
    public int Idx { get; set; }
}

public class IntCodeOutputEventArgs : EventArgs
{
    public int Idx { get; set; }
    public long Value { get; set; }
}