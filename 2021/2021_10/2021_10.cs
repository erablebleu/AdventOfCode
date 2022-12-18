namespace AdventOfCode;

public class _2021_10 : Problem
{
    public override void Solve()
    {
        var data = Inputs.Select(l => GetScore(l)).ToList();

        Solutions.Add(data.Sum(d => d.ErrorScore).ToString());

        data = data.Where(d => d.CompletionScore > 0).ToList();

        long score = data.OrderBy(d => d.CompletionScore).ElementAt(data.Count / 2).CompletionScore;

        if (data.Where(d => d.CompletionScore > score).Count() == data.Where(d => d.CompletionScore < score).Count())
            Console.WriteLine("yes");

        Solutions.Add(data.OrderBy(d => d.CompletionScore).ElementAt(data.Count / 2).CompletionScore.ToString());
    }
    private (long ErrorScore, long CompletionScore) GetScore(string line)
    {
        List<char> opens = new();
        foreach(char c in line)
        {
            switch(c)
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
                        return (1197, 0) ;
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

    private static int GetCompletionValue(char c) => c switch
    {
        '(' => 1,
        '[' => 2,
        '{' => 3,
        '<' => 4,
        _ => 0,
    };
}