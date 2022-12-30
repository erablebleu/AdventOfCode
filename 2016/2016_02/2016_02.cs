namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/02
/// </summary>
public class _2016_02 : Problem
{
    private int[][] _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => l.Select(c => c switch
        {
            'U' => 0,
            'R' => 1,
            'D' => 2,
            'L' => 3,
            _ => throw new ArgumentException()
        }).ToArray()).ToArray();
    }

    public override object PartOne() => GetCode(new IPoint2D(1, 1), new string[]
    {
        "123",
        "456",
        "789",
    });

    public override object PartTwo() => GetCode(new IPoint2D(0, 2), new string[]
    {
        "  1  ",
        " 234 ",
        "56789",
        " ABC ",
        "  D  ",
    });

    private string GetCode(IPoint2D p, string[] keyPad)
    {
        char[] result = new char[_data.Length];

        for (int i = 0; i < _data.Length; i++)
        {
            foreach (int dir in _data[i])
            {
                IPoint2D np = p + IVector2D.DirectionNESW[dir];
                if (np.X < 0 || np.X >= keyPad[0].Length
                    || np.Y < 0 || np.Y >= keyPad.Length
                        || keyPad[np.Y][np.X] == ' ')
                    continue;
                p = np;
            }
            result[i] = keyPad[p.Y][p.X];
        }

        return new string(result);
    }
}