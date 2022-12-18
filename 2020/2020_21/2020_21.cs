using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Food
    {
        public List<string> Ingredients { get; set; }
        public List<string> Allergens { get; set; }
    }
    public class _2020_21 : Problem
    {
        #region Methods

        public override void Solve()
        {
            Dictionary<string, string> allergens = new Dictionary<string, string>();
            List<Food> foods = Inputs.Select(l => l.Split(" (contains ")).Select(el => new Food { Ingredients = el[0].Split(" ").ToList(), Allergens = el[1].Split(", ").Select(a => a.Replace(")", "")).ToList() }).ToList();
            allergens = foods.SelectMany(f => f.Allergens).GroupBy(f => f).ToDictionary(g => g.Key, g => (string)null);

            while(allergens.Values.Any(v => v is null))
            {
                foreach (var all in allergens.Keys)
                {
                    if (allergens[all] != null) continue;

                    var lf = foods.Where(f => f.Allergens.Contains(all)).ToList();
                    var targets = lf.SelectMany(f => f.Ingredients).GroupBy(f => f).Where(g => !allergens.Values.Contains(g.Key) && lf.All(f => f.Ingredients.Contains(g.Key))).ToList();
                    if (targets.Count == 1)
                        allergens[all] = targets[0].Key;
                }
            }

            Solutions.Add($"{foods.SelectMany(f => f.Ingredients).GroupBy(f => f).Where(g => !allergens.Values.Contains(g.Key)).Sum(g => foods.Sum(f => f.Ingredients.Contains(g.Key) ? 1 : 0))}");

            Solutions.Add(string.Join(",", allergens.OrderBy(kv => kv.Key).Select(kv => kv.Value)));
        }

        #endregion
    }
}