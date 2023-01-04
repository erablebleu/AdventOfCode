namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/12
/// </summary>
public class _2019_12 : Problem
{
    public int GetIncVel(int posA, int posB)
    {
        if (posA > posB) return -1;
        if (posA < posB) return +1;
        else return 0;
    }

    public override void Parse()
    {
    }

    public override object PartOne()
    {
        int N = 1000000;
        List<Moon> moons = new();
        Point[,] points = new Point[N, 4];
        moons.AddRange(Inputs.Select(l => new Moon(l)));
        Point[] set = new Point[4];
        Point[] init = new Point[4];
        init[0] = moons[0].Pos.Copy();
        init[1] = moons[1].Pos.Copy();
        init[2] = moons[2].Pos.Copy();
        init[3] = moons[3].Pos.Copy();
        int i, j, k;

        for (i = 0; i < N; i++)
        {
            // Apply gravity
            for (j = 0; j < moons.Count; j++)
            {
                for (k = j + 1; k < moons.Count; k++)
                {
                    GetIncVel(moons[j], moons[k]);
                    GetIncVel(moons[k], moons[j]);
                }
            }
            // Apply velocity
            set = new Point[moons.Count];
            for (j = 0; j < moons.Count; j++)
            {
                moons[j].Pos.X += moons[j].Vel.X;
                moons[j].Pos.Y += moons[j].Vel.Y;
                moons[j].Pos.Z += moons[j].Vel.Z;
                set[j] = moons[j].Pos.Copy();

                points[i, j] = moons[j].Pos.Copy();
            }
        }
        long[] mult = new long[3 * 4];
        int sam = 10;
        for (j = 0; j < moons.Count; j++)
        {
            for (i = 1; i < N; i++)
            {
                for (k = 0; k < sam && points[i + k, j].X == points[k, j].X; k++) ;
                if (k == sam)
                    break;
            }
            mult[3 * j + 0] = i;

            for (i = 1; i < N; i++)
            {
                for (k = 0; k < sam && points[i + k, j].Y == points[k, j].Y; k++) ;
                if (k == sam)
                    break;
            }
            mult[3 * j + 1] = i;

            for (i = 1; i < N; i++)
            {
                for (k = 0; k < sam && points[i + k, j].Z == points[k, j].Z; k++) ;
                if (k == sam)
                    break;
            }
            mult[3 * j + 2] = i;
        }

        return LCM(mult);
    }

    public override object PartTwo()
    {
        return null;
    }

    private static long GCD(long val1, long val2)
    {
        long temp;

        if (val1 > val2)
        {
            temp = val1;
            val1 = val2;
            val2 = temp;
        }
        while (val2 % val1 != 0)
        {
            temp = val2 % val1;
            val2 = val1;
            val1 = temp;

            if (val1 > val2)
            {
                temp = val1;
                val1 = val2;
                val2 = temp;
            }
        }

        //if (val2 % val1 == 0) return val1;
        //else return GCD(val2 % val1, val1);
        return val1;
    }

    private static long LCM(long num1, long num2)
    {
        long gcd = GCD(num1, num2);
        return num1 * num2 / gcd;
    }

    private static long LCM(long[] array)
    {
        long res = 1;
        for (int i = 0; i < array.Length; i++)
            res = LCM(res, array[i]);
        return res;
    }

    private void GetIncVel(Moon a, Moon b)
    {
        a.Vel.X += GetIncVel(a.Pos.X, b.Pos.X);
        a.Vel.Y += GetIncVel(a.Pos.Y, b.Pos.Y);
        a.Vel.Z += GetIncVel(a.Pos.Z, b.Pos.Z);
    }

    private class Moon
    {
        public Moon(string line)
        {
            Pos = new Point(line);
            Vel = new Point();
        }

        public int Kin => Math.Abs(Vel.X) + Math.Abs(Vel.Y) + Math.Abs(Vel.Z);
        public Point Pos { get; set; }
        public int Pot => Math.Abs(Pos.X) + Math.Abs(Pos.Y) + Math.Abs(Pos.Z);
        public Point Vel { get; set; }

        public override string ToString()
        {
            return $"pos={Pos}, vel={Vel}";
        }
    }

    private class Point
    {
        public Point()
        {
        }

        public Point(string line)
        {
            string[] inputs = line.Replace("<x=", "").Replace("y=", "").Replace("z=", "").Replace(">", "").Split(',');
            X = int.Parse(inputs[0]);
            Y = int.Parse(inputs[1]);
            Z = int.Parse(inputs[2]);
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public static bool operator !=(Point a, Point b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public Point Copy()
        {
            return new Point() { X = X, Y = Y, Z = Z };
        }

        public override string ToString() => $"<x={X}, y={Y}, z={Z}>";
    }
}