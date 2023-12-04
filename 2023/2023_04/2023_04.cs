namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2023/day/04
/// </summary>
public class _2023_04 : Problem
{
    private record Card(int Number, int[] WinningNumbers, int[] Numbers);
    private Card[] _cards;

    public override void Parse()
    {
        _cards = Inputs.Select(l =>
        {
            string[] el = l.Split(':', '|');
            return new Card(int.Parse(el[0].Substring(5)),
                el[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(e => int.Parse(e)).ToArray(),
                el[2].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(e => int.Parse(e)).ToArray());
        }).ToArray();
    }

    public override object PartOne()
        => _cards
        .Select(c => c.Numbers.Count(n => c.WinningNumbers.Contains(n)))
        .Sum(s => s == 0 ? 0 : Math.Pow(2, s - 1));

    public override object PartTwo()
    {
        int[] result = _cards.Select(c => 1).ToArray();

        for(int i = 0; i < _cards.Length; i++)
        {
            int cnt = _cards[i].Numbers.Count(n => _cards[i].WinningNumbers.Contains(n));

            for (int j = 0; j < cnt; j++)
                if (i + j + 1 < result.Length)
                    result[i + j + 1] += result[i];
        }

        return result.Sum();
    }
}