using System.Collections.Generic;

namespace AdventOfCode;

public class _2021_21 : Problem
{
    internal class Dice
    {
        private int _value = 0;
        public int RollCount { get; private set; }
        public int Get()
        {
            _value = _value % 100 + 1;
            RollCount++;
            return _value;
        }
    }
    internal class Player
    {
        public int Score { get; set; }
        public int Position { get; set; }
        public Player(int pos)
        {
            Position = pos;
        }
        public bool Play(Dice dice)
        {
            Position = (Position + Enumerable.Range(0, 3).Sum(i => dice.Get()) - 1) % 10 + 1;
            Score += Position;
            return Score >= 1000;
        }
    }
    public override void Solve()
    {
        Dice dice = new();
        Player[] players = Inputs.Select(l => new Player(int.Parse(l[28..]))).ToArray();

        while(!players.Any(p => p.Score >= 1000))
        {
            foreach (Player player in players)
                if (player.Play(dice))
                    break;
        }

        Solutions.Add($"{dice.RollCount * players.First(p => p.Score < 1000).Score}");


        int[] p = Inputs.Select(l => int.Parse(l[28..])).ToArray();
        int[] s = new int[] { 0, 0 };
        long[] win = new long[] { 0, 0 };

        Dictionary<(int p1, int p2, int s1, int s2, bool turn, int toss), (long, long) > cache = new();

        for(int i =1; i <= 3; i++)
            for (int j = 1; j <= 3; j++)
                for (int k = 1; k <= 3; k++)
                {
                    (long a, long b) = Turn(p[0], 0, p[1], 0, false, i + j + k);
                    win[0] += a;
                    win[1] += b;
                }

        (long, long) Turn(int p1, int s1, int p2, int s2, bool player, int toss)
        {
            if (cache.TryGetValue((p1, p2, s1, s2, player, toss), out (long w1, long w2) result))
                return result;

            int pos = player ? p2 : p1;
            int score = player ? s2 : s1;

            pos += toss;
            pos = (pos - 1) % 10 + 1;
            score += pos;
            if(score >= 21)
            {
                result = player ? (0, 1) : (1, 0);
                cache[(p1, p2, s1, s2, player, toss)] = result;
                return result;
            }

            (int pr1, int pr2, int sc1, int sc2) = (p1, p2, s1, s2);
            if(!player)
            {
                p1 = pos;
                s1 = score;
            }
            else
            {
                p2 = pos;
                s2 = score;
            }
            long win1 = 0;
            long win2 = 0;
            for (int i = 1; i <= 3; i++)
                for (int j = 1; j <= 3; j++)
                    for (int k = 1; k <= 3; k++)
                    {
                        (long a, long b) = Turn(p1, s1, p2, s2, !player, i + j + k);
                        win1 += a;
                        win2 += b;
                    }

            cache[(pr1, pr2, sc1, sc2, player, toss)] = (win1, win2);
            return (win1, win2);
        }


        Solutions.Add($"{win.Max()}");
    }
}