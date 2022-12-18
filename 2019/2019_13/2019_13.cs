using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode
{
   public class _2019_13 : Problem
   {
      private class Point
      {
         public long X { get; set; }
         public long Y { get; set; }
         public long Type { get; set; }
      }

      #region Fields

      private IntCode _computer;
      private List<Point> _points;
      private Point _currentPoint;
      private Point _ball;
      private Point _paddle;

      #endregion

      #region Constructors

      public _2019_13()
      {
      }

      #endregion

      #region Methods

      public override void Solve()
      {
         _points = new List<Point>();
         _computer = new IntCode(Inputs[0].Split(',').Select(s => long.Parse(s)).ToArray());
         _ball = new Point();
         _paddle = new Point();
         _computer.Data[0] = 2;
         _computer.End += OnIntCodeEnd;
         _computer.NewOutput += OnNewOutput;
         _computer.Exec();
      }
      private void OnIntCodeEnd(object sender, EventArgs e)
      {
         Solutions.Add(_points.Where(p => p.Type == 2).Count().ToString());
      }
      private bool _bypass;
      private void OnNewOutput(object sender, IntCodeOutputEventArgs e)
      { 
         switch(e.Idx % 3)
         {
            case 0:
               _currentPoint = new Point();
               _currentPoint.X = e.Value;
               break;
            case 1:
               _currentPoint.Y = e.Value;
               break;
            case 2:
               _currentPoint.Type = e.Value;
               if(_currentPoint.X < 0)
                  Solutions.Add(_currentPoint.Type.ToString());
               else
                  _points.Add(_currentPoint);
               switch(_currentPoint.Type)
               {
                  case 3:
                     _paddle.X = _currentPoint.X;
                     _paddle.Y = _currentPoint.Y;
                     break;
                  case 4:
                     _ball.X = _currentPoint.X;
                     _ball.Y = _currentPoint.Y;
                     break;
               }
               if (_points.Count == 26*46 || _bypass)
                  Draw();
               break;
         }
      }
      private char[][] _screen;
      private long _width;
      private long _height;
      private void Draw()
      {
         if(_bypass)
         {
            foreach(var point in _points.ToList())
            {
               if (point.X < 0)
                  continue;
               _screen[point.Y][ point.X] = GetChar(point.Type);
               _points.Remove(point);
            }
         }
         else
         {
            _width = _points.Max(p => p.X) + 1;
            _height = _points.Max(p => p.Y) + 1;
            _screen = new char[_height][];
            for (int i = 0; i < _height; i++)
            {
               _screen[i] = new char[_width];
               for (int j = 0; j < _width; j++)
               {
                  var point = _points.FirstOrDefault(p => p.X == j && p.Y == i);
                  _screen[i][j] = GetChar(point?.Type ?? 0);
                  if (point != null)
                     _points.Remove(point);
               }
            }
            _bypass = true;
         }
            
         /*
         Console.Clear();
         for(int i=0; i<_height; i++)
            Console.WriteLine(new string(_screen[i]));
         Console.WriteLine();
         Console.WriteLine();
         Console.WriteLine();
         foreach (var point in _points)
            Console.WriteLine(point.Type);*/

         _points.Clear();
         _computer.Input = GetInput();
         //Thread.Sleep(50);
      }
      private long GetInput()
      {/*
         switch (Console.ReadKey().Key)
         {
            case ConsoleKey.LeftArrow: return -1;
            case ConsoleKey.RightArrow: return 1;
            default: return 0;
         }*/
         if (_ball.X > _paddle.X)
            return 1;
         else if (_ball.X < _paddle.X)
            return -1;
         else
            return 0;
      }
      private char GetChar(long type)
      {
         switch (type)
         {
            case 1: return '#';
            case 2: return '+';
            case 3: return '_';
            case 4: return 'o';
            case 0:
            default:
               return ' ';
         }
      }

      #endregion
   }
}