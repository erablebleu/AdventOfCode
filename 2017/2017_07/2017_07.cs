namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2017/day/07
/// </summary>
public class _2017_07 : Problem
{
    private Dictionary<string, Program> _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => Parse(l)).ToDictionary(p => p.Name, p => p);
    }

    public override object PartOne() => _data.Values.First(p => _data.Values.All(p2 => !p2.SubPrograms.Contains(p.Name))).Name;

    public override object PartTwo()
    {
        Program root = _data.Values.First(p => _data.Values.All(p2 => !p2.SubPrograms.Contains(p.Name)));
        int targetW = root.GetWeight(_data);
        do
        {
            int[] weight = root.SubPrograms.Select(n => _data[n].GetWeight(_data)).ToArray();
            int majW = weight.GroupBy(w => w).First(g => g.Count() > 1).Key;

            if (weight.All(w => w == majW))
                return targetW - weight.Sum();

            targetW = majW;
            int idx = Array.IndexOf(weight, weight.GroupBy(w => w).First(g => g.Count() == 1).Key);
            root = _data[root.SubPrograms[idx]];
        }
        while (root.SubPrograms.Any());

        return targetW - root.Weight;
    }

    private static Program Parse(string line)
    {
        string[] el = line.Remove(",").Split(' ');
        Program result = new()
        {
            Name = el[0],
            Weight = int.Parse(el[1][1..^1]),
        };
        if (el.Length < 3)
            return result;

        result.SubPrograms = el.Skip(3).ToArray();
        return result;
    }

    private class Program
    {
        public string Name;
        public string[] SubPrograms = Array.Empty<string>();
        public int Weight;

        public int GetWeight(Dictionary<string, Program> data) => Weight + SubPrograms.Sum(n => data[n].GetWeight(data));
    }
}