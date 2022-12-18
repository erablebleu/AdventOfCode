namespace AdventOfCode;

public class _2022_16 : Problem
{
    public class Valve
    {
        public string Name { get; set; }
        public int FlowRate { get; set; }
        public string[] Access { get; set; }
        public Valve(string value)
        {
            string[] el = value.Split(" ");
            Name = el[1];
            FlowRate = int.Parse(el[4].Replace("rate=", "").Replace(";", ""));
            Access = el.Skip(9).Select(l => l.Replace(",", "")).ToArray();
        }
        public override string ToString() => $"{Name} - {FlowRate} - {string.Join(",", Access)}";
    }
    private Dictionary<string, Valve> _valves;
    private Dictionary<string, Dictionary<string, int>> _distances;
    public override void Solve()
    {
        _valves = Inputs.Select(l => new Valve(l)).ToDictionary(v => v.Name, valves => valves);
        _distances = new Dictionary<string, Dictionary<string, int>>();
        foreach(string source in _valves.Keys)
        {
            Dictionary<string, int> map = new();
            foreach (string target in _valves.Keys)
            {
                if(target == source)
                {
                    map[target] = 0;
                    continue;
                }
                List<string> dest = _valves[source].Access.ToList();
                int count = 1;
                while (!dest.Contains(target))
                {
                    dest = dest.SelectMany(d => _valves[d].Access).Distinct().ToList();
                    count++;
                }
                map[target] = count;
            }
            _distances.Add(source, map);
        }

        AddSolution($"{SearchPath(30, 0, _valves["AA"], new List<string>())}");
        AddSolution($"{SearchDualPath(26, 0, _valves["AA"], _valves["AA"], 0, 0, new List<string>())}");

        //Solutions.Add($"{combinations.Max(c => EmulateDual(c))}");
    }

    public int SearchPath(int time, int pressure, Valve valve, List<string> opened)
    {
        int res = pressure;
        List<Valve> accessibleValves = _valves.Values.Where(v => !opened.Contains(v.Name) && v.FlowRate > 0 && _distances[valve.Name][v.Name] < time).ToList();

        if (!accessibleValves.Any())
        {
            res += time * opened.Sum(v => _valves[v].FlowRate);
        }
        else
        {
            foreach (Valve v2 in accessibleValves)
            {
                int d = _distances[valve.Name][v2.Name];
                List<string> o2 = opened.ToList();
                o2.Add(v2.Name);
                res = Math.Max(res, SearchPath(time - d - 1, pressure + (d + 1) * opened.Sum(v => _valves[v].FlowRate), v2, o2));
            }
        }
        return res;
    }

    public int SearchDualPath(int time, int pressure, Valve v0, Valve v1, int d0, int d1, List<string> opened)
    {
        int res = pressure;

        if (time <= 0)
        {
            //Console.WriteLine($"{res} - {string.Join(", ", opened)}");
            return pressure;
        }

        List<Valve> t0 = d0 > 0 ? new() : _valves.Values.Where(v => !opened.Contains(v.Name) && v.FlowRate > 0 && v != v1 && _distances[v0.Name][v.Name] < time - 1).ToList();
        List<Valve> t1 = d1 > 0 ? new() : _valves.Values.Where(v => !opened.Contains(v.Name) && v.FlowRate > 0 && v != v0 && _distances[v1.Name][v.Name] < time - 1).ToList();

        if(!t0.Any() && !t1.Any())
            return pressure;

        if (!t0.Any())
            t0.Add(v0);
        if (!t1.Any())
            t1.Add(v1);

        foreach (Valve target0 in t0)
        {
            foreach(Valve target1 in t1)
            {
                if (target0 == target1)
                    continue;

                int newP = pressure;

                int dist0 = d0;
                int dist1 = d1;

                List<string> o2 = opened.ToList();
                if (target0 != v0)
                {
                    dist0 = _distances[v0.Name][target0.Name] + 1;
                    o2.Add(target0.Name);
                    newP += (time - dist0) * target0.FlowRate;
                }
                if (target1 != v1)
                {
                    dist1 = _distances[v1.Name][target1.Name] + 1;
                    o2.Add(target1.Name);
                    newP += (time - dist1) * target1.FlowRate;
                }

                int mind = Math.Min(dist0, dist1);

                res = Math.Max(res, SearchDualPath(time - mind, newP, target0, target1, dist0 - mind, dist1 - mind, o2));
            }
        }

        return res;
    }
}