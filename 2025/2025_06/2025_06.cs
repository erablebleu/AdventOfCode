namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2025/day/06
/// </summary>
public class _2025_06 : Problem
{
    private string[] _operations;

    public override void Parse()
    {
        _operations = Inputs.Last().Split(" ", StringSplitOptions.RemoveEmptyEntries);
    }

    public override object PartOne()
    {
        long[][] data = [.. Inputs
            .Take(Inputs.Length - 1)
            .Select(l => l.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray())];

        long result = 0;

        for (int i = 0; i < _operations.Length; i++)
            result += _operations[i] switch
            {
                "+" => data.Select(d => d[i]).Sum(),
                "*" => data.Select(d => d[i]).Product(),
                _ => throw new NotSupportedException(),
            };

        return result;
    }

    public override object PartTwo()
    {
        List<string> strs = [];

        for (int i = 0; i < Inputs[0].Length; i++)
            strs.Add(new string([.. Inputs.Take(Inputs.Length - 1).Select(l => l[i])]));

        List<long> list = [];
        long result = 0;
        int j = 0;

        void AddSet()
        {
            result += _operations[j] switch
            {
                "+" => list.Sum(),
                "*" => list.Product(),
                _ => throw new NotSupportedException(),
            };
            list = [];
            j++;
        }

        foreach (string str in strs)
        {
            if (string.IsNullOrWhiteSpace(str))
                AddSet();
            else
                list.Add(long.Parse(str));
        }

        AddSet();

        return result;
    }
}