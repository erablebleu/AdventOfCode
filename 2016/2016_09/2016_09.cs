namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/09
/// </summary>
public class _2016_09 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne() => GetLength(Inputs[0], false);

    public override object PartTwo() => GetLength(Inputs[0], true);

    private static long GetLength(string value, bool recurse)
    {
        long length = 0;
        int oIdx = value.IndexOf('(');
        int cIdx = 0;

        while (oIdx >= 0)
        {
            length += oIdx;
            cIdx = value.IndexOf(')');
            string marker = value.Substring(oIdx + 1, cIdx - oIdx - 1);
            int[] nb = marker.Split('x').Select(e => int.Parse(e)).ToArray();
            value = value.Remove(0, cIdx + 1);

            string sub = value.Substring(0, nb[0]);

            if (sub.Contains('(') && recurse)
            {
                length += nb[1] * GetLength(sub, true);
                value = value.Remove(0, sub.Length);
            }
            else
            {
                value = value.Remove(0, nb[0]);
                length += nb[0] * nb[1];
            }
            oIdx = value.IndexOf('(');
        }
        return length;
    }
}