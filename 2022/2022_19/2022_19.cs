using System.Threading.Tasks;

namespace AdventOfCode;

public class _2022_19 : Problem
{
    public override void Solve()
    {
        List<BluePrint> blueprints = Inputs.Select(l => new BluePrint(l)).ToList();
        int[] res = new int[blueprints.Count];

        Parallel.ForEach(blueprints, b => { res[blueprints.IndexOf(b)] = b.Emulate(24); });
        AddSolution(Enumerable.Range(0, blueprints.Count).Sum(i => blueprints[i].Number * res[i]));

        res = new int[3];
        Parallel.ForEach(blueprints.Take(3), b => { res[blueprints.IndexOf(b)] = b.Emulate(32); });
        AddSolution(res.Product());
    }

    public class BluePrint
    {
        private int _emulationResult = 0;

        public BluePrint(string line)
        {
            // Blueprint 30: Each ore robot costs 4 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 20 clay. Each geode robot costs 2 ore and 19 obsidian.
            int[] values = line.ParseExact("Blueprint {0}: Each ore robot costs {1} ore. Each clay robot costs {2} ore. Each obsidian robot costs {3} ore and {4} clay. Each geode robot costs {5} ore and {6} obsidian.")
                .Select(v => int.Parse(v)).ToArray();
            Number = values[0];
            Costs = new int[,]
            {
                { values[1], 0, 0, 0 },
                { values[2], 0, 0, 0 },
                { values[3], values[4], 0, 0 },
                { values[5], 0, values[6], 0 },
            };
        }

        /* Indexes :
         * 0 : Ore
         * 1 : Clay
         * 2 : Obsidian
         * 3 : Geode
         */
        public int[,] Costs { get; set; }
        public int Number { get; set; }

        public int Emulate(int time)
        {
            _emulationResult = 0;
            Emulate(time, new int[] { 0, 0, 0, 0 }, new int[] { 1, 0, 0, 0 });
            return _emulationResult;
        }

        private bool CanCreate(int r, int[] prod)
        {
            for (int i = 0; i < 4; i++)
                if (Costs[r, i] > 0 && prod[i] == 0)
                    return false;
            return true;
        }

        private void Emulate(int time, int[] stock, int[] prod)
        {
            _emulationResult = Math.Max(_emulationResult, stock[3] + time * prod[3]);

            // filter of possibility if max if not reachable by adding 1 robot every minute
            if (_emulationResult > stock[3] + time * prod[3] + Enumerable.Range(1, time - 1).Sum())
                return;

            // Priority to geode robots
            for (int r = 3; r >= 0; r--)
            {
                // Test if buildable with current production
                if (!CanCreate(r, prod))
                    continue;

                // Search in how much time it could be build
                int t = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (Costs[r, i] == 0)
                        continue;
                    t = Math.Max(t, (Costs[r, i] - stock[i]) / prod[i] + ((Costs[r, i] - stock[i]) % prod[i] > 0 ? 1 : 0));
                }
                t += 1;
                if (t >= time)
                    continue;

                // build and update production
                int[] s2 = new int[4];
                int[] p2 = new int[4];
                for (int i = 0; i < 4; i++)
                {
                    s2[i] = stock[i] + t * prod[i] - Costs[r, i];
                    p2[i] = prod[i] + (i == r ? 1 : 0);
                }

                // launch new emulation with this new data
                Emulate(time - t, s2, p2);
            }
        }
    }
}