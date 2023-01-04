namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/10
/// </summary>
public class _2020_10 : Problem
{
    private List<int> _data;

    public override void Parse()
    {
        _data = Inputs.Select(c => int.Parse(c)).ToList();
        _data.Add(0);
        _data.Add(_data.Max() + 3);
        _data = _data.OrderBy(i => i).ToList();
    }

    public override object PartOne()
    {
        int dif1Cnt = _data.SelectConcecutives((i, j) => j - i).Count(i => i == 1);
        int dif3Cnt = _data.SelectConcecutives((i, j) => j - i).Count(i => i == 3);

        return dif1Cnt * dif3Cnt;
    }

    public override object PartTwo() => CountPathToLast2(_data);

    private void CountPathTo(Dictionary<int, long> result, List<int> ls)
    {
        int st = ls.First();
        if (result.ContainsKey(st))
            result[st]++;
        else if (ls.Skip(1).Any(i => result.ContainsKey(i)))
            foreach (int n in ls.Skip(1).Where(i => i - st <= 3))
                CountPathTo(result, ls.Where(i => i >= n).ToList());
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
}