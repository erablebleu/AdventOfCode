namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/10
/// </summary>
public class _2019_10 : Problem
{
    private List<Asteroid> _asts;

    public override void Parse()
    {
        int width = Inputs[0].Length;
        int height = Inputs.Length;
        bool[][] map = new bool[height][];
        int[][] eye = new int[height][];
        _asts = new List<Asteroid>();

        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
                if (Inputs[i][j] == '#')
                    _asts.Add(new Asteroid(j, i));

        for (int i = 0; i < _asts.Count; i++)
        {
            for (int j = 0; j < _asts.Count; j++)
            {
                _asts[i].AddAst(_asts[j]);
            }
        }

        _asts = _asts.OrderByDescending(a => a.Angles.Count).ToList();
    }

    public override object PartOne() => _asts.First().Angles.Count();

    public override object PartTwo()
    {
        Asteroid ast = _asts.First().GetNth(200);
        return ast.X * 100 + ast.Y;
    }

    private class Asteroid
    {
        public Asteroid(int x, int y)
        {
            X = x;
            Y = y;
            Angles = new SortedDictionary<double, List<Asteroid>>();
        }

        public SortedDictionary<double, List<Asteroid>> Angles { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void AddAst(Asteroid ast)
        {
            if (X == ast.X && Y == ast.Y)
                return;

            double angle = Math.Atan2(ast.X - X, ast.Y - Y);
            if (Angles.ContainsKey(angle))
                Angles[angle].Add(ast);
            else
                Angles.Add(angle, new List<Asteroid>() { ast });
        }

        public Asteroid GetNth(int n)
        {
            int cnt = 0;
            Asteroid ast = null;

            while (cnt < n)
            {
                for (int i = Angles.Count - 1; i >= 0 && cnt < n; i--)
                {
                    cnt++;
                    var kv = Angles.ElementAt(i);
                    ast = kv.Value.First();
                    kv.Value.Remove(ast);
                    if (kv.Value.Count <= 0)
                        Angles.Remove(kv.Key);
                }
            }

            return ast;
        }

        public override string ToString() => $"X:{X} Y:{Y} Cnt:{Angles.Count}";
    }
}