namespace AdventOfCode.Tools;

public static class ArrayExtension
{
    public static int Count<T>(this T[,] data, Func<T[,], int, int, bool> func)
    {
        int result = 0;
        for (int i = 0; i < data.GetLength(0); i++)
            for (int j = 0; j < data.GetLength(1); j++)
                if (func(data, i, j))
                    result++;
        return result;
    }
    public static int Max<T>(this T[,] data, Func<T[,], int, int, int> func)
    {
        int result = int.MinValue;
        for (int i = 0; i < data.GetLength(0); i++)
            for (int j = 0; j < data.GetLength(1); j++)
                result = Math.Max(result, func(data, i, j));
        return result;
    }
    public static T[,] ToArray<T>(this T[,] data)
    {
        T[,] result = new T[data.GetLength(0), data.GetLength(1)];
        for (int i = 0; i < data.GetLength(0); i++)
            for (int j = 0; j < data.GetLength(1); j++)
                result[i, j] = data[i, j];
        return result;
    }
}
