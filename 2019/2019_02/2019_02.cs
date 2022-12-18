using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
   public class _2019_02 : Problem
   {
      #region Constructors

      public _2019_02()
      {

      }

      #endregion

      #region Methods

      public override void Solve()
      {
         var intcode = Inputs[0].Split(',').Select(s => int.Parse(s)).ToArray();

         intcode[1] = 12; // noun
         intcode[2] = 2; // verb

         ExecCode(intcode);

         Solutions.Add(intcode[0].ToString());


         for (int i = 0; i < 10000; i++)
         {
            intcode = Inputs[0].Split(',').Select(s => int.Parse(s)).ToArray();
            intcode[1] = i / 100; // 0-99
            intcode[2] = i % 100; // 0-99
            ExecCode(intcode);
            if (intcode[0] == 19690720)
               break;
         }

         Solutions.Add((intcode[1]*100 + intcode[2]).ToString());
      }

      private void ExecCode(int[] intcode)
      {
         for (int i = 0; i < intcode.Length; i++)
         {
            switch (intcode[i])
            {
               case 1: // +
                  intcode[intcode[i + 3]] = intcode[intcode[i + 1]] + intcode[intcode[i + 2]];
                  i += 3;
                  break;
               case 2: // *
                  intcode[intcode[i + 3]] = intcode[intcode[i + 1]] * intcode[intcode[i + 2]];
                  i += 3;
                  break;
               case 99:
               default:
                  return;
            }
         }
      }

      #endregion
   }
}
