namespace AdventOfCode;

public class _2022_03 : Problem
{
    public override void Solve()
    {
        int prioritySum = 0;

        int[][] data = Inputs.Select(l => l.Select(c => GetPriority(c)).ToArray()).ToArray();


        foreach(int[] line in data)
        {
            int[] c1 = line.Take(line.Length / 2).ToArray();
            int[] c2 = line.Skip(line.Length / 2).ToArray();

            foreach (int c in c1)
            {
                if (!c2.Contains(c))
                    continue;
                prioritySum += c;
                break;
            }
        }

        Solutions.Add($"{prioritySum}");

        prioritySum = 0;
        for (int i = 0; i < data.Length; i+=3)
        {
            foreach (int c in data[i])
            {
                if (!data[i + 1].Contains(c)
                    || !data[i + 2].Contains(c))
                    continue;
                prioritySum += c;
                break;
            }
        }

        Solutions.Add($"{prioritySum}");
    }
    public static int GetPriority(char c)
    {
        if (c >= 'a' && c <= 'z') return (int)c - 'a' + 1;
        return c - 'A' + 27;
    }
}