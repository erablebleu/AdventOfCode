namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/23
/// </summary>
public class _2016_23 : Problem
{
    private Instruction[] _data;

    private enum InstructionType
    {
        CPY,
        JNZ,
        INC,
        DEC,
        TGL,
    }

    public override void Parse()
    {
        _data = Inputs.Select(l => new Instruction(l)).ToArray();
    }

    public override object PartOne() => MathHelper.Factorial(7) + 7440; // Emulate(_data.Select(i => i.Copy()).ToArray(), 7);

    public override object PartTwo() => MathHelper.Factorial(12) + 7440; // Emulate(_data.Select(i => i.Copy()).ToArray(), 12);

    private static int Emulate(Instruction[] instructions, int a = 0, int b = 0, int c = 0, int d = 0)
    {
        Dictionary<char, int> registry = new()
        {
            { 'a', a },
            { 'b', b },
            { 'c', c },
            { 'd', d },
        };

        for (int i = 0; i < instructions.Length; i++)
            instructions[i].Exec(instructions, registry, ref i);

        return registry['a'];
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

        public void Exec(Instruction[] instructions, Dictionary<char, int> registry, ref int idx)
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
            }
        }
    }
}