using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
   public class _2019_06 : Problem
   {
      #region Fields

      private Dictionary<string, Orbit> _orbits = new Dictionary<string, Orbit>();

      #endregion

      #region Constructors

      public _2019_06()
      {
      }

      #endregion

      #region Methods

      public override void Solve()
      {
         // AAA)BBB : BBB is in orbit around AAA

         foreach (string line in Inputs)
         {
            string[] el = line.Split(')');
            var a = _orbits.GetOrAdd(el[0], () => new Orbit(el[0]));
            var b = _orbits.GetOrAdd(el[1], () => new Orbit(el[1]));
            b.Orbit2 = a;
         }
                 
         Solutions.Add(CountOrbits().ToString());
         Solutions.Add(GetDistance("YOU", "SAN").ToString());
      }

      private int CountOrbits()
      {
         int cnt = 0;
         foreach (var orbit in _orbits.Values)
         {
            Orbit orb = orbit;
            while (orb.Orbit2 != null)
            {
               orb = orb.Orbit2;
               cnt++;
            }
         }
         return cnt;
      }

      private int GetDistance(string a, string b)
      {
         List<Orbit> pathA = GetPathToCom(a);
         List<Orbit> pathB = GetPathToCom(b);

         for (int i = 0; i < pathA.Count; i++)
            for (int j = 0; j < pathB.Count; j++)
               if (pathA[i].Name == pathB[j].Name)
                  return i + j;
         return int.MaxValue;
      }

      private List<Orbit> GetPathToCom(string name)
      {
         List<Orbit> path = new List<Orbit>();
         var orb = _orbits[name].Orbit2;

         while (orb.Orbit2 != null)
         {
            path.Add(orb);
            orb = orb.Orbit2;
         }
         return path;
      }

      #endregion
   }

   public class Orbit
   {
      public string Name { get; set; }
      public Orbit Orbit2 { get; set; }
      public Orbit(string name)
      {
         Name = name;
      }
      public override string ToString()
      {
         return Name;
      }
   }

   public static class DicExtension
   {
      public static T2 GetOrAdd<T1, T2>(this Dictionary<T1, T2> dic, T1 key, Func<T2> create)
      {
         if (dic.ContainsKey(key))
            return dic[key];
         var obj = create();
         dic.Add(key, obj);
         return obj;
      }
   }
}
