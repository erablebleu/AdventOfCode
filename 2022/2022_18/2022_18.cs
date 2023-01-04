namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/18
/// </summary>
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

    private bool[,,] _grid;
    private IPoint3D[] _points;

    public override void Parse()
    {
        _points = Inputs.Select(l => new IPoint3D(l.Split(",").Select(el => int.Parse(el)).ToArray())).ToArray();
        _grid = new bool[_points.Max(p => p.X) + 1, _points.Max(p => p.Y) + 1, _points.Max(p => p.Z) + 1];

        foreach (IPoint3D p in _points)
            _grid[p.X, p.Y, p.Z] = true;
    }

    public override object PartOne()
    {
        int exposedFaces = 0;
        foreach (IPoint3D p in _points)
        {
            foreach (IVector3D d in Directions)
            {
                IPoint3D p2 = p + d;
                if (p2.X >= 0 && p2.X < _grid.GetLength(0)
                    && p2.Y >= 0 && p2.Y < _grid.GetLength(1)
                    && p2.Z >= 0 && p2.Z < _grid.GetLength(2)
                    && _grid[p2.X, p2.Y, p2.Z])
                    continue;
                exposedFaces++;
            }
        }

        return exposedFaces;
    }

    public override object PartTwo()
    {
        bool[,,] airGrid = new bool[_grid.GetLength(0), _grid.GetLength(1), _grid.GetLength(2)];
        int addAirCnt = 0;
        int exposedFaces = 0;
        do
        {
            addAirCnt = 0;
            for (int x = 0; x < _grid.GetLength(0); x++)
            {
                for (int y = 0; y < _grid.GetLength(1); y++)
                {
                    for (int z = 0; z < _grid.GetLength(2); z++)
                    {
                        if (_grid[x, y, z] || airGrid[x, y, z])
                            continue;

                        IPoint3D p = new(x, y, z);
                        foreach (IVector3D d in Directions)
                        {
                            IPoint3D p2 = p + d;
                            if (p2.X >= 0 && p2.X < _grid.GetLength(0)
                                && p2.Y >= 0 && p2.Y < _grid.GetLength(1)
                                && p2.Z >= 0 && p2.Z < _grid.GetLength(2)
                                && (_grid[p2.X, p2.Y, p2.Z] || !airGrid[p2.X, p2.Y, p2.Z]))
                                continue;
                            addAirCnt++;
                            airGrid[x, y, z] = true;
                            break;
                        }
                    }
                }
            }
        }
        while (addAirCnt > 0);

        foreach (IPoint3D p in _points)
        {
            foreach (IVector3D d in Directions)
            {
                IPoint3D p2 = p + d;
                if (p2.X >= 0 && p2.X < _grid.GetLength(0)
                    && p2.Y >= 0 && p2.Y < _grid.GetLength(1)
                    && p2.Z >= 0 && p2.Z < _grid.GetLength(2)
                    && (_grid[p2.X, p2.Y, p2.Z] || !airGrid[p2.X, p2.Y, p2.Z]))
                    continue;
                exposedFaces++;
            }
        }

        return exposedFaces;
    }
}