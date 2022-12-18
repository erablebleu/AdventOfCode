namespace AdventOfCode;

public class _2021_19 : Problem
{
    private static Matrix[] RotationsMatrix = new Matrix[]
    {
        new Matrix( new int[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } }),
        new Matrix( new int[,] { { 1, 0, 0 }, { 0, 0, -1 }, { 0, 1, 0 } }),
        new Matrix( new int[,] { { 1, 0, 0 }, { 0, -1, 0 }, { 0, 0, -1 } }),
        new Matrix( new int[,] { { 1, 0, 0 }, { 0, 0, 1 }, { 0, -1, 0 } }),

        new Matrix( new int[,] { { 0, -1, 0 }, { 1, 0, 0 }, { 0, 0, 1 } }),
        new Matrix( new int[,] { { 0, 0, 1 }, { 1, 0, 0 }, { 0, 1, 0 } }),
        new Matrix( new int[,] { { 0, 1, 0 }, { 1, 0, 0 }, { 0, 0, -1 } }),
        new Matrix( new int[,] { { 0, 0, -1 }, { 1, 0, 0 }, { 0, -1, 0 } }),

        new Matrix( new int[,] { { -1, 0, 0 }, { 0, -1, 0 }, { 0, 0, 1 } }),
        new Matrix( new int[,] { { -1, 0, 0 }, { 0, 0, -1 }, { 0, -1, 0 } }),
        new Matrix( new int[,] { { -1, 0, 0 }, { 0, 1, 0 }, { 0, 0, -1 } }),
        new Matrix( new int[,] { { -1, 0, 0 }, { 0, 0, 1 }, { 0, 1, 0 } }),

        new Matrix( new int[,] { { 0, 1, 0 }, { -1, 0, 0 }, { 0, 0, 1 } }),
        new Matrix( new int[,] { { 0, 0, 1 }, { -1, 0, 0 }, { 0, -1, 0 } }),
        new Matrix( new int[,] { { 0, -1, 0 }, { -1, 0, 0 }, { 0, 0, -1 } }),
        new Matrix( new int[,] { { 0, 0, -1 }, { -1, 0, 0 }, { 0, 1, 0 } }),

        new Matrix( new int[,] { { 0, 0, -1 }, { 0, 1, 0 }, { 1, 0, 0 } }),
        new Matrix( new int[,] { { 0, 1, 0 }, { 0, 0, 1 }, { 1, 0, 0 } }),
        new Matrix( new int[,] { { 0, 0, 1 }, { 0, -1, 0 }, { 1, 0, 0 } }),
        new Matrix( new int[,] { { 0, -1, 0 }, { 0, 0, -1 }, { 1, 0, 0 } }),

        new Matrix( new int[,] { { 0, 0, -1 }, { 0, -1, 0 }, { -1, 0, 0 } }),
        new Matrix( new int[,] { { 0, -1, 0 }, { 0, 0, 1 }, { -1, 0, 0 } }),
        new Matrix( new int[,] { { 0, 0, 1 }, { 0, 1, 0 }, { -1, 0, 0 } }),
        new Matrix( new int[,] { { 0, 1, 0 }, { 0, 0, -1 }, { -1, 0, 0 } }),
    };

    public override void Solve()
    {
        List<Scanner> scanners = new();
        foreach (var line in Inputs)
        {
            if (string.IsNullOrEmpty(line))
                continue;
            if (line.StartsWith("---"))
                scanners.Add(new Scanner { Number = int.Parse(line.Split(" ")[2]) });
            else
                scanners.Last().Beacons.Add(new Point3D(line.Split(",").Select(el => int.Parse(el))));
        }

        scanners.First().Fix();

        while(scanners.Any(s => !s.IsFixed))
        {
            Console.WriteLine(scanners.Count(s => s.IsFixed));
            scanners.Where(s => !s.IsFixed).Any(sc => scanners.Where(s => s.IsFixed).Any(t => sc.Match(t)));
        }

        Solutions.Add($"{scanners.SelectMany(s => s.FixedBeacons).Distinct().Count()}");

        int max = 0;
        for(int i = 0; i < scanners.Count -1; i++)
        {
            for (int j = i + 1; j < scanners.Count; j++)
            {
                max = Math.Max(max, Math.Abs(scanners[i].Position.X - scanners[j].Position.X)
                    + Math.Abs(scanners[i].Position.Y - scanners[j].Position.Y)
                    + Math.Abs(scanners[i].Position.Z - scanners[j].Position.Z));
            }
        }
        Solutions.Add($"{max}");
    }

    internal class Matrix
    {
        private readonly int[,] _values;

        public Matrix()
        { _values = new int[3, 3]; }

        public Matrix(int[,] array)
        {
            _values = array;
        }

        public int this[int i, int j]
        {
            get => _values[i, j];
            set => _values[i, j] = value;
        }
    }

    internal class Point3D
    {
        public Point3D()
        { }

        public Point3D(int x, int y, int z)
        {
            X = x; Y = y; Z = z;
        }

        public Point3D(IEnumerable<int> coord)
        {
            X = coord.ElementAt(0);
            Y = coord.ElementAt(1);
            Z = coord.ElementAt(2);
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public static Point3D operator -(Point3D a) => new(-a.X, -a.Y, -a.Z);

        public static Point3D operator -(Point3D a, Point3D b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Point3D operator *(Point3D a, Matrix m) => new(a.X * m[0, 0] + a.Y * m[0, 1] + a.Z * m[0, 2],
                                                                     a.X * m[1, 0] + a.Y * m[1, 1] + a.Z * m[1, 2],
                                                                     a.X * m[2, 0] + a.Y * m[2, 1] + a.Z * m[2, 2]);

        public static Point3D operator *(Matrix m, Point3D a) => new(a.X * m[0, 0] + a.Y * m[1, 0] + a.Z * m[2, 0],
                                                                     a.X * m[0, 1] + a.Y * m[1, 1] + a.Z * m[2, 1],
                                                                     a.X * m[0, 2] + a.Y * m[1, 2] + a.Z * m[2, 2]);

        public static Point3D operator +(Point3D a) => a;

        public static Point3D operator +(Point3D a, Point3D b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public override string ToString() => $"{X},{Y},{Z}"; 
        public override bool Equals(object obj)
        {
            if (obj is not Point3D p)
                return false;

            return p.X == X && p.Y == Y && p.Z == Z;
        }
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }
    }

    internal class Scanner
    {
        public List<Point3D> Beacons { get; set; } = new();
        public List<Point3D> FixedBeacons { get; set; } = new();
        public bool IsFixed { get; set; }
        public int Number { get; set; }
        public Point3D Position { get; set; } = new Point3D();
        public Matrix Rotation { get; set; } = RotationsMatrix.First();
        public void Fix()
        {
            FixedBeacons = Beacons.Select(b => b * Rotation + Position).ToList();
            IsFixed = true;
        }
        public void Fix(Point3D position, Matrix rotation)
        {
            Position = position;
            Rotation = rotation;
            Fix();
        }

        public bool Match(Scanner target)
        {
            foreach(Matrix rotation in RotationsMatrix)
            {
                foreach(Point3D p1 in target.FixedBeacons)
                {
                    foreach (Point3D p2 in Beacons)
                    {
                        Point3D tmpP2 = p2 * rotation;
                        Point3D tr = p1 - tmpP2;

                        int cnt = 1;

                        foreach (Point3D p3 in Beacons)
                        {
                            if (p2 == p3) continue;
                            if (!target.FixedBeacons.Contains(p3 * rotation + tr)) continue;
                            cnt++;
                            if (cnt >= 12) break;
                        }

                        if (cnt < 12)
                            continue;

                        Fix(tr, rotation);

                        return true;
                    }
                }
            }
            return false;
        }

        public override string ToString() => $"--- scanner {Number} ---\r\n{string.Join("\r\n", Beacons)}";
    }
}