namespace AdventOfCode;

public class _2021_05 : Problem
{
    internal class Line
    {
        public System.Drawing.Point P1 { get; set; }
        public System.Drawing.Point P2 { get; set; }
        public Line(string data)
        {
            string[] el = data.Split(" -> ");
            P1 = GetPoint(el[0]);
            P2 = GetPoint(el[1]);
        }
        private static System.Drawing.Point GetPoint(string data)
        {
            string[] el = data.Split(",");
            return new System.Drawing.Point(int.Parse(el[0]), int.Parse(el[1]));
        }
    }
    public override void Solve()
    {
        List<Line> lines = Inputs.Select(l => new Line(l)).ToList();

        int[,] grid = new int[Math.Max(lines.Max(l => l.P1.X), lines.Max(l => l.P2.X)) + 1,
                              Math.Max(lines.Max(l => l.P1.Y), lines.Max(l => l.P2.Y)) + 1];

        foreach (Line line in lines)
        {
            if (line.P1.X == line.P2.X)
                for (int i = Math.Min(line.P1.Y, line.P2.Y); i <= Math.Max(line.P1.Y, line.P2.Y); i++)
                grid[line.P1.X, i]++;

            else if (line.P1.Y == line.P2.Y)
                for (int i = Math.Min(line.P1.X, line.P2.X); i <= Math.Max(line.P1.X, line.P2.X); i++)
                grid[i, line.P1.Y]++;
        }

        Solutions.Add($"{grid.Cast<int>().Count(v => v >= 2)}");

        foreach (Line line in lines)
        {
            int dx = line.P2.X - line.P1.X;
            int dy = line.P2.Y - line.P1.Y;

            if (dx == 0 || dy == 0 || Math.Abs(dx) != Math.Abs(dy))
                continue;

            int cx = dx / Math.Abs(dx);
            int cy = dy / Math.Abs(dy);

            for (int i = 0; i <= Math.Abs(dx); i++)
                grid[line.P1.X + i * cx, line.P1.Y + i * cy]++;
        }

        Solutions.Add($"{grid.Cast<int>().Count(v => v >= 2)}");
    }
}