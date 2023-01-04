namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/24
/// </summary>
public class _2020_24 : Problem
{
    private HashSet<Tile> _data;

    public override void Parse()
    {
        _data = new HashSet<Tile>();

        foreach (var line in Inputs)
        {
            string str = line.Replace("nw", "1")
                             .Replace("ne", "2")
                             .Replace("se", "4")
                             .Replace("sw", "5");
            Tile tile = new();
            foreach (var c in str)
            {
                switch (c)
                {
                    case '0':
                    case 'w':
                        tile.X--;
                        tile.Y++;
                        break;

                    case '1':
                        tile.Y++;
                        tile.Z--;
                        break;

                    case '2':
                        tile.X++;
                        tile.Z--;
                        break;

                    case '3':
                    case 'e':
                        tile.X++;
                        tile.Y--;
                        break;

                    case '4':
                        tile.Y--;
                        tile.Z++;
                        break;

                    case '5':
                        tile.X--;
                        tile.Z++;
                        break;
                }
            }

            if (_data.Any(t => t.X == tile.X && t.Y == tile.Y && t.Z == tile.Z))
                tile = _data.First(t => t.X == tile.X && t.Y == tile.Y && t.Z == tile.Z);
            else
                _data.Add(tile);
            tile.IsBlack = !tile.IsBlack;
        }
    }

    public override object PartOne() => _data.Count(t => t.IsBlack);

    public override object PartTwo()
    {
        _data = _data.Where(t => t.IsBlack).ToHashSet();

        Tile min = new Tile { X = _data.Min(t => t.X), Y = _data.Min(t => t.Y), Z = _data.Min(t => t.Z) };
        Tile max = new Tile { X = _data.Max(t => t.X), Y = _data.Max(t => t.Y), Z = _data.Max(t => t.Z) };

        Dictionary<int, Dictionary<int, Dictionary<int, Tile>>> ls = new();
        foreach (var tile in _data) ls.Add(tile.X, tile.Y, tile.Z);

        for (int d = 0; d < 100; d++)
        {
            Dictionary<int, Dictionary<int, Dictionary<int, Tile>>> tmpList = new();

            Tile tmpMin = new Tile { X = int.MaxValue, Y = int.MaxValue, Z = int.MaxValue };
            Tile tmpMax = new Tile { X = int.MinValue, Y = int.MinValue, Z = int.MinValue };

            for (int x = min.X - 1; x <= max.X + 1; x++)
                for (int y = min.Y - 1; y <= max.Y + 1; y++)
                    for (int z = min.Z - 1; z <= max.Z + 1; z++)
                    {
                        bool exist = ls.Contains(x, y, z);
                        int cnt = Tile.CountAdj(x, y, z, ls);

                        if (exist && cnt > 0 && cnt <= 2
                            || !exist && cnt == 2)
                        {
                            tmpList.Add(x, y, z);
                            tmpMin.X = Math.Min(tmpMin.X, x);
                            tmpMin.Y = Math.Min(tmpMin.Y, y);
                            tmpMin.Z = Math.Min(tmpMin.Z, z);
                            tmpMax.X = Math.Max(tmpMax.X, x);
                            tmpMax.Y = Math.Max(tmpMax.Y, y);
                            tmpMax.Z = Math.Max(tmpMax.Z, z);
                        }
                    }

            //Console.WriteLine($"Day {d}: {ls.CountRecursive()}");

            ls = tmpList;
            min = tmpMin;
            max = tmpMax;
        }

        return ls.CountRecursive();
    }

    private class Tile
    {
        public bool IsBlack { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public static int CountAdj(int x, int y, int z, Dictionary<int, Dictionary<int, Dictionary<int, Tile>>> _data)
        {
            int cnt = 0;
            for (int dx = -1; dx < 2; dx++)
                for (int dy = -1; dy < 2; dy++)
                    for (int dz = -1; dz < 2; dz++)
                        if (dx + dy + dz != 0 || (dx == 0 && dy == 0 && dz == 0)) continue;
                        else if (_data.Contains(x + dx, y + dy, z + dz))
                            cnt++;
            return cnt;
        }

        public int CountAdj(Dictionary<int, Dictionary<int, Dictionary<int, Tile>>> _data) => CountAdj(X, Y, Z, _data);

        public override bool Equals(object obj)
        {
            if (obj is not Tile t) return false;
            return t.X == X && t.Y == Y && t.Z == Z;
        }
    }
}