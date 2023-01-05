using System.Text;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2017/day/10
/// </summary>
public class _2017_10 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne()
    {
        int[] data = GetHash(Enumerable.Range(0, 256).ToArray(), Inputs[0].Split(',').Select(e => int.Parse(e)).ToArray(), 1);
        return data[0] * data[1];
    }

    public override object PartTwo()
    {
        int[] lengths = Encoding.ASCII.GetBytes(Inputs[0]).Concat(new byte[] { 17, 31, 73, 47, 23 }).Select(b => (int)b).ToArray();
        int[] sparseHash = GetHash(Enumerable.Range(0, 256).ToArray(), lengths, 64);
        int[] denseHash = Enumerable.Range(0, 16).Select(i => sparseHash.Skip(i * 16).Take(16).Aggregate((x, y) => x ^ y)).ToArray();

        return string.Join(string.Empty, denseHash.Select(i => i.ToString("x2")));
    }

    private static T[] GetHash<T>(T[] data, int[] lengths, int count)
    {
        int skip = 0;
        int idx = 0;
        for (int i = 0; i < count; i++)
        {
            foreach (int l in lengths)
            {
                data = Knot(data, idx, l);
                idx = (idx + l + skip) % data.Length;
                skip++;
            }
        }
        return data;
    }

    private static T[] Knot<T>(T[] data, int sIdx, int length)
    {
        T[] result = data.ToArray();
        int eIdx = (sIdx + length - 1).Loop(0, data.Length);

        for (int i = 0; i < length / 2; i++)
        {
            result[eIdx] = data[sIdx];
            result[sIdx] = data[eIdx];

            sIdx = (sIdx + 1).Loop(0, data.Length);
            eIdx = (eIdx - 1).Loop(0, data.Length);
        }

        return result;
    }
}