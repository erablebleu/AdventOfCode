namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/24
/// </summary>
public class _2016_24 : Problem
{
    private Dictionary<int, Node> _points = new();
    private bool[,] _walls;

    public override void Parse()
    {
        _walls = Inputs.Select(l => l.Select(c => c == '#')).To2DArray().Transpose();
        for (int x = 0; x < Inputs[0].Length; x++)
            for (int y = 0; y < Inputs.Length; y++)
                if (Inputs[y][x] >= '0' && Inputs[y][x] <= '9')
                    _points.Add(Inputs[y][x] - '0', new Node { Number = Inputs[y][x] - '0', Position = new IPoint2D(x, y) });

        foreach(Node node0 in _points.Values)
        {
            int[,] heatmap = GetHeatMap(_walls, node0.Position);
            foreach (Node node1 in _points.Values)
                node0.Distances[node1.Number] = heatmap[node1.Position.X, node1.Position.Y];
        }
    }

    public override object PartOne() => FindShortestPath();

    public override object PartTwo() => FindShortestPath(true);

    private static int[,] GetHeatMap(bool[,] walls, IPoint2D src)
    {
        int width = walls.GetLength(0);
        int height = walls.GetLength(1);
        int[,] result = new int[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                result[x, y] = int.MinValue;

        result[src.X, src.Y] = 0;
        int count;

        do
        {
            count = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (result[x, y] < 0 || walls[x, y])
                        continue;

                    foreach (IVector2D dir in IVector2D.DirectionNESW)
                    {
                        int x2 = x + dir.X;
                        int y2 = y + dir.Y;

                        if (!x2.IsInRange(0, width)
                            || !y2.IsInRange(0, height)
                            || walls[x2, y2]
                            || result[x2, y2] >= 0)
                            continue;

                        count++;
                        result[x2, y2] = result[x, y] + 1;
                    }
                }
            }
        }
        while (count > 0);

        return result;
    }

    private int FindShortestPath(bool withReturn = false)
    {
        // Look for permutation;
        int result = int.MaxValue;
        foreach (int[] perm in CombinatoryHelper.GetPermutations(_points.Values.Where(p => p.Number != 0).Select(p => p.Number).ToArray()))
        {
            int src = 0;
            int path = 0;
            foreach (int dst in perm)
            {
                path += _points[src].Distances[dst];
                src = dst;
            }
            if (withReturn)
                path += _points[src].Distances[0];

            result = Math.Min(result, path);
        }
        return result;
    }

    private class Node
    {
        public Dictionary<int, int> Distances = new();
        public int Number;
        public IPoint2D Position;
    }
}