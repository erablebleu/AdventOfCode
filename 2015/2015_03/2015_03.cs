using System.Runtime.CompilerServices;

namespace AdventOfCode;

public class _2015_03 : Problem
{
    private static IVector2D[] Directions = new IVector2D[]
    {
        new IVector2D(1, 0),
        new IVector2D(0, 1),
        new IVector2D(-1, 0),
        new IVector2D(0, -1),
    };
    private int[] _data;

    public override void Parse()
    {
        _data = Inputs[0].Select(c => c switch
        {
            '>' => 0,
            'v' => 1,
            '<' => 2,
            _ => 3
        }).ToArray();
    }

    public override object PartOne() => Emulate();

    public override object PartTwo() => Emulate(2);

    private int Emulate(int pCount = 1)
    {
        IPoint2D[] positions = Enumerable.Range(0, pCount).Select(i => new IPoint2D(0, 0)).ToArray();
        HashSet<IPoint2D> result = new() { positions[0] };

        for (int i = 0; i < _data.Length; i += pCount)
            for (int j = 0; j < positions.Length; j++)
                result.Add(positions[j] += Directions[_data[i + j]]);

        return result.Count;
    }
}