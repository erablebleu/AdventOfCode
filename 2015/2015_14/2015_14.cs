namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/14
/// </summary>
public class _2015_14 : Problem
{
    private Reindeer[] _data;

    public override void Parse()
    {
        _data = new Reindeer[Inputs.Length];
        for (int i = 0; i < Inputs.Length; i++)
        {
            string[] el = Inputs[i].ParseExact("{0} can fly {1} km/s for {2} seconds, but then must rest for {3} seconds.");
            int[] values = el.Skip(1).Select(e => int.Parse(e)).ToArray();
            _data[i] = new Reindeer { Name = el[0], Speed = values[0], FlyTime = values[1], RestTime = values[2] };
        }
    }

    public override object PartOne() => _data.Max(r => r.GetDistance(2503));

    public override object PartTwo()
    {
        int[] points = new int[_data.Length];
        for (int i = 1; i <= 2503; i++)
        {
            int[] dists = _data.Select(r => r.GetDistance(i)).ToArray();
            int max = dists.Max();
            for (int j = 0; j < _data.Length; j++)
                if (dists[j] == max)
                    points[j]++;
        }
        return points.Max();
    }

    private class Reindeer
    {
        public int FlyTime;
        public string Name;
        public int RestTime;
        public int Speed;

        public int GetDistance(int time)
        {
            int cycleCnt = time / (FlyTime + RestTime);
            int result = cycleCnt * Speed * FlyTime;
            time = time % (FlyTime + RestTime);
            result += Math.Min(time, FlyTime) * Speed;
            return result;
        }

        public override string ToString() => $"{Name} can fly {Speed} km/s for {FlyTime} seconds, but then must rest for {RestTime} seconds.";
    }
}