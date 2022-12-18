using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
   public class _2019_16 : Problem
   {
      #region Fields

      #endregion

      #region Constructors

      public _2019_16()
      {

      }

      #endregion

      #region Methods

      public override void Solve()
      {
         string input;
         var time = new Stopwatch();

         time.Restart();
         input = Inputs[0];
         for (int i = 0; i < 100; i++)
            input = GetFFT(input);
         time.Stop();
         Console.WriteLine($"Method 1: {time.ElapsedMilliseconds}ms - {input}");

         time.Restart();
         input = Inputs[0];
         for (int i = 0; i < 100; i++)
            input = GetFFT2(input);
         time.Stop();
         Console.WriteLine($"Method 2: {time.ElapsedMilliseconds}ms - {input}");

         
         // PART 2

         int offset = int.Parse(Inputs[0].Substring(0, 7));
         input = Inputs[0].Substring(offset % Inputs[0].Length);
         while (input.Length < Inputs[0].Length * 10000 - offset)
            input += Inputs[0];
         input = input.Substring(0, Inputs[0].Length * 10000 - offset);

         int[] array = input.Select(c => (int)c - 48).ToArray();
         for (int i = 0; i < 100; i++)
            array = GetEasyAnswer2(array);

         Solutions.Add($"{array[0]}{array[1]}{array[2]}{array[3]}{array[4]}{array[5]}{array[6]}{array[7]}");

      }
      private static int[] Pattern = { 0, 1, 0, -1 };
      public string GetFFT(string input)
      {
         string res = string.Empty;
         for (int i = 0; i < input.Length; i++)
         {
            int cha = 0;
            for (int j = 0; j < input.Length; j++)
               cha += (input[j] - 48) * Pattern[(j + 1) / (i + 1) % 4];
            res += Math.Abs(cha % 10).ToString();
         }
         return res;
      }
      public string GetFFT2(string input)
      {
         string res = string.Empty;
         for (int i = 0; i < input.Length; i++)
         {
            int cha = 0;
            for (int j = i; j < input.Length; j++)
            {
               int mult = Pattern[(j + 1) / (i + 1) % 4];
               if (mult == 0)
                  j += i;
               else
                  cha += (input[j] - 48) * mult;
            }
            res += Math.Abs(cha % 10).ToString();
         }
         return res;
      }
      public int[] MultArray(int[] array, int factor)
      {
         int[] res = new int[array.Length * factor];
         for (int i = 0; i < array.Length * factor; i++)
            res[i] = array[i % array.Length];
         return res;
      }
      public int[] GetEasyAnswer2(int[] input)
      {
         for (int i = input.Length - 2; i >= 0; i--)
            input[i] = (input[i] + input[i + 1]) % 10;
         return input;
      }
      public int[] GetFFT3(int[] input)
      {
         int[] res = new int[input.Length];
         for (int i = 0; i < input.Length; i++)
         {
            int cha = 0;
            for (int j = i; j < input.Length; j++)
            {
               int mult = Pattern[(j + 1) / (i + 1) % 4];
               if (mult == 0)
                  j += i;
               else
                  cha += input[j] * mult;
            }
            res[i] = cha % 10;
         }
         return res;
      }

      #endregion
   }
}