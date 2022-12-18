using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class _2020_22 : Problem
    {
        #region Methods

        public override void Solve()
        {
            Queue<int> p1 = new();
            Queue<int> p2 = new();
            Queue<int> p = p1;

            foreach (var line in Inputs.Skip(1))
            {
                if (line.StartsWith("Pla"))
                    p = p2;
                else if (line == string.Empty) ;
                else p.Enqueue(int.Parse(line));
            }

            List<int> savep1 = p1.ToList();
            List<int> savep2 = p2.ToList();

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
            Solutions.Add($"{Score(p1.Any() ? p1 : p2)}");


            // Game Part 2
            p1 = new Queue<int>(savep1);
            p2 = new Queue<int>(savep2);

            bool res = Game(ref p1, ref p2);

            Solutions.Add($"{Score(res ? p1 : p2)}");

        }
        private long Score(Queue<int> p)
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
        private string GetHash(Queue<int> p1, Queue<int> p2) => $"{string.Join(",", p1)}|{string.Join(",", p2)}";

        #endregion
    }
}