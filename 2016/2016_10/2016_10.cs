namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/10
/// </summary>
public class _2016_10 : Problem
{
    private Dictionary<int, Bot> _bots = new();
    private Dictionary<int, Bot> _outputs = new();

    public override void Parse()
    {
        _bots = new();
        _outputs = new();
        foreach (string instruction in Inputs.Where(l => l.StartsWith("value")))
        {
            int[] el = instruction.ParseExact("value {0} goes to bot {1}").Select(e => int.Parse(e)).ToArray();
            Bot bot = _bots.GetOrAdd(el[1], () => new Bot());
            bot.Values.Add(el[0]);
        }
        foreach (string instruction in Inputs.Where(l => l.StartsWith("bot")))
        {
            string[] el = instruction.ParseExact("bot {0} gives low to {1} {2} and high to {3} {4}");
            Bot src = _bots.GetOrAdd(int.Parse(el[0]), () => new Bot());
            src.LowTo = (el[1] == "bot" ? _bots : _outputs).GetOrAdd(int.Parse(el[2]), () => new Bot());
            src.HighTo = (el[3] == "bot" ? _bots : _outputs).GetOrAdd(int.Parse(el[4]), () => new Bot());
        }
        int moveCnt;
        do
        {
            moveCnt = 0;
            foreach (KeyValuePair<int, Bot> kv in _bots)
            {
                Bot bot = kv.Value;
                if (bot.Values.Count < 2)
                    continue;

                if (!bot.LowTo.Values.Contains(bot.Low))
                {
                    bot.LowTo.Values.Add(bot.Low);
                    moveCnt++;
                }
                if (!bot.HighTo.Values.Contains(bot.High))
                {
                    bot.HighTo.Values.Add(bot.High);
                    moveCnt++;
                }
            }
        }
        while (moveCnt > 0);
    }

    public override object PartOne() => _bots.First(kv => kv.Value.Low == 017 && kv.Value.High == 61).Key;

    public override object PartTwo() => Enumerable.Range(0, 3).SelectMany(i => _outputs[i].Values).Product();

    private class Bot
    {
        public Bot HighTo;
        public Bot LowTo;
        public List<int> Values = new();
        public int High => Values.Max();
        public int Low => Values.Min();
    }
}