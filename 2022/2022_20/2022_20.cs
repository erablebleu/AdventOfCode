using static AdventOfCode._2022_16;

namespace AdventOfCode;

public class _2022_20 : Problem
{
    // Use record to manage multiple identical input
    record Item(int Idx, long Value);
    public override void Solve()
    {
        AddSolution(GetGroveCoordinates(Mix(1, Enumerable.Range(0, Inputs.Length).Select(i => new Item(i, int.Parse(Inputs[i]))).ToArray())));
        AddSolution(GetGroveCoordinates(Mix(10, Enumerable.Range(0, Inputs.Length).Select(i => new Item(i, 811589153 * long.Parse(Inputs[i]))).ToArray())));
    }

    private static long GetGroveCoordinates(List<Item> data)
    {
        int zidx = data.IndexOf(data.First(i => i.Value == 0));
        return data[zidx + 1000].Value
             + data[zidx + 2000].Value
             + data[zidx + 3000].Value;
    }

    private static List<Item> Mix(int count, Item[] data)
    {
        List<Item> result = data.ToList();
        for (int c = 0; c < count; c++)
        {
            foreach (Item i in data)
            {
                long d = i.Value;
                int idx = result.IndexOf(i);
                result.RemoveAt(idx);
                long nIdx = (idx + d) % result.Count;
                if (nIdx < 0) nIdx += result.Count;
                result.Insert((int)(nIdx % result.Count), i);
            }
        }
        return result;
    }
}