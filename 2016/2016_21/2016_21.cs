namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/21
/// </summary>
public class _2016_21 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne() => Scramble("abcdefgh");

    public override object PartTwo() => Scramble("fbgdceah", true);

    private string Scramble(string input, bool reverse = false)
    {
        char[] data = input.ToCharArray();

        foreach (string instruction in reverse ? Inputs.Reverse() : Inputs)
        {
            string[] el = instruction.Split(' ');
            char[] next = data.ToArray();

            switch (el[0])
            {
                case "swap" when el[1] == "position":
                    {
                        int x = int.Parse(el[2]);
                        int y = int.Parse(el[5]);

                        (next[y], next[x]) = (next[x], next[y]);
                    }
                    break;

                case "swap" when el[1] == "letter":
                    {
                        char x = el[2][0];
                        char y = el[5][0];
                        for (int i = 0; i < data.Length; i++)
                            if (next[i] == x)
                                next[i] = y;
                            else if (next[i] == y)
                                next[i] = x;
                    }
                    break;

                case "rotate":
                    {
                        int FromChar(char c)
                        {
                            int idx = Array.IndexOf(data, el[6][0]);

                            if (!reverse)
                                return idx + 1 + (idx >= 4 ? 1 : 0);

                            // look for original position
                            for (int i = 0; i < data.Length; i++)
                            {
                                int idx2 = i + 1 + (i >= 4 ? 1 : 0);
                                if (idx == (i + idx2).Loop(0, data.Length))
                                    return idx2;
                            }
                            return 0;
                        }
                        int d = el[1] switch
                        {
                            "left" => -int.Parse(el[2]),
                            "right" => int.Parse(el[2]),
                            "based" => FromChar(el[6][0]),
                            _ => throw new ArgumentException(),
                        };

                        if (reverse)
                            d *= -1;

                        for (int i = 0; i < data.Length; i++)
                            next[(i + d).Loop(0, data.Length)] = data[i];
                    }
                    break;

                case "reverse":
                    {
                        int x = int.Parse(el[2]);
                        int y = int.Parse(el[4]);

                        for (int i = x; i <= y; i++)
                            next[i] = data[y - i + x];
                    }
                    break;

                case "move":
                    {
                        List<char> list = data.ToList();
                        int x = int.Parse(el[2]);
                        int y = int.Parse(el[5]);

                        if (reverse)
                            (x, y) = (y, x);

                        char c = next[x];
                        list.RemoveAt(x);
                        list.Insert(y, c);
                        next = list.ToArray();
                    }
                    break;
            }

            data = next;
        }
        return new string(data);
    }
}