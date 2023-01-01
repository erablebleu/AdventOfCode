namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/15
/// </summary>
public class _2016_15 : Problem
{
    private int[][] _disks;

    public override void Parse()
    {
        _disks = Inputs.Select(l => l.ParseExact("Disc #{0} has {1} positions; at time=0, it is at position {2}.").Select(e => int.Parse(e)).ToArray()).ToArray();
    }

    public override object PartOne() => GetIndex(_disks);

    public override object PartTwo()
    {
        Array.Resize(ref _disks, _disks.Length + 1);
        _disks[_disks.Length - 1] = new int[] { _disks.Length, 11, 0 };
        return GetIndex(_disks);
    }

    private static int GetIndex(int[][] disks)
    {
        int idx = 0;

        while (Enumerable.Range(0, disks.Length).Any(i => (disks[i][2] + idx + disks[i][0]) % disks[i][1] != 0)) // Everything is here
            idx++;

        return idx;
    }
}