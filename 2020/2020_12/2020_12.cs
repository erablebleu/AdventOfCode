namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/12
/// </summary>
public class _2020_12 : Problem
{
    private record NavigationInstruction(char Instruction, int Value);
    private List<NavigationInstruction> _data;

    public override void Parse()
    {
        _data = Inputs.Select(i => new NavigationInstruction(i[0], int.Parse(i[1..]))).ToList();
    }

    public override object PartOne()
    {
        MapPoint ship = new MapPoint();

        foreach (var d in _data)
        {
            switch (d.Instruction)
            {
                case 'N': ship.Y += d.Value; break;
                case 'S': ship.Y -= d.Value; break;
                case 'E': ship.X += d.Value; break;
                case 'W': ship.X -= d.Value; break;
                case 'R': ship.Teta -= d.Value * Math.PI / 180; break;
                case 'L': ship.Teta += d.Value * Math.PI / 180; break;
                case 'F':
                    ship.X += d.Value * Math.Cos(ship.Teta);
                    ship.Y += d.Value * Math.Sin(ship.Teta);
                    break;
            }
        }

        return (int)Math.Round(Math.Abs(ship.X) + Math.Abs(ship.Y));
    }

    public override object PartTwo()
    {
        MapPoint ship = new MapPoint();
        MapPoint wayPoint = new MapPoint { X = 10, Y = 1 };

        foreach (var d in _data)
        {
            switch (d.Instruction)
            {
                case 'N': wayPoint.Y += d.Value; break;
                case 'S': wayPoint.Y -= d.Value; break;
                case 'E': wayPoint.X += d.Value; break;
                case 'W': wayPoint.X -= d.Value; break;
                case 'R':
                case 'L':
                    double angle = d.Value * Math.PI / 180 * (d.Instruction == 'R' ? -1.0 : 1.0);
                    double cos = Math.Cos(angle);
                    double sin = Math.Sin(angle);
                    wayPoint = new MapPoint
                    {
                        X = cos * wayPoint.X - sin * wayPoint.Y,
                        Y = sin * wayPoint.X + cos * wayPoint.Y
                    };
                    break;

                case 'F':
                    ship.X += d.Value * wayPoint.X;
                    ship.Y += d.Value * wayPoint.Y;
                    break;
            }
            //Console.WriteLine($"{d.Instruction}{d.Value} => ship:{ship.X},{ship.Y}    wayPoint:{wayPoint.X},{wayPoint.Y}");
        }

        return (int)Math.Round(Math.Abs(ship.X) + Math.Abs(ship.Y));
    }

    private class MapPoint
    {
        public double Teta { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}