namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/01
/// </summary>
public class _2016_01 : Problem
{
    private string[] _data;

    public override void Parse()
    {
        _data = Inputs[0].Split(", ");
    }

    public override object PartOne()
    {
        int dir = 0;
        IPoint2D p = new();

        foreach (string d in _data)
        {
            dir = (dir + 4 + (d[0] == 'R' ? 1 : -1)) % 4;
            p += IVector2D.DirectionNESW[dir] * int.Parse(d[1..]);
        }

        return Math.Abs(p.X) + Math.Abs(p.Y);
    }

    public override object PartTwo()
    {
        int dir = 0;
        IPoint2D p = new();
        HashSet<IPoint2D> hash = new() { p };

        foreach (string d in _data)
        {
            dir = (dir + 4 + (d[0] == 'R' ? 1 : -1)) % 4;
            int cnt = int.Parse(d[1..]);
            for (int i = 0; i < cnt; i++)
            {
                p += IVector2D.DirectionNESW[dir];
                if (hash.Contains(p))
                    return Math.Abs(p.X) + Math.Abs(p.Y);
                hash.Add(p);
            }
        }

        return null;
    }
}