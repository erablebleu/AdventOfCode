namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/12
/// </summary>
public class _2016_12 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne() => Emulate();

    public override object PartTwo() => Emulate(c: 1);

    private int Emulate(int a = 0, int b = 0, int c = 0, int d = 0)
    {
        Dictionary<string, int> registry = new()
        {
            { "a", a },
            { "b", b },
            { "c", c },
            { "d", d },
        };

        for (int i = 0; i < Inputs.Length; i++)
        {
            string[] el = Inputs[i].Split(' ');
            int value;

            switch (Inputs[i][..3])
            {
                case "cpy":
                    registry[el[2]] = int.TryParse(el[1], out value) ? value : registry[el[1]];
                    break;

                case "inc":
                    registry[el[1]]++;
                    break;

                case "dec":
                    registry[el[1]]--;
                    break;

                case "jnz":
                    if ((int.TryParse(el[1], out value) ? value : registry[el[1]]) == 0)
                        break;
                    i += (int.TryParse(el[2], out value) ? value : registry[el[2]]) - 1;
                    break;
            }
        }
        return registry["a"];
    }
}