namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/02
/// </summary>
public class _2020_02 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne()
    {
        //1-3 a: abcde
        int validCnt = 0;

        foreach (var line in Inputs)
        {
            var el = line.Split(" ");
            var el2 = el[0].Split("-");
            int min = int.Parse(el2[0]);
            int max = int.Parse(el2[1]);

            int cnt = el[2].Count(c => c == el[1][0]);
            validCnt += (cnt >= min && cnt <= max) ? 1 : 0;
        }

        return validCnt;
    }

    public override object PartTwo()
    {
        int validCnt = 0;
        foreach (var line in Inputs)
        {
            var el = line.Split(" ");
            var pos = el[0].Split("-").Select(s => int.Parse(s) - 1).ToArray();

            int cnt = el[2].Count(c => c == el[1][0]);
            validCnt += (el[2][pos[0]] == el[1][0] ^ el[2][pos[1]] == el[1][0]) ? 1 : 0;
        }

        return validCnt;
    }
}