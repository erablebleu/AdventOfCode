using System.Threading;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2023/day/05
/// </summary>
public class _2023_05 : Problem
{
    private class Map
    {
        private record Range(long Position, long Length, long Offset);
        private readonly Range[] _ranges;
        public readonly string Source;
        public readonly string Destination;

        public Map(IEnumerable<string> lines)
        {
            string[] el = lines.First().Replace(" map:", string.Empty).Split("-to-");
            Source = el[0];
            Destination = el[1];
            _ranges = lines
                .Skip(1)
                .Select(l => l.Split(" ").Select(long.Parse).ToArray())
                .Select(l => new Range(l[1], l[2], l[0] - l[1]))
                .ToArray();
        }

        public long Convert(long value)
        {
            foreach (Range range in _ranges)
                if (value >= range.Position && value < range.Position + range.Length)
                    return value + range.Offset;

            return value;
        }
    }

    private Map[] _maps;
    private long[] _seeds;

    public override void Parse()
    {
        _seeds = Inputs[0][7..].Split(' ').Select(long.Parse).ToArray();
        _maps = Inputs.Skip(2).Batch(string.Empty).Select(b => new Map(b)).ToArray();
    }

    public override object PartOne()
        => _seeds
        .Select(GetLocation)
        .ToArray()
        .Min();

    private long GetLocation(long seed)
    {
        foreach (Map map in _maps)
            seed = map.Convert(seed);

        return seed;
    }

    public override object PartTwo()
    {
        return null;
    }
}