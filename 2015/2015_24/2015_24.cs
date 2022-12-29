using System.Collections.Immutable;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/24
/// </summary>
public class _2015_24 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => int.Parse(l)).ToArray();
    }

    public override object PartOne() => Solve(_data, 3);

    public override object PartTwo() => Solve(_data, 4);

    private IEnumerable<ImmutableList<int>> Pick(int[] nums, int count, int i, int sum)
    {
        if (sum == 0)
        {
            yield return ImmutableList.Create<int>();
            yield break;
        }

        if (count < 0 || sum < 0 || i >= nums.Length)
            yield break;

        if (nums[i] <= sum)
            foreach (ImmutableList<int> x in Pick(nums, count - 1, i + 1, sum - nums[i]))
                yield return x.Add(nums[i]);

        foreach (ImmutableList<int> x in Pick(nums, count, i + 1, sum))
            yield return x;

        yield break;
    }

    private long? Solve(int[] nums, int groups)
    {
        for (int i = 0; i < nums.Length; i++)
        {
            IEnumerable<ImmutableList<int>> parts = Pick(nums, i, 0, nums.Sum() / groups);
            if (parts.Any())
                return parts.Select(l => l.Aggregate(1L, (m, x) => m * x)).Min();
        }

        return null;
    }
}