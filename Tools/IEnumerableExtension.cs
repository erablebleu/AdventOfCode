using AdventOfCode.Tools;

namespace AdventOfCode.Tools
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<Tout> SelectConcecutives<Tin, Tout>(this IEnumerable<Tin> ls, Func<Tin, Tin, Tout> func)
        {
            for (int i = 0; i < ls.Count() - 1; i++)
                yield return func.Invoke(ls.ElementAt(i), ls.ElementAt(i + 1));
            yield break;
        }
        public static long Product(this IEnumerable<int> ls)
        {
            long result = 1;
            foreach (int i in ls)
                result *= i;
            return result;
        }
        public static T[,] To2DArray<T>(this IEnumerable<IEnumerable<T>> src)
        {
            T[][] data = src.Select(x => x.ToArray()).ToArray();

            var res = new T[data.Length, data.Max(x => x.Length)];
            for (var i = 0; i < data.Length; ++i)
                for (var j = 0; j < data[i].Length; ++j)
                    res[i, j] = data[i][j];

            return res;
        }
    }
}
