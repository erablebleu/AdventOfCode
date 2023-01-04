namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/16
/// </summary>
public class _2022_16 : Problem
{
    private int _toOpen;
    private Valve[] _valves;

    public override void Parse()
    {
        Dictionary<string, Valve> valves = Inputs.Select(l => Parse(l)).ToDictionary(v => v.Name, v => v);
        _valves = valves.Values.Where(v => v.FlowRate > 0).Append(valves["AA"]).ToArray();

        foreach (Valve source in _valves)
        {
            foreach (Valve target in _valves)
            {
                if (target == source
                    || source.Distances.ContainsKey(Array.IndexOf(_valves, target)))
                    continue;

                List<string> dest = source.Access.ToList();
                int count = 1;
                while (!dest.Contains(target.Name))
                {
                    dest = dest.SelectMany(d => valves[d].Access).Distinct().ToList();
                    count++;
                }
                source.Distances[Array.IndexOf(_valves, target)] = count;
                target.Distances[Array.IndexOf(_valves, source)] = count;
            }
        }

        _toOpen = (int)Math.Pow(2, _valves.Length - 1) - 1;
    }

    public override object PartOne() => SearchPath(30, 0, _valves.Length - 1, _toOpen);

    public override object PartTwo() => SearchDualPath(26, 0, _valves.Length - 1, _valves.Length - 1, 0, 0, _toOpen);

    private static Valve Parse(string line)
    {
        string[] el = line.Replace(",", "").Split(' ');
        return new Valve
        {
            Name = el[1],
            FlowRate = int.Parse(el[4].Substring(5, el[4].Length - 6)),
            Access = el.Skip(9).ToArray(),
        };
    }

    private int SearchDualPath(int time, int pressure, int idx0, int idx1, int d0, int d1, int toOpen)
    {
        if (toOpen == 0 || time < 0) return pressure;

        int res = pressure;

        List<int> t0 = d0 != 0 ? new List<int>() : Enumerable.Range(0, _valves.Length - 1).Where(i => toOpen.GetBit(i) && i != idx1 && _valves[idx0].Distances[i] < time - 1).ToList();
        List<int> t1 = d1 != 0 ? new List<int>() : Enumerable.Range(0, _valves.Length - 1).Where(i => toOpen.GetBit(i) && i != idx0 && _valves[idx1].Distances[i] < time - 1).ToList();

        if (!t0.Any())
            t0.Add(idx0);
        if (!t1.Any())
            t1.Add(idx1);

        HashSet<int> set = new();

        foreach (int target0 in t0)
        {
            foreach (int target1 in t1)
            {
                int hash = target0.SetBit(target1);
                if (set.Contains(hash))
                    continue;
                set.Add(hash);

                if (target0 == idx0 && target1 == idx1)
                    return pressure;

                if (target0 == target1)
                    continue;

                int newP = pressure;

                int dist0 = d0;
                int dist1 = d1;

                int nToOpen = toOpen;

                if (target0 != idx0)
                {
                    dist0 = _valves[idx0].Distances[target0] + 1;
                    nToOpen = nToOpen.ResetBit(target0);
                    newP += (time - dist0) * _valves[target0].FlowRate;
                }
                if (target1 != idx1)
                {
                    dist1 = _valves[idx1].Distances[target1] + 1;
                    nToOpen = nToOpen.ResetBit(target1);
                    newP += (time - dist1) * _valves[target1].FlowRate;
                }

                int mind = Math.Min(dist0, dist1);

                res = Math.Max(res, SearchDualPath(time - mind, newP, target0, target1, dist0 - mind, dist1 - mind, nToOpen));
            }
        }

        return res;
    }

    private int SearchPath(int time, int pressure, int idx, int toOpen)
    {
        if (toOpen == 0 || time < 0) return pressure;

        int res = pressure;
        List<int> accessibleValves = Enumerable.Range(0, _valves.Length - 1).Where(i => toOpen.GetBit(i)).ToList();

        foreach (int idx1 in accessibleValves)
        {
            int d = _valves[idx].Distances[idx1];
            int nToOpen = toOpen.ResetBit(idx1);
            res = Math.Max(res, SearchPath(time - d - 1, pressure + (time - d - 1) * _valves[idx1].FlowRate, idx1, nToOpen));
        }
        return res;
    }

    private class Valve
    {
        public string[] Access = Array.Empty<string>();
        public Dictionary<int, int> Distances = new();
        public int FlowRate;
        public string Name;

        public override string ToString() => $"{Name} - {FlowRate} - {string.Join(",", Access)}";
    }
}