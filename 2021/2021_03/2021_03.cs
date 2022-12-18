namespace AdventOfCode;

public class _2021_03 : Problem
{
    public override void Solve()
    {
        int cnt = Inputs.Length;

        int gammaRate  = Convert.ToInt32(new string(Enumerable.Range(0, Inputs.First().Length).Select(i => Inputs.Count(v => v[i] == '1') > cnt / 2 ? '1' : '0').ToArray()), 2);
        int epsilonRate = Convert.ToInt32(new string(Enumerable.Range(0, Inputs.First().Length).Select(i => Inputs.Count(v => v[i] == '1') > cnt / 2 ? '0' : '1').ToArray()), 2);

        int oxygenRating = GetRating(Inputs, true);
        int co2Rating = GetRating(Inputs, false);

        Solutions.Add($"{gammaRate * epsilonRate}");
        Solutions.Add($"{oxygenRating * co2Rating}");
    }

    private int GetRating(IEnumerable<string> inputs, bool most = true)
    {
        int idx = 0;
        int cnt = inputs.Count();

        while(cnt > 1)
        {
            bool val = inputs.Count(v => v[idx] == '1') * 2 >= cnt;
            inputs = inputs.Where(v => v[idx] == ((val ^ !most) ? '1' : '0')).ToList();
            cnt = inputs.Count();
            idx++;
        }

        return Convert.ToInt32(inputs.First(), 2);
    }
}