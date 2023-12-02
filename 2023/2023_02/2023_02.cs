namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2023/day/02
/// </summary>
public class _2023_02 : Problem
{
    private record Game(int Number, Dictionary<string, int>[] Sets);

    private Game[] _games;

    public override void Parse()
    {
        _games = Inputs.Select(l =>
        {
            string[] el = l.Split(": ");
            return new Game(int.Parse(el[0][5..]),

            el[1]
            .Split("; ")
            .Select(s => s.Split(", ").Select(v => v.Split(" ")).ToDictionary(v => v[1], v => int.Parse(v[0])))
            .ToArray());
        }).ToArray();
    }

    public override object PartOne()
        => _games
        .Where(g => g.Sets.All(s =>
            s.GetValueOrDefault("red") <= 12
            && s.GetValueOrDefault("green") <= 13
            && s.GetValueOrDefault("blue") <= 14))
        .Sum(g => g.Number);

    public override object PartTwo()
        => _games
        .Sum(g => g.Sets.Max(s => s.GetValueOrDefault("red"))
            * g.Sets.Max(s => s.GetValueOrDefault("green"))
            * g.Sets.Max(s => s.GetValueOrDefault("blue")));
}