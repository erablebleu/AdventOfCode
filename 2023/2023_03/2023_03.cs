namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2023/day/03
/// </summary>
public class _2023_03 : Problem
{
    public override void Parse()
    { }

    public override object PartOne()
    {
        int sum = 0;

        for(int i = 0; i < Inputs.Length; i++)
        {
            for(int j = 0; j < Inputs[0].Length; j++)
            {
                IPoint2D p = new(j, i);

                if (!IsDigit(Inputs[i][j]))
                    continue;

                bool isPart = IsPart(p);
                int k;
                for (k = j + 1; k < Inputs[0].Length && IsDigit(Inputs[i][k]); k++)
                    isPart |= IsPart(new IPoint2D(k, i));

                if (!isPart)
                    continue;

                sum += int.Parse(Inputs[i].Substring(j, k - j));
                j = k;
            }
        }

        return sum;
    }

    public override object PartTwo()
    {
        Dictionary<IPoint2D, List<int>> result = [];

        for (int i = 0; i < Inputs.Length; i++)
        {
            for (int j = 0; j < Inputs[0].Length; j++)
            {
                if (!IsDigit(Inputs[i][j]))
                    continue;

                int k;
                for (k = j + 1; k < Inputs[0].Length && IsDigit(Inputs[i][k]); k++) ;
                int value = int.Parse(Inputs[i].Substring(j, k - j));

                List<IPoint2D> points = [];

                for(; j < k; j++)
                {
                    foreach (IPoint2D p2 in IVector2D.Direction8
                        .Select(v => new IPoint2D(j, i) + v)
                        .Where(p => IsIn(p) && Inputs[p.Y][p.X] == '*' && !points.Contains(p)))
                        points.Add(p2);
                }

                foreach(IPoint2D p in points)
                    if (result.TryGetValue(p, out List<int> l))
                        l.Add(value);
                    else
                        result[p] = [value];
            }
        }

        return result.Where(kv => kv.Value.Count == 2).Sum(kv => kv.Value.Product());
    }

    public static bool IsDigit(char c)
        => c >= '0' && c <= '9';

    public static bool IsSymbol(char c)
        => !IsDigit(c) && c != '.';

    public bool IsPart(IPoint2D p)
        => IVector2D.Direction8
        .Select(v => p + v)
        .Where(IsIn)
        .Any(p2 => IsSymbol(Inputs[p2.Y][p2.X]));

    public bool IsIn(IPoint2D p)
        => p.X >= 0 && p.X < Inputs[0].Length
        && p.Y >= 0 && p.Y < Inputs.Length;
}