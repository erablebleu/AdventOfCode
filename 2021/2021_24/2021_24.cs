namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2021/day/24
/// </summary>
public class _2021_24 : Problem
{
    private int[][] _data;

    public override void Parse()
    {
        _data = new int[7][];
        int[,] stack = new int[7, 2];
        int sIdx = 0;
        int dIdx = 0;

        // Inputs looks like 14 blocks of 18 instructions
        for (int i = 0; i < 14; i++)
        {
            // with 3 variations
            int a = int.Parse(Inputs[18 * i + 4].Split(" ")[2]);
            int b = int.Parse(Inputs[18 * i + 5].Split(" ")[2]);
            int c = int.Parse(Inputs[18 * i + 15].Split(" ")[2]);

            if (a == 1)
            {
                stack[sIdx, 0] = i;
                stack[sIdx, 1] = c;
                sIdx++;
            }
            else
            {
                sIdx--;
                _data[dIdx] = new int[]
                {
                    stack[sIdx, 0],
                    i,
                    stack[sIdx, 1] + b,
                };
                dIdx++;
            }
        }
    }

    public override object PartOne()
    {
        long result = 0;

        for (int i = 0; i < _data.GetLength(0); i++)
        {
            for (int w = 9; w > 0; w--)
            {
                if (w + _data[i][2] >= 10)
                    continue;

                result += AddValue(w, _data[i]);
                break;
            }
        }

        return result;
    }

    public override object PartTwo()
    {
        long result = 0;

        for (int i = 0; i < _data.GetLength(0); i++)
        {
            for (int w = 1; w < 10; w++)
            {
                if (w + _data[i][2] <= 0)
                    continue;

                result += AddValue(w, _data[i]);
                break;
            }
        }
        return result;
    }

    private static long AddValue(int w, int[] d) => w * (long)Math.Pow(10, 13 - d[0]) + (w + d[2]) * (long)Math.Pow(10, 13 - d[1]);
}