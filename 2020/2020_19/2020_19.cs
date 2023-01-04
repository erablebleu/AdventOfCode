namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/19
/// </summary>
public class _2020_19 : Problem
{
    private string[] _messages;
    private Dictionary<int, TextRule> _rules;

    public override void Parse()
    {
        int endRules = Array.IndexOf(Inputs, string.Empty);
        _messages = Inputs.Skip(endRules + 1).ToArray();
        _rules = Inputs.Take(endRules).Select(l => new TextRule(l)).ToDictionary(r => r.Number, r => r);

        foreach (var rule in _rules.Values)
            rule.AttachRules(_rules);
    }

    public override object PartOne() => _messages.Count(l => _rules[0].Match(l));

    public override object PartTwo()
    {
        _rules[8].Update("42 | 42 8");
        _rules[8].AttachRules(_rules);

        _rules[11].Update("42 31 | 42 11 31");
        _rules[11].AttachRules(_rules);

        return _messages.Count(l => _rules[0].Match(l));
    }

    private class TextRule
    {
        public bool EqualChar;

        public int Number;

        public TextRule[][] SubRules;

        public int[][] SubRulesIdx = Array.Empty<int[]>();

        public char TargetChar;

        public TextRule(string rule)
        {
            string[] el = rule.Split(": ");
            Number = int.Parse(el[0]);
            Update(el[1]);
        }

        public void AttachRules(Dictionary<int, TextRule> rules)
        {
            SubRules = SubRulesIdx.Select(ar => ar.Select(i => rules[i]).ToArray()).ToArray();
        }

        public bool Match(string txt) => Match(txt, 0).Contains(txt.Length);

        public int[] Match(string txt, int idx)
        {
            if (idx >= txt.Length)
                return Array.Empty<int>();

            if (EqualChar)
            {
                if (txt[idx] == TargetChar)
                    return new int[] { 1 };

                return Array.Empty<int>();
            }

            List<int> result = new();
            foreach (TextRule[] group in SubRules)
            {
                List<int> perms = new() { 0 };
                foreach (TextRule rule in group)
                    perms = perms.SelectMany(i => rule.Match(txt, idx + i).Select(j => i + j)).ToList();

                result.AddRange(perms);
            }

            return result.ToArray();
        }

        public override string ToString() => $"{Number}: " + (EqualChar ? $"\"{TargetChar}\"" : string.Join(" | ", SubRulesIdx.Select(g => string.Join(" ", g))));

        public void Update(string rule)
        {
            if (rule.Contains('"'))
            {
                EqualChar = true;
                TargetChar = rule[1];
            }
            else
                SubRulesIdx = rule.Split(" | ").Select(l => l.Split(" ").Select(i => int.Parse(i)).ToArray()).ToArray();
        }
    }
}