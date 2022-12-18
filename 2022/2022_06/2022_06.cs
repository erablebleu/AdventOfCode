namespace AdventOfCode;

public class _2022_06 : Problem
{
    public override void Solve()
    {

        for(int i = 0; i < Inputs[0].Length; i++)
        {
            string marker = Inputs[0].Substring(i, 4);
            if(marker.Distinct().Count() == marker.Length)
            {
                Solutions.Add($"{i + 4}");
                break;
            }
        }


        for (int i = 0; i < Inputs[0].Length; i++)
        {
            string marker = Inputs[0].Substring(i, 14);
            if (marker.Distinct().Count() == marker.Length)
            {
                Solutions.Add($"{i + 14}");
                break;
            }
        }
    }
}