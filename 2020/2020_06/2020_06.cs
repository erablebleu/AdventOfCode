using System.Linq;

namespace AdventOfCode
{
    public class _2020_06 : Problem
    {
        #region Methods

        public override void Solve()
        {
            for (int i = 0; i < Inputs.Length; i++)
                if (Inputs[i] == string.Empty)
                    Inputs[i] = "#";

            Solutions.Add($"{string.Join("", Inputs).Split("#").Sum(l => l.GroupBy(c => c).Count())}");


            Solutions.Add($"{string.Join("/", Inputs).Split("#").Sum(l => l.Replace("/", "").GroupBy(c => c).Count(g => g.Count() == l.Split("/", System.StringSplitOptions.RemoveEmptyEntries).Length))}");
        }

        #endregion
    }
}