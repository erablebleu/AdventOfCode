using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class _2020_15 : Problem
    {
        #region Methods

        public override void Solve()
        {
            List<int> input = Inputs[0].Split(",").Select(i => int.Parse(i)).ToList();

            Solutions.Add($"{PlayN(2020, input)}");
            Solutions.Add($"{PlayN(30000000, input)}");
        }

        public static int PlayN(int roundCount, List<int> input)
        {
            Dictionary<int, int> data = new Dictionary<int, int>();
            int last = 0;

            for (int i = 0; i < roundCount; i++)
            {
                int n;

                if (i < input.Count)
                    n = input[i];
                else if (data.ContainsKey(last))
                    n = i - data[last];
                else
                    n = 0;

                if (i > 0) data[last] = i;
                last = n;
            }
            return last;
        }

        #endregion
    }
}