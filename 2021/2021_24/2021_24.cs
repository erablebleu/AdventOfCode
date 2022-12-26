using System.Runtime.CompilerServices;

namespace AdventOfCode;

public class _2021_24 : Problem
{
    public override void Solve()
    {
        long[] result = new long[2];
        int[][] data = new int[7][];
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
                data[dIdx] = new int[]
                {
                    stack[sIdx, 0],
                    i,
                    stack[sIdx, 1] + b,
                };
                dIdx++;
            }
        }

        long AddValue(int w, int[] d) => w * (long)Math.Pow(10, 13 - d[0]) + (w + d[2]) * (long)Math.Pow(10, 13 - d[1]);

        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int w = 9; w > 0; w--)
            {
                if (w + data[i][2] >= 10)
                    continue;
                
                result[0] += AddValue(w, data[i]);
                break;                
            }
            for (int w = 1; w < 10; w++)
            {
                if (w + data[i][2] <= 0)
                    continue;
                
                result[1] += AddValue(w, data[i]);
                break;                
            }
        }

        AddSolution(result[0]);
        AddSolution(result[1]);
    }
}