namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/22
/// </summary>
public class _2016_22 : Problem
{
    private Node[] _data;

    public override void Parse()
    {
        _data = Inputs.Skip(2).Select(l => Parse(l)).ToArray();
    }

    public override object PartOne() => _data.Sum(n0 => _data.Count(n1 => n0.Used > 0 && n0 != n1 && n1.Available > n0.Used));

    public override object PartTwo()
    {
        int width = _data.Max(n => n.X) + 1;
        int height = _data.Max(n => n.Y) + 1;
        int[,] capacity = new int[width, height];
        int[,] initialState = new int[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                Node node = _data.First(n => n.X == x && n.Y == y);
                capacity[x, y] = node.Size;
                initialState[x, y] = node.Used;
            }

        Node emptyNode = _data.First(n => n.Used == 0);
        IPoint2D empty = new(emptyNode.X, emptyNode.Y);
        IPoint2D source = new(width - 1, 0);
        IPoint2D target = new(0, 0);
        //Log(target, initialState);
        int result = Move(empty, initialState, source);

        return result + (source.X - 1) * 5;
    }

    private static void Log(IPoint2D p, int[,] grid)
    {
        Console.WriteLine();
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                char c = x == 0 && y == 0 ? 'G' : grid[x, y] > 100 ? '#' : grid[x, y] > 0 ? '.' : '_';
                Console.Write(p.X == x && p.Y == y ? $"({c})" : $" {c} ");
            }
            Console.WriteLine();
        }
    }

    private static int Move(IPoint2D source, int[,] grid, IPoint2D target)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);
        HashSet<int> set = new();
        List<IPoint2D> list = new() { source };
        int result = 0;

        while (list.Any())
        {
            List<IPoint2D> next = new();
            result++;

            foreach (IPoint2D position in list)
            {
                foreach (IVector2D dir in IVector2D.DirectionNESW)
                {
                    IPoint2D p2 = position + dir;
                    if (!p2.X.IsInRange(0, width)
                        || !p2.Y.IsInRange(0, height)
                        || grid[p2.X, p2.Y] > 100)
                        continue;

                    if (p2 == target)
                        return result;
                    int hash = p2.X << 16 | p2.Y;

                    if (set.Contains(hash))
                        continue;

                    set.Add(hash);
                    next.Add(p2);
                }
            }

            list = next;
        }

        return 0;
    }

    private static Node Parse(string line)
    {
        string[] el = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int[] coord = el[0].ParseExact("/dev/grid/node-x{0}-y{1}").Select(e => int.Parse(e)).ToArray();

        return new Node
        {
            X = coord[0],
            Y = coord[1],
            Size = int.Parse(el[1].Substring(0, el[1].Length - 1)),
            Used = int.Parse(el[2].Substring(0, el[2].Length - 1)),
        };
    }

    internal class Node
    {
        public int Size;
        public int Used;
        public int X;
        public int Y;
        public int Available => Size - Used;
    }
}