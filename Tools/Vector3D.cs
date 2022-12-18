using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Tools;

public struct IPoint3D
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public IPoint3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public IPoint3D(int[] v)
    {
        X = v[0];
        Y = v[1];
        Z = v[2];
    }

    public static IPoint3D operator +(IPoint3D a, IVector3D b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static IPoint3D operator -(IPoint3D a, IVector3D b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static IVector3D operator -(IPoint3D a, IPoint3D b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static bool operator ==(IPoint3D a, IPoint3D b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    public static bool operator !=(IPoint3D a, IPoint3D b) => a.X != b.X || a.Y != b.Y || a.Z != b.Z;
    public override string ToString() => $"{X}, {Y}, {Z}";
    public override int GetHashCode()
    {
        unchecked
        {
            int hashcode = 1430287;
            hashcode *= 7302013 ^ X.GetHashCode();
            hashcode *= 7302013 ^ Y.GetHashCode();
            hashcode *= 7302013 ^ Z.GetHashCode();
            return hashcode;
        }
    }
    public override bool Equals([NotNullWhen(true)] object obj)
    {
        return obj is IPoint3D p && p.X == X && p.Y == Y && p.Z == Z;
    }
}
public struct IVector3D
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public IVector3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static IVector3D operator +(IVector3D a, IVector3D b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static IVector3D operator -(IVector3D a, IVector3D b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static IVector3D operator *(IVector3D a, double b) => new((int)(a.X * b), (int)(a.Y * b), (int)(a.Z * b));
    public static IVector3D operator /(IVector3D a, double b) => new((int)(a.X / b), (int)(a.Y / b), (int)(a.Z / b));

    public override string ToString() => $"{X}, {Y}";
}
