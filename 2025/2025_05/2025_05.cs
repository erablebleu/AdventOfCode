namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2025/day/05
/// </summary>
public class _2025_05 : Problem
{
    private LMultiRange _range;
    private long[] _ingredientIds;

    public override void Parse()
    {
        int idx = Array.IndexOf(Inputs, "");
        _range = new LMultiRange();
        for (int i = 0; i < idx; i++)
        {
            long[] el = [.. Inputs[i].Split("-").Select(long.Parse)];
            _range.Add(el[0], el[1]);
        }

        _ingredientIds = [.. Inputs.Skip(idx + 1).Select(long.Parse)];
    }

    public override object PartOne()
        => _ingredientIds.Count(_range.Contains);

    public override object PartTwo()
        => _range.Ranges.Sum(r => r.End + 1 - r.Start);
}