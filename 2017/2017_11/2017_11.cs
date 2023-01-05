namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2017/day/11
/// </summary>
public class _2017_11 : Problem
{
    private IVector3D[] _data;

    public static int GetDistance(IPoint3D a, IPoint3D b)
        => Math.Max(Math.Abs(a.Y - b.Y), Math.Max(Math.Abs(a.X - b.X), Math.Abs(a.Z - b.Z)));

    public override void Parse()
    {
        _data = Inputs[0].Split(',').Select(e => e switch
        {
            "n" => 0,
            "nw" => 1,
            "sw" => 2,
            "s" => 3,
            "se" => 4,
            "ne" => 5,
            _ => throw new NotImplementedException(),
        }).Select(i => IVector3D.HexagonalDirections[i]).ToArray();
    }

    public override object PartOne()
    {
        IPoint3D p = new();

        foreach (IVector3D direction in _data)
            p += direction;

        return GetDistance(new IPoint3D(), p);
    }

    public override object PartTwo()
    {
        IPoint3D start = new();
        IPoint3D p = start;
        int result = 0;

        foreach (IVector3D direction in _data)
        {
            p += direction;
            result = Math.Max(GetDistance(start, p), result);
        }

        return result;
    }
}