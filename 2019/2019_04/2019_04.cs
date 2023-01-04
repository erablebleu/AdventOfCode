namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/04
/// </summary>
public class _2019_04 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs[0].Split('-').Select(e => int.Parse(e)).ToArray();
    }

    public override object PartOne()
    {
        int cnt = 0;

        for (int i = _data[0]; i < _data[1]; i++)
        {
            bool adj = false;
            int j;
            for (j = 0; j < 6 - 1; j++)
            {
                adj |= GetDigit(i, j) == GetDigit(i, j + 1);
                if (GetDigit(i, j) < GetDigit(i, j + 1))
                    break;
            }
            if (adj && j == 5)
                cnt++;
        }

        return cnt;
    }

    public override object PartTwo()
    {
        int cnt = 0;

        for (int i = _data[0]; i < _data[1]; i++)
        {
            bool adj = false;
            bool inc = false;
            int j;
            int lastDig = 10;
            int digCnt = 1;

            for (j = 0; j < 6; j++)
            {
                int dig = GetDigit(i, j);
                inc |= dig > lastDig;
                if (lastDig == dig)
                    digCnt++;
                else
                {
                    lastDig = dig;
                    adj |= digCnt == 2;
                    digCnt = 1;
                }
            }
            adj |= digCnt == 2;
            if (adj && !inc)
                cnt++;
        }

        return cnt;
    }

    private static int GetDigit(int i, int digit) => i / (int)Math.Pow(10, digit) % 10;
}