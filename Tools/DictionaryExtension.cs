namespace AdventOfCode.Tools;

public static class DictionaryExtension
{
    public static void Add<T>(this Dictionary<int, Dictionary<int, Dictionary<int, T>>> dic, int x, int y, int z) where T : new()
    {
        if (!dic.ContainsKey(x)) dic[x] = new();
        if (!dic[x].ContainsKey(y)) dic[x][y] = new();
        if (!dic[x][y].ContainsKey(z)) dic[x][y][z] = new();
    }

    public static void AddOrInc<T>(this Dictionary<T, long> dic, T key, long value)
    {
        if (dic.ContainsKey(key))
            dic[key] += value;
        else
            dic.Add(key, value);
    }

    public static bool Contains<T>(this Dictionary<int, Dictionary<int, Dictionary<int, T>>> dic, int x, int y, int z) where T : new()
                => dic.ContainsKey(x) && dic[x].ContainsKey(y) && dic[x][y].ContainsKey(z);

    public static int CountRecursive<T>(this Dictionary<int, Dictionary<int, Dictionary<int, T>>> dic) where T : new()
        => dic.Values.Sum(d2 => d2.Values.Sum(d3 => d3.Count));

    public static T2 GetOrAdd<T1, T2>(this Dictionary<T1, T2> dic, T1 key, Func<T2> create)
    {
        if (dic.ContainsKey(key))
            return dic[key];
        var obj = create();
        dic.Add(key, obj);
        return obj;
    }
    public static T2 AddOrUpdate<T1, T2>(this Dictionary<T1, T2> dic, T1 key, T2 value, Func<T2, T2> update)
    {
        if (dic.ContainsKey(key))
            dic[key] = update(dic[key]);
        else
            dic[key] = value;
        return dic[key];
    }
}