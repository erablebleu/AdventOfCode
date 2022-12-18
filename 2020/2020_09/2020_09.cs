using System.Linq;

namespace AdventOfCode
{
    public class _2020_09 : Problem
    {
        #region Methods

        public override void Solve()
        {
            long[] array = Inputs.Select(v => long.Parse(v)).ToArray();
            long invalidNumber = 0;

            for(int i = 25; i < array.Length; i++)
                if(!IsSumOfTwo(array.Skip(i - 25).Take(25).ToArray(), array[i]))
                {
                    invalidNumber = array[i];
                    Solutions.Add($"{invalidNumber}");
                    break;
                }

            for(int i = 0; i <array.Length; i++)
            {
                int j = 0;
                long sum = array[i];
                for (j = i + 1; j < array.Length && sum < invalidNumber; j++)
                    sum += array[j];

                if (sum != invalidNumber)
                    continue;

                var arr = array.Skip(i).Take(j - i).ToArray();

                Solutions.Add($"{(arr.Min() + arr.Max())}");
                break;
            }
        }

        private bool IsSumOfTwo(long[] array, long value)
        {
            for (int i = 0; i < array.Length; i++)
                for (int j = i + 1; j < array.Length; j++)
                    if (array[i] + array[j] == value)
                        return true;
            return false;
        }

        #endregion
    }
}