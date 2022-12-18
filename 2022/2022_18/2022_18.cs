using AdventOfCode.Tools;

namespace AdventOfCode;

public class _2022_18 : Problem
{
    private static IVector3D[] Directions = new IVector3D[]
    {
        new IVector3D(1, 0, 0),
        new IVector3D(-1, 0, 0),
        new IVector3D(0, 1, 0),
        new IVector3D(0, -1, 0),
        new IVector3D(0, 0, 1),
        new IVector3D(0, 0, -1),
    };
    public override void Solve()
    {
        IPoint3D[] points = Inputs.Select(l => new IPoint3D(l.Split(",").Select(el => int.Parse(el)).ToArray())).ToArray();
        bool[,,] grid = new bool[points.Max(p => p.X) + 1, points.Max(p => p.Y) + 1, points.Max(p => p.Z) + 1];

        foreach(IPoint3D p in points)
            grid[p.X, p.Y, p.Z] = true;

        int exposedFaces = 0;
        foreach (IPoint3D p in points)
        {
            foreach(IVector3D d in Directions)
            {
                IPoint3D p2 = p + d;
                if (p2.X >= 0 && p2.X < grid.GetLength(0)
                    && p2.Y >= 0 && p2.Y < grid.GetLength(1)
                    && p2.Z >= 0 && p2.Z < grid.GetLength(2)
                    && grid[p2.X, p2.Y, p2.Z])
                    continue;
                exposedFaces++;
            }
        }

        AddSolution(exposedFaces);

        bool[,,] airGrid = new bool[grid.GetLength(0), grid.GetLength(1), grid.GetLength(2)];
        int addAirCnt = 0;
        exposedFaces = 0;
        do
        {
            addAirCnt = 0;
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for(int y = 0; y < grid.GetLength(1); y++)
                {
                    for(int z = 0; z < grid.GetLength(2); z++)
                    {
                        if (grid[x, y, z] || airGrid[x, y, z])
                            continue;

                        IPoint3D p = new(x, y, z);
                        foreach (IVector3D d in Directions)
                        {
                            IPoint3D p2 = p + d;
                            if (p2.X >= 0 && p2.X < grid.GetLength(0)
                                && p2.Y >= 0 && p2.Y < grid.GetLength(1)
                                && p2.Z >= 0 && p2.Z < grid.GetLength(2)
                                && (grid[p2.X, p2.Y, p2.Z] || !airGrid[p2.X, p2.Y, p2.Z]))
                                continue;
                            addAirCnt++;
                            airGrid[x, y, z] = true;
                            break;
                        }
                    }
                }
            }
        }
        while(addAirCnt > 0);

        foreach (IPoint3D p in points)
        {
            foreach (IVector3D d in Directions)
            {
                IPoint3D p2 = p + d;
                if (p2.X >= 0 && p2.X < grid.GetLength(0)
                    && p2.Y >= 0 && p2.Y < grid.GetLength(1)
                    && p2.Z >= 0 && p2.Z < grid.GetLength(2)
                    && (grid[p2.X, p2.Y, p2.Z] || !airGrid[p2.X, p2.Y, p2.Z]))
                    continue;
                exposedFaces++;
            }
        }

        AddSolution(exposedFaces);
    }
}