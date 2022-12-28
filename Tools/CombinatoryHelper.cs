namespace AdventOfCode.Tools;

public static class CombinatoryHelper
{
    public static long Factorial(int n)
    {
        if (n < 0) throw new ArgumentException("Must be positive");
        if (n > 256) throw new Exception("Must be lower than 256");
        if (n == 0) { return 1; }

        long result = 1;
        for (int i = 1; i <= n; i++)
            result *= i;
        return result;
    }

    public static IEnumerable<T[]> GetCombination<T, T2>(T[] data, Func<IEnumerable<T>, T2> targetFunc, T2 targetSum)
    {
        // Use int to find combinations :
        // bit index = index of integer in _data
        // bit value : 1 if data used, otherwise 0
        int combCnt = (int)Math.Pow(2, data.Length) - 1;

        for (int i = 0; i < combCnt; i++)
        {
            List<T> subset = new();

            for (int j = 0; j < data.Length; j++)
                if ((i & (1 << j)) != 0)
                    subset.Add(data[j]);

            if (targetFunc.Invoke(subset).Equals(targetSum))
                yield return subset.ToArray();
        }

        yield break;
    }

    public static T[] GetKthPermutation<T>(long k, T[] objs)
    {
        T[] permutedObjs = new T[objs.Length];

        for (int i = 0; i < objs.Length; i++)
            permutedObjs[i] = objs[i];

        for (int j = 2; j < objs.Length + 1; j++)
        {
            k /= j - 1;
            long i1 = k % j;
            long i2 = j - 1;
            if (i1 != i2)
            {
                T tmpObj1 = permutedObjs[i1];
                T tmpObj2 = permutedObjs[i2];
                permutedObjs[i1] = tmpObj2;
                permutedObjs[i2] = tmpObj1;
            }
        }
        return permutedObjs;
    }

    public static IEnumerable<T[]> GetPermutations<T>(T[] objs, long? limit = null)
    {
        long n = Factorial(objs.Length);
        n = (!limit.HasValue || limit.Value > n) ? n : limit.Value;

        for (long k = 0; k < n; k++)
            yield return GetKthPermutation(k, objs);

        yield break;
    }
}