using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
   public class _2019_01 : Problem
   {
      #region Constructors

      public _2019_01()
      {
      }

      #endregion

      #region Methods

      public override void Solve()
      {
         GetRecursiveFuel(1969);
         Solutions.Add(Inputs.Sum(c => GetFuel(int.Parse(c))).ToString());
         Solutions.Add(Inputs.Sum(c => GetRecursiveFuel(int.Parse(c))).ToString());
      }

      private int GetFuel(int mass)
      {
         return Math.Max(mass / 3 - 2, 0);
      }
      private int GetRecursiveFuel(int mass)
      {
         int fuel = 0;
         while (mass > 0)
            fuel += mass = GetFuel(mass);
         return fuel;
      }

      #endregion
   }
}
