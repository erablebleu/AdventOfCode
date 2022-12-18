using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
   public class _2019_18 : Problem
   {
      private class Point
      {
         public int X { get; set; }
         public int Y { get; set; }
         public int Direction { get; set; }
         public static Dictionary<int, Point> Directions = new Dictionary<int, Point>()
         {
            { 0, new Point() { X = -1 } }, // north (1), south (2), west (3), and east (4)
            { 1, new Point() { Y = -1 } },
            { 2, new Point() { X = 1 } },
            { 3, new Point() { Y = 1 } },
         };
         public Point() { }
         public Point(int x, int y)
         {
            X = x;
            Y = y;
         }
         public Point Move()
         {
            X += Directions[Direction].X;
            Y += Directions[Direction].Y;
            return this;
         }
         public override string ToString()
         {
            return $"X:{X} Y:{Y}";
         }
         public Point TurnRight()
         {
            return Turn(true);
         }
         public Point TurnLeft()
         {
            return Turn(false);
         }
         public Point Turn(bool right)
         {
            if (right) Direction++;
            else Direction--;

            if (Direction > 3) Direction = 0;
            else if (Direction < 0) Direction = 3;

            return this;
         }
         public Point Copy()
         {
            return new Point(X, Y) { Direction = Direction };
         }
         public bool CanMove(string[] map, List<char> keys)
         {
            int x = X + Directions[Direction].X;
            int y = Y + Directions[Direction].Y;
            if (x < 0
                || x >= map[0].Length
                || y < 0
                || y >= map.Length)
               return false;
            char c = map[y][x];
            if (c == '#')
               return false;
            return !Char.IsUpper(c) || keys.Contains(Char.ToLower(c));
         }
      }
      private class MapSolver
      {
         public int X { get; set; }
         public int Y { get; set; }
         public string[] Map { get; set; }
         public List<char> Keys { get; set; }
         public Point Position { get; set; }
         public int MoveCount { get; set; }
         public MapSolver(string[] map, int x, int y)
         {
            Map = map;
            Position = new Point(x, y);
            Keys = new List<char>();
         }
         public IEnumerable<int> GetAvailableDirections(bool allowBackward = false)
         {
            List<Point> points = new List<Point>();
            points.Add(Position);
            points.Add(Position.Copy().TurnRight());
            points.Add(Position.Copy().TurnLeft());
            if (allowBackward)
               points.Add(Position.Copy().TurnRight().TurnRight());
            return points.Where(p => p.CanMove(Map, Keys)).Select(p => p.Direction);
         }
         public MapSolver Copy()
         {
            var result = new MapSolver(Map, Position.X, Position.Y);
            result.Position.Direction = Position.Direction;
            result.MoveCount = MoveCount;
            foreach (char k in Keys)
               result.Keys.Add(k);
            return result;
         }
         public bool MoveToDir(int dir)
         {
            Position.Direction = dir;
            Position.Move();
            MoveCount++;
            char c = Map[Position.Y][Position.X];
            switch (c)
            {
               case '.':
               case '#':
                  break;
               default:
                  if (!Char.IsUpper(c) && !Keys.Contains(c))
                  {
                     Keys.Add(c);
                     return true;
                  }
                  break;
            }
            return false;
         }
      }

      #region Fields

      public string[] Map;
      public List<char> Keys;
      private MapSolver _shortestPath;

      #endregion

      #region Constructors

      public _2019_18()
      {

      }

      #endregion

      #region Methods

      public override void Solve()
      {
         Map = Inputs;
         Keys = new List<char>();
         MapSolver resolver = null;

         for (int x = 0; x < Map[0].Length; x++)
         {
            for (int y = 0; y < Map.Length; y++)
            {
               switch (Map[y][x])
               {
                  case '@':
                     resolver = new MapSolver(Map, x, y);
                     Map[y] = Map[y].Replace('@', '.');
                     break;
                  case '.':
                  case '#':
                     break;
                  default:
                     char c = Map[y][x];
                     if (Char.IsLower(c))
                        Keys.Add(Map[y][x]);
                     break;
               }
            }
         }

         RecursiveSearch(resolver, true);
         Solutions.Add(_shortestPath.MoveCount.ToString());
      }

      private void RecursiveSearch(MapSolver solver, bool canBack)
      {
         if(Keys.All(c => solver.Keys.Contains(c)))
         {
            if (_shortestPath is null || _shortestPath.MoveCount > solver.MoveCount)
               _shortestPath = solver;
            return;
         }
         if (_shortestPath != null && _shortestPath.MoveCount < solver.MoveCount)
            return;
         foreach (int dir in solver?.GetAvailableDirections(canBack))
         {
            MapSolver cpy = solver.Copy();
            bool res = cpy.MoveToDir(dir);
            RecursiveSearch(cpy, res);
         }
      }

      #endregion
   }
}