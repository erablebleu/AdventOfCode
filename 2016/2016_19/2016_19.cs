namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/19
/// </summary>
public class _2016_19 : Problem
{
    private int _data;

    public override void Parse()
    {
        _data = int.Parse(Inputs[0]);
    }

    public override object PartOne()
    {
        List<int> circle = Enumerable.Range(1, _data).ToList();

        while (circle.Count > 1)
            circle = circle.Where((x, i) => i % 2 == 0 && (circle.Count % 2 == 0 || i > 0)).ToList();

        return circle.First();
    }

    public override object PartTwo()
    {
        //Console.WriteLine();
        //for(int i = 5; i < 300; i++)
        //    Console.WriteLine($"{i} => {TestPart2(i)}");

        int result = 3;
        while (result * 3 < _data)
            result *= 3;
        if (_data <= 2 * result)
            return _data - result;
        return result + 2 * (_data - 2 * result);
    }

    private int TestPart2(int cnt) // to slow for get Part2 answer, but nice to see a pattern
    {
        List<int> circle = Enumerable.Range(1, cnt).ToList();

        while (circle.Count > 1)
        {
            for (int i = 0; i < circle.Count; i++)
            {
                int idx = (i + circle.Count / 2) % circle.Count;
                circle.RemoveAt(idx);
                if (i > idx) i--;
            }
        }

        return circle.First();
    }
}