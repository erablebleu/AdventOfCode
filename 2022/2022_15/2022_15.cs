using AdventOfCode.Tools;
using static AdventOfCode._2022_15;

namespace AdventOfCode;

public class _2022_15 : Problem
{
    public class Sensor
    {
        public IPoint2D Position { get; set; }
        public IPoint2D Beacon { get; set; }
        public int Distance { get; set; }
        public Sensor(string line)
        {
            string[] el = line.Split(": closest beacon is at ");
            Position = GetPoint(el[0].Replace("Sensor at ", ""));
            Beacon = GetPoint(el[1]);
            Distance = Math.Abs(Position.X - Beacon.X) + Math.Abs(Position.Y - Beacon.Y);

        }
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
    public override void Solve()
    {
        List<Sensor> sensors = Inputs.Select(l => new Sensor(l)).ToList();
        List<IPoint2D> beacons = sensors.Select(s => s.Beacon).ToList();

        int yTarget = 2000000;
        IMultiRange mr = GetMr(sensors, yTarget);
        int bc = beacons.Where(b => b.Y == yTarget).Select(b => b.X).Distinct().Where(x => mr.Ranges.Any(r => r.Contain(x))).Count();
        Solutions.Add($"{mr.Ranges.Sum(r => r.End - r.Start + 1) - bc}");

        for(int y = 0; y < 4000000; y++)
        {
            mr = GetMr(sensors, y);
            int? x = mr.GetOutRange(4000000);
            if(x.HasValue)
            {
                Solutions.Add($"{GetTuningFrequency(x.Value, y)}");

                break;
            }
        }
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
    public static long GetTuningFrequency(long x, long y) => x * 4000000 + y;
}