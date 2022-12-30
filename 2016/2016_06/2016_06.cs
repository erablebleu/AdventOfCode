namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/06
/// </summary>
public class _2016_06 : Problem
{
    private char[,] _data;

    private delegate IOrderedEnumerable<TSource> OrderDelegate<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector);

    public override void Parse()
    {
        _data = Inputs.To2DArray();
    }

    public override object PartOne() => GetMessage(Enumerable.OrderByDescending);

    public override object PartTwo() => GetMessage(Enumerable.OrderBy);

    private string GetMessage(OrderDelegate<IGrouping<char, int>, int> order)
        => new string(Enumerable.Range(0, _data.GetLength(1)).Select(x => order(Enumerable.Range(0, _data.GetLength(0)).GroupBy(y => _data[y, x]), g => g.Count()).First().Key).ToArray());
}