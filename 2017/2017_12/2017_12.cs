namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2017/day/12
/// </summary>
public class _2017_12 : Problem
{
    private Dictionary<int, Program> _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => new Program(l)).ToDictionary(p => p.Number, p => p);
    }

    public override object PartOne() => GetLinked(0).Count;

    public override object PartTwo()
    {
        List<int> indexes = _data.Keys.ToList();
        int count = 0;

        while (indexes.Count > 0)
        {
            count++;
            foreach (int i in GetLinked(indexes.First()))
                indexes.Remove(i);
        }

        return count;
    }

    private HashSet<int> GetLinked(int idx)
    {
        HashSet<int> set = new() { idx };
        List<int> adds = new() { idx };

        do
        {
            adds = adds.SelectMany(i => _data[i].SubPrograms.Where(si => !set.Contains(si))).Distinct().ToList();
            foreach (int i in adds)
                set.Add(i);
        }
        while (adds.Count > 0);

        return set;
    }

    private class Program
    {
        public int Number;
        public int[] SubPrograms = Array.Empty<int>();

        public Program(string line)
        {
            string[] el = line.Split(" <-> ");
            Number = int.Parse(el[0]);
            SubPrograms = el[1].Split(", ").Select(e => int.Parse(e)).ToArray();
        }
    }
}