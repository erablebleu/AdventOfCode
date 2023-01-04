namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/11
/// </summary>
public class _2022_11 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne() 
        => Emulate(Enumerable.Range(0, Inputs.Length / 7 + 1).Select(i => new Monkey(Inputs.Skip(i * 7).Take(6).ToArray())).ToList(), 20, (l, p) => l / 3);

    public override object PartTwo()
        => Emulate(Enumerable.Range(0, Inputs.Length / 7 + 1).Select(i => new Monkey(Inputs.Skip(i * 7).Take(6).ToArray())).ToList(), 10000, (l, p) => l % p);

    private static long Emulate(List<Monkey> monkeys, int count, Func<long, long, long> function)
    {
        long prod = monkeys.Select(m => m.Test).Product();

        for (int round = 0; round < count; round++)
        {
            foreach (Monkey monkey in monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    long val = function(monkey.NewVal(monkey.Items[0]), prod);
                    monkey.Items.RemoveAt(0);

                    if (val % monkey.Test == 0)
                        monkeys[monkey.TargetTrue].Items.Add(val);
                    else
                        monkeys[monkey.TargetFalse].Items.Add(val);
                }
            }
        }

        return monkeys.Select(m => m.InspectCount).OrderByDescending(m => m).Take(2).Product();
    }

    private class Monkey
    {
        private readonly int _opMult = 1;
        private readonly int _opOffs = 0;
        private readonly bool _opSquare = false;

        public Monkey(string[] lines)
        {
            Number = int.Parse(lines[0].Substring(7).Replace(":", ""));
            Items = lines[1].Replace("  Starting items: ", "").Split(", ").Select(i => long.Parse(i)).ToList();
            switch (lines[2][23])
            {
                case '*':
                    if (lines[2].Substring(25) == "old")
                        _opSquare = true;
                    else
                        _opMult = int.Parse(lines[2].Substring(25));
                    break;

                case '+':
                    _opOffs = int.Parse(lines[2].Substring(25));
                    break;
            }
            Test = int.Parse(lines[3].Replace("  Test: divisible by ", ""));
            TargetTrue = int.Parse(lines[4].Replace("    If true: throw to monkey ", ""));
            TargetFalse = int.Parse(lines[5].Replace("    If false: throw to monkey ", ""));
        }

        public int InspectCount { get; private set; }
        public List<long> Items { get; set; } = new();
        public int Number { get; private set; }
        public int TargetFalse { get; private set; }
        public int TargetTrue { get; private set; }
        public int Test { get; private set; }

        public long NewVal(long value)
        {
            long result;
            InspectCount++;
            if (_opSquare)
                result = value * value;
            else
                result = value * _opMult + _opOffs;

            //if (result % Test == 0)
            //    result = 0;

            return result;
        }
    }
}