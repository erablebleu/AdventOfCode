namespace AdventOfCode;

public class _2021_07 : Problem
{
    public override void Solve()
    {
        int[] positions = Inputs.First().Split(",").Select(v => int.Parse(v)).ToArray();

        //int median = positions.OrderBy(v => v).ElementAt(positions.Length / 2);
        //Solutions.Add($"{positions.Sum(v => Math.Abs(v - median))}");

        Solutions.Add($"{Minimize(positions, (v, i) => Math.Abs(v - i))}");
        
        //Solutions.Add($"{Minimize(positions, (v, i) => Enumerable.Range(1, Math.Abs(v - i)).Sum())}"); // slow
        Solutions.Add($"{Minimize(positions, (v, i) => Math.Abs(v - i) * (Math.Abs(v - i) + 1) / 2)}"); // Triangular numbers
    }

    private int Minimize(int[] values, Func<int, int, int> predicate)
    {
        int[] ordered = values.OrderBy(v => v).ToArray();

        int value = 0;
        int result = int.MaxValue;

        for(int i = ordered.First(); i < ordered.Last(); i++)
        {
            int sum = values.Sum(v => predicate.Invoke(v, i));

            if (sum >= result)
                continue;

            result = sum;
            value = i;
        }

        return values.Sum(v => predicate.Invoke(v, value));
    }
}