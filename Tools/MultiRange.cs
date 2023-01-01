namespace AdventOfCode.Tools;

public class IRange
{
    public IRange(int start, int end)
    {
        Start = start;
        End = end;
    }

    public int End;
    public int Start;

    public bool Contain(int v) => v >= Start && v <= End;

    public bool Contains(IRange r2) => r2.Start >= Start && r2.End <= End;

    public bool Cross(IRange r2) => !(r2.End < Start || r2.Start > End);
}

public class LRange
{
    public LRange(long start, long end)
    {
        Start = start;
        End = end;
    }

    public long End;
    public long Start;

    public bool Contain(long v) => v >= Start && v <= End;

    public bool Contains(LRange r2) => r2.Start >= Start && r2.End <= End;

    public bool Cross(LRange r2) => !(r2.End < Start || r2.Start > End);
    public bool CrossOrConsecutive(LRange r2) => !(r2.End + 1 < Start || r2.Start > End + 1);

    public override string ToString() => $"[{Start};{End}]";
}

public class IMultiRange
{
    public List<IRange> Ranges = new();

    public void Add(int start, int end)
    {
        IRange range = new(start, end);
        IRange rc = Ranges.FirstOrDefault(r => r.Cross(range));
        while (rc != null)
        {
            Ranges.Remove(rc);
            range = new IRange(Math.Min(rc.Start, range.Start), Math.Max(rc.End, range.End));
            rc = Ranges.FirstOrDefault(r => r.Cross(range));
        }
        Ranges.Add(range);
    }

    public int? GetOutRange(int max = int.MaxValue)
    {
        foreach (IRange r in Ranges)
        {
            if (r.Start - 1 > 0) return r.Start - 1;
            if (r.End + 1 < max) return r.End + 1;
        }
        return null;
    }
}

public class LMultiRange
{
    public List<LRange> Ranges = new();

    public void Add(long start, long end)
    {
        LRange range = new(start, end);
        if (Ranges.Any(r => r.Contains(range)))
            return;

        LRange toRemove = Ranges.FirstOrDefault(r => range.Contains(r));
        if (toRemove is not null)
            Ranges.Remove(toRemove);

        LRange rc = Ranges.FirstOrDefault(r => r.CrossOrConsecutive(range));
        while (rc != null)
        {
            Ranges.Remove(rc);
            range = new LRange(Math.Min(rc.Start, range.Start), Math.Max(rc.End, range.End));
            rc = Ranges.FirstOrDefault(r => r.CrossOrConsecutive(range));
        }
        Ranges.Add(range);
        Ranges = Ranges.OrderBy(r => r.Start).ToList();
    }

    public long? GetFirstOut(long min = 0, long max = long.MaxValue)
    {
        foreach (LRange r in Ranges)
        {
            if (r.Start > min) return min;
            if (r.End < max) return r.End + 1;
        }
        return null;
    }


    public long CountOut(long min = 0, long max = long.MaxValue)
    {
        long result = 0;
        long start = min;

        foreach (LRange r in Ranges)
        {
            if (r.Start > start)
                result += (r.Start - start);
            start = r.End + 1;
        }

        return result;
    }

    public override string ToString() => string.Join(" ", Ranges);
}