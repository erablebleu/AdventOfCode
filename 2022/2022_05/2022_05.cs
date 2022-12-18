using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using static AdventOfCode._2021_05;

namespace AdventOfCode;

public class _2022_05 : Problem
{
    public record MoveInstruction(int Count, int Source, int Destination);
    public override void Solve()
    {
        int instructionLine = 0;

        Stack<char>[] stacks = Enumerable.Range(0, Inputs[0].Length / 4 + 1).Select(i => new Stack<char>()).ToArray();

        for (instructionLine = 0; instructionLine < Inputs.Length && !Inputs[instructionLine].StartsWith(" 1 "); instructionLine++) ;
        instructionLine += 2;

        for(int i = instructionLine - 3; i >= 0; i--)
        {
            for(int j = 0; j < Inputs[0].Length / 4 + 1; j++)
            {
                char c = Inputs[i][4 * j + 1];
                if (c == ' ') continue;
                stacks[j].Push(Inputs[i][4*j + 1]);
            }
        }

        List<MoveInstruction> moves = Inputs.Skip(instructionLine).Select(l => GetInstruction(l)).ToList();
        Stack<char>[] stacks2 = stacks.Select(s => new Stack<char>(s.Reverse())).ToArray();

        foreach (MoveInstruction move in moves)
        {
            for(int i = 0; i < move.Count; i++)
            {
                stacks[move.Destination - 1].Push(stacks[move.Source - 1].Pop());
            }


            foreach (char c in Enumerable.Range(0, move.Count).Select(i => stacks2[move.Source - 1].Pop()).Reverse())
            {
                stacks2[move.Destination - 1].Push(c);
            }
        }

        Solutions.Add($"{new string(stacks.Select(s => s.Peek()).ToArray())}");
        Solutions.Add($"{new string(stacks2.Select(s => s.Peek()).ToArray())}");
    }

    public static MoveInstruction GetInstruction(string line)
    {
        string[] el = line.Split(' ');
        return new MoveInstruction(int.Parse(el[1]), int.Parse(el[3]), int.Parse(el[5]));
    }
}