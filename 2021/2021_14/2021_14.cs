using System;
using AdventOfCode.Tools;

namespace AdventOfCode;

public class _2021_14 : Problem
{
    internal record PairInsertionRule(string Pair, string Inserted);
    private PairInsertionRule[] _pairInsertionRules;

    public override void Solve()
    {
        _pairInsertionRules = Inputs.Skip(2).Select(l => new PairInsertionRule(l[..2], l.Substring(6, 1))).ToArray();
        Dictionary<string, long> template = GetTemplate(Inputs.First());

        for (int i = 0; i < 10; i++)
            template = NextStep(template);

        Solutions.Add($"{MostMinusLeast(template)}");

        for (int i = 0; i < 30; i++)
            template = NextStep(template);

        Solutions.Add($"{MostMinusLeast(template)}");
    }

    private Dictionary<string, long> GetTemplate(string template)
    {
        Dictionary<string, long> result = new();
        for (int i = 0; i < template.Length - 1; i++)
            result.AddOrInc(template.Substring(i, 2), 1);
        return result;
    }

    private static long MostMinusLeast(Dictionary<string, long> template)
    {
        Dictionary<char, long> result = new();
        foreach(KeyValuePair<string, long> pair in template)
        {
            result.AddOrInc(pair.Key[0], pair.Value);
            result.AddOrInc(pair.Key[1], pair.Value);
        }
        IOrderedEnumerable<long> ordered = result.Values.OrderBy(v => v);
        return (ordered.Last() - ordered.First())/2;
    }

    private Dictionary<string, long> NextStep(Dictionary<string, long> template)
    {
        Dictionary<string, long> result = new();

        foreach (KeyValuePair<string, long> pair in template)
        {
            PairInsertionRule rule = _pairInsertionRules.First(r => r.Pair == pair.Key);
            result.AddOrInc(string.Concat(rule.Pair.AsSpan(0, 1), rule.Inserted), pair.Value);
            result.AddOrInc(string.Concat(rule.Inserted, rule.Pair.AsSpan(1, 1)), pair.Value);
        }

        return result;
    }

    private string NextStep(string template)
    {
        string result = template[0].ToString();
        for (int i = 1; i < template.Length; i++)
        {
            PairInsertionRule rule = _pairInsertionRules.FirstOrDefault(r => r.Pair == template.Substring(i - 1, 2));
            if (rule != null)
                result += rule.Inserted;
            result += template[i];
        }
        return result;
    }
}