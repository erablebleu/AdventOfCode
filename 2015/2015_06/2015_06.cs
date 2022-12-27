using static AdventOfCode._2021_05;

namespace AdventOfCode;


public class _2015_06 : Problem
{
    internal struct Instruction 
    {
        public bool Toggle;
        public bool On; 
        public int X0;
        public int Y0; 
        public int X1;
        public int Y1;
    }

    private Instruction[] _instructions;
    public override void Parse()
    {
        _instructions = Inputs.Select(l => Parse(l)).ToArray();
    }
    private static Instruction Parse(string line)
    {
        bool toggle = line.StartsWith("toggle");
        bool on = false;
        int[] coord;

        if (toggle)
            coord = line.ParseExact("toggle {0},{1} through {2},{3}").Select(e => int.Parse(e)).ToArray();
        else
        {
            string[] el = line.ParseExact("turn {0} {1},{2} through {3},{4}");
            on = el[0] == "on";
            coord = el.Skip(1).Select(i => int.Parse(i)).ToArray();
        }

        return new Instruction { Toggle = toggle, On = on, X0 = coord[0], Y0 = coord[1], X1 = coord[2], Y1 = coord[3] };
    }

    public override object PartOne()
    {
        bool[,] grid = new bool[1000, 1000];

        foreach (Instruction instruction in _instructions)
        {
            for (int x = instruction.X0; x <= instruction.X1; x++)
                for (int y = instruction.Y0; y <= instruction.Y1; y++)
                    grid[x, y] = instruction.Toggle ? !grid[x, y] : instruction.On;
        }

        return grid.Count(b => b);
    }

    public override object PartTwo()
    {
        int[,] grid = new int[1000, 1000];

        foreach (Instruction instruction in _instructions)
        {
            int d = instruction.Toggle ? 2 : instruction.On ? 1 : -1;
            for (int x = instruction.X0; x <= instruction.X1; x++)
                for (int y = instruction.Y0; y <= instruction.Y1; y++)
                    grid[x, y] = Math.Max(0, grid[x, y] + d);
        }

        return grid.Sum();
    }
}