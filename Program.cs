using System.Diagnostics;

namespace AdventOfCode
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            //Solve(2015, 3, false);
            SolveBatch(2015, 21, 1, 2);
        }

        private static void Solve(int year, int day, bool showData = false)
        {
            Problem pb = Problem.Get(year, day);
            Stopwatch sw = new();

            if (showData)
            {
                foreach (string s in pb.Inputs)
                    Console.WriteLine(s);
            }

            Console.WriteLine($"┌──────────────┬─────────────────────────┬─────────────────────────┐");
            Console.WriteLine($"│   {year}  {day:D2}   │                    Time │                  Result │");
            Console.WriteLine($"├──────────────┼─────────────────────────┼─────────────────────────┤");
            sw.Restart();
            pb.Parse();
            sw.Stop();
            Console.WriteLine($"│ Parse        │{sw.Elapsed.TotalMilliseconds,22:F2} ms│                         │");
            sw.Restart();
            object sol = pb.PartOne();
            sw.Stop();
            Console.WriteLine($"│ Part One     │{sw.Elapsed.TotalMilliseconds,22:F2} ms│{sol,25}│");
            sw.Restart();
            sol = pb.PartTwo();
            sw.Stop();
            if (sol != null)
                Console.WriteLine($"│ Part Two     │{sw.Elapsed.TotalMilliseconds,22:F2} ms│{sol,25}│");
            Console.WriteLine($"└──────────────┴─────────────────────────┴─────────────────────────┘");
        }

        private static void SolveBatch(int year, int day, int yearCount = 1, int dayCount = 1)
        {
            Console.WriteLine($"                          ┌───────────────────────────────────┬───────────────────────────────────┐");
            Console.WriteLine($"                          │             Part One              │             Part Two              │");
            Console.WriteLine($"┌─────────────────────────┼───────────────────────────────────┼───────────────────────────────────┤");
            Console.WriteLine($"│ Problem │ Parse         │ Result                       Time │ Result                       Time │");
            Console.WriteLine($"├─────────┼───────────────┼───────────────────────────────────┼───────────────────────────────────┤");
            for (int y = year; y < year + yearCount; y++)
            {
                for (int d = day; d < day + dayCount; d++)
                {
                    Console.Write($"│ {y} {d:D2} │");
                    (int l, int r) = Console.GetCursorPosition();
                    Console.Write($"    generating class and downloading data");
                    Problem pb = Problem.Get(y, d);
                    Console.SetCursorPosition(l, r);
                    Console.Write($"                                         ");
                    Console.SetCursorPosition(l, r);
                    Stopwatch sw = new();
                    if (pb is null)
                    {
                        Console.WriteLine($"               │                                   │                                   │*");
                        continue;
                    }
                    sw.Restart();
                    pb.Parse();
                    sw.Stop();
                    Console.Write($" {sw.Elapsed.TotalMilliseconds,10:F2} ms │");
                    sw.Restart();
                    object sol1 = pb.PartOne();
                    sw.Stop();
                    Console.Write($" {sol1,-19} {sw.Elapsed.TotalMilliseconds,10:F2} ms │");
                    sw.Restart();
                    object sol2 = pb.PartTwo();
                    sw.Stop();
                    Console.WriteLine($" {sol2,-19} {sw.Elapsed.TotalMilliseconds,10:F2} ms │");
                }
            }
            Console.WriteLine($"└─────────┴───────────────┴───────────────────────────────────┴───────────────────────────────────┘");
        }
    }
}