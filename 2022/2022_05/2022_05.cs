namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/05
/// </summary>
public class _2022_05 : Problem
{
    private Stack<char>[] _stacks;
    private Stack<char>[] _stacks2;

    public override void Parse()
    {
        int instructionLine = 0;

        _stacks = Enumerable.Range(0, Inputs[0].Length / 4 + 1).Select(i => new Stack<char>()).ToArray();

        for (instructionLine = 0; instructionLine < Inputs.Length && !Inputs[instructionLine].StartsWith(" 1 "); instructionLine++) ;
        instructionLine += 2;

        for (int i = instructionLine - 3; i >= 0; i--)
        {
            for (int j = 0; j < Inputs[0].Length / 4 + 1; j++)
            {
                char c = Inputs[i][4 * j + 1];
                if (c == ' ') continue;
                _stacks[j].Push(Inputs[i][4 * j + 1]);
            }
        }

        List<MoveInstruction> moves = Inputs.Skip(instructionLine).Select(l => GetInstruction(l)).ToList();
        _stacks2 = _stacks.Select(s => new Stack<char>(s.Reverse())).ToArray();

        foreach (MoveInstruction move in moves)
        {
            for (int i = 0; i < move.Count; i++)
            {
                _stacks[move.Destination - 1].Push(_stacks[move.Source - 1].Pop());
            }

            foreach (char c in Enumerable.Range(0, move.Count).Select(i => _stacks2[move.Source - 1].Pop()).Reverse())
            {
                _stacks2[move.Destination - 1].Push(c);
            }
        }
    }

    public override object PartOne() => new string(_stacks.Select(s => s.Peek()).ToArray());

    public override object PartTwo() => new string(_stacks2.Select(s => s.Peek()).ToArray());

    private record MoveInstruction(int Count, int Source, int Destination);

    private static MoveInstruction GetInstruction(string line)
    {
        string[] el = line.Split(' ');
        return new MoveInstruction(int.Parse(el[1]), int.Parse(el[3]), int.Parse(el[5]));
    }
}