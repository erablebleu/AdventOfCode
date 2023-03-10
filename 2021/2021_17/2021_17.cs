namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2021/day/17
/// </summary>
public class _2021_17 : Problem
{
    private int _cnt;
    private int _maxY;
    private System.Drawing.Rectangle _target;

    public override void Parse()
    {
        int[] coord = Inputs.First().Replace("target area: ", "").Split(", ").SelectMany(el => el.Split("=")[1].Split("..").Select(c => int.Parse(c))).ToArray();
        _target = new(Math.Min(coord[0], coord[1]),
                      Math.Min(coord[2], coord[3]),
                      Math.Abs(coord[1] - coord[0]),
                      Math.Abs(coord[3] - coord[2]));

        _maxY = 0;
        _cnt = 0;

        for (int x = 1; x <= _target.X + _target.Width; x++)
            for (int y = _target.Y; y <= -_target.Y; y++)
            {
                if (ReachTarget(new System.Drawing.Point(x, y), out int max, out var lastPos))
                {
                    _cnt++;
                    _maxY = Math.Max(max, _maxY);
                }
            }
    }

    public override object PartOne() => _maxY;

    public override object PartTwo() => _cnt;

    private static bool Contain(System.Drawing.Rectangle rect, System.Drawing.Point p)
    {
        return p.X >= rect.X && p.X <= rect.X + rect.Width
            && p.Y >= rect.Y && p.Y <= rect.Y + rect.Height;
    }

    private bool ReachTarget(System.Drawing.Point vel, out int maxY, out System.Drawing.Point lastPos)
    {
        lastPos = new System.Drawing.Point(0, 0);
        maxY = 0;

        while (!Contain(_target, lastPos)
               && lastPos.Y > _target.Y
               && lastPos.X < _target.X + _target.Width)
        {
            lastPos.X += vel.X;
            lastPos.Y += vel.Y;

            maxY = Math.Max(lastPos.Y, maxY);

            vel.X = Math.Abs(vel.X) > 0 ? vel.X - vel.X / Math.Abs(vel.X) : 0;
            vel.Y -= 1;
        }

        return Contain(_target, lastPos);
    }
}