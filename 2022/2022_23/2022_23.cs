using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml.Serialization;

namespace AdventOfCode;

public class _2022_23 : Problem
{
    private static List<KeyValuePair<IVector2D, IVector2D[]>> Directions = new()
        {
            new KeyValuePair<IVector2D, IVector2D[]>(new IVector2D(0, -1), new IVector2D[] { new IVector2D(-1, -1), new IVector2D(0, -1), new IVector2D(1, -1) }), // N
            new KeyValuePair<IVector2D, IVector2D[]>(new IVector2D(0, 1), new IVector2D[] { new IVector2D(-1, 1), new IVector2D(0, 1), new IVector2D(1, 1) }), // S
            new KeyValuePair<IVector2D, IVector2D[]>(new IVector2D(-1, 0), new IVector2D[] { new IVector2D(-1, -1), new IVector2D(-1, 0), new IVector2D(-1, 1) }), // W
            new KeyValuePair<IVector2D, IVector2D[]>(new IVector2D(1, 0), new IVector2D[] { new IVector2D(1, -1), new IVector2D(1, 0), new IVector2D(1, 1) }), // E
        };
    private class Map
    {
        private readonly int _xMin;
        private readonly int _xMax;
        private readonly int _yMin;
        private readonly int _yMax;

        private readonly bool[,] _map;
        private readonly int[,] _targets;

        public bool this[int x, int y] => _map[x - _xMin, y - _yMin];
        public bool this[IPoint2D p] => this[p.X, p.Y];

        public void AddTarget(IPoint2D p)
        {
            _targets[p.X - _xMin, p.Y - _yMin]++;
        }
        public int GetTarget(IPoint2D p) => _targets[p.X - _xMin, p.Y - _yMin]++;

        public Map(List<Elf> elves)
        {
            _xMin = int.MaxValue;
            _yMin = int.MaxValue;
            _xMax = int.MinValue;
            _yMax = int.MinValue;

            foreach (Elf e in elves)
            {
                _xMin = Math.Min(_xMin, e.Position.X);
                _xMax = Math.Max(_xMax, e.Position.X);
                _yMin = Math.Min(_yMin, e.Position.Y);
                _yMax = Math.Max(_yMax, e.Position.Y);
            }
            // Add 1 to all direction to allow elves moves
            _xMin--;
            _yMin--;
            _xMax++;
            _yMax++;

            _map = new bool[_xMax - _xMin + 1, _yMax - _yMin + 1];
            _targets = new int[_xMax - _xMin + 1, _yMax - _yMin + 1];

            foreach (Elf e in elves)
                _map[e.Position.X - _xMin, e.Position.Y - _yMin] = true;
        }

        public void Log()
        {
            for (int y = _yMin; y < _yMax + 1; y++)
            {
                string line = string.Empty;
                for (int x = _xMin; x < _xMax + 1; x++)
                    line += this[x, y] ? "#" : ".";

                Console.WriteLine(line);
            }
        }
        public int CountEmptyTiles()
        {
            int count = 0;
            for(int x = _xMin + 1; x < _xMax; x++)
                for (int y = _yMin + 1; y < _yMax; y++)
                    if (!this[x, y]) count++;
            return count;
        }
    }

    private class Elf
    {
        public IPoint2D Position { get; set; }
        public IPoint2D Target { get; set; }

        public Elf(int x, int y)
        {
            Position = new IPoint2D(x, y);
        }

        public void SearchDirection(Map map)
        {
            KeyValuePair<IVector2D, IVector2D[]>? dir = null;
            Target = Position;
            int count = 0;

            foreach(KeyValuePair<IVector2D, IVector2D[]> kv in Directions)
            {
                int dirCount = kv.Value.Count(d => map[Position + d]);
                count += dirCount;
                if (dirCount > 0 || dir is not null)
                    continue;
                dir = kv;
            }

            // No direction accessible -> No move
            if (dir is null)
                return;
            // No elf arround -> No move
            if (count == 0)
            {
                dir = null;
                return;
            }

            // Save Target and add to the map
            Target = Position + dir.Value.Key;
            map.AddTarget(Target);

            return;
        }

        public bool TryMove(Map map)
        {
            if (Target == Position
                || map.GetTarget(Target) > 1)
                return false;

            Position = Target;
            return true;
        }
    }
    public override void Solve()
    {
        List<Elf> elves = new();

        for (int y = 0; y < Inputs.Length; y++)
            for(int x = 0; x < Inputs[y].Length; x++)
                if (Inputs[y][x] == '#')
                    elves.Add(new Elf(x, y));

        int moveCount = 0;
        int round = 0;
        do
        {
            // Use a map to store all position
            Map map = new(elves);

            foreach (Elf elf in elves)
                elf.SearchDirection(map);

            moveCount = elves.Count(e => e.TryMove(map));

            KeyValuePair<IVector2D, IVector2D[]> kv = Directions.First();
            Directions.Remove(kv);
            Directions.Add(kv);

            /*Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"round: {round}");
            map.Log();*/

            if (round == 10)
                AddSolution(map.CountEmptyTiles());

            round++;
        } while (moveCount > 0);

        AddSolution(round);
    }
}