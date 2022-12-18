using static AdventOfCode.Redditor;

namespace AdventOfCode;

public class _2022_01 : Problem
{
    public class Elf
    {

    }
    public override void Solve()
    {
        List<int> elvesTotal = new();
        int cnt = 0;
        cnt = 0;
        foreach (string line in Inputs)
        {
            if (string.IsNullOrEmpty(line))
            {
                elvesTotal.Add(cnt);
                cnt = 0;
            }
            else
            {
                cnt += int.Parse(line);
            }
        }

        Solutions.Add($"{elvesTotal.OrderByDescending(v => v).First()}");
        Solutions.Add($"{elvesTotal.OrderByDescending(v => v).Take(3).Sum()}");
    }
}