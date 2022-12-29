namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/21
/// </summary>
public class _2015_21 : Problem
{
    private static int[][] _armors = new int[][] // 0-1
    {
        new int[] { 13, 0, 1 },
        new int[] { 31, 0, 2 },
        new int[] { 53, 0, 3 },
        new int[] { 75, 0, 4 },
        new int[] { 102, 0, 5 },
    };

    private static int[][] _rings = new int[][] // 0-2
    {
        new int[] { 25, 1, 0 },
        new int[] { 50, 2, 0 },
        new int[] { 100, 3, 0 },
        new int[] { 20, 0, 1 },
        new int[] { 40, 0, 2 },
        new int[] { 80, 0, 3 },
    };

    private static int[][] _weapons = new int[][] // 1
            {
        new int[] { 8, 4, 0 },
        new int[] { 10, 5, 0 },
        new int[] { 25, 6, 0 },
        new int[] { 40, 7, 0 },
        new int[] { 74, 8, 0 },
    };

    private int[] _boss;
    // [0] : Hit Points
    // [1] : Damage
    // [2] : Armor

    public override void Parse()
    {
        _boss = Inputs.Select(l => int.Parse(l.Split(": ")[1])).ToArray();
    }

    public override object PartOne()
    {
        int gold = int.MaxValue;

        foreach (IEnumerable<int[]> stuff in GetStuffPermutations())
            if (Emulate(out int c, stuff.ToArray()) && c < gold)
                gold = c;

        return gold;
    }

    public override object PartTwo()
    {
        int gold = int.MinValue;

        foreach (IEnumerable<int[]> stuff in GetStuffPermutations())
            if (!Emulate(out int c, stuff.ToArray()) && c > gold)
                gold = c;

        return gold;
    }

    private bool Emulate(out int cost, IEnumerable<int[]> stuff)
    {
        cost = 0;
        int[] me = new int[]
        {
            100, 0, 0
        };
        foreach (int[] item in stuff)
        {
            if (item is null)
                continue;
            cost += item[0];
            for (int i = 1; i < 3; i++)
                me[i] += item[i];
        }

        int degB = Math.Max(1, me[1] - _boss[2]);
        int degM = Math.Max(1, _boss[1] - me[2]);

        int cB = _boss[0] / degB;
        int cM = me[0] / degM;

        return cB <= cM;
    }

    private IEnumerable<IEnumerable<int[]>> GetStuffPermutations()
    {
        foreach (int[] weapon in _weapons)
        {
            yield return new List<int[]>() { weapon };

            foreach (int[] armor in _armors)
            {
                yield return new List<int[]>() { weapon, armor };
                foreach (int[] ring0 in _rings)
                {
                    yield return new List<int[]>() { weapon, armor, ring0 };
                    foreach (int[] ring1 in _rings)
                    {
                        if (ring0 == ring1)
                            continue;

                        yield return new List<int[]>() { weapon, armor, ring0, ring1 };
                    }
                }
            }
            foreach (int[] ring0 in _rings)
            {
                yield return new List<int[]>() { weapon, ring0 };
                foreach (int[] ring1 in _rings)
                {
                    if (ring0 == ring1)
                        continue;

                    yield return new List<int[]>() { weapon, ring0, ring1 };
                }
            }
        }
        yield break;
    }
}