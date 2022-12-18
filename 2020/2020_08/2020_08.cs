using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class _2020_08 : Problem
    {
        #region Methods

        public override void Solve()
        {
            int acc = 0;
            int i;
            List<int> lines = new List<int>();

            for (i = 0; i < Inputs.Length && i >= 0; i++)
            {
                if(lines.Contains(i))
                {
                    Solutions.Add($"{acc}");
                    break;
                }
                lines.Add(i);

                var el = Inputs[i].Split(" ");
                int val = int.Parse(el[1]);
                switch (el[0])
                {
                    case "acc": acc += val; break;
                    case "jmp": i += val - 1; break;
                    case "nop":
                    default:
                        break;
                }
            }

            for(int j = 0; j < Inputs.Length; j++)
            {
                if (Inputs[j].Split(" ")[0] == "nop") continue;

                lines.Clear();
                acc = 0;
                int last = 0;

                for (i = 0; i < Inputs.Length && i >= 0; i++)
                {
                    last = i;
                    if (lines.Contains(i))
                        break;

                    lines.Add(i);

                    var el = Inputs[i].Split(" ");
                    int val = int.Parse(el[1]);
                    switch (el[0])
                    {
                        case "jmp" when i == j: break;
                        case "nop" when i == j: i += val - 1; break;
                        case "acc": acc += val; break;
                        case "jmp": i += val - 1; break;
                        case "nop":
                        default:
                            break;
                    }
                }

                if (last == Inputs.Length - 1)
                {
                    Solutions.Add($"{acc}");
                    break;
                }
            }
        }

        #endregion
    }
}