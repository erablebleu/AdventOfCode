namespace AdventOfCode;

public class _2021_02 : Problem
{
    private class Submarine_01
    {
        public int Depth { get; private set; }
        public int HorPos { get; private set; }
        public void Move(IEnumerable<string> instructions)
        {
            foreach(string instruction in instructions)
                Move(instruction);
        }
        public void Move(string instruction)
        {
            string[] el = instruction.Split(" ");

            if (el.Length < 2
                || !int.TryParse(el[1], out int length)) return;

            switch(el[0])
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
        public int Depth { get; private set; }
        public int HorPos { get; private set; }
        public int Aim { get; private set; }
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

    public override void Solve()
    {
        Submarine_01 sub_01 = new();
        Submarine_02 sub_02 = new();

        sub_01.Move(Inputs);
        sub_02.Move(Inputs);

        Solutions.Add($"{sub_01.Depth * sub_01.HorPos}");
        Solutions.Add($"{sub_02.Depth * sub_02.HorPos}");
    }
}