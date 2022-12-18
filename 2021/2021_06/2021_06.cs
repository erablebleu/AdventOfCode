namespace AdventOfCode;

public class _2021_06 : Problem
{
    internal static class DictionaryExtension
    {
        public static void AddOrInc(Dictionary<int, long> dic, int key, long value)
        {
            if (dic.ContainsKey(key))
                dic[key] += value;
            else
                dic.Add(key, value);
        }
    }

    public override void Solve()
    {
        Dictionary<int, long> state = Inputs.First().Split(",").Select(v => int.Parse(v)).GroupBy(v => v).ToDictionary(g => g.Key, g => (long)g.Count());

        Simulate(ref state, 80);        

        Solutions.Add($"{state.Sum(kv => kv.Value)}");

        Simulate(ref state, 256 - 80);

        Solutions.Add($"{state.Sum(kv => kv.Value)}");
    }

    private void Simulate(ref Dictionary<int, long> state, int dayCount)
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
    }
}