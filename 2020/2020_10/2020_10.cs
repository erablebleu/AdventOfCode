using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Tools;

namespace AdventOfCode
{
    public class _2020_10 : Problem
    {
        #region Methods

        public override void Solve()
        {
            List<int> adapters = Inputs.Select(c => int.Parse(c)).ToList();
            adapters.Add(0);
            adapters.Add(adapters.Max() + 3);
            adapters = adapters.OrderBy(i => i).ToList();
            int dif1Cnt = adapters.SelectConcecutives((i, j) => j - i).Count(i => i == 1);
            int dif3Cnt = adapters.SelectConcecutives((i, j) => j - i).Count(i => i == 3);

            Solutions.Add($"{dif1Cnt * dif3Cnt}");

            //Solutions.Add($"{CountPathToLast(adapters)}");
            Solutions.Add($"{CountPathToLast2(adapters)}");
        }

        private int CountPathToLast(List<int> ls) // work but too slow
        {
            int st = ls.First();
            if (ls.Count == 1) return 1;

            return ls.Skip(1).Where(i => i - st <= 3).Sum(n => CountPathToLast(ls.Where(i => i >= n).ToList()));
        }
        private long CountPathToLast2(List<int> ls)
        {
            int cnt = 0;
            int dsMin;
            Dictionary<int, long> res = new Dictionary<int, long> { { 0, 1 } };
            do
            {
                cnt += 10;
                dsMin = Math.Min(cnt, ls.Last());
                var tmpSum = ls.Where(i => i >= dsMin && i < dsMin + 3).ToDictionary(i => i, i => 0l);
                foreach (var kv in res)
                {
                    var tmpRes = tmpSum.Keys.ToDictionary(k => k, k => 0l);
                    CountPathTo(tmpRes, ls.Where(i => i >= kv.Key).ToList());

                    foreach (var k in tmpRes.Keys) 
                        tmpSum[k] += kv.Value * tmpRes[k];
                }

                res = tmpSum;
            }
            while (dsMin < ls.Last());

            return res.Values.First();
        }

        private void CountPathTo(Dictionary<int, long> result, List<int> ls)
        {
            int st = ls.First();
            if (result.ContainsKey(st))
                result[st]++;
            else if (ls.Skip(1).Any(i => result.ContainsKey(i)))
                foreach (int n in ls.Skip(1).Where(i => i - st <= 3))
                    CountPathTo(result, ls.Where(i => i >= n).ToList());
        }

        #endregion
    }
}