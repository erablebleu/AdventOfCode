namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2017/day/04
/// </summary>
public class _2017_04 : Problem
{
    private string[][] _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => l.Split(' ').ToArray()).ToArray();
    }

    public override object PartOne() => _data.Count(p => IsValid(p));

    public override object PartTwo() => _data.Count(p => IsValid2(p));

    private static bool IsValid(string[] phrase) => phrase.Distinct().Count() == phrase.Length;

    private static bool IsValid2(string[] phrase) => phrase.Distinct().Count() == phrase.Length
        && phrase.Select(w => new string(w.OrderBy(c => c).ToArray())).Distinct().Count() == phrase.Length;
}