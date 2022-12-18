using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public record NumberRange(int Min, int Max);
    public class _2020_16 : Problem
    {
        #region Methods
        private Dictionary<string, List<NumberRange>> _fields;
        private Dictionary<string, List<int>> _fieldIndexes;
        private List<List<int>> _tickets;
        private List<List<int>> _validTickets;

        public override void Solve()
        {
            string inp = string.Join("\r\n", Inputs);
            string[] el = inp.Split("\r\n\r\n");

            _fields = el[0].Split("\r\n").Select(l => l.Split(": "))
                .ToDictionary(l => l[0], l => l[1].Split(" or ").Select(i => new NumberRange(int.Parse(i.Split("-")[0]), int.Parse(i.Split("-")[1]))).ToList());

            List<int> myticket = el[1].Split("\r\n")[1].Split(",").Select(i => int.Parse(i)).ToList();
            _tickets = el[2].Split("\r\n").Skip(1).Select(l => l.Split(",").Select(i => int.Parse(i)).ToList()).ToList();

            Solutions.Add($"{_tickets.Sum(l => l.Where(i => !_fields.Values.Any(f => f.Any(r => RangeRespect(r, i)))).Sum())}");

            _validTickets = _tickets.Where(l => l.All(i => _fields.Values.Any(f => f.Any(r => RangeRespect(r, i))))).ToList();
            _fieldIndexes = _fields.ToDictionary(kv => kv.Key, kv => Enumerable.Range(0, myticket.Count).Where(i => _validTickets.All(t => kv.Value.Any(r => RangeRespect(r, t[i])))).ToList());
            _fieldIndexes = _fieldIndexes.OrderBy(kv => kv.Value.Count).ToDictionary(kv => kv.Key, kv => kv.Value);

            List<string> fieldsOrder = GetOrder(new List<string>());


            Solutions.Add($"{Enumerable.Range(0, myticket.Count).Where(i => fieldsOrder[i].StartsWith("departure")).Select(i => (long)myticket[i]).Aggregate(1L, (a, b) => a * b)}");

        }

        private static bool RangeRespect(NumberRange range, int value) => value >= range.Min && value <= range.Max;
        private List<string> GetOrder(List<string> current)
        {
            if (current.Count == _fields.Count) return current;
            int idx = current.Count;
            foreach (var field in _fieldIndexes.Where(kv => !current.Contains(kv.Key) && kv.Value.Contains(idx)))
            {
                var ls = current.ToList();
                ls.Add(field.Key);
                ls = GetOrder(ls);
                if (ls?.Count == _fields.Count)
                    return ls;
            }
            return null;
        }


        #endregion
    }
}