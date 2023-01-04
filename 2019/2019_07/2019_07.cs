namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/07
/// </summary>
public class _2019_07 : Problem
{
    private const int AMP_COUNT = 5;
    private const int MAX_PHASE = 5;
    private readonly Amplifier[] _amps = new Amplifier[AMP_COUNT];

    public override void Parse()
    {
    }

    public override object PartOne()
    {
        int max = int.MinValue;
        int input;

        for (int i = 0; i < _amps.Length; i++)
            _amps[i] = new Amplifier(Inputs[0].Split(',').Select(s => int.Parse(s)).ToArray());

        for (int p = 0; p < Math.Pow(MAX_PHASE, _amps.Length); p++)
        {
            bool dbl = false;
            input = 0;

            for (int i = 0; i < _amps.Length; i++)
                _amps[i].Phase = p.GetDigit(i, MAX_PHASE);

            for (int i = 0; i < _amps.Length; i++)
                for (int j = i + 1; j < _amps.Length; j++)
                    dbl |= _amps[i].Phase == _amps[j].Phase;

            if (dbl)
                continue;

            for (int i = 0; i < _amps.Length; i++)
            {
                _amps[i].Reset();
                _amps[i].Exec(input, ref input);
            }

            max = Math.Max(input, max);
        }

        return max;
    }

    public override object PartTwo()
    {
        int max = int.MinValue;

        for (int p = 0; p < Math.Pow(MAX_PHASE, _amps.Length); p++)
            max = Math.Max(Ex2(_amps, p), max);

        return max;
    }

    private static int Ex2(Amplifier[] _amps, int p)
    {
        bool dbl = false;
        int input = 0;
        int output = 0;

        for (int i = 0; i < _amps.Length; i++)
            _amps[i].Phase = p.GetDigit(i, MAX_PHASE) + 5;

        for (int i = 0; i < _amps.Length; i++)
            for (int j = i + 1; j < _amps.Length; j++)
                dbl |= _amps[i].Phase == _amps[j].Phase;

        if (dbl)
            return 0;

        for (int i = 0; i < _amps.Length; i++)
            _amps[i].Reset();

        while (true)
        {
            for (int i = 0; i < _amps.Length; i++)
                if (!_amps[i].Exec(input, ref input))
                    return output;
            output = input;
        }
    }

    private class Amplifier
    {
        private int _i;
        private int _inputCnt;
        private int[] _intcode;
        private int[] _intcode_bak;

        public Amplifier(int[] intcode)
        {
            _intcode_bak = intcode;
        }

        public int Phase { get; set; }

        public bool Exec(int input, ref int output)
        {
            for (int i = _i; i < _intcode.Length; i++)
            {
                switch (_intcode[i] % 100)
                {
                    case 1: // +
                        _intcode[_intcode[i + 3]] = GetValue(_intcode[i].GetDigit(2), _intcode[i + 1])
                                                  + GetValue(_intcode[i].GetDigit(3), _intcode[i + 2]);
                        i += 3;
                        break;

                    case 2: // *
                        _intcode[_intcode[i + 3]] = GetValue(_intcode[i].GetDigit(2), _intcode[i + 1])
                                                  * GetValue(_intcode[i].GetDigit(3), _intcode[i + 2]);
                        i += 3;
                        break;

                    case 3: // input
                        if (_inputCnt == 0)
                            _intcode[_intcode[i + 1]] = Phase;
                        else
                            _intcode[_intcode[i + 1]] = input;
                        _inputCnt++;
                        i += 1;
                        break;

                    case 4: // output
                        _i = i + 2;
                        output = _intcode[_intcode[i + 1]];
                        return true;

                    case 5: // jump-if-true
                        if (GetValue(_intcode[i].GetDigit(2), _intcode[i + 1]) != 0)
                            i = GetValue(_intcode[i].GetDigit(3), _intcode[i + 2]) - 1;
                        else
                            i += 2;
                        break;

                    case 6: // jump-if-false
                        if (GetValue(_intcode[i].GetDigit(2), _intcode[i + 1]) == 0)
                            i = GetValue(_intcode[i].GetDigit(3), _intcode[i + 2]) - 1;
                        else
                            i += 2;
                        break;

                    case 7: // less than
                        _intcode[_intcode[i + 3]] = GetValue(_intcode[i].GetDigit(2), _intcode[i + 1])
                                                    < GetValue(_intcode[i].GetDigit(3), _intcode[i + 2])
                                                    ? 1 : 0;
                        i += 3;
                        break;

                    case 8: // equals
                        _intcode[_intcode[i + 3]] = GetValue(_intcode[i].GetDigit(2), _intcode[i + 1])
                                                    == GetValue(_intcode[i].GetDigit(3), _intcode[i + 2])
                                                    ? 1 : 0;
                        i += 3;
                        break;

                    case 99:
                    default:
                        _i = i;
                        return false;
                }
            }
            return false;
        }

        public void Reset()
        {
            _intcode = new int[_intcode_bak.Length];
            Buffer.BlockCopy(_intcode_bak, 0, _intcode, 0, _intcode_bak.Length * sizeof(int));
            _inputCnt = 0;
            _i = 0;
        }

        private int GetValue(int paramMode, int addr)
        {
            switch (paramMode)
            {
                case 0: return _intcode[addr];
                case 1: return addr;
            }
            return 0;
        }
    }
}

public static class IntExtension
{
    public static int GetDigit(this int val, int digit, int pow = 10)
    {
        return val / (int)Math.Pow(pow, digit) % pow;
    }

    public static int GetDigit(this long val, int digit, int pow = 10)
    {
        return (int)(val / (long)Math.Pow(pow, digit) % pow);
    }

    public static int[] GetDigits(this long val)
    {
        int bitCnt = 8 * sizeof(long);
        int[] res = new int[bitCnt];
        for (int i = 0; i < bitCnt; i++)
            res[i] = GetDigit(val, i);
        return res;
    }
}