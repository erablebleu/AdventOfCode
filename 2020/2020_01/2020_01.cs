using System.Linq;

namespace AdventOfCode
{
    public class _2020_01 : Problem
    {
        #region Methods

        public override void Solve()
        {
            int[] data = Inputs.Select(i => int.Parse(i)).ToArray();

            for (int i = 0; i < data.Length; i++)
                for (int j = i + 1; j < data.Length; j++)
                    if (data[i] + data[j] == 2020)
                    {
                        Solutions.Add((data[i] * data[j]).ToString());
                        i = data.Length;
                        j = data.Length;
                    }

            for (int i = 0; i < data.Length; i++)
                for (int j = i + 1; j < data.Length; j++)
                    for (int k = j + 1; k < data.Length; k++)
                        if (data[i] + data[j] + data[k] == 2020)
                        {
                            Solutions.Add((data[i] * data[j] * data[k]).ToString());
                            i = data.Length;
                            j = data.Length;
                            k = data.Length;
                        }
        }

        #endregion
    }
}