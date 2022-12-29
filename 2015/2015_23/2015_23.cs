namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/23
/// </summary>
public class _2015_23 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne() => Emulate(0, 0);

    public override object PartTwo() => Emulate(1, 0);

    private int Emulate(int a, int b)
    {
        Dictionary<char, int> registry = new()
        {
            { 'a', a },
            { 'b', b },
        };

        for (int i = 0; i < Inputs.Length; i++)
        {
            string[] el = Inputs[i].Split(' ');
            char r = el[1][0];
            int value;

            if (el.Length > 2)
                value = int.Parse(el[2]);
            else
                _ = int.TryParse(el[1], out value);

            switch (Inputs[i][..3])
            {
                case "hlf":
                    registry[r] /= 2;
                    break;

                case "tpl":
                    registry[r] *= 3;
                    break;

                case "inc":
                    registry[r] += 1;
                    break;

                case "jmp":
                    i += value - 1;
                    break;

                case "jie" when registry[r] % 2 == 0:
                    i += value - 1;
                    break;

                case "jio" when registry[r] == 1:
                    i += value - 1;
                    break;
            }
        }
        return registry['b'];
    }
}