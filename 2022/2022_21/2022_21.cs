namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/21
/// </summary>
public class _2022_21 : Problem
{
    private Dictionary<string, MonkeyNumber> _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => new MonkeyNumber(l)).ToDictionary(m => m.Name, m => m);
        foreach (MonkeyNumber m in _data.Values)
            m.SetDependencies(_data);
    }

    public override object PartOne() => _data["root"].GetValue();

    public override object PartTwo() => _data["root"].ReverseValue("humn");

    private class MonkeyNumber
    {
        private readonly bool _isOperation;
        private readonly string _m0;
        private readonly string _m1;
        private readonly string _op;
        private readonly long _value;

        public MonkeyNumber(string line)
        {
            string[] el = line.Split(": ");
            Name = el[0];
            if (long.TryParse(el[1], out long value))
            {
                _value = value;
            }
            else
            {
                _isOperation = true;
                string[] op = el[1].Split(" ");
                _m0 = op[0];
                _m1 = op[2];
                _op = op[1];
            }
        }

        public bool KeyDependent { get; private set; }
        public MonkeyNumber M0 { get; set; }
        public MonkeyNumber M1 { get; set; }
        public string Name { get; set; }

        public long GetValue()
        {
            if (!_isOperation) return _value;

            long v0 = M0.GetValue();
            long v1 = M1.GetValue();
            switch (_op)
            {
                case "+": return v0 + v1;
                case "-": return v0 - v1;
                case "*": return v0 * v1;
                case "/": return v0 / v1;
                default:
                    break;
            }
            return 0;
        }

        public double ReverseValue(string keySearch)
        {
            // fix dependencies
            M0.SearchDependency(keySearch);
            M1.SearchDependency(keySearch);
            return M0.KeyDependent ? M0.ReverseValue(keySearch, M1.GetValue()) : M1.ReverseValue(keySearch, M0.GetValue());
        }

        public double ReverseValue(string keySearch, double value)
        {
            if (Name == keySearch)
                return value;
            if (!_isOperation)
                return _value;

            switch (_op)
            {
                case "+": return M0.KeyDependent ? M0.ReverseValue(keySearch, value - M1.GetValue()) : M1.ReverseValue(keySearch, value - M0.GetValue());
                case "-": return M0.KeyDependent ? M0.ReverseValue(keySearch, value + M1.GetValue()) : M1.ReverseValue(keySearch, M0.GetValue() - value);
                case "*": return M0.KeyDependent ? M0.ReverseValue(keySearch, value / M1.GetValue()) : M1.ReverseValue(keySearch, value / M0.GetValue());
                case "/": return M0.KeyDependent ? M0.ReverseValue(keySearch, value * M1.GetValue()) : M1.ReverseValue(keySearch, M0.GetValue() / value);
                default:
                    break;
            }
            return 0;
        }

        public bool SearchDependency(string key)
        {
            if (Name == key)
                KeyDependent = true;
            else if (!_isOperation)
                KeyDependent = false;
            else
                KeyDependent = M0.SearchDependency(key) || M1.SearchDependency(key);
            return KeyDependent;
        }

        public void SetDependencies(Dictionary<string, MonkeyNumber> _data)
        {
            if (!_isOperation)
                return;
            M0 = _data[_m0];
            M1 = _data[_m1];
        }
    }
}