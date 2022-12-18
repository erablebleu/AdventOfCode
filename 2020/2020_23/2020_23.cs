using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class _2020_23 : Problem
    {
        #region Methods

        public override void Solve()
        {
            //LinkedList<int> ls = new LinkedList<int>("389125467".Select(c => int.Parse(c.ToString())).ToArray());
            LinkedList<int> ls = new LinkedList<int>(Inputs[0].Select(c => int.Parse(c.ToString())).ToArray());
            LinkedListNode<int>[] refs = GetData(ls);
            var cup = ls.First;

            for (int i = 0; i < 100; i++)
                cup = Round(cup, refs);

            AddSolution1(refs[1]);

            // Part 2
            //List<int> data = "389125467".Select(c => int.Parse(c.ToString())).ToList();
            List<int> data = Inputs[0].Select(c => int.Parse(c.ToString())).ToList();
            int max = data.Max();
            while (data.Count < 1000000)
                data.Add(++max);

            ls = new LinkedList<int>(data);
            refs = GetData(ls);
            cup = ls.First;

            for (int i = 0; i < 10000000; i++)
                cup = Round(cup, refs);

            Solutions.Add($"{(long)refs[1].NextCircular().Value * (long)refs[1].NextCircular().NextCircular().Value}");
        }
        private static LinkedListNode<int>[] GetData(LinkedList<int> data)
        {
            LinkedListNode<int>[] res = new LinkedListNode<int>[data.Count + 1];
            LinkedListNode<int> c = data.First;
            for (int i = 0; i < data.Count; i++)
            {
                res[c.Value] = c;
                c = c.NextCircular();
            }
            return res;
        }
        private void AddSolution1(LinkedListNode<int> res)
        {
            var str = string.Empty;
            for (int i = 0; i < res.List.Count - 1; i++)
            {
                res = res.NextCircular();
                str += res.Value.ToString();
            }
            Solutions.Add(str);
        }
        public static LinkedListNode<int> Round(LinkedListNode<int> c, LinkedListNode<int>[] data)
        {
            var c1 = c.NextCircular();
            var c2 = c1.NextCircular();
            var c3 = c2.NextCircular();

            c.List.Remove(c1);
            c.List.Remove(c2);
            c.List.Remove(c3);

            int dest = c.Value;
            do
            {
                dest--;
                if (dest < 1)
                    dest = data.Length - 1;
            }
            while (c1.Value == dest
                   || c2.Value == dest
                   || c3.Value == dest);

            c.List.AddAfter(data[dest], c1);
            c.List.AddAfter(c1, c2);
            c.List.AddAfter(c2, c3);

            return c.NextCircular();
        }

        #endregion
    }
    public static class LinkedListNodeExtension
    {
        public static LinkedListNode<T> NextCircular<T>(this LinkedListNode<T> node) => node.Next ?? node.List.First;
    }
}