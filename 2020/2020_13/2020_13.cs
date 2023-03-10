namespace AdventOfCode;

public class BusTime
{
    public BusTime(long line, int idx)
    {
        Line = line;
        Idx = idx;
    }

    public int Idx { get; set; }
    public long Line { get; set; }
}

/// <summary>
/// https://adventofcode.com/2020/day/13
/// </summary>
public class _2020_13 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne()
    {
        long time = long.Parse(Inputs[0]);
        long[] bus = Inputs[1].Split(",").Where(b => b != "x").Select(b => long.Parse(b)).ToArray();

        long line = bus.OrderBy(b => (time / b + 1) * b).First();
        long wait = (time / line + 1) * line - time;

        return line * wait;
    }

    public override object PartTwo()
    {
        var data = Inputs[1].Split(",").ToList();
        List<BusTime> bus2 = data.Where(b => b != "x").Select(b => new BusTime(long.Parse(b), data.IndexOf(b))).ToList();

        long res = 0;
        long p = 1;
        foreach (var b in bus2)
        {
            while ((res + b.Idx) % b.Line > 0)
                res += p;
            p *= b.Line;
        }

        return res;
    }
}