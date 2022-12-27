namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/11
/// </summary>
public class _2015_11 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne() => GetNext(Inputs[0]);

    public override object PartTwo() => GetNext(GetNext(Inputs[0]));

    private static string GetNext(string pwd)
    {
        char[] data = pwd.ToCharArray();

        do
        {
            bool overlap;
            int idx = data.Length - 1;

            do
            {
                data[idx]++;
                overlap = data[idx] > 'z';
                if (overlap)
                    data[idx] = 'a';
                idx--;
            }
            while (overlap && idx >= 0);
        }
        while (!IsValid(data));

        return new string(data);
    }

    private static bool IsValid(char[] pwd) => Enumerable.Range(0, pwd.Length - 3).Any(i => pwd[i + 1] == pwd[i] + 1 && pwd[i + 2] == pwd[i] + 2)
            && !pwd.Any(c => c == 'i' || c == 'o' || c == 'l')
            && Enumerable.Range(0, pwd.Length - 3).Any(i => Enumerable.Range(i + 2, pwd.Length - i - 3).Any(j => pwd[i] == pwd[i + 1] && pwd[j] == pwd[j + 1]));
}