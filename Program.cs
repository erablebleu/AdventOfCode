namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Solve(2022, 22);
        }

        private static void Solve(int year, int day, bool showData = true)
        {
            Problem pb = null;
            try
            {
                pb = Problem.Get(year, day);

                if (showData)
                {
                    pb.WriteHeader();
                    pb.WriteData();
                }
                pb.WriteHeader();
                pb.SolveWithTime();
                pb.WriteSolutions();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{pb} - {e}");
            }
        }
    }
}
