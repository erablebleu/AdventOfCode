using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class _2020_05 : Problem
    {
        #region Methods

        public override void Solve()
        {
            List<int> l = Inputs.Select(line => Convert.ToInt32(line.Replace("F", "0").Replace("B", "1").Replace("L", "0").Replace("R", "1"), 2)).ToList();

            Solutions.Add($"{l.Max()}");
            Solutions.Add($"{l.First(p => l.Contains(p + 2) && !l.Contains(p + 1)) + 1}");
        }

        #endregion
    }
}