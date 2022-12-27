namespace AdventOfCode;

public class _2015_01 : Problem
{
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs[0].Select(c => c == '(' ? 1 : -1).ToArray();
    }

    public override object PartOne()
    {
        return _data.Sum();
    }

    public override object PartTwo()
    {
        int cnt = 0;
        for (int x = 0; x < Inputs[0].Length; x++)
        {
            cnt += Inputs[0][x] == '(' ? 1 : -1;
            if (cnt >= 0)
                continue;
            return x + 1;
        }
        return null;
    }
}