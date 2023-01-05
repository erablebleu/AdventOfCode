namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2017/day/08
/// </summary>
public class _2017_08 : Problem
{
    private Instruction[] _data;

    public enum ConditionType
    {
        Sup,
        SupEq,
        Inf,
        InfEq,
        Eq,
        NEq,
    }

    private enum InstructionType
    {
        INC,
        DEC,
    }

    public override void Parse()
    {
        _data = Inputs.Select(l => new Instruction(l)).ToArray();
    }

    public override object PartOne()
    {
        Dictionary<string, int> registry = new();

        foreach (Instruction instruction in _data)
            instruction.Exec(registry);

        return registry.Values.Max();
    }

    public override object PartTwo()
    {
        Dictionary<string, int> registry = new();
        int result = int.MinValue;

        foreach (Instruction instruction in _data)
        {
            instruction.Exec(registry);
            result = Math.Max(result, registry.Values.Max());
        }

        return result;
    }

    private class Instruction
    {
        public string ConditionTarget;
        public ConditionType ConditionType;
        public int ConditionValue;
        public string Target;
        public InstructionType Type;
        public int Value;

        public Instruction(string line)
        {
            string[] el = line.Split(' ');
            Target = el[0];
            Type = (InstructionType)Enum.Parse(typeof(InstructionType), el[1].ToUpper());
            Value = int.Parse(el[2]);
            ConditionType = el[5] switch
            {
                ">" => ConditionType.Sup,
                ">=" => ConditionType.SupEq,
                "<" => ConditionType.Inf,
                "<=" => ConditionType.InfEq,
                "==" => ConditionType.Eq,
                "!=" => ConditionType.NEq,
                _ => throw new NotImplementedException()
            };
            ConditionTarget = el[4];
            ConditionValue = int.Parse(el[6]);
        }

        public void Exec(Dictionary<string, int> registry)
        {
            if (!Condition(registry.GetOrAdd(ConditionTarget, 0)))
                return;

            registry[Target] = registry.GetOrAdd(Target, 0) + Type switch
            {
                InstructionType.INC => Value,
                InstructionType.DEC => -Value,
                _ => throw new NotImplementedException(),
            };
        }

        private bool Condition(int value) => ConditionType switch
        {
            ConditionType.Sup => value > ConditionValue,
            ConditionType.SupEq => value >= ConditionValue,
            ConditionType.Inf => value < ConditionValue,
            ConditionType.InfEq => value <= ConditionValue,
            ConditionType.Eq => value == ConditionValue,
            ConditionType.NEq => value != ConditionValue,
            _ => throw new NotImplementedException()
        };
    }
}