using System.Runtime.CompilerServices;

namespace AdventOfCode;

public class _2021_23 : Problem
{
    public class AmphipodDiagram
    {
        private static readonly char[] AmphipodType = new char[] { 'A', 'B', 'C', 'D' };
        private static readonly int[] EnergyCost = new int[] { 1, 10, 100, 1000 };
        private static readonly int[] Rooms = new int[] { 3, 5, 7, 9 };
        private static readonly int[] WaitingSpots = new int[] { 1, 2, 4, 6, 8, 10, 11 };

        private static int RoomSize;
        private readonly char[,] _map;
        public int Energy { get; set; }
        public AmphipodDiagram(IEnumerable<string> lines, params string[] insert)
        {
            List<string> list = lines.ToList();
            for(int i = 0; i < insert.Length; i++)
                list.Insert(3 + i, insert[i]);
            RoomSize = 2 + insert.Length;
            _map = list.Select(l => l.ToArray()).To2DArray();
        }
        public AmphipodDiagram(AmphipodDiagram model, int xSrc, int ySrc, int xDst, int yDst)
        {
            _map = model._map.ToArray();
            int idx = Array.IndexOf(AmphipodType, _map[ySrc, xSrc]);
            _map[yDst, xDst] = _map[ySrc, xSrc];
            _map[ySrc, xSrc] = '.';
            Energy = model.Energy + EnergyCost[idx] * (Math.Abs(xSrc - xDst) + (ySrc - 1) + (yDst - 1));
        }
        public bool IsSorted()
        {
            for (int i = 0; i < AmphipodType.Length; i++)
                for(int y = 2; y < 2 + RoomSize; y++)
                if (_map[y, Rooms[i]] != AmphipodType[i])
                    return false;

            return true;
        }
        private static Dictionary<string, int?> _explored;
        public int? Emulate()
        {
            _explored = new();
            return Solve();
        }
        private string GetHash()
        {
            char[] result = new char[WaitingSpots.Length + 4 * RoomSize];
            for(int i = 0; i < WaitingSpots.Length; i++)
                result[i] = _map[1, WaitingSpots[i]];
            for(int i = 0; i < Rooms.Length; i++)
            {
                for (int y = 0; y < RoomSize; y++)
                    result[WaitingSpots.Length + i * RoomSize + y] = _map[2 + y, Rooms[i]];
            }
            return new string(result);
        }
        public int? Solve()
        {
            //Log();
            string hash = GetHash();
            if (_explored.TryGetValue(hash, out int? sol))
                return sol is null ? null : Energy + sol.Value;

            if (IsSorted())
                return Energy;


            int? solution = null;
            foreach (Tuple<int, AmphipodDiagram> t in GetMoves().OrderBy(t => t.Item1).ToList())
            {
                int? tmpSol = t.Item2.Solve();
                if (tmpSol is null)
                    continue;
                solution = solution is null ? tmpSol : Math.Min(solution.Value, tmpSol.Value);
            }

            _explored[hash] = solution - Energy;

            return solution;
        }

        public IEnumerable<Tuple<int, AmphipodDiagram>> GetMoves()
        {
            foreach(int x in WaitingSpots)
            {
                char c = _map[1, x];
                if (c == '.')
                    continue;

                int destIdx = Array.IndexOf(AmphipodType, c);
                int destX = Rooms[destIdx];

                if (!CanStack(c, out int destY) || !CanMove(x, destX))
                    continue;

                yield return new Tuple<int, AmphipodDiagram>(destIdx, new AmphipodDiagram(this, x, 1, destX, destY));
            }

            for(int i = 0; i < 4 ; i++)
            { 
                int x = Rooms[i];
                char targetC = AmphipodType[i];
                for (int y = 2; y < 2 + RoomSize; y++)
                {
                    char c = _map[y, x];
                    int destIdx = Array.IndexOf(AmphipodType, c);

                    if (c == '.' || !NeedMove(x, y))
                        continue;

                    // Move direct to dest
                    {
                        int destX = Rooms[destIdx];

                        if (CanStack(c, out int destY) && CanMove(x, destX, y))
                            yield return new Tuple<int, AmphipodDiagram>(destIdx, new AmphipodDiagram(this, x, y, destX, destY));
                    }

                    // Move to waiting spot
                    foreach (int destX in WaitingSpots)
                    {
                        if (_map[1, destX] != '.'
                            || !CanMove(x, destX, y))
                            continue;

                        yield return new Tuple<int, AmphipodDiagram>(destIdx, new AmphipodDiagram(this, x, y, destX, 1));
                    }
                }
            }

            yield break;
        }

        private bool CanStack(char c, out int yDst)
        {
            yDst = 0;
            int idx = Array.IndexOf(AmphipodType, c);
            int x = Rooms[idx];
            for (int y = 1 + RoomSize; y >= 2; y--)
            {
                if (_map[y, x] == c)
                    continue;
                yDst = y;
                return _map[y, x] == '.';
            }
            return false;
        }
        private bool CanMove(int x0, int x1, int y = 1)
        {
            int xMin = Math.Min(x0, x1);
            int xMax = Math.Max(x0, x1);
            foreach (int x in WaitingSpots)
                if (x > xMin && x < xMax && _map[1, x] != '.')
                    return false;

            for (int y2 = y - 1; y >= 2; y--)
                if (_map[y2, x0] != '.')
                    return false;

            return true;
        }
        private bool NeedMove(int x, int y)
        {
            int destIdx = Array.IndexOf(Rooms, x);
            char c = AmphipodType[destIdx];

            for (int y2 = 1 + RoomSize; y2 >= y; y2--)
                if (_map[y2, x] != c)
                    return true;

            return false;
        }
        public void Log()
        {
            Console.WriteLine($"Energy cost: {Energy}");
            for (int y = 0; y < _map.GetLength(0); y++)
            {
                char[] line = new char[_map.GetLength(1)];
                for(int x = 0; x < _map.GetLength(1); x++)
                    line[x] = _map[y, x];
                Console.WriteLine(new string(line));
            }
            Console.WriteLine();
        }
    }

    public override void Solve()
    {
        AddSolution(new AmphipodDiagram(Inputs).Emulate());
        AddSolution(new AmphipodDiagram(Inputs, "  #D#C#B#A#", "  #D#B#A#C#").Emulate());
    }
}