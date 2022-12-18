using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
   public class _2019_11 : Problem
   {
      private class Point
      {
         public int X { get; set; }
         public int Y { get; set; }
         public bool IsPainted { get; set; }
         public long Color { get; set; }

         public static bool operator ==(Point a, Point b)
         {
            return a.X == b.X && a.Y == b.Y;
         }
         public static bool operator !=(Point a, Point b)
         {
            return a.X != b.X || a.Y != b.Y;
         }
         public Point Copy()
         {
            return new Point { X = X, Y = Y, IsPainted = IsPainted, Color = Color };
         }
         public void Move(Point p)
         {
            X += p.X;
            Y += p.Y;
         }
         public void Paint(int color)
         {
            IsPainted = true;
            Color = color;
         }
         public override string ToString()
         {
            return $"X:{X} Y:{Y} Color:{Color}";
         }
      }
      private class Map
      {
         public List<Point> Points = new List<Point>();
         public void Paint(Point point)
         {
            Points.RemoveAll(p => p.X == point.X && p.Y == point.Y);
            Points.Add(point);
         }
         public Point GetPosition(Point point)
         {
            return Points.FirstOrDefault(p => p.X == point.X && p.Y == point.Y);
         }
         public void PrintMap()
         {
            int x = Points.Min(p => p.X);
            int y = Points.Min(p => p.Y);
            int width = Points.Max(p => p.X) - x + 1;
            int height = Points.Max(p => p.Y) - y + 1;

            for (int j = y; j < height; j++)
            {
               char[] line = new char[width];

               for (int i = x; i < width; i++)
                  line[i] = ' ';

               foreach (var p in Points.Where(p => p.Y == j))
                  line[p.X] = p.Color == 1 ? 'X' : ' ';

               Console.WriteLine(new string(line));
            }
         }
      }

      #region Fields

      private static Point[] Dirs =
      {
         new Point {X = -1},
         new Point {Y = -1},
         new Point {X = 1},
         new Point {Y = 1},
      };

      private int _dir;
      private Point _pos;
      private Map _map;
      private IntCode _computer;

      #endregion

      #region Constructors

      public _2019_11()
      {

      }

      #endregion

      #region Methods

      public override void Solve()
      {/*
         _computer = new IntCode(Inputs[0].Split(',').Select(s => long.Parse(s)).ToArray());
         _pos = new Point();
         _map = new Map();
         _dir = 1;

         _computer.Input = 0;
         _computer.End += OnIntCodeEnd;
         _computer.NewOutput += OnNewOutput;

         _computer.Exec();*/
         

         _computer = new IntCode(Inputs[0].Split(',').Select(s => long.Parse(s)).ToArray());
         _pos = new Point();
         _map = new Map();
         _dir = 1;

         _computer.End += OnIntCodeEnd;
         _computer.NewOutput += OnNewOutput;

         _pos.Color = 1;
         _computer.Input = _pos.Color;
         _computer.Exec();
      }
      private void OnIntCodeEnd(object sender, EventArgs e)
        {
            Solutions.Add(_map.Points.Count().ToString());
         _map.PrintMap();
      }
      private void OnNewOutput(object sender, IntCodeOutputEventArgs e)
      {
         if (e.Idx % 2 == 0) // paint
         {
            _pos.IsPainted = true;
            _pos.Color = e.Value;
            _map.Paint(_pos);
         }
         else // move
         {
            _pos = _pos.Copy();
            if (e.Value == 0) // tourne
            {
               _dir--;
               if (_dir < 0)
                  _dir = 3;
            }
            else
            {
               _dir++;
               if (_dir > 3)
                  _dir = 0;
            }
            _pos.Move(Dirs[_dir]);
            _pos = _map.GetPosition(_pos) ?? new Point() { X = _pos.X, Y = _pos.Y };
            _computer.Input = _pos.Color;
         }
      }

      #endregion
   }
}