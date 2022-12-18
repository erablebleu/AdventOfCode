using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class TextRule
    {
        public int Idx { get; set; }
        public bool EqualChar { get; set; }
        public char TargetChar { get; set; }
        public List<List<int>> SubRulesIdx { get; set; }
        public List<List<TextRule>> SubRules { get; set; }
        public TextRule(int idx, string rule)
        {
            Idx = idx;
            if (rule.Contains("\""))
            {
                EqualChar = true;
                TargetChar = rule.Replace("\"", "")[0];
            }
            else
                SubRulesIdx = rule.Split(" | ").Select(l => l.Split(" ").Select(i => int.Parse(i)).ToList()).ToList();
        }
        public bool Match(string txt) => Match(ref txt) && txt.Length == 0;
        public bool Match(ref string txt)
        {
            if (EqualChar)
            {
                bool res = txt.Length >= 1 && txt[0] == TargetChar;
                if (res) txt = txt[1..];
                return res;
            }
            foreach (var ls in SubRules)
            {
                string tmp = txt;
                if (ls.All(r => r.Match(ref tmp)))
                {
                    txt = tmp;
                    return true;
                }
            }
            return false;
        }
        public bool Match2(string txt) => Match2(ref txt) && txt.Length == 0;
        public bool Match2(ref string txt)
        {
            if (txt.Length == 0)
                return false;
            if (EqualChar)
            {
                bool res = txt[0] == TargetChar;
                if (res) txt = txt[1..];
                return res;
            }
            foreach (var ls in SubRules)
            {
                string tmp = txt;
                if (ls.All(r => r.Match2(ref tmp)))
                {
                    txt = tmp;
                    return true;
                }
            }
            return false;
        }
    }
    public class _2020_19 : Problem
    {
        private readonly Dictionary<int, TextRule> _rules = new();
        private readonly Dictionary<int, TextRule> _rules2 = new();
        #region Methods

        public override void Solve()
        {
            int lineEndRule = Inputs.ToList().IndexOf(string.Empty);
            for (int i = 0; i < lineEndRule; i++)
            {
                string[] el = Inputs[i].Split(": ");
                int idx = int.Parse(el[0]);
                string rule = el[1];
                _rules.Add(idx, new TextRule(idx, rule));
                switch (idx)
                {
                    case 8: rule = "42 | 42 8"; break;
                    case 11: rule = "42 31 | 42 11 31"; break;
                }
                _rules2.Add(idx, new TextRule(idx, rule));
            }
            foreach (var rule in _rules.Values.Where(r => !r.EqualChar))
                rule.SubRules = rule.SubRulesIdx.Select(l => l.Select(i => _rules[i]).ToList()).ToList();
            foreach (var rule in _rules2.Values.Where(r => !r.EqualChar))
                rule.SubRules = rule.SubRulesIdx.Select(l => l.Select(i => _rules2[i]).ToList()).ToList();

            Solutions.Add($"{Inputs.Skip(lineEndRule + 1).Count(l => _rules[0].Match(l))}");
            Solutions.Add($"{Inputs.Skip(lineEndRule + 1).Count(l => _rules[0].Match2(l))}");
            Solutions.Add($"{Inputs.Skip(lineEndRule + 1).Count(l => _rules2[0].Match2(l))}");
            Redditor red = new Redditor();
            Solutions.Add($"{Redditor.Run(Redditor.ParseInput(string.Join("\r\n", Inputs)), true)}");
        }

        #endregion
    }
    public class Redditor
    {

        public static int Run(Input input, bool p2)
        {
            var rules = input.Rules.ToList();
            if (p2)
            {
                rules.Add(new SeqRule { Num = 8, Seq = new() { 42, 8 } });
                rules.Add(new SeqRule { Num = 11, Seq = new() { 42, 11, 31 } });
            }
            var matcher = new Matcher(rules);
            return input.Messages.Count(m => matcher.IsMach(m));
        }

        public class Matcher
        {
            private ILookup<int, Rule> _rules;

            public Matcher(IEnumerable<Rule> rules)
            {
                _rules = rules.ToLookup(r => r.Num);
            }

            public bool IsMach(string input) => Match(input, 0, 0).Any(m => m == input.Length);

            IEnumerable<int> Match(string input, int num, int pos)
            {
                foreach (var rule in _rules[num])
                    switch(rule)
                    {
                        case LitRule lit:
                            if (string.CompareOrdinal(input, pos, lit.Lit, 0, lit.Lit.Length) == 0)
                                yield return pos + lit.Lit.Length;
                            break;
                        case SeqRule seq:
                            foreach (var end in MatchSeq(input, seq, pos, 0))
                                yield return end;
                            break;
                    }
            }

            IEnumerable<int> MatchSeq(string input, SeqRule seq, int pos, int index)
            {
                if (index == seq.Seq.Count)
                {
                    yield return pos;
                    yield break;
                }
                foreach (var end in Match(input, seq.Seq[index], pos))
                    foreach (var end2 in MatchSeq(input, seq, end, index + 1))
                        yield return end2;
            }
        }

        public static Input ParseInput(string input)
        {
            var result = new Input();
            var lines = input.Trim().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var pat = new Regex(@"^(\d+):(?: ""([^""]*)""|(?: ((?:\| )?)(\d+))+)\s*$");
            foreach (var line in lines)
            {
                if (pat.Match(line) is { Success: true } m)
                {
                    if (m.Groups[2].Success)
                    {
                        result.Rules.Add(new LitRule
                        {
                            Num = int.Parse(m.Groups[1].Value),
                            Lit = m.Groups[2].Value
                        });
                    }
                    else
                    {
                        var seq = new List<int>();
                        var alt = new List<List<int>> { seq };
                        for (int i = 0; i < m.Groups[4].Captures.Count; i++)
                        {
                            if (m.Groups[3].Captures[i].Length != 0)
                            {
                                alt.Add(seq = new List<int>());
                            }
                            seq.Add(int.Parse(m.Groups[4].Captures[i].Value));
                        }
                        foreach (var seql in alt)
                        {
                            result.Rules.Add(new SeqRule
                            {
                                Num = int.Parse(m.Groups[1].Value),
                                Seq = seql
                            });
                        }
                    }
                }
                else
                {
                    result.Messages.Add(line);
                }
            }
            return result;
        }
        public abstract class Rule
        {
            public int Num { get; set; }
        }
        public class LitRule : Rule
        {
            public string Lit { get; set; }
        }
        public class SeqRule : Rule
        {
            public List<int> Seq { get; set; }
        }
        public class Input
        {
            public List<Rule> Rules { get; } = new();
            public List<string> Messages { get; } = new();
        }
    }
}