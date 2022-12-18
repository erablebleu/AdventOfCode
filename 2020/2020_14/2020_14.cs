using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class _2020_14 : Problem
    {
        #region Methods

        public override void Solve()
        {
            long orMask = 0;
            long andMask = long.MaxValue;
            string maskStr = string.Empty;
            Dictionary<long, long> data = new Dictionary<long, long>();
            Dictionary<long, long> data2 = new Dictionary<long, long>();

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
                    data[el[0]] = (el[1] | orMask) & andMask;
                    foreach (long address in GetAddressesVariations(maskStr, el[0]))
                        data2[address] = el[1];
                }
            }

            Solutions.Add($"{data.Sum(kv => kv.Value)}");
            Solutions.Add($"{data2.Sum(kv => kv.Value)}");
        }
        public static IEnumerable<long> GetAddressesVariations(string maskStr, long value)
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

        #endregion
    }
}