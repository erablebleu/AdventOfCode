namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/04
/// </summary>
public class _2022_04 : Problem
{
    private List<Pair<Range>> _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => new Pair<Range>(l.Split(',').Select(i => GetRange(i)))).ToList();
    }

    public override object PartOne() => _data.Count(r => r.Item1.Contains(r.Item2) || r.Item2.Contains(r.Item1));

    public override object PartTwo() => _data.Count(r => r.Item1.Cross(r.Item2));

    private static Range GetRange(string ser)
    {
        string[] el = ser.Split('-');
        return new Range(int.Parse(el[0]), int.Parse(el[1]));
    }

    private class Pair<T>
    {
        public Pair(T item1, T item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public Pair(T[] items)
        {
            Item1 = items[0];
            Item2 = items[1];
        }

        public Pair(IEnumerable<T> items)
        {
            Item1 = items.ElementAt(0);
            Item2 = items.ElementAt(1);
        }

        public T Item1 { get; set; }
        public T Item2 { get; set; }
    }

    private class Range
    {
        public Range(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int End { get; set; }
        public int Start { get; set; }

        public bool Contain(int v) => v >= Start && v <= End;

        public bool Contains(Range r2) => r2.Start >= Start && r2.End <= End;

        public bool Cross(Range r2) => !(r2.End < Start || r2.Start > End);
    }
}