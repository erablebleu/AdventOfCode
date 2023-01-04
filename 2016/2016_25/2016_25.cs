namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/25
/// </summary>
public class _2016_25 : Problem
{
    private Instruction[] _data;

    private enum InstructionType
    {
        CPY,
        JNZ,
        INC,
        DEC,
        TGL,
        OUT,
    }

    public override void Parse()
    {
        _data = Inputs.Select(l => new Instruction(l)).ToArray();
    }

    public override object PartOne()
    {
        for (int a = 0; ; a++)
            if (Emulate(_data, a))
                return a;
    }

    public override object PartTwo() => "Merry Christmas!";

    private static bool Emulate(Instruction[] instructions, int a = 0, int b = 0, int c = 0, int d = 0)
    {
        Dictionary<char, int> registry = new()
        {
            { 'a', a },
            { 'b', b },
            { 'c', c },
            { 'd', d },
        };

        List<int[]> regHistory = new();

        for (int i = 0; i < instructions.Length; i++)
        {
            object ret = instructions[i].Exec(instructions, registry, ref i);

            if (ret is null
                || ret is not int val)
                continue;

            if (val % 2 != regHistory.Count % 2)
                return false;

            regHistory.Add(registry.Values.ToArray());

            if (regHistory.Count % 2 != 0)
                continue;

            // look for loop in registry history
            if (HasLoop(regHistory))
                return true;
        }

        return false;
    }

    private static bool HasLoop(List<int[]> list)
    {
        for (int i = 2; i < list.Count / 2; i++)
        {
            bool loop = true;

            for (int j = 0; j < i; j++)
                for (int k = 0; k < 4; k++)
                    if (list[list.Count - j - 1][k] != list[list.Count - i - j - 1][k])
                        loop = false;

            if (loop)
                return true;
        }
        return false;
    }

    private class Instruction
    {
        private char[] _indexes = new char[2];
        private bool[] _isValue = new bool[2];
        private InstructionType _type;
        private int[] _values = new int[2];

        public Instruction()
        { }

        public Instruction(string line)
        {
            string[] el = line.Split(' ');
            _type = (InstructionType)Enum.Parse(typeof(InstructionType), el[0].ToUpper());
            for (int i = 1; i < el.Length; i++)
            {
                _isValue[i - 1] = int.TryParse(el[i], out int value);
                _values[i - 1] = value;
                _indexes[i - 1] = el[i][0];
            }
        }

        public Instruction Copy() => new()
        {
            _type = _type,
            _isValue = _isValue,
            _indexes = _indexes,
            _values = _values
        };

        public object Exec(Instruction[] instructions, Dictionary<char, int> registry, ref int idx)
        {
            int GetValue(int idx) => _isValue[idx] ? _values[idx] : registry[_indexes[idx]];

            switch (_type)
            {
                case InstructionType.CPY:
                    registry[_indexes[1]] = GetValue(0);
                    break;

                case InstructionType.INC:
                    registry[_indexes[0]]++;
                    break;

                case InstructionType.DEC:
                    registry[_indexes[0]]--;
                    break;

                case InstructionType.JNZ:
                    if (GetValue(0) == 0)
                        break;
                    idx += GetValue(1) - 1;
                    break;

                case InstructionType.TGL:
                    int i = idx + GetValue(0);
                    if (!i.IsInRange(0, instructions.Length))
                        break;
                    Instruction instr = instructions[i];
                    instr._type = instr._type switch
                    {
                        InstructionType.CPY => InstructionType.JNZ,
                        InstructionType.JNZ => InstructionType.CPY,
                        InstructionType.INC => InstructionType.DEC,
                        InstructionType.DEC => InstructionType.INC,
                        InstructionType.TGL => InstructionType.INC,
                        _ => throw new NotImplementedException()
                    };

                    break;

                case InstructionType.OUT: return GetValue(0);
            }

            return null;
        }
    }
}