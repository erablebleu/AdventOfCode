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
    public class MultiRange
    {
        public List<_2022_04.Range> Ranges { get; set; } = new();
        public void Add(int start, int end)
        {
            _2022_04.Range range = new(start, end);
            _2022_04.Range rc = Ranges.FirstOrDefault(r => r.Cross(range));
            while(rc != null)
            {
                Ranges.Remove(rc);
                range = new _2022_04.Range(Math.Min(rc.Start, range.Start), Math.Max(rc.End, range.End));
                rc = Ranges.FirstOrDefault(r => r.Cross(range));
            }
            Ranges.Add(range);
        }
        public int? GetOutRange(int max)
        {
            foreach(_2022_04.Range r in Ranges)
            {
                if (r.Start - 1 > 0) return r.Start - 1;
                if(r.End + 1 < max) return r.End + 1;
            }
            return null;
        }
    }
    public override void Solve()
    {
        List<Sensor> sensors = Inputs.Select(l => new Sensor(l)).ToList();
        List<IPoint2D> beacons = sensors.Select(s => s.Beacon).ToList();

        int yTarget = 2000000;
        MultiRange mr = GetMr(sensors, yTarget);
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
    private static MultiRange GetMr(List<Sensor> sensors, int y)
    {
        MultiRange mr = new();

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