namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2025/day/11
/// </summary>
public class _2025_11 : Problem
{
    private Dictionary<string, string[]> _map;

    public override void Parse()
    {
        _map = Inputs.Select(l => l.Split(" "))
            .ToDictionary(el => el[0][..^1], el => el.Skip(1).ToArray());
    }

    private long CountPaths(string origin, string target)
    {
        Dictionary<string, long> context = [];

        long CountPaths(string origin)
        {
            if (origin == target)
                return 1;

            if (origin == "out")
                return 0;

            if (context.TryGetValue(origin, out long length))
                return length;

            long result = _map[origin].Sum(CountPaths);
            context[origin] = result;

            return result;
        }

        return CountPaths(origin);
    }

    public override object PartOne()
        => CountPaths("you", "out");

    public override object PartTwo()
        => CountPaths("svr", "fft") * CountPaths("fft", "dac") * CountPaths("dac", "out")
         + CountPaths("svr", "dac") * CountPaths("dac", "fft") * CountPaths("fft", "out");
}