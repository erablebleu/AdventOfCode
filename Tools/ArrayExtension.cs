﻿namespace AdventOfCode.Tools;

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
    public static int Count<T>(this T[,] src, Func<T, bool> predicate)
    {
        int result = 0;
        for (int i = 0; i < src.GetLength(0); i++)
            for (int j = 0; j < src.GetLength(1); j++)
                if (predicate.Invoke(src[i, j]))
                    result++;
        return result;
    }
    public static int Count<T>(this T[,] src, T value)
    {
        int result = 0;
        for (int i = 0; i < src.GetLength(0); i++)
            for (int j = 0; j < src.GetLength(1); j++)
                if (src[i, j].Equals(value))
                    result++;
        return result;
    }
    public static int Sum(this int[,] src)
    {
        int result = 0;
        for (int i = 0; i < src.GetLength(0); i++)
            for (int j = 0; j < src.GetLength(1); j++)
                result += src[i, j];
        return result;
    }
    public static T[] RemoveRange<T>(this T[] src, IEnumerable<T> toRemove)
    {
        List<T> result = src.ToList();
        foreach(T item in toRemove)
            result.Remove(item);
        return result.ToArray();
    }

    public static T[] Concat<T>(this T[] arrA, T[] arrB)
    {
        T[] result = new T[arrA.Length + arrB.Length];
        Array.Copy(arrA, result, arrA.Length);
        Array.Copy(arrB, 0, result, arrA.Length, arrB.Length);
        return result;
    }
}
