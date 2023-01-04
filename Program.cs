using System.Diagnostics;

namespace AdventOfCode
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            SolveBatch(2017, 1, 1);
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

        private static void SolveBatch(int year, int day, int dayCount = 1)
        {
            int annotCount = 0;
            Dictionary<int, string> annotations = new();

            string ValidateSolution(object solution)
            {
                if (solution is null) return null;
                string solText = solution.ToString();
                if (solText.Length <= 25)
                    return solText;
                annotations.Add(++annotCount, solText);
                return $"{solText[..(25 - 6 - annotCount.ToString().Length)]}... ({annotCount})";
            }

            Console.WriteLine($"                          ┌─────────────────────────────────────────┬─────────────────────────────────────────┐");
            Console.WriteLine($"                          │                Part One                 │                Part Two                 │");
            Console.WriteLine($"┌─────────┬───────────────┼─────────────────────────────────────────┼─────────────────────────────────────────┤");
            Console.WriteLine($"│ Problem │ Parse    Time │ Result                             Time │ Result                             Time │");
            Console.WriteLine($"├─────────┼───────────────┼─────────────────────────────────────────┼─────────────────────────────────────────┤");

            int d = day-1;
            int y = year;
            for(int i = 0; i < dayCount; i++)
            {
                d++;
                if(d > 25)
                {
                    d = 1;
                    y++;
                }
                if(i > 0 && d == 1)
                    Console.WriteLine($"├─────────┼───────────────┼─────────────────────────────────────────┼─────────────────────────────────────────┤");

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
                Console.Write($" {ValidateSolution(sol1),-25} {sw.Elapsed.TotalMilliseconds,10:F2} ms │");
                sw.Restart();
                object sol2 = pb.PartTwo();
                sw.Stop();
                Console.WriteLine($" {ValidateSolution(sol2),-25} {sw.Elapsed.TotalMilliseconds,10:F2} ms │");
            }
            Console.WriteLine($"└─────────┴───────────────┴─────────────────────────────────────────┴─────────────────────────────────────────┘");
            foreach (KeyValuePair<int, string> kv in annotations)
                Console.WriteLine((kv.Value.Contains(Environment.NewLine) ? Environment.NewLine : string.Empty) + $"({kv.Key}) {kv.Value}");
        }
    }
}