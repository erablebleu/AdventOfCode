using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Tools;

public struct IPoint2D
{
    public int X { get; set; }
    public int Y { get; set; }

    public IPoint2D(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static IPoint2D operator +(IPoint2D a, IVector2D b) => new(a.X + b.X, a.Y + b.Y);
    public static IPoint2D operator -(IPoint2D a, IVector2D b) => new(a.X - b.X, a.Y - b.Y);
    public static IVector2D operator -(IPoint2D a, IPoint2D b) => new(a.X - b.X, a.Y - b.Y);
    public static IPoint2D operator /(IPoint2D a, int b) => new(a.X / b, a.Y / b);
    public static bool operator ==(IPoint2D a, IPoint2D b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(IPoint2D a, IPoint2D b) => a.X != b.X || a.Y != b.Y;
    public override string ToString() => $"{X}, {Y}";
    public override int GetHashCode()
    {
        unchecked
        {
            int hashcode = 1430287;
            hashcode *= 7302013 ^ X.GetHashCode();
            hashcode *= 7302013 ^ Y.GetHashCode();
            return hashcode;
        }
    }
    public override bool Equals([NotNullWhen(true)] object obj)
    {
        return obj is IPoint2D p && p.X == X && p.Y == Y;
    }
}
public struct IVector2D
{
    public int X { get; set; }
    public int Y { get; set; }

    public IVector2D(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static IVector2D operator +(IVector2D a, IVector2D b) => new(a.X + b.X, a.Y + b.Y);
    public static IVector2D operator -(IVector2D a, IVector2D b) => new(a.X - b.X, a.Y - b.Y);
    public static IVector2D operator *(IVector2D a, double b) => new((int)(a.X * b), (int)(a.Y * b));
    public static IVector2D operator /(IVector2D a, double b) => new((int)(a.X / b), (int)(a.Y / b));

    public override string ToString() => $"{X}, {Y}";
}
