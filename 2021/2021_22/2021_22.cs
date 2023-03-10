namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2021/day/22
/// </summary>
public class _2021_22 : Problem
{
    private Cuboid[] _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => new Cuboid(l)).ToArray();
    }

    public override object PartOne()
    {
        Dictionary<(int x, int y, int z), bool> states = new();

        foreach (Cuboid i in _data)
        {
            if (i.X.Cross(-50, 50) && i.Y.Cross(-50, 50) && i.Z.Cross(-50, 50))
                for (int x = Math.Max(i.X.From, -50); x <= Math.Min(i.X.To, 50); x++)
                    for (int y = Math.Max(i.Y.From, -50); y <= Math.Min(i.Y.To, 50); y++)
                        for (int z = Math.Max(i.Z.From, -50); z <= Math.Min(i.Z.To, 50); z++)
                            states[(x, y, z)] = i.IsOn;
        }

        return states.Values.Count(v => v);
    }

    public override object PartTwo()
    {
        List<Cuboid> resulting = new();

        foreach (Cuboid cube in _data)
        {
            List<Cuboid> next = new();
            if (cube.IsOn)
                next.Add(cube);
            foreach (Cuboid cube2 in resulting)
            {
                Cuboid inter = cube.Intersect(cube2);

                if (inter is null)
                    continue;

                next.Add(inter);
            }
            resulting.AddRange(next);
        }

        return resulting.Sum(c => c.IsOn ? c.GetVolume() : -c.GetVolume());
    }

    private class Cuboid
    {
        public Cuboid()
        { }

        public Cuboid(string line)
        {
            IsOn = line[1] == 'n';
            line = line.Substring(line.IndexOf(' ') + 1);
            string[] el = line.Split(',').Select(l => l.Substring(2)).ToArray();
            X = GetRange(el[0]);
            Y = GetRange(el[1]);
            Z = GetRange(el[2]);
        }

        public bool IsOn { get; set; }

        public Range X { get; set; }

        public Range Y { get; set; }

        public Range Z { get; set; }

        public long GetVolume() => X.GetDist() * Y.GetDist() * Z.GetDist();

        public Cuboid Intersect(Cuboid cube)
        {
            if (cube.X.From > X.To
               || cube.Y.From > Y.To
               || cube.Z.From > Z.To
               || cube.X.To < X.From
               || cube.Y.To < Y.From
               || cube.Z.To < Z.From)
                return null;

            return new Cuboid
            {
                X = new(Math.Max(cube.X.From, X.From), Math.Min(cube.X.To, X.To)),
                Y = new(Math.Max(cube.Y.From, Y.From), Math.Min(cube.Y.To, Y.To)),
                Z = new(Math.Max(cube.Z.From, Z.From), Math.Min(cube.Z.To, Z.To)),
                IsOn = !cube.IsOn,
            };
        }

        public override string ToString() => $"x={X},y={Y},z={Z}";

        private static Range GetRange(string line)
        {
            int[] el = line.Split("..").Select(e => int.Parse(e)).ToArray();
            return new Range(el[0], el[1]);
        }

        public class Range
        {
            public Range(int from, int to)
            {
                From = from;
                To = to;
            }

            public int From { get; set; }
            public int To { get; set; }

            public bool Cross(int min, int max) => From >= min && From <= max || To >= min && To <= max || From <= min && To >= max;

            public long GetDist() => To - From + 1;

            public override string ToString() => $"{From}..{To}";
        }
    }
}