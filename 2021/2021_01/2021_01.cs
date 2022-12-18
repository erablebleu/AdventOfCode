namespace AdventOfCode;

public class _2021_01 : Problem
{
    public override void Solve()
    {
        var depths = Inputs.Select(v => int.Parse(v)).ToList();
        var slidingWin = depths.Skip(2).Select((v, i) => depths[i] + depths[i + 1] + depths[i + 2]).ToList();

        Solutions.Add($"{depths.Zip(depths.Skip(1)).Where(v => v.Second > v.First).Count()}");
        Solutions.Add($"{slidingWin.Zip(slidingWin.Skip(1)).Where(v => v.Second > v.First).Count()}");
    }
}