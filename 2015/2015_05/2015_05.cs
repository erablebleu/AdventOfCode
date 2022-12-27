using static AdventOfCode._2022_16;

namespace AdventOfCode;

public class _2015_05 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne() => Inputs.Count(l => l.Count(c => c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u') >= 3
            && Enumerable.Range(0, l.Length - 1).Any(i => l[i] == l[i + 1])
            && !l.Contains("ab")
            && !l.Contains("cd")
            && !l.Contains("pq")
            && !l.Contains("xy"));

    public override object PartTwo() => Inputs.Count(l => Enumerable.Range(0, l.Length - 3).Any(i => Enumerable.Range(i + 2, l.Length - i - 3).Any(j => l[i] == l[j] && l[i + 1] == l[j + 1]))
            && Enumerable.Range(0, l.Length - 2).Any(i => l[i] == l[i + 2]));
}