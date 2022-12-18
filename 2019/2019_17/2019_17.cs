using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
   public class _2019_17 : Problem
   {
      private class Point
      {
         public int X { get; set; }
         public int Y { get; set; }
         public char Type { get; set; }
         public int Dir { get; set; }
         public char[,] Map { get; set; }
         public bool CanMove()
         {
            return X + Directions[Dir].X >= 0
                    && X + Directions[Dir].X < Width
                    && Y + Directions[Dir].Y >= 0
                    && Y + Directions[Dir].Y < Height
                    && (Map[X + Directions[Dir].X, Y + Directions[Dir].Y] == 'x'
                        || Map[X + Directions[Dir].X, Y + Directions[Dir].Y] == '#');
         }
         public void Move()
         {
            X += Directions[Dir].X;
            Y += Directions[Dir].Y;
         }
         public void Turn(char or)
         {
            if (or == 'R')
               Dir++;
            else
               Dir--;
            if (Dir > 3)
               Dir = 0;
            else if (Dir < 0)
               Dir = 3;
         }
         public override string ToString()
         {
            return $"X:{X} Y:{Y} Type:{Type}";
         }
      }
      private static Dictionary<int, Point> Directions = new Dictionary<int, Point>()
      {
         { 0, new Point() { X = -1, Type = '<' } }, // north (1), south (2), west (3), and east (4)
         { 1, new Point() { Y = -1, Type = '^' } },
         { 2, new Point() { X = 1, Type = '>' } },
         { 3, new Point() { Y = 1, Type = 'v' } },
      };
      #region Fields

      private IntCode _computer;
      private Point _robot;
      private List<Point> _points;
      private char[,] _map;
      private static int _x;
      private static int _y;
      public static int Width;
      public static int Height;

      #endregion

      #region Constructors

      public _2019_17()
      {

      }

      #endregion

      #region Methods

      public override void Solve()
      {
         _computer = new IntCode(Inputs[0].Split(',').Select(s => long.Parse(s)).ToArray());
         _points = new List<Point>();

         _computer.End += OnIntCodeEndPart1;
         _computer.NewOutput += OnNewOutputPart1;

         _computer.Exec();
      }

      private void DrawMap()
      {
         for (int y = 0; y < Height; y++)
         {
            string tmp = string.Empty;
            for (int x = 0; x < Width; x++)
               tmp += _map[x, y];
            Console.WriteLine(tmp);
         }
      }

      private void OnIntCodeEndPart1(object sender, EventArgs e)
      {
         Width = _points.Max(p => p.X) + 1;
         Height = _points.Max(p => p.Y) + 1;
         int sum = 0;
         _map = new char[Width, Height];

         for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            {
               var point = _points.First(p => p.X == x && p.Y == y);
               if (Directions.Values.Any(p => p.Type == point.Type))
               {
                  _robot = point;
                  _robot.Dir = Directions.First(kv => kv.Value.Type == point.Type).Key;
               }
               _map[x, y] = _points.First(p => p.X == x && p.Y == y).Type;
            }


         for (int x = 1; x < Width - 1; x++)
            for (int y = 1; y < Height - 1; y++)
               if (_map[x, y] == '#'
                   && _map[x + 1, y] == '#'
                   && _map[x - 1, y] == '#'
                   && _map[x, y + 1] == '#'
                   && _map[x, y - 1] == '#')
                  sum += x * y;

         DrawMap();

         Console.WriteLine($"Solution PART I : {sum}");

         LookForPath();

         _computer = new IntCode(Inputs[0].Split(',').Select(s => long.Parse(s)).ToArray());

         _computer.End += OnIntCodeEndPart2;
         _computer.NewOutput += OnNewOutputPart2;
         _computer.NewInput += OnNewInputPart2;

         _computer.Data[0] = 2;

         _computer.Exec();
      }
      private void OnNewOutputPart1(object sender, IntCodeOutputEventArgs e)
      {
         if (e.Value == 10)
         {
            _y++;
            _x = 0;
         }
         else
         {
            _points.Add(new Point() { X = _x, Y = _y, Type = (char)e.Value });
            _x++;
         }
      }
      private void LookForPath()
      {
         string[] routines = null;
         string path = FindPath();

         DrawMap();
         Console.WriteLine(path);

         for (int a = 1; a < (path.Length - 1) / 4 && routines is null; a++)
            for (int b = 1; (a + b) < (path.Length - 1) / 4 && routines is null; b++)
               routines = TestPathCut(path, a, b);

         path = path.Replace(routines[0], "A");
         path = path.Replace(routines[1], "B");
         path = path.Replace(routines[2], "C");

         _input = path + (char)10;
         for (int i = 0; i < routines.Length; i++)
            _input += routines[i] + (char)10;
         _input += "n" + (char)10;
      }
      private string FindPath()
      {
         string result = string.Empty;
         _robot.Map = _map;
         int cnt = 0;

         while (true)
         {
            _robot.Turn('R');
            if (_robot.CanMove())
               result += $",R";
            else
            {
               _robot.Turn('L');
               _robot.Turn('L');
               if (_robot.CanMove())
                  result += $",L";
               else
                  break;
            }
            while (_robot.CanMove())
            {
               cnt++;
               _robot.Move();
               _map[_robot.X, _robot.Y] = 'x';
            }
            result += $",{cnt}";
            cnt = 0;
         }

         return result.Substring(1);
      }
      private string[] TestPathCut(string path, int a, int b)
      {
         string[] result = new string[3];
         string[] el = path.Split(',');

         result[0] = string.Empty;
         result[1] = string.Empty;
         result[2] = string.Empty;
         for (int i = 0; i < a * 2 && i < el.Length; i++)
            result[0] += $",{el[i]}";
         for (int i = a * 2; i < (a + b) * 2 && i < el.Length; i++)
            result[1] += $",{el[i]}";
         if(result[0].Length > 1)
         {
            result[0] = result[0].Substring(1);
            path = path.Replace(result[0], "A");
         }
         if (result[1].Length > 1)
         {
            result[1] = result[1].Substring(1);
            path = path.Replace(result[1], "B");
         }
         string tmp = path;
         while (tmp.StartsWith("A,") || tmp.StartsWith("B,"))
            tmp = tmp.Substring(2);
         int idx = tmp.IndexOf('A');
         int idx2 = tmp.IndexOf('B');
         if (idx < 0) idx = tmp.Length;
         if (idx2 < 0) idx2 = tmp.Length;
         idx = Math.Min(idx, idx2);
         result[2] = tmp.Substring(0, idx);
         if (result[2].EndsWith(","))
            result[2] = result[2].Substring(0, result[2].Length - 1);
         path = path.Replace(result[2], "C");

         el = path.Split(',');
         if (el.All(c => c == "A" || c == "B" || c == "C"))
            return result;

         return null;
      }

      private void OnIntCodeEndPart2(object sender, EventArgs e)
      {
      }
      private void OnNewOutputPart2(object sender, IntCodeOutputEventArgs e)
      {
         Console.WriteLine($"Output PART II n°{e.Idx}: {e.Value}");
      }
      private string _input;
      private long OnNewInputPart2(object sender, IntCodeInputEventArgs e)
      {
         if (e.Idx < _input.Length)
            return _input[e.Idx];

         return 0;
      }

      #endregion
   }
}