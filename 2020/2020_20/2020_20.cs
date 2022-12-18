using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Tile
    {
        public int Idx { get; set; }
        public char[,] Data { get; set; }
        public char[,] TransformedData { get; set; }
        public string Edges { get; set; }
        public string TransformedEdge { get; set; }
        public string Left => (TransformedEdge ?? Edges).Substring(0, 10);
        public string Top => (TransformedEdge ?? Edges).Substring(10, 10);
        public string Right => (TransformedEdge ?? Edges).Substring(20, 10);
        public string Bottom => (TransformedEdge ?? Edges).Substring(30, 10);
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsPositioned { get; set; }
        public Tile(int idx, string[] data)
        {
            Idx = idx;
            Data = new char[10, 10];
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    Data[i, j] = data[i][j];
            Edges = string.Concat(new string(data.Select(s => s[0]).Reverse().ToArray()), data[0], new string(data.Select(s => s[9]).ToArray()), new string(data[9].Reverse().ToArray()));
        }
        public static string Rotate(string str, int cnt)
        {
            while (cnt < 0) cnt += 40;
            return str[cnt..] + str.Substring(0, cnt);
        }
        public static string Reverse(string str) => str is null ? null : new string(str.Reverse().ToArray());

        public bool CanFit(bool apply = false, string left = null, string top = null, string right = null, string bottom = null)
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
                    if (apply)
                    {
                        TransformedEdge = line;
                        ApplyChange(i, false, false);
                    }
                    return true;
                }
                // Flip h
                line = Rotate(Reverse(mem), 10);
                if (Fit(line, left, top, right, bottom))
                {
                    if (apply)
                    {
                        TransformedEdge = line;
                        ApplyChange(i, true, false);
                    }
                    return true;
                }
                // Flip w
                line = Rotate(Reverse(mem), -10);
                if (Fit(line, left, top, right, bottom))
                {
                    if (apply)
                    {
                        TransformedEdge = line;
                        ApplyChange(i, false, true);
                    }
                    return true;
                }

                mem = Rotate(mem, 10);
            }
            return false;
        }
        public void ApplyChange(int rot, bool flipH, bool flipW)
        {
            var tmpData = new char[10, 10];

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    tmpData[i, j] = Data[i, j];

            for (int i = 0; i < rot; i++)
                tmpData = ArrayHelper.Rotate(tmpData);

            if (flipH)
                TransformedData = ArrayHelper.FlipH(tmpData);
            else if (flipW)
                TransformedData = ArrayHelper.FlipW(tmpData);
            else
                TransformedData = tmpData;
        }
        private static bool Fit(string line, string left, string top, string right, string bottom)
        {
            return (left is null || left == line.Substring(0, 10))
                    && (top is null || top == line.Substring(10, 10))
                    && (right is null || right == line.Substring(20, 10))
                    && (bottom is null || bottom == line.Substring(30, 10));
        }
    }
    public static class ArrayHelper
    {
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
    }
    public class _2020_20 : Problem
    {
        #region Methods

        public override void Solve()
        {
            List<Tile> tiles = new List<Tile>();
            for (int i = 0; i < Inputs.Length; i += 12)
                tiles.Add(new Tile(int.Parse(Inputs[i].Substring(5, 4)), Inputs.Skip(i + 1).Take(10).ToArray()));

            int size = (int)Math.Sqrt(tiles.Count);

            var t1 = tiles.Where(t => !tiles.Any(t2 => t2 != t && t2.CanFit(right: t.Left)) && !tiles.Any(t2 => t2 != t && t2.CanFit(bottom: t.Top))).ToList();
            var t2 = tiles.Where(t => !tiles.Any(t2 => t2 != t && t2.CanFit(right: t.Left)) && !tiles.Any(t2 => t2 != t && t2.CanFit(top: t.Bottom))).ToList();
            var t3 = tiles.Where(t => !tiles.Any(t2 => t2 != t && t2.CanFit(left: t.Right)) && !tiles.Any(t2 => t2 != t && t2.CanFit(bottom: t.Top))).ToList();
            var t4 = tiles.Where(t => !tiles.Any(t2 => t2 != t && t2.CanFit(left: t.Right)) && !tiles.Any(t2 => t2 != t && t2.CanFit(top: t.Bottom))).ToList();

            var tile = tiles.First();
            tile.IsPositioned = true;
            tile.ApplyChange(0, false, false);
            char[,] img = new char[8 * size, 8 * size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (x == 0 && y == 0) ;
                    else
                    {
                        tile = tiles.First(t => !t.IsPositioned
                                                && t.CanFit(true,
                                                            tiles.FirstOrDefault(t2 => t2.IsPositioned && t2.X == x - 1 && t2.Y == y)?.Right,
                                                            tiles.FirstOrDefault(t2 => t2.IsPositioned && t2.X == x && t2.Y == y - 1)?.Bottom,
                                                            tiles.FirstOrDefault(t2 => t2.IsPositioned && t2.X == x + 1 && t2.Y == y)?.Left,
                                                            tiles.FirstOrDefault(t2 => t2.IsPositioned && t2.X == x && t2.Y == y + 1)?.Top));
                        tile.X = x;
                        tile.Y = y;
                        tile.IsPositioned = true;
                    }

                    for (int i = 0; i < 8; i++)
                        for (int j = 0; j < 8; j++)
                            img[y * 8 + i, x * 8 + j] = tile.TransformedData[i + 1, j + 1];

                }
            }

            long a = 1;
            a *= tiles.First(t => t.X == 0 && t.Y == 0).Idx;
            a *= tiles.First(t => t.X == 0 && t.Y == size - 1).Idx;
            a *= tiles.First(t => t.X == size - 1 && t.Y == 0).Idx;
            a *= tiles.First(t => t.X == size - 1 && t.Y == size - 1).Idx;
            Solutions.Add($"{a}");

            char[,] data = img;

            for(int i = 0; i <4; i++)
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

            int replaced = ReplaceTemplate(data);
            int cnt = 0;
            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                    if (data[i, j] == '#')
                        cnt++;
            Solutions.Add($"{cnt}");

        }
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
        private bool ContainsTemplate(char[,] data)
        {
            for (int x = 0; x < data.GetLength(1) - 19; x++)
                for (int y = 0; y < data.GetLength(0) - 2; y++)
                    if (Template.All(p => data[y + p.Y, x + p.X] == '#'))
                        return true;
            return false;
        }
        private int ReplaceTemplate(char[,] data)
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

        #endregion
    }
}