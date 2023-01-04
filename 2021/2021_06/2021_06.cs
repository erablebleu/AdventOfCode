namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2021/day/06
/// </summary>
public class _2021_06 : Problem
{
    private Dictionary<int, long> _state;

    public override void Parse()
    {
        _state = Inputs.First().Split(",").Select(v => int.Parse(v)).GroupBy(v => v).ToDictionary(g => g.Key, g => (long)g.Count());
    }

    public override object PartOne() => Simulate(_state, 80).Sum(kv => kv.Value);

    public override object PartTwo() => Simulate(_state, 256).Sum(kv => kv.Value);

    private static Dictionary<int, long> Simulate(Dictionary<int, long> state, int dayCount)
    {
        for (int i = 0; i < dayCount; i++)
        {
            Dictionary<int, long> newState = new();

            foreach (KeyValuePair<int, long> kv in state)
            {
                if (kv.Key > 0)
                    DictionaryExtension.AddOrInc(newState, kv.Key - 1, kv.Value);
                else
                {
                    DictionaryExtension.AddOrInc(newState, 6, kv.Value);
                    DictionaryExtension.AddOrInc(newState, 8, kv.Value);
                }
            }

            state = newState;
        }

        return state;
    }

    private static class DictionaryExtension
    {
        public static void AddOrInc(Dictionary<int, long> dic, int key, long value)
        {
            if (dic.ContainsKey(key))
                dic[key] += value;
            else
                dic.Add(key, value);
        }
    }
}