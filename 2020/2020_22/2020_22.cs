namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/22
/// </summary>
public class _2020_22 : Problem
{
    private List<int> _p1;
    private List<int> _p2;

    public override void Parse()
    {
        _p1 = new List<int>();
        _p2 = new List<int>();
        List<int> p = _p1;

        foreach (var line in Inputs.Skip(1))
        {
            if (line.StartsWith("Pla"))
                p = _p2;
            else if (line == string.Empty) ;
            else p.Add(int.Parse(line));
        }
    }

    public override object PartOne()
    {
        Queue<int> p1 = new(_p1);
        Queue<int> p2 = new(_p2);

        // Game part1
        while (p1.Any() && p2.Any())
        {
            int c1 = p1.Dequeue();
            int c2 = p2.Dequeue();
            if (c1 > c2)
            {
                p1.Enqueue(c1);
                p1.Enqueue(c2);
            }
            else
            {
                p2.Enqueue(c2);
                p2.Enqueue(c1);
            }
        }
        return Score(p1.Any() ? p1 : p2);
    }

    public override object PartTwo()
    {
        Queue<int> p1 = new(_p1);
        Queue<int> p2 = new(_p2);

        bool res = Game(ref p1, ref p2);

        return Score(res ? p1 : p2);
    }

    private static long Score(Queue<int> p)
    {
        var ls = p.ToList();
        long score = 0;
        for (int i = 0; i < ls.Count; i++)
            score += ls[i] * (ls.Count - i);
        return score;
    }

    private bool Game(ref Queue<int> p1, ref Queue<int> p2)
    {
        HashSet<string> saves = new();
        while (true)
        {
            if (!p1.Any()) return false;
            if (!p2.Any()) return true;

            string hash = GetHash(p1, p2);

            if (saves.Contains(hash))
                return true;

            saves.Add(hash);

            int c1 = p1.Dequeue();
            int c2 = p2.Dequeue();

            bool p1Win;

            if (p1.Count < c1 || p2.Count < c2)
                p1Win = c1 > c2;
            else
            {
                var np1 = new Queue<int>(p1.Take(c1));
                var np2 = new Queue<int>(p2.Take(c2));
                // Sub-Game
                p1Win = Game(ref np1, ref np2);
            }

            if (p1Win)
            {
                p1.Enqueue(c1);
                p1.Enqueue(c2);
            }
            else
            {
                p2.Enqueue(c2);
                p2.Enqueue(c1);
            }
        }
    }

    private static string GetHash(Queue<int> p1, Queue<int> p2) => $"{string.Join(",", p1)}|{string.Join(",", p2)}";
}