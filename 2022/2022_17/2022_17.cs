using AdventOfCode.Tools;

namespace AdventOfCode;

public class _2022_17 : Problem
{
    private static IVector2D[][] RockShapes = new IVector2D[][]
    {
        new IVector2D[] {new IVector2D(0, 0), new IVector2D(1, 0), new IVector2D(2, 0), new IVector2D(3, 0) },
        new IVector2D[] {new IVector2D(1, 0), new IVector2D(0, 1), new IVector2D(1, 1), new IVector2D(2, 1), new IVector2D(1, 2) },
        new IVector2D[] {new IVector2D(0, 0), new IVector2D(1, 0), new IVector2D(2, 0), new IVector2D(2, 1), new IVector2D(2, 2) },
        new IVector2D[] {new IVector2D(0, 0), new IVector2D(0, 1), new IVector2D(0, 2), new IVector2D(0, 3) },
        new IVector2D[] {new IVector2D(0, 0), new IVector2D(1, 0), new IVector2D(0, 1), new IVector2D(1, 1) },
    };

    private static int[][] IRockShapes = new int[][]
    {
        new int[] { 0b0011110 },
        new int[] { 0b0001000, 0b0011100, 0b0001000 },
        new int[] { 0b0011100, 0b0000100, 0b0000100 },
        new int[] { 0b0010000, 0b0010000, 0b0010000, 0b0010000 },
        new int[] { 0b0011000, 0b0011000 },
    };

    public override void Solve()
    {
        //AddSolution(SolveVector(2022));
        AddSolution(SolveInteger(2022));
        AddSolution(SolveInteger(1000000000000));
    }

    private int SolveVector(int count)
    {
        int rockIdx = 0;
        int rockCnt = 0;
        int jetIdx = 0;
        IPoint2D rockPos = new IPoint2D(2, 3);
        List<IPoint2D> map = new();
        IVector2D fallVector = new IVector2D(0, -1);
        int maxY = 0;

        while (rockCnt < count)
        {
            bool TestPos(IPoint2D pos)
            {
                return RockShapes[rockIdx].Any(r =>
                {
                    IPoint2D np = pos + r;
                    if (np.Y < 0) return true;
                    return map.Contains(np);
                });
            }
            void RestRock()
            {
                foreach (IVector2D v in RockShapes[rockIdx])
                    map.Add(rockPos + v);
                rockCnt++;
                rockIdx = (rockIdx + 1) % RockShapes.Length;
                rockPos = new IPoint2D(2, 3 + map.Max(r => r.Y) + 1);
                //Log(map);
            }

            // jet move
            IVector2D jet = Inputs[0][jetIdx] == '<' ? new IVector2D(-1, 0) : new IVector2D(1, 0);
            jetIdx = (jetIdx + 1) % Inputs[0].Length;
            IPoint2D np = rockPos + jet;
            np.X = Math.Min(Math.Max(np.X, 0), 7 - RockShapes[rockIdx].Max(r => r.X) - 1);
            if (!TestPos(np))
                rockPos = np;

            // fall
            if (TestPos(rockPos + fallVector))
            {
                RestRock();
            }
            else
                rockPos += fallVector;
        }

        return map.Max(r => r.Y) + 1;
    }
    private long SolveInteger(long count)
    {
        int rockIdx = 0;
        long rockCnt = 0;
        int jetIdx = 0;
        List<int> layers = new();
        long layersOff = 0;
        int y = 3;
        bool searchPattern = true;
        int[] rock = IRockShapes[rockIdx].ToArray();
        Dictionary<long, long> rocksHeight = new();

        while (rockCnt < count)
        {
            bool TestLayer(int y, int value)
            {
                if (y < 0) return true;
                if (y >= layers.Count) return false;
                return (layers[y] & value) != 0;
            }
            bool TestPos(int y, int[] r) => Enumerable.Range(0, r.Length).Any(i => TestLayer(y + i, r[i]));

            void RestRock()
            {
                for(int i = 0; i < rock.Length; i++)
                {
                    if(y + i < layers.Count)
                        layers[y + i] |= rock[i];
                    else
                        layers.Add(rock[i]);
                }
                rockCnt++;
                rockIdx = (rockIdx + 1) % RockShapes.Length;
                rock = IRockShapes[rockIdx].ToArray();
                y = layers.Count + 3;
                if(!rocksHeight.ContainsKey(layers.Count))
                    rocksHeight[layers.Count] = rockCnt;

                //if(rockCnt < 10)
                //    Log(layers);

                // look for pattern
                if (searchPattern)
                {
                    List<int> ltest = layers.ToList();
                    ltest.Reverse();
                    for (int size = 100; size < layers.Count / 2; size++)
                    {
                        if (CompareSequeces(ltest.Take(size), ltest.Skip(size).Take(size)))
                        {
                            Console.WriteLine($"Pattern of ({size}) at y={layers.Count}");

                            long dRock = rocksHeight[layers.Count] - rocksHeight[layers.Count - size];
                            long cnt = (count - rockCnt) / dRock;

                            rockCnt += cnt * dRock;

                            layersOff = cnt * size;

                            searchPattern = false;
                            break;
                        }
                    }
                }
            }

            int[] MoveLR(char dir, int[] r)
            {
                if (dir == '<')
                {
                    if (r.Any(i => (i & 0b1000000) != 0))
                        return r;
                    if (Enumerable.Range(0, r.Length).Any(i => TestLayer(y + i, r[i] << 1)))
                        return r;
                    return r.Select(i => i << 1).ToArray();
                }
                else
                {
                    if (r.Any(i => (i & 0b1) != 0))
                        return r;
                    if (Enumerable.Range(0, r.Length).Any(i => TestLayer(y + i, r[i] >> 1)))
                        return r;
                    return r.Select(i => i >> 1).ToArray();
                }
            }

            // jet move
            rock = MoveLR(Inputs[0][jetIdx], rock);
            jetIdx = (jetIdx + 1) % Inputs[0].Length;

            // fall
            if (TestPos(y - 1, rock))
            {
                RestRock();
            }
            else
                y -= 1;
        }

        return layers.Count + layersOff;
    }
    private static bool CompareSequeces(IEnumerable<int> s0, IEnumerable<int> s1)
    {
        IEnumerator<int> e0 = s0.GetEnumerator();
        IEnumerator<int> e1 = s1.GetEnumerator();
        while(e0.MoveNext() && e1.MoveNext())
            if(e0.Current != e1.Current) 
                return false;
        return true;
    }
    private static void Log(List<int> list)
    {
        for (int y = list.Count - 1; y >= 0; y--)
        {
            Console.WriteLine($"|{Convert.ToString(list[y], 2).PadLeft(7, '.').Replace("0", ".").Replace("1", "#")}|");
        }
        Console.WriteLine($"*-------+");
    }
    private static void Log(List<IPoint2D> list)
    {
        int ym = list.Max(p => p.Y);
        for(int y = ym; y>= 0; y--)
        {
            char[] line = ".......".ToArray();
            foreach (IPoint2D point in list.Where(p => p.Y == y))
                line[point.X] = '#';
            Console.WriteLine($"|{new string(line)}|");
        }
        Console.WriteLine($"*-------+");
    }
}