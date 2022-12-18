using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
   public class _2019_08 : Problem
   {
      #region Fields

      #endregion

      #region Constructors

      public _2019_08()
      {
      }

      #endregion

      #region Methods

      public override void Solve()
      {
         int width = 25;
         int height = 6;
         string[] layers = new string[Inputs[0].Length / (25 * 6)];
         string decoded = string.Empty;

         for (int i = 0; i < layers.Length; i++)
            layers[i] = Inputs[0].Substring(width * height * i, width * height);

         string minZeroLayer = layers.OrderBy(l => l.Count(c => c == '0')).First();

         Solutions.Add((minZeroLayer.Count(c => c == '1') * minZeroLayer.Count(c => c == '2')).ToString());

         for(int i = 0; i < width * height; i++)
         {
            foreach (string layer in layers)
            {
               if (layer[i] == '0')
               {
                  decoded += ' ';
                  break;
               }
               else if (layer[i] == '1')
               {
                  decoded += '#';
                  break;
               }
               else
                  continue;
            }

         }
         for (int i = 0; i < height; i++)
            Console.WriteLine(decoded.Substring(width * i, width));
            AddSolution(0);

      }

      #endregion
   }
}
