using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
   public class _2019_10 : Problem
   {
      #region Fields

      #endregion

      #region Constructors

      public _2019_10()
      {

      }

      #endregion

      #region Methods

      public override void Solve()
      {
         int width = Inputs[0].Length;
         int height = Inputs.Length;
         bool[][] map = new bool[height][];
         int[][] eye = new int[height][];
         List<Asteroid> asts = new List<Asteroid>();

         for (int i = 0; i < height; i++)
            for(int j=0; j < width; j++)
               if(Inputs[i][j] == '#')
                  asts.Add(new Asteroid(j, i));

         for (int i = 0; i < asts.Count; i++)
         {
            for (int j = 0; j < asts.Count; j++)
            {
               asts[i].AddAst(asts[j]);
            }
         }

         asts = asts.OrderByDescending(a => a.Angles.Count).ToList();
         var ast = asts.First();

         Console.WriteLine($"Max asts : {ast}");

         Solutions.Add(ast.Angles.Count().ToString());

         var ast2 = ast.GetNth(200);

         Solutions.Add((ast2.X * 100 + ast2.Y).ToString());
      }

      #endregion
   }

}
public class Asteroid
{
   public int X { get; set; }
   public int Y { get; set; }
   public SortedDictionary<double, List<Asteroid>> Angles { get; set; }
   public Asteroid(int x, int y)
   {
      X = x;
      Y = y;
      Angles = new SortedDictionary<double, List<Asteroid>>();
   }
   public override string ToString()
   {
      return $"X:{X} Y:{Y} Cnt:{Angles.Count}";
   }
   public void AddAst(Asteroid ast)
   {
      if (X == ast.X && Y == ast.Y)
         return;

      double angle = Math.Atan2(ast.X - X, ast.Y - Y);
      if (Angles.ContainsKey(angle))
         Angles[angle].Add(ast);
      else
         Angles.Add(angle, new List<Asteroid>() { ast });
   }

   public Asteroid GetNth(int n)
   {
      int cnt = 0;
      Asteroid ast = null;


      while(cnt < n)
      {
         for(int i = Angles.Count - 1; i>= 0 && cnt < n; i--)
         {
            cnt++;
            var kv = Angles.ElementAt(i);
            ast = kv.Value.First();
            kv.Value.Remove(ast);
            if (kv.Value.Count <= 0)
               Angles.Remove(kv.Key);
         }
      }

      return ast;
   }
}