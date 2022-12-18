using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
   public class _2019_04 : Problem
   {
      #region Constructors

      public _2019_04()
      {
      }

      #endregion

      #region Methods

      public override void Solve()
      {
         string[] el = Inputs[0].Split('-');
         int min = int.Parse(el[0]);
         int max = int.Parse(el[1]);
         int cnt = 0;

         for (int i = min; i < max; i++)
         {
            bool adj = false;
            int j;
            for (j = 0; j < 6 - 1; j++)
            {
               adj |= GetDigit(i, j) == GetDigit(i, j + 1);
               if (GetDigit(i, j) < GetDigit(i, j + 1))
                  break;
            }
            if (adj && j == 5)
               cnt++;
         }

         Solutions.Add(cnt.ToString());

         cnt = 0;
         for (int i = min; i < max; i++)
         {
            bool adj = false;
            bool inc = false;
            int j;
            int lastDig = 10;
            int digCnt = 1;

            for (j = 0; j < 6; j++)
            {
               int dig = GetDigit(i, j);
               inc |= dig > lastDig;
               if (lastDig == dig)
                  digCnt++;
               else
               {
                  lastDig = dig;
                  adj |= digCnt == 2; 
                  digCnt = 1;
               }
            }
            adj |= digCnt == 2;
            if (adj && !inc)
               cnt++;
         }

         Solutions.Add(cnt.ToString());
      }

      public int GetDigit(int i, int digit)
      {
         return i / (int)Math.Pow(10, digit) % 10;
      }

      #endregion
   }
}
