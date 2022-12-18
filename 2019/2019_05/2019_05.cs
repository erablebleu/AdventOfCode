using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
   public class _2019_05 : Problem
   {
      // suite _2019_05

      #region Fields

      private int[] _intcode;

      #endregion

      #region Constructors

      public _2019_05()
      {
      }

      #endregion

      #region Methods

      public override void Solve()
      {
         _intcode = Inputs[0].Split(',').Select(s => int.Parse(s)).ToArray();

         AddSolution(ExecCode(1));
        _intcode = Inputs[0].Split(',').Select(s => int.Parse(s)).ToArray();
        AddSolution(ExecCode(5));
      }

      private int ExecCode(int input)
      {
            int result = 0;
         for (int i = 0; i < _intcode.Length; i++)
         {
            switch (_intcode[i] % 100)
            {
               case 1: // +
                  _intcode[_intcode[i + 3]] = GetValue(GetDigit(_intcode[i], 2), _intcode[i + 1])
                                            + GetValue(GetDigit(_intcode[i], 3), _intcode[i + 2]);
                  i += 3;
                  break;
               case 2: // *
                  _intcode[_intcode[i + 3]] = GetValue(GetDigit(_intcode[i], 2), _intcode[i + 1])
                                            * GetValue(GetDigit(_intcode[i], 3), _intcode[i + 2]);
                  i += 3;
                  break;
               case 3: // input
                  _intcode[_intcode[i + 1]] = input;
                  i += 1;
                  break;
               case 4: // output
                  //Console.WriteLine($"{_intcode[i - 4]},{_intcode[i - 3]},{_intcode[i - 2]},{_intcode[i - 1]}  ->  {_intcode[_intcode[i + 1]]}");
                  result = _intcode[_intcode[i + 1]];
                  i += 1;
                  break;
               case 5: // jump-if-true
                  if (GetValue(GetDigit(_intcode[i], 2), _intcode[i + 1]) != 0)
                     i = GetValue(GetDigit(_intcode[i], 3), _intcode[i + 2]) - 1;
                  else
                     i += 2;
                  break;
               case 6: // jump-if-false
                  if (GetValue(GetDigit(_intcode[i], 2), _intcode[i + 1]) == 0)
                     i = GetValue(GetDigit(_intcode[i], 3), _intcode[i + 2]) - 1;
                  else
                     i += 2;
                  break;
               case 7: // less than
                  _intcode[_intcode[i + 3]] = GetValue(GetDigit(_intcode[i], 2), _intcode[i + 1])
                                              < GetValue(GetDigit(_intcode[i], 3), _intcode[i + 2])
                                              ? 1 : 0;
                  i += 3;
                  break;
               case 8: // equals
                  _intcode[_intcode[i + 3]] = GetValue(GetDigit(_intcode[i], 2), _intcode[i + 1])
                                              == GetValue(GetDigit(_intcode[i], 3), _intcode[i + 2])
                                              ? 1 : 0;
                  i += 3;
                  break;
               case 99:
               default:
                  return result;
            }
         }
        return result;
      }

      private int GetValue(int paramMode, int addr)
      {
         switch(paramMode)
         {
            case 0: return _intcode[addr];
            case 1: return addr;
         }
         return 0;
      }

      public int GetDigit(int val, int digit)
      {
         return val / (int)Math.Pow(10, digit) % 10;
      }

      #endregion
   }
}
