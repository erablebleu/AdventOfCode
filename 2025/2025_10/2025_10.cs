namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2025/day/10
/// </summary>
public class _2025_10 : Problem
{
    private record Machine(int Diagram, int[][] Wires, int[] Joltages);

    private Machine[] _machines;

    public override void Parse()
    {
        _machines = [.. Inputs.Select(l =>
        {
            string[] el = l.Split(" ");
            int diagram = Convert.ToInt32(new string([.. el[0].Remove("[", "]").Replace('.', '0').Replace('#', '1').Reverse()]), 2);
            int[][] wires = [.. el.Skip(1).Take(el.Length - 2).Select(w => w.Remove("(", ")").Split(",").Select(int.Parse).ToArray())];
            int[] joltages = [.. el.Last().Remove("{", "}").Split(",").Select(int.Parse)];

            return new Machine(diagram, wires, joltages);
        })];
    }

    private static int GetMinPress(Machine machine)
    {
        int result = int.MaxValue;
        int[] wires = [.. machine.Wires.Select(w => {
            int wire = 0;

            foreach(int b in w)
                wire |= 1 << b;

            return wire;
        })];

        for (int i = 0; i < Math.Pow(2, machine.Wires.Length); i++)
        {
            int r = 0;
            int count = 0;

            for (int b = 0; b < machine.Wires.Length; b++)
            {
                if (!i.GetBit(b))
                    continue;
                r ^= wires[b];
                count++;
            }

            if (machine.Diagram != r)
                continue;

            result = Math.Min(result, count);
        }

        return result;
    }

    public override object PartOne()
        => _machines.Sum(GetMinPress);

    public override object PartTwo()
        => null;
}