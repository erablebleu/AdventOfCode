using System.Text.RegularExpressions;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/14
/// </summary>
public class _2019_14 : Problem
{
    private const long TARGET = 1000000000000;

    private Dictionary<string, int> _levels;

    private Recipe _recipe;
    private Dictionary<string, Recipe> _recipes;
    private long _resOne;

    // a X => b Y   -   need c Y
    public static long CustomDiv(long a, long b, long c) => a * (c / b + (c % b > 0 ? 1 : 0));

    public override void Parse()
    {
    }

    public override object PartOne()
    {
        _recipes = new Dictionary<string, Recipe>();

        foreach (string line in Inputs)
        {
            var recipe = new Recipe(line);
            _recipes.Add(recipe.Name, recipe);
        }

        InitLevels();

        _recipe = _recipes["FUEL"];
        _resOne = SimplifyRecipe(ref _recipe, 1);
        return _resOne;
    }

    public override object PartTwo()
    {
        long start = TARGET / _resOne;
        long pas = start;

        while (pas > 0)
        {
            do
            {
                start += pas;
                _resOne = SimplifyRecipe(ref _recipe, start);
            }
            while (_resOne < TARGET);
            start -= pas;
            pas /= 2;
        }

        return start;
    }

    private void InitLevels()
    {
        _levels = new Dictionary<string, int>
        {
            { "ORE", 0 }
        };

        while (!_recipes.Values.All(r => _levels.ContainsKey(r.Name)))
            foreach (var r in _recipes.Values.Where(r => !_levels.ContainsKey(r.Name)
                                    && r.Inputs.All(i => _levels.ContainsKey(i.Name))).ToList())
                _levels.Add(r.Name, r.Inputs.Max(i => _levels[i.Name]) + 1);
    }

    private long SimplifyRecipe(ref Recipe recipe, long cnt)
    {
        long result = 0;
        List<Element> inputs = new List<Element>();
        foreach (var inp in recipe.Inputs)
        {
            var cpy = inp.Copy();
            cpy.Quantity *= cnt;
            inputs.Add(cpy);
        }

        while (inputs.Any())
        {
            var input = inputs.OrderByDescending(i => _levels[i.Name]).First();
            inputs.Remove(input);

            foreach (var el in _recipes[input.Name].Inputs)
            {
                var tmpIn = inputs.FirstOrDefault(i => i.Name == el.Name);
                var tmpCnt = CustomDiv(el.Quantity, _recipes[input.Name].Quantity, input.Quantity);
                if (el.Name == "ORE")
                    result += tmpCnt;
                else if (tmpIn is null)
                    inputs.Add(new Element() { Name = el.Name, Quantity = tmpCnt });
                else
                    tmpIn.Quantity += tmpCnt;
            }
        }
        return result;
    }

    private class Element
    {
        public Element(string line)
        {
            string[] el = line.Split(' ');
            Quantity = long.Parse(el[0]);
            Name = el[1];
        }

        public Element()
        {
        }

        public string Name { get; set; }
        public long Quantity { get; set; }

        public Element Copy()
        {
            return new Element()
            {
                Name = Name,
                Quantity = Quantity
            };
        }
    }

    private class Recipe
    {
        public Recipe(string line) : base()
        {
            Inputs = new List<Element>();
            string[] el = Regex.Split(line, " => ");
            string[] ins = Regex.Split(el[0], ", ");
            string[] el2 = el[1].Split(' ');
            Quantity = long.Parse(el2[0]);
            Name = el2[1];

            foreach (string a in ins)
                Inputs.Add(new Element(a));
        }

        public List<Element> Inputs { get; set; }
        public string Name { get; set; }
        public long Quantity { get; set; }
    }
}