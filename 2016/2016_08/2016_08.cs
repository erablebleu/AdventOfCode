using System.Data;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/08
/// </summary>
public class _2016_08 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne()
    {
        bool[,] grid = new bool[50, 6];

        foreach (string line in Inputs)
        {
            bool[,] next = grid.ToArray();
            string[] el = line.Split(' ');

            switch (el[0])
            {
                case "rect":
                    int[] c = el[1].Split('x').Select(e => int.Parse(e)).ToArray();
                    for (int x = 0; x < c[0]; x++)
                        for (int y = 0; y < c[1]; y++)
                            next[x, y] = true;
                    break;

                case "rotate":
                    int cnt = int.Parse(el[4]);
                    int idx = int.Parse(el[2].Split('=')[1]);

                    if (el[1] == "row")
                        for (int x = 0; x < 50; x++)
                            next[x, idx] = grid[(x - cnt).Loop(0, 50), idx];
                    else
                        for (int y = 0; y < 6; y++)
                            next[idx, y] = grid[idx, (y - cnt).Loop(0, 6)];
                    break;
            }
            grid = next;
        }

        /* // Uncomment to read PartTwo
        Console.WriteLine();
        for(int i = 0; i < grid.GetLength(1); i++)
            Console.WriteLine(new string(Enumerable.Range(0, 50).Select(x => grid[x, i] ? '#' : '.').ToArray()));
        */

        return grid.Count(true);
    }

    public override object PartTwo() => "AFBUPZBJPS";
}