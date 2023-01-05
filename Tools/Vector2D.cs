using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Tools;

public struct IPoint2D
{
    public IPoint2D(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X;
    public int Y;

    public static IPoint2D operator -(IPoint2D a, IVector2D b) => new(a.X - b.X, a.Y - b.Y);

    public static IVector2D operator -(IPoint2D a, IPoint2D b) => new(a.X - b.X, a.Y - b.Y);

    public static bool operator !=(IPoint2D a, IPoint2D b) => a.X != b.X || a.Y != b.Y;

    public static IPoint2D operator /(IPoint2D a, int b) => new(a.X / b, a.Y / b);

    public static IPoint2D operator +(IPoint2D a, IVector2D b) => new(a.X + b.X, a.Y + b.Y);

    public static bool operator ==(IPoint2D a, IPoint2D b) => a.X == b.X && a.Y == b.Y;

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        return obj is IPoint2D p && p.X == X && p.Y == Y;
    }

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

    public override string ToString() => $"{X}, {Y}";
}

public struct IVector2D
{
    public static IVector2D North = new(0, -1);
    public static IVector2D South = new(0, 1);
    public static IVector2D East = new(1, 0);
    public static IVector2D West = new(-1, 0);
    public static IVector2D[] DirectionNESW = new IVector2D[] { North, East, South, West };
    public static IVector2D[] DirectionNWSE = new IVector2D[] { North, West, South, East };
    public static IVector2D[] DirectionNSEW = new IVector2D[] { North, South, East, West };
    public static IVector2D[] DirectionWNES = new IVector2D[] { West, North, East, South };
    public static IVector2D[] Direction8 = new IVector2D[] { West, new IVector2D(-1, -1), North, new IVector2D(1, -1), East, new IVector2D(1, 1), South, new IVector2D(-1, 1) };

    public IVector2D(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X;
    public int Y;

    public static IVector2D operator -(IVector2D a, IVector2D b) => new(a.X - b.X, a.Y - b.Y);

    public static IVector2D operator *(IVector2D a, double b) => new((int)(a.X * b), (int)(a.Y * b));

    public static IVector2D operator *(IVector2D a, int b) => new(a.X * b, a.Y * b);

    public static IVector2D operator /(IVector2D a, double b) => new((int)(a.X / b), (int)(a.Y / b));

    public static IVector2D operator /(IVector2D a, int b) => new(a.X / b, a.Y / b);

    public static IVector2D operator +(IVector2D a, IVector2D b) => new(a.X + b.X, a.Y + b.Y);
    
    public static bool operator ==(IVector2D a, IVector2D b) => a.X == b.X && a.Y == b.Y;

    public static bool operator !=(IVector2D a, IVector2D b) => a.X != b.X || a.Y != b.Y;

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        return obj is IVector2D p && p.X == X && p.Y == Y;
    }

    public override string ToString() => $"{X}, {Y}";
}