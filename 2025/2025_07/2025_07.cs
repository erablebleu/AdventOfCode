namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2025/day/07
/// </summary>
public class _2025_07 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne()
    {
        long result = 0;

        string last = Inputs[0];

        for (int i = 1; i< Inputs.Length; i++)
        {
            string l = Inputs[i];
            char[] next = [.. l];

            for(int j = 0; j < l.Length; j++)
            {
                char p = last[j];

                switch(Inputs[i][j])
                {
                    case '.' when p == '|' || p == 'S':
                        next[j] = '|';
                        break;
                    case '^' when p == '|':
                        next[j - 1] = '|';
                        next[j + 1] = '|';
                        result ++;
                        break;                    
                }
            }

            last = new string(next);
        }

        return result;
    }

    public override object PartTwo()
    {
        long[] last = [..Inputs[0].Select(x => x == 'S' ? 1 : 0)];

        for (int i = 1; i< Inputs.Length; i++)
        {
            string l = Inputs[i];
            long[] next = new long[l.Length];

            for(int j = 0; j < l.Length; j++)
            {
                long p = last[j];

                switch(Inputs[i][j])
                {
                    case '.' when p > 0:
                        next[j] += p;
                        break;
                    case '^' when p > 0:
                        next[j - 1] += p;
                        next[j + 1] += p;
                        break;                    
                }
            }

            last = next;
        }

        return last.Sum();
    }
}