namespace AdventOfCode;

public class _2021_25 : Problem
{
    private static Dictionary<char, IPoint2D> Directions = new Dictionary<char, IPoint2D>()
    {
        { '>', new IPoint2D(1, 0) },
        { 'v', new IPoint2D(0, 1) },
    };
    public override void Solve()
    {
        char[,] map = Inputs.Select(l => l.ToArray()).To2DArray();
        int moveCount = 0;
        int stepCount = 0;        

        do
        {
            moveCount = 0;
            foreach (KeyValuePair<char, IPoint2D> direction in Directions)
            {
                char[,] nmap = map.ToArray();
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    for (int y = 0; y < map.GetLength(0); y++)
                    {
                        if (map[y, x] != direction.Key)
                            continue;

                        int x2 = x + direction.Value.X;
                        int y2 = y + direction.Value.Y;
                        if (x2 >= map.GetLength(1)) x2 = 0;
                        if (y2 >= map.GetLength(0)) y2 = 0;

                        if (map[y2, x2] != '.')
                            continue;

                        nmap[y2, x2] = direction.Key;
                        nmap[y, x] = '.';
                        moveCount++;
                    }
                }
                map = nmap;
            }
            stepCount++;
        }
        while (moveCount > 0);

        AddSolution(stepCount);
    }
}