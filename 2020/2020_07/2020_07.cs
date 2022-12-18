using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Bag
    {
        public string Type { get; set; }
        public Dictionary<Bag, int> SubTypes { get; set; } = new Dictionary<Bag, int>();
        public bool Contains(string type) => SubTypes.Keys.Any(c => c.Type == type || c.Contains(type));
        public int Count() => SubTypes.Sum(kv => kv.Value * (1 + kv.Key.Count()));
    }
    public class _2020_07 : Problem
    {
        #region Methods

        public override void Solve()
        {
            Dictionary<string, Bag> bags = Inputs.Select(l => new Bag { Type = l.Split(" contain ")[0].Replace("bags", "bag") }).ToDictionary(b => b.Type, b => b);

            foreach (var line in Inputs)
            {
                var el = line.Split(" contain ");
                var bag = bags[el[0].Replace("bags", "bag")];
                foreach (var c in el[1].Replace(".", "").Split(", "))
                {
                    if (int.TryParse(c.Split(" ")[0], out var cnt))
                    {
                        bag.SubTypes.Add(bags[c[(cnt.ToString().Length + 1)..].Replace("bags", "bag")], cnt);
                    }
                }
            }

            Solutions.Add($"{bags.Values.Count(b => b.Contains("shiny gold bag"))}");
            Solutions.Add($"{bags["shiny gold bag"].Count()}");
        }

        #endregion
    }
}