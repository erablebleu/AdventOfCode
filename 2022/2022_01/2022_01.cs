namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/01
/// </summary>
public class _2022_01 : Problem
{
    private List<int> _elves;

    public override void Parse()
    {
        _elves = new();
        int cnt = 0;
        cnt = 0;
        foreach (string line in Inputs)
        {
            if (string.IsNullOrEmpty(line))
            {
                _elves.Add(cnt);
                cnt = 0;
            }
            else
            {
                cnt += int.Parse(line);
            }
        }
    }

    public override object PartOne() => _elves.OrderByDescending(v => v).First();

    public override object PartTwo() => _elves.OrderByDescending(v => v).Take(3).Sum();
}