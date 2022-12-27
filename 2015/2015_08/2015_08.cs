namespace AdventOfCode;

public class _2015_08 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne()
    {
        int cnt = 0;
        foreach (string line in Inputs)
        {
            for (int i = 0; i < line.Length; i++)
            {
                switch (line[i])
                {
                    case '"':
                        cnt++;
                        break;

                    case '\\':
                        int size = line[i + 1] == 'x' ? 3 : 1;
                        cnt += size;
                        i += size;
                        break;
                }
            }
        }
        return cnt;
    }

    public override object PartTwo()
    {
        int cnt = 0;
        foreach (string line in Inputs)
        {
            cnt += 2;
            for (int i = 0; i < line.Length; i++)
            {
                switch (line[i])
                {
                    case '"':
                    case '\\':
                        cnt++;
                        break;
                }
            }
        }
        return cnt;
    }
}