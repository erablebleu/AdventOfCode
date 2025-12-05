namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/15
/// </summary>
public class _2022_15 : Problem
{
    private List<Sensor> _sensors;

    public override void Parse()
    {
        _sensors = Inputs.Select(l => new Sensor(l)).ToList();
    }

    public override object PartOne()
    {
        List<IPoint2D> beacons = _sensors.Select(s => s.Beacon).ToList();

        int yTarget = 2000000;
        IMultiRange mr = GetMr(_sensors, yTarget);
        int bc = beacons.Where(b => b.Y == yTarget).Select(b => b.X).Distinct().Where(x => mr.Ranges.Any(r => r.Contains(x))).Count();
        return mr.Ranges.Sum(r => r.End - r.Start + 1) - bc;
    }

    public override object PartTwo()
    {
        for (int y = 0; y < 4000000; y++)
        {
            IMultiRange mr = GetMr(_sensors, y);
            int? x = mr.GetOutRange(4000000);
            if (x.HasValue)
                return GetTuningFrequency(x.Value, y);
        }

        return null;
    }

    private static IMultiRange GetMr(List<Sensor> sensors, int y)
    {
        IMultiRange mr = new();

        foreach (Sensor sensor in sensors)
        {
            if (!sensor.Reach(y))
                continue;
            int cnt = sensor.Distance - Math.Abs(sensor.Position.Y - y);
            mr.Add(sensor.Position.X - cnt, sensor.Position.X + cnt);
        }
        return mr;
    }

    private static long GetTuningFrequency(long x, long y) => x * 4000000 + y;

    private class Sensor
    {
        public Sensor(string line)
        {
            string[] el = line.Split(": closest beacon is at ");
            Position = GetPoint(el[0].Replace("Sensor at ", ""));
            Beacon = GetPoint(el[1]);
            Distance = Math.Abs(Position.X - Beacon.X) + Math.Abs(Position.Y - Beacon.Y);
        }

        public IPoint2D Beacon { get; set; }
        public int Distance { get; set; }
        public IPoint2D Position { get; set; }

        public static IPoint2D GetPoint(string value)
        {
            string[] el = value.Split(", ");
            return new IPoint2D(int.Parse(el[0].Replace("x=", "")), int.Parse(el[1].Replace("y=", "")));
        }

        public bool Reach(int y)
        {
            return Math.Abs(Position.Y - y) <= Distance;
        }
    }
}