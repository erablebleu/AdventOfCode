namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/18
/// </summary>
public class _2020_18 : Problem
{
    private static readonly string Numbers = "0123456789";

    public override void Parse()
    {
    }

    public override object PartOne() => Inputs.Sum(l => EvaluateParentheses(l.Replace(" ", ""), out var _));

    public override object PartTwo() => Inputs.Sum(l => EvaluateParentheses2(l.Replace(" ", "")));

    private static long Evaluate(char op, long v1, long v2) => op switch
    {
        '*' => v1 * v2,
        '+' => v1 + v2,
        _ => v2
    };

    private static long Evaluate(Func<long, long, long> calc, string exp, int idx, out int a, out int b)
    {
        for (a = idx - 1; a >= 0 && Numbers.Contains(exp[a]); a--) ;
        for (b = idx + 1; b < exp.Length && Numbers.Contains(exp[b]); b++) ;
        return calc.Invoke(long.Parse(exp.Substring(a + 1, idx - a - 1)), long.Parse(exp.Substring(idx + 1, b - idx - 1)));
    }

    private static long EvaluateParentheses(string exp, out int idx)
    {
        long res = 0;
        string v = string.Empty;
        char lastop = ' ';

        for (int i = 0; i < exp.Length; i++)
        {
            switch (exp[i])
            {
                case '(':
                    res = Evaluate(lastop, res, EvaluateParentheses(exp[(i + 1)..], out var idx2));
                    i += idx2 + 1;
                    break;

                case ')':
                    if (v.Length > 0)
                        res = Evaluate(lastop, res, long.Parse(v));
                    idx = i;
                    return res;

                case '*':
                case '+':
                    if (v.Length > 0)
                        res = Evaluate(lastop, res, long.Parse(v));
                    v = string.Empty;
                    lastop = exp[i];
                    break;

                default:
                    v += exp[i];
                    break;
            }
        }
        if (v.Length > 0)
            res = Evaluate(lastop, res, long.Parse(v));
        idx = exp.Length;
        return res;
    }

    private static long EvaluateParentheses2(string exp)
    {
        string tempo = exp;
        while (exp.Contains('('))
        {
            int i = exp.IndexOf('(');
            int j = 0;
            int cnt = 0;
            for (j = i + 1; cnt >= 0; j++)
            {
                switch (exp[j])
                {
                    case '(': cnt++; break;
                    case ')': cnt--; break;
                }
            }
            string p = exp.Substring(i + 1, j - i - 2);
            exp = exp.Replace($"({p})", $"{EvaluateParentheses2(p)}");
        }
        while (exp.Contains('+'))
        {
            long val = Evaluate((a, b) => a + b, exp, exp.IndexOf('+'), out int a, out int b);
            exp = exp.Substring(0, a + 1) + $"{val}" + exp.Substring(b);
        }
        while (exp.Contains('*'))
        {
            long val = Evaluate((a, b) => a * b, exp, exp.IndexOf('*'), out int a, out int b);
            exp = exp.Substring(0, a + 1) + $"{val}" + exp.Substring(b);
        }
        return long.Parse(exp);
    }
}