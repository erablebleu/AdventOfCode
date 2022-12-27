namespace AdventOfCode;

public class _2015_07 : Problem
{
    private Dictionary<string, Gate> _gates;

    public override void Parse()
    {
        _gates = Inputs.Select(l => new Gate(l)).ToDictionary(g => g.Name, g => g);
        foreach (Gate g in _gates.Values)
            for (int i = 0; i < 2; i++)
                if (g.DependancyNames[i] is not null)
                    g.Dependancies[i] = _gates[g.DependancyNames[i]];
    }

    public override object PartOne() => _gates["a"].Evaluate();

    public override object PartTwo()
    {
        int value = _gates["a"].Evaluate();
        foreach (Gate g in _gates.Values)
            g.Reset();
        _gates["b"].Reset(value);
        return _gates["a"].Evaluate();
    }

    internal class Gate
    {
        public Gate[] Dependancies = new Gate[2];
        public string[] DependancyNames = new string[2];
        public string Name;

        private GateType _type;
        private int? _value;
        private int[] _values = new int[2];

        public Gate(string value)
        {
            string[] el = value.Split(' ');
            Name = el.Last();
            if (el.Length > 3)
                _type = (GateType)Enum.Parse(typeof(GateType), el[^4]);
            switch (_type)
            {
                case GateType._:
                    GetDependency(0, el[0]);
                    break;

                case GateType.AND:
                case GateType.OR:
                    GetDependency(0, el[0]);
                    GetDependency(1, el[2]);
                    break;

                case GateType.LSHIFT:
                case GateType.RSHIFT:
                    GetDependency(0, el[0]);
                    _values[1] = int.Parse(el[2]);
                    break;

                case GateType.NOT:
                    GetDependency(0, el[1]);
                    break;
            }
        }

        private enum GateType
        {
            _,
            AND,
            OR,
            LSHIFT,
            RSHIFT,
            NOT,
        }

        public int Evaluate()
        {
            if (!_value.HasValue)
                _value = GetValue();

            return _value.Value;
        }

        public void Reset(int? value = null)
        {
            _value = value;
        }

        private void GetDependency(int idx, string text)
        {
            if (!int.TryParse(text, out _values[idx]))
                DependancyNames[idx] = text;
        }

        private int GetValue(int idx) => Dependancies[idx]?.Evaluate() ?? _values[idx];

        private int GetValue() => _type switch
        {
            GateType._ => GetValue(0),
            GateType.AND => GetValue(0) & GetValue(1),
            GateType.OR => GetValue(0) | GetValue(1),
            GateType.LSHIFT => GetValue(0) << GetValue(1),
            GateType.RSHIFT => GetValue(0) >> GetValue(1),
            GateType.NOT => ~GetValue(0),
            _ => throw new NotImplementedException(),
        };
    }
}