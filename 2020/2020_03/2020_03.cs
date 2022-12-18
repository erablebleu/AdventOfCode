using System.Linq;

namespace AdventOfCode
{
    public class _2020_03 : Problem
    {
        #region Methods

        public override void Solve()
        {
            Solutions.Add($"{CountChar('#', 3, 1)}");


            Solutions.Add($"{CountChar('#', 1, 1) * CountChar('#', 3, 1) * CountChar('#', 5, 1) * CountChar('#', 7, 1) * CountChar('#', 1, 2)}");
        }

        private long CountChar(char searched, int right, int bot)
        {
            int col = 0;
            long result = 0;
            int width = Inputs.FirstOrDefault()?.Length ?? 0;
            for (int line = bot; line < Inputs.Length; line += bot)
            {
                col += right;
                result += Inputs[line][col % width] == searched ? 1 : 0;
            }

            return result;
        }

        #endregion
    }
}