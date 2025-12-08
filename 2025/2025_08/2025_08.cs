using System.Collections;
using System.Collections.Concurrent;
using System.Globalization;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2025/day/08
/// </summary>
public class _2025_08 : Problem
{
    public record Dist(int Id0, int Id1, double Distance);

    private IPoint3D[] _boxes;
    private Dist[] _dists;

    public override void Parse()
    {
        _boxes = [..Inputs.Select((l, index) =>
        {
            int[] el = [..l.Split(",").Select(int.Parse)];
            return new IPoint3D(el[0], el[1], el[2]);
        })];

        List<Dist> tmpDists = [];

        for (int i = 0; i < _boxes.Length; i++)
            for (int j = i + 1; j < _boxes.Length; j++)
            {
                IPoint3D p0 = _boxes[i];
                IPoint3D p1 = _boxes[j];
                tmpDists.Add(new Dist(i, j, Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2) + Math.Pow(p1.Z - p0.Z, 2))));
            }

        _dists = [.. tmpDists];
    }

    public override object PartOne()
    {
        Queue<Dist> dists = new(_dists.OrderBy(x => x.Distance));
        List<List<int>> groups = [];

        for (int i = 0; i < 1000; i++)
        {
            Dist dist = dists.Dequeue();
            List<int>? g0 = groups.FirstOrDefault(g => g.Contains(dist.Id0));
            List<int>? g1 = groups.FirstOrDefault(g => g.Contains(dist.Id1));

            if (g0 is null && g1 is null)
                groups.Add([dist.Id0, dist.Id1]);
            else if (g0 is null)
                g1.Add(dist.Id0);
            else if (g1 is null)
                g0.Add(dist.Id1);
            else if (g0 != g1) // merge
            {
                g0.AddRange(g1);
                groups.Remove(g1);
            }
        }

        return groups.Select(gr => gr.Count).OrderDescending().Take(3).Product();
    }

    public override object PartTwo()
    {
        Queue<Dist> dists = new(_dists.OrderBy(x => x.Distance));
        List<List<int>> groups = [];

        while (dists.TryDequeue(out Dist dist))
        {
            List<int>? g0 = groups.FirstOrDefault(g => g.Contains(dist.Id0));
            List<int>? g1 = groups.FirstOrDefault(g => g.Contains(dist.Id1));

            if (g0 is null && g1 is null)
                groups.Add([dist.Id0, dist.Id1]);
            else if (g0 is null)
                g1.Add(dist.Id0);
            else if (g1 is null)
                g0.Add(dist.Id1);
            else if (g0 != g1) // merge
            {
                g0.AddRange(g1);
                groups.Remove(g1);
            }
            else continue;

            if (groups.Count == 1 && groups[0].Count == _boxes.Length)
            {
                IPoint3D p0 = _boxes[dist.Id0];
                IPoint3D p1 = _boxes[dist.Id1];

                return p0.X * p1.X;
            }
        }

        return null;
    }
}