using AdventOfCode.Tools;

namespace AdventOfCode;

public class _2022_09 : Problem
{
    private static readonly Dictionary<char, IVector2D> Directions = new Dictionary<char, IVector2D>()
    {
        { 'L', new IVector2D(-1, 0)},
        { 'U', new IVector2D(0, -1)},
        { 'R', new IVector2D(1, 0)},
        { 'D', new IVector2D(0, 1)},
    };

    public override void Solve()
    {
        IPoint2D head = new(0, 0);
        IPoint2D tail = new(0, 0);
        List<IPoint2D> positions = new();
        positions.Add(tail);


        foreach (string line in Inputs)
        {
            IVector2D dir = Directions[line[0]];
            int length = int.Parse(line.Substring(2));

            for(int i = 0; i < length; i++)
            {
                head += dir;
                IVector2D d = tail - head;
                int div = Math.Max(Math.Abs(d.X), Math.Abs(d.Y));
                if (div > 1)
                {
                    tail = head + d / div;
                }
                if (!positions.Contains(tail))
                    positions.Add(tail);
            }
        }

        Solutions.Add($"{positions.Count}");


        List<IPoint2D> positions2 = new() { new IPoint2D(0, 0) };
        IPoint2D[] rope = Enumerable.Range(0, 10).Select(i => new IPoint2D(0, 0)).ToArray();

        foreach (string line in Inputs)
        {
            IVector2D dir = Directions[line[0]];
            int length = int.Parse(line.Substring(2));

            for (int i = 0; i < length; i++)
            {
                rope[0] += dir;
                for(int j = 1; j < rope.Length; j++)
                {
                    IVector2D d = rope[j] - rope[j - 1];
                    int div = Math.Max(Math.Abs(d.X), Math.Abs(d.Y));
                    if (div > 1)
                    {
                        rope[j] = rope[j - 1] + d / div;
                    }
                }
                if (!positions2.Contains(rope[9]))
                    positions2.Add(rope[9]);
            }
        }

        Solutions.Add($"{positions2.Count}");

    }
}