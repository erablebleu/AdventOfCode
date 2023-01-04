namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2019/day/06
/// </summary>
public class _2019_06 : Problem
{
    private Dictionary<string, Orbit> _orbits = new();

    public override void Parse()
    {
        foreach (string line in Inputs)
        {
            string[] el = line.Split(')');
            var a = _orbits.GetOrAdd(el[0], () => new Orbit(el[0]));
            var b = _orbits.GetOrAdd(el[1], () => new Orbit(el[1]));
            b.Orbit2 = a;
        }
    }

    public override object PartOne() => CountOrbits().ToString();

    public override object PartTwo() => GetDistance("YOU", "SAN");

    private int CountOrbits()
    {
        int cnt = 0;
        foreach (var orbit in _orbits.Values)
        {
            Orbit orb = orbit;
            while (orb.Orbit2 != null)
            {
                orb = orb.Orbit2;
                cnt++;
            }
        }
        return cnt;
    }

    private int GetDistance(string a, string b)
    {
        List<Orbit> pathA = GetPathToCom(a);
        List<Orbit> pathB = GetPathToCom(b);

        for (int i = 0; i < pathA.Count; i++)
            for (int j = 0; j < pathB.Count; j++)
                if (pathA[i].Name == pathB[j].Name)
                    return i + j;
        return int.MaxValue;
    }

    private List<Orbit> GetPathToCom(string name)
    {
        List<Orbit> path = new List<Orbit>();
        var orb = _orbits[name].Orbit2;

        while (orb.Orbit2 != null)
        {
            path.Add(orb);
            orb = orb.Orbit2;
        }
        return path;
    }

    private class Orbit
    {
        public string Name;

        public Orbit Orbit2;

        public Orbit(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }
}