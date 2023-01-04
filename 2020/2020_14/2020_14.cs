namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/14
/// </summary>
public class _2020_14 : Problem
{
    private Dictionary<long, long> _data;

    private Dictionary<long, long> _data2;

    public override void Parse()
    {
        _data = new Dictionary<long, long>();
        _data2 = new Dictionary<long, long>();
        long orMask = 0;
        long andMask = long.MaxValue;
        string maskStr = string.Empty;

        foreach (var line in Inputs)
        {
            if (line.StartsWith("mask"))
            {
                maskStr = line.Replace("mask = ", "");
                orMask = Convert.ToInt64(maskStr.Replace("X", "0"), 2);
                andMask = Convert.ToInt64(maskStr.Replace("X", "1"), 2);
            }
            else if (line.StartsWith("mem"))
            {
                long[] el = line.Replace(" ", "").Replace("mem[", "").Replace("]", "").Split("=").Select(l => long.Parse(l)).ToArray();
                _data[el[0]] = (el[1] | orMask) & andMask;
                foreach (long address in GetAddressesVariations(maskStr, el[0]))
                    _data2[address] = el[1];
            }
        }
    }

    public override object PartOne() => _data.Sum(kv => kv.Value);

    public override object PartTwo() => _data2.Sum(kv => kv.Value);

    private static IEnumerable<long> GetAddressesVariations(string maskStr, long value)
    {
        long mask = Convert.ToInt64(maskStr.Replace("0", "1").Replace("X", "0"), 2);
        long maskValue = Convert.ToInt64(maskStr.Replace("X", "0"), 2);
        List<int> indexes = Enumerable.Range(0, maskStr.Length).Where(i => maskStr[maskStr.Length - 1 - i] == 'X').ToList();

        for (long i = 0; i < Math.Pow(2, indexes.Count); i++)
        {
            long val = 0;
            for (int j = 0; j < indexes.Count; j++)
                val |= (i & (1 << j)) == 0 ? 0L : 1L << indexes[j];

            yield return val | ((value & mask) | maskValue);
        }

        yield break;
    }
}