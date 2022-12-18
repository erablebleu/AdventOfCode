using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Cube3D
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int W { get; set; }
        public Cube3D(int x, int y, int z = 0, int w = 0)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
    }
    public class _2020_17 : Problem
    {
        #region Methods

        public override void Solve()
        {
            List<Cube3D> cubes = Enumerable.Range(0, Inputs.Length).SelectMany(i => Enumerable.Range(0, Inputs[i].Length).Where(j => Inputs[i][j] == '#').Select(j => new Cube3D(j, i))).ToList();
            Cube3D min = new Cube3D(cubes.Min(c => c.X), cubes.Min(c => c.Y));
            Cube3D max = new Cube3D(cubes.Max(c => c.X), cubes.Max(c => c.Y));

            for (int i = 0; i < 6; i++)
            {
                List<Cube3D> next = new List<Cube3D>();
                Cube3D nextMin = new Cube3D(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
                Cube3D nextMax = new Cube3D(0, 0);
                for (int x = min.X - 1; x <= max.X + 1; x++)
                    for (int y = min.Y - 1; y <= max.Y + 1; y++)
                        for (int z = min.Z - 1; z <= max.Z + 1; z++)
                        {
                            bool state = cubes.Any(c => c.X == x && c.Y == y && c.Z == z);
                            int cnt = 0;
                            for (int dx = -1; dx <= 1; dx++)
                                for (int dy = -1; dy <= 1; dy++)
                                    for (int dz = -1; dz <= 1; dz++)
                                        if (dx == 0 && dy == 0 && dz == 0) continue;
                                        else if (cubes.Any(c => c.X == x + dx && c.Y == y + dy && c.Z == z + dz))
                                            cnt++;


                            if ((state && (cnt == 2 || cnt == 3))
                                || (!state && cnt == 3))
                            {
                                next.Add(new Cube3D(x, y, z));
                                nextMin.X = Math.Min(nextMin.X, x);
                                nextMin.Y = Math.Min(nextMin.Y, y);
                                nextMin.Z = Math.Min(nextMin.Z, z);
                                nextMax.X = Math.Max(nextMax.X, x);
                                nextMax.Y = Math.Max(nextMax.Y, y);
                                nextMax.Z = Math.Max(nextMax.Z, z);
                            }
                        }

                cubes = next;
                min = nextMin;
                max = nextMax;
            }

            Solutions.Add($"{cubes.Count}");


            cubes = Enumerable.Range(0, Inputs.Length).SelectMany(i => Enumerable.Range(0, Inputs[i].Length).Where(j => Inputs[i][j] == '#').Select(j => new Cube3D(j, i))).ToList();
            min = new Cube3D(cubes.Min(c => c.X), cubes.Min(c => c.Y));
            max = new Cube3D(cubes.Max(c => c.X), cubes.Max(c => c.Y));

            for (int i = 0; i < 6; i++)
            {
                List<Cube3D> next = new List<Cube3D>();
                Cube3D nextMin = new Cube3D(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
                Cube3D nextMax = new Cube3D(0, 0);
                for (int x = min.X - 1; x <= max.X + 1; x++)
                    for (int y = min.Y - 1; y <= max.Y + 1; y++)
                        for (int z = min.Z - 1; z <= max.Z + 1; z++)
                            for (int w = min.W - 1; w <= max.W + 1; w++)
                            {
                                bool state = cubes.Any(c => c.X == x && c.Y == y && c.Z == z && c.W == w);
                                int cnt = 0;
                                for (int dx = -1; dx <= 1; dx++)
                                    for (int dy = -1; dy <= 1; dy++)
                                        for (int dz = -1; dz <= 1; dz++)
                                            for (int dw = -1; dw <= 1; dw++)
                                                if (dx == 0 && dy == 0 && dz == 0 && dw == 0) continue;
                                                else if (cubes.Any(c => c.X == x + dx && c.Y == y + dy && c.Z == z + dz && c.W == w + dw))
                                                    cnt++;


                                if ((state && (cnt == 2 || cnt == 3))
                                    || (!state && cnt == 3))
                                {
                                    next.Add(new Cube3D(x, y, z, w));
                                    nextMin.X = Math.Min(nextMin.X, x);
                                    nextMin.Y = Math.Min(nextMin.Y, y);
                                    nextMin.Z = Math.Min(nextMin.Z, z);
                                    nextMin.W = Math.Min(nextMin.W, w);
                                    nextMax.X = Math.Max(nextMax.X, x);
                                    nextMax.Y = Math.Max(nextMax.Y, y);
                                    nextMax.Z = Math.Max(nextMax.Z, z);
                                    nextMax.W = Math.Max(nextMax.W, w);
                                }
                            }

                cubes = next;
                min = nextMin;
                max = nextMax;
            }

            Solutions.Add($"{cubes.Count}");
        }


        #endregion
    }
}