namespace AdventOfCode;

public static class ArrayHelper
{
    public static char[,] FlipH(char[,] data)
    {
        char[,] res = new char[data.GetLength(0), data.GetLength(1)];
        for (int i = 0; i < data.GetLength(0); i++)
            for (int j = 0; j < data.GetLength(1); j++)
                res[i, data.GetLength(1) - 1 - j] = data[i, j];
        return res;
    }

    public static char[,] FlipW(char[,] data)
    {
        char[,] res = new char[data.GetLength(0), data.GetLength(1)];
        for (int i = 0; i < data.GetLength(0); i++)
            for (int j = 0; j < data.GetLength(1); j++)
                res[data.GetLength(0) - 1 - i, j] = data[i, j];
        return res;
    }

    public static char[,] Rotate(char[,] data)
    {
        char[,] res = new char[data.GetLength(1), data.GetLength(0)];
        int newColumn, newRow = 0;
        for (int oldColumn = data.GetLength(1) - 1; oldColumn >= 0; oldColumn--)
        {
            newColumn = 0;
            for (int oldRow = 0; oldRow < data.GetLength(0); oldRow++)
            {
                res[newRow, newColumn] = data[oldRow, oldColumn];
                newColumn++;
            }
            newRow++;
        }
        return res;
    }
}

/// <summary>
/// https://adventofcode.com/2020/day/20
/// </summary>
public class _2020_20 : Problem
{
    public static List<System.Drawing.Point> Template = new List<System.Drawing.Point>
        {
            new System.Drawing.Point(18, 0),
            new System.Drawing.Point(0, 1),
            new System.Drawing.Point(5, 1),
            new System.Drawing.Point(6, 1),
            new System.Drawing.Point(11, 1),
            new System.Drawing.Point(12, 1),
            new System.Drawing.Point(17, 1),
            new System.Drawing.Point(18, 1),
            new System.Drawing.Point(19, 1),
            new System.Drawing.Point(1, 2),
            new System.Drawing.Point(4, 2),
            new System.Drawing.Point(7, 2),
            new System.Drawing.Point(10, 2),
            new System.Drawing.Point(13, 2),
            new System.Drawing.Point(16, 2),
        };

    private List<Tile> _tiles;

    public override void Parse()
    {
        _tiles = new List<Tile>();
        for (int i = 0; i < Inputs.Length; i += 12)
            _tiles.Add(new Tile(int.Parse(Inputs[i].Substring(5, 4)), Inputs.Skip(i + 1).Take(10).ToArray()));

        _tiles.First().Place(new IPoint2D(), 0, false, false);

        while (_tiles.Any(t => !t.IsPositioned))
        {
            foreach (Tile tile in _tiles.Where(t => t.IsPositioned))
            {
                foreach (IVector2D direction in IVector2D.DirectionNSEW)
                {
                    IPoint2D p = tile.Position + direction;

                    if (_tiles.Any(t => t.IsPositioned && t.Position == p))
                        continue;

                    List<(Tile, (int, bool, bool)?)> tiles = 
                        _tiles.Where(t => !t.IsPositioned).Select(t => (t, t.CanFit(_tiles.FirstOrDefault(t2 => t2.IsPositioned && t2.Position.X == p.X - 1 && t2.Position.Y == p.Y)?.Right,
                                                        _tiles.FirstOrDefault(t2 => t2.IsPositioned && t2.Position.X == p.X && t2.Position.Y == p.Y - 1)?.Bottom,
                                                        _tiles.FirstOrDefault(t2 => t2.IsPositioned && t2.Position.X == p.X + 1 && t2.Position.Y == p.Y)?.Left,
                                                        _tiles.FirstOrDefault(t2 => t2.IsPositioned && t2.Position.X == p.X && t2.Position.Y == p.Y + 1)?.Top)))
                        .Where(s => s.Item2 is not null).ToList();

                    if (tiles.Count != 1)
                        continue;

                    (int rot, bool flipH, bool flipW) = tiles.First().Item2.Value;
                    tiles.First().Item1.Place(p, rot, flipH, flipW);
                }
            }
        }
    }

    public override object PartOne()
    {
        long a = 1;
        int minX = _tiles.Min(t => t.Position.X);
        int minY = _tiles.Min(t => t.Position.Y);
        int maxX = _tiles.Max(t => t.Position.X);
        int maxY = _tiles.Max(t => t.Position.Y);

        return _tiles.Where(t => t.Position.X == minX && t.Position.Y == minY
                              || t.Position.X == minX && t.Position.Y == maxY
                              || t.Position.X == maxX && t.Position.Y == minY
                              || t.Position.X == maxX && t.Position.Y == maxY).Select(t => t.Idx).Product();
    }

    public override object PartTwo()
    {
        int minX = _tiles.Min(t => t.Position.X);
        int minY = _tiles.Min(t => t.Position.Y);
        int maxX = _tiles.Max(t => t.Position.X);
        int maxY = _tiles.Max(t => t.Position.Y);
        char[,] data = new char[(maxX - minX + 1) * 8, (maxY - minY + 1) * 8];

        foreach (Tile tile in _tiles)
        {
            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    data[8 * (tile.Position.Y - minY) + y, 8 * (tile.Position.X - minX) + x] = tile.TransformedData[y + 1, x + 1];
        }

        string[] pattern = new string[] {
            "                  # ",
            "#    ##    ##    ###",
            " #  #  #  #  #  #   ",
        };

        for (int i = 0; i < 4; i++)
        {
            if (ContainsTemplate(data))
                break;
            var d2 = ArrayHelper.FlipH(data);
            if (ContainsTemplate(d2))
            {
                data = d2;
                break;
            }
            d2 = ArrayHelper.FlipW(data);
            if (ContainsTemplate(d2))
            {
                data = d2;
                break;
            }

            data = ArrayHelper.Rotate(data);
        }

        _ = ReplaceTemplate(data);
        int cnt = 0;
        for (int i = 0; i < data.GetLength(0); i++)
            for (int j = 0; j < data.GetLength(1); j++)
                if (data[i, j] == '#')
                    cnt++;
        return cnt;
    }

    private static bool ContainsTemplate(char[,] data)
    {
        for (int x = 0; x < data.GetLength(1) - 19; x++)
            for (int y = 0; y < data.GetLength(0) - 2; y++)
                if (Template.All(p => data[y + p.Y, x + p.X] == '#'))
                    return true;
        return false;
    }

    private static int ReplaceTemplate(char[,] data)
    {
        int cnt = 0;
        for (int x = 0; x < data.GetLength(1) - 19; x++)
            for (int y = 0; y < data.GetLength(0) - 2; y++)
                if (Template.All(p => data[y + p.Y, x + p.X] == '#' || data[y + p.Y, x + p.X] == 'O'))
                {
                    foreach (var p in Template)
                        data[y + p.Y, x + p.X] = 'O';
                    cnt++;
                }
        return cnt;
    }

    private class Tile
    {
        public IPoint2D Position;

        public Tile(int idx, string[] data)
        {
            Idx = idx;
            Data = new char[10, 10];
            TransformedData = Data;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    Data[i, j] = data[i][j];
            Edges = string.Concat(new string(data.Select(s => s[0]).Reverse().ToArray()), data[0], new string(data.Select(s => s[9]).ToArray()), new string(data[9].Reverse().ToArray()));
        }

        public string Bottom => (TransformedEdge ?? Edges).Substring(30, 10);
        public char[,] Data { get; set; }
        public string Edges { get; set; }
        public int Idx { get; set; }
        public bool IsPositioned { get; set; }
        public string Left => (TransformedEdge ?? Edges).Substring(0, 10);
        public string Right => (TransformedEdge ?? Edges).Substring(20, 10);
        public string Top => (TransformedEdge ?? Edges).Substring(10, 10);
        public char[,] TransformedData { get; set; }
        public string TransformedEdge { get; set; }

        public static string Reverse(string str) => str is null ? null : new string(str.Reverse().ToArray());

        public static string Rotate(string str, int cnt)
        {
            while (cnt < 0) cnt += 40;
            return str[cnt..] + str.Substring(0, cnt);
        }

        public void Place(IPoint2D position, int rot, bool flipH, bool flipW)
        {
            IsPositioned = true;
            Position = position;

            var tmpData = Data.ToArray();

            for (int i = 0; i < rot; i++)
                tmpData = ArrayHelper.Rotate(tmpData);

            TransformedData = flipH ? ArrayHelper.FlipH(tmpData) : flipW ? ArrayHelper.FlipW(tmpData) : tmpData;
        }

        public (int Rotations, bool FlipH, bool FlipW)? CanFit(string left = null, string top = null, string right = null, string bottom = null)
        {
            left = Reverse(left);
            top = Reverse(top);
            right = Reverse(right);
            bottom = Reverse(bottom);
            string mem = Edges;
            for (int i = 0; i < 4; i++)
            {
                string line = mem;
                if (Fit(line, left, top, right, bottom))
                {
                    TransformedEdge = line;
                    return (i, false, false);
                }
                // Flip h
                line = Rotate(Reverse(mem), 10);
                if (Fit(line, left, top, right, bottom))
                {
                    TransformedEdge = line;
                    return (i, true, false);
                }
                // Flip w
                line = Rotate(Reverse(mem), -10);
                if (Fit(line, left, top, right, bottom))
                {
                    TransformedEdge = line;
                    return (i, false, true);
                }

                mem = Rotate(mem, 10);
            }
            return null;
        }

        private static bool Fit(string line, string left, string top, string right, string bottom)
        {
            return (left is null || left == line.Substring(0, 10))
                    && (top is null || top == line.Substring(10, 10))
                    && (right is null || right == line.Substring(20, 10))
                    && (bottom is null || bottom == line.Substring(30, 10));
        }
    }
}