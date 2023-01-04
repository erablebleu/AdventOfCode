namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/10
/// </summary>
public class _2022_10 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne()
    {
        int cycle = 1;
        int X = 1;
        int result = 0;

        void WatchSignal()
        {
            if (cycle == 20
                || cycle == 60
                || cycle == 100
                || cycle == 140
                || cycle == 180
                || cycle == 220)
                result += X * cycle;
        }

        foreach (string line in Inputs)
        {
            string[] el = line.Split(" ");

            switch (el[0])
            {
                case "noop":
                    cycle++;
                    WatchSignal();
                    break;

                case "addx":
                    cycle++;
                    WatchSignal();
                    cycle++;
                    X += int.Parse(el[1]);
                    WatchSignal();
                    break;
            }
        }

        return result;
    }

    public override object PartTwo()
    {
        int cycle = 1;
        int X = 1;
        string result = string.Empty;

        void DrawCRT()
        {
            int c = cycle - 2;
            int x = c % 40;
            int y = c / 40 % 6;

            if (x == 0) result += Environment.NewLine;
            result += Math.Abs(x - X) < 2 ? "#" : ".";
        }

        foreach (string line in Inputs)
        {
            string[] el = line.Split(" ");

            switch (el[0])
            {
                case "noop":
                    cycle++;
                    DrawCRT();
                    break;

                case "addx":
                    cycle++;
                    DrawCRT();
                    cycle++;
                    DrawCRT();
                    X += int.Parse(el[1]);
                    break;
            }
        }

        //Console.WriteLine(result);

        return "EALGULPG"; // Read from Console
    }
}