namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2021/day/10
/// </summary>
public class _2021_10 : Problem
{
    private List<(long ErrorScore, long CompletionScore)> _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => GetScore(l)).ToList();
    }

    public override object PartOne() => _data.Sum(d => d.ErrorScore);

    public override object PartTwo()
    {
        _data = _data.Where(d => d.CompletionScore > 0).ToList();

        long score = _data.OrderBy(d => d.CompletionScore).ElementAt(_data.Count / 2).CompletionScore;

        //if (_data.Where(d => d.CompletionScore > score).Count() == _data.Where(d => d.CompletionScore < score).Count())
        //    Console.WriteLine("yes");

        return _data.OrderBy(d => d.CompletionScore).ElementAt(_data.Count / 2).CompletionScore;
    }

    private static int GetCompletionValue(char c) => c switch
    {
        '(' => 1,
        '[' => 2,
        '{' => 3,
        '<' => 4,
        _ => 0,
    };

    private static (long ErrorScore, long CompletionScore) GetScore(string line)
    {
        List<char> opens = new();
        foreach (char c in line)
        {
            switch (c)
            {
                case '(':
                case '[':
                case '{':
                case '<':
                    opens.Add(c);
                    break;

                case ')':
                    if (opens.LastOrDefault() == '(')
                        opens.RemoveAt(opens.Count - 1);
                    else
                        return (3, 0);
                    break;

                case ']':
                    if (opens.LastOrDefault() == '[')
                        opens.RemoveAt(opens.Count - 1);
                    else
                        return (57, 0);
                    break;

                case '}':
                    if (opens.LastOrDefault() == '{')
                        opens.RemoveAt(opens.Count - 1);
                    else
                        return (1197, 0);
                    break;

                case '>':
                    if (opens.LastOrDefault() == '<')
                        opens.RemoveAt(opens.Count - 1);
                    else
                        return (25137, 0);
                    break;
            }
        }
        long completionScore = 0;
        opens.Reverse();
        foreach (char c in opens)
            completionScore = completionScore * 5 + GetCompletionValue(c);

        return (0, completionScore);
    }
}