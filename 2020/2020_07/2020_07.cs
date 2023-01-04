namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/07
/// </summary>
public class _2020_07 : Problem
{
    private Dictionary<string, Bag> _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => new Bag { Type = l.Split(" contain ")[0].Replace("bags", "bag") }).ToDictionary(b => b.Type, b => b);

        foreach (var line in Inputs)
        {
            var el = line.Split(" contain ");
            var bag = _data[el[0].Replace("bags", "bag")];
            foreach (var c in el[1].Replace(".", "").Split(", "))
            {
                if (int.TryParse(c.Split(" ")[0], out var cnt))
                {
                    bag.SubTypes.Add(_data[c[(cnt.ToString().Length + 1)..].Replace("bags", "bag")], cnt);
                }
            }
        }
    }

    public override object PartOne() => _data.Values.Count(b => b.Contains("shiny gold bag"));

    public override object PartTwo() => _data["shiny gold bag"].Count();

    private class Bag
    {
        public Dictionary<Bag, int> SubTypes { get; set; } = new Dictionary<Bag, int>();
        public string Type { get; set; }

        public bool Contains(string type) => SubTypes.Keys.Any(c => c.Type == type || c.Contains(type));

        public int Count() => SubTypes.Sum(kv => kv.Value * (1 + kv.Key.Count()));
    }
}