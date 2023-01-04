namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/21
/// </summary>
public class _2020_21 : Problem
{
    private Dictionary<string, string> _allergens;

    private List<Food> _foods;

    public override void Parse()
    {
        _allergens = new Dictionary<string, string>();
        _foods = Inputs.Select(l => l.Split(" (contains ")).Select(el => new Food { Ingredients = el[0].Split(" ").ToList(), _allergens = el[1].Split(", ").Select(a => a.Replace(")", "")).ToList() }).ToList();
        _allergens = _foods.SelectMany(f => f._allergens).GroupBy(f => f).ToDictionary(g => g.Key, g => (string)null);

        while (_allergens.Values.Any(v => v is null))
        {
            foreach (var all in _allergens.Keys)
            {
                if (_allergens[all] != null) continue;

                var lf = _foods.Where(f => f._allergens.Contains(all)).ToList();
                var targets = lf.SelectMany(f => f.Ingredients).GroupBy(f => f).Where(g => !_allergens.Values.Contains(g.Key) && lf.All(f => f.Ingredients.Contains(g.Key))).ToList();
                if (targets.Count == 1)
                    _allergens[all] = targets[0].Key;
            }
        }
    }

    public override object PartOne() => _foods.SelectMany(f => f.Ingredients).GroupBy(f => f).Where(g => !_allergens.Values.Contains(g.Key)).Sum(g => _foods.Sum(f => f.Ingredients.Contains(g.Key) ? 1 : 0));

    public override object PartTwo() => string.Join(",", _allergens.OrderBy(kv => kv.Key).Select(kv => kv.Value));

    private class Food
    {
        public List<string> _allergens { get; set; }
        public List<string> Ingredients { get; set; }
    }
}