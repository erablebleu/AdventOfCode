namespace AdventOfCode;

public class _2022_10 : Problem
{
    public override void Solve()
    {
        int cycle = 1;
        int X = 1;
        int sum = 0;
        string s2 = string.Empty;

        void WatchSignal()
        {
            if (cycle == 20
                || cycle == 60
                || cycle == 100
                || cycle == 140
                || cycle == 180
                || cycle == 220)
                sum += X * cycle;
        }
        void DrawCRT()
        {
            int c = cycle - 2;
            int x = c % 40;
            int y = (c / 40) % 6;

            if (x == 0) s2 += Environment.NewLine;
            s2 += Math.Abs(x - X) < 2 ? "#" : ".";
        }

        foreach(string line in Inputs)
        {
            string[] el = line.Split(" ");

            switch(el[0])
            {
                case "noop":
                    cycle++;
                    DrawCRT();
                    WatchSignal();
                    break;
                case "addx":
                    cycle++;
                    DrawCRT();
                    WatchSignal();
                    cycle++;
                    DrawCRT();
                    X += int.Parse(el[1]);
                    WatchSignal();
                    break;
            }
        }

        Solutions.Add($"{sum}");
        Solutions.Add($"{s2}");
    }
}