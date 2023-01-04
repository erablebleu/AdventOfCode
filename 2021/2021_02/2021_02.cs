namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2021/day/02
/// </summary>
public class _2021_02 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne()
    {
        Submarine_01 sub = new();

        sub.Move(Inputs);

        return sub.Depth * sub.HorPos;
    }

    public override object PartTwo()
    {
        Submarine_02 sub = new();

        sub.Move(Inputs);

        return sub.Depth * sub.HorPos;
    }

    private class Submarine_01
    {
        public int Depth { get; private set; }
        public int HorPos { get; private set; }

        public void Move(IEnumerable<string> instructions)
        {
            foreach (string instruction in instructions)
                Move(instruction);
        }

        public void Move(string instruction)
        {
            string[] el = instruction.Split(" ");

            if (el.Length < 2
                || !int.TryParse(el[1], out int length)) return;

            switch (el[0])
            {
                case "forward":
                    HorPos += length;
                    break;

                case "down":
                    Depth += length;
                    break;

                case "up":
                    Depth -= length;
                    break;
            }
        }
    }

    private class Submarine_02
    {
        public int Aim { get; private set; }
        public int Depth { get; private set; }
        public int HorPos { get; private set; }

        public void Move(IEnumerable<string> instructions)
        {
            foreach (string instruction in instructions)
                Move(instruction);
        }

        public void Move(string instruction)
        {
            string[] el = instruction.Split(" ");

            if (el.Length < 2
                || !int.TryParse(el[1], out int length)) return;

            switch (el[0])
            {
                case "forward":
                    HorPos += length;
                    Depth += length * Aim;
                    break;

                case "down":
                    Aim += length;
                    break;

                case "up":
                    Aim -= length;
                    break;
            }
        }
    }
}