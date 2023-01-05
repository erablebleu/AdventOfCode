namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2017/day/09
/// </summary>
public class _2017_09 : Problem
{
    private string _data;

    public override void Parse()
    {
        _data = Inputs[0];
    }

    public override object PartOne() => GetScore(_data, out int canceled);

    public override object PartTwo()
    {
        _ = GetScore(_data, out int canceled);
        return canceled;
    }

    private static int GetScore(string data, out int canceled)
    {
        bool garbage = false;
        int result = 0;
        int score = 0;
        canceled = 0;

        for (int i = 0; i < data.Length; i++)
        {
            switch (data[i])
            {
                case '{' when !garbage:
                    score++;
                    result += score;
                    break;

                case '}' when !garbage:
                    score--;
                    break;

                case '<' when !garbage:
                    garbage = true;
                    break;

                case '>' when garbage:
                    garbage = false;
                    break;

                case '!':
                    i++;
                    break;

                default:
                    if (garbage)
                        canceled++;
                    break;
            }
        }

        return result;
    }
}