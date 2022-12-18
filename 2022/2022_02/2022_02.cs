namespace AdventOfCode;

public class _2022_02 : Problem
{
    public override void Solve()
    {
        // Opponent
        // A: Rock
        // B: Paper
        // C: Scissors

        // Response
        // X: Rock
        // Y: Paper
        // Z: Scissors

        // Score = 1/2/3 (R/P/S) + 0/3/6 (Lose/Draw/Win)

        int score = 0;

        foreach(string line in Inputs)
        {
            int o = GetPlay(line[0]);
            int m = GetPlay(line[2]);
            score += m;
            if (o == m)
                score += 3;
            else if (m == o % 3 + 1)
                score += 6;
        }

        Solutions.Add($"{score}");


        // Response
        // X: lose
        // Y: draw
        // Z: win

        score = 0;

        foreach (string line in Inputs)
        {
            int o = GetPlay(line[0]);
            int m = 0;

            switch (line[2])
            {
                case 'X':// Lose
                    m = o - 1;
                    if (m <= 0) m = 3;
                    break;
                case 'Y':// Draw
                    m = o;
                    score += 3;
                    break;
                case 'Z':// Win
                    m = o + 1;
                    if (m > 3) m = 1;
                    score += 6;
                    break;
            }
            score += m;
        }

        Solutions.Add($"{score}");

    }
    public static int GetPlay(char input) => input switch
    {
        'A' => 1,
        'B' => 2,
        'C' => 3,
        'X' => 1,
        'Y' => 2,
        'Z' => 3,
    };
}