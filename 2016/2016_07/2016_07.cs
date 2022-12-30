namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/07
/// </summary>
public class _2016_07 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne() => Inputs.Count(l => SupportTLS(l));

    public override object PartTwo() => Inputs.Count(l => SupportSSL(l));

    private static IEnumerable<string> GetABA(IEnumerable<string> values)
    {
        foreach (string value in values)
            for (int i = 0; i < value.Length - 2; i++)
                if (value[i] == value[i + 2] && value[i] != value[i + 1])
                    yield return value.Substring(i, 3);
        yield break;
    }

    private static bool IsABBA(string value)
    {
        for (int i = 0; i < value.Length - 3; i++)
            if (value[i] == value[i + 3] && value[i + 1] == value[i + 2] && value[i] != value[i + 1])
                return true;
        return false;
    }

    private static bool SupportSSL(string value)
    {
        string[] el = value.Split('[', ']');
        List<string> supernet = GetABA(el.Where((x, i) => i % 2 == 0)).ToList();
        List<string> hypernet = GetABA(el.Where((x, i) => i % 2 != 0)).ToList();

        return supernet.Any(s => hypernet.Any(h => s[0] == h[1] && s[1] == h[0]));
    }

    private static bool SupportTLS(string value)
    {
        string[] el = value.Split('[', ']');
        for (int i = 1; i < el.Length; i += 2)
            if (IsABBA(el[i]))
                return false;
        for (int i = 0; i < el.Length; i += 2)
            if (IsABBA(el[i]))
                return true;
        return false;
    }
}