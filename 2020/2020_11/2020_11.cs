using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public record SeatState
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char InitState { get; set; }
        public char State { get; set; }
        public char NewState { get; set; }
        public List<SeatState> Adj { get; set; } = new List<SeatState>();
        public List<SeatState> AdjEye { get; set; } = new List<SeatState>();
        public void Reset()
        {
            State = InitState;
            NewState = InitState;
        }
        public void GetEye(List<SeatState> data)
        {
            List<List<SeatState>> tmp = new List<List<SeatState>>();

            tmp.Add(data.Where(n => n.Y == Y && n.X < X).ToList());
            tmp.Add(data.Where(n => n.Y == Y && n.X > X).ToList());
            tmp.Add(data.Where(n => n.X == X && n.Y < Y).ToList());
            tmp.Add(data.Where(n => n.X == X && n.Y > Y).ToList());
            tmp.Add(data.Where(n => X - n.X == Y - n.Y && X < n.X).ToList());
            tmp.Add(data.Where(n => X - n.X == Y - n.Y && X > n.X).ToList());
            tmp.Add(data.Where(n => X - n.X == n.Y - Y && X < n.X).ToList());
            tmp.Add(data.Where(n => X - n.X == n.Y - Y && X > n.X).ToList());

            AdjEye = tmp.Select(l => l.OrderBy(n => Math.Abs(n.X - X) + Math.Abs(n.Y - Y)).FirstOrDefault(n => n.InitState != '.')).ToList();

            AdjEye.RemoveAll(l => l is null);
        }
    }
    public class _2020_11 : Problem
    {
        #region Methods

        public override void Solve()
        {
            int width = Inputs[0].Length;
            int height = Inputs.Length;
            string state = string.Concat(Inputs);
            string newState = string.Empty;
            List<SeatState> data = new List<SeatState>();
            int step = 0;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    data.Add(new SeatState { X = x, Y = y, InitState = Inputs[y][x] });

            data.ForEach(d => d.Adj = data.Where(n => Math.Abs(n.X - d.X) <= 1 && Math.Abs(n.Y - d.Y) <= 1 && (d.X != n.X || d.Y != n.Y)).ToList());
            data.ForEach(d => d.GetEye(data));

            data.ForEach(d => d.Reset());
            newState = new string(data.Select(d => d.State).ToArray());
            do
            {
                state = newState;
                step++;

                data.Where(d => d.State == 'L' && d.Adj.All(n => n.State != '#')).ToList().ForEach(d => d.NewState = '#');
                data.Where(d => d.State == '#' && d.Adj.Count(n => n.State == '#') >= 4).ToList().ForEach(d => d.NewState = 'L');

                data.ForEach(d => d.State = d.NewState);
                newState = new string(data.Select(d => d.State).ToArray());
            }
            while (newState != state);
            Solutions.Add($"{newState.Count(c => c == '#')}");

            data.ForEach(d => d.Reset());
            newState = new string(data.Select(d => d.State).ToArray());
            do
            {
                state = newState;
                step++;

                data.Where(d => d.State == 'L' && d.AdjEye.All(n => n.State != '#')).ToList().ForEach(d => d.NewState = '#');
                data.Where(d => d.State == '#' && d.AdjEye.Count(n => n.State == '#') >= 5).ToList().ForEach(d => d.NewState = 'L');

                data.ForEach(d => d.State = d.NewState);
                newState = new string(data.Select(d => d.State).ToArray());
            }
            while (newState != state);
            Solutions.Add($"{newState.Count(c => c == '#')}");
        }

        #endregion
    }
}