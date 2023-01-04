namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/11
/// </summary>
public class _2020_11 : Problem
{
    private List<SeatState> _data;

    public override void Parse()
    {
        int width = Inputs[0].Length;
        int height = Inputs.Length;
        _data = new List<SeatState>();

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                _data.Add(new SeatState { X = x, Y = y, InitState = Inputs[y][x] });

        _data.ForEach(d => d.Adj = _data.Where(n => Math.Abs(n.X - d.X) <= 1 && Math.Abs(n.Y - d.Y) <= 1 && (d.X != n.X || d.Y != n.Y)).ToList());
        _data.ForEach(d => d.GetEye(_data));
    }

    public override object PartOne()
    {
        string state = string.Concat(Inputs);
        int step = 0;
        _data.ForEach(d => d.Reset());
        string newState = new string(_data.Select(d => d.State).ToArray());
        do
        {
            state = newState;
            step++;

            _data.Where(d => d.State == 'L' && d.Adj.All(n => n.State != '#')).ToList().ForEach(d => d.NewState = '#');
            _data.Where(d => d.State == '#' && d.Adj.Count(n => n.State == '#') >= 4).ToList().ForEach(d => d.NewState = 'L');

            _data.ForEach(d => d.State = d.NewState);
            newState = new string(_data.Select(d => d.State).ToArray());
        }
        while (newState != state);

        return newState.Count(c => c == '#');
    }

    public override object PartTwo()
    {
        string state = string.Concat(Inputs);
        int step = 0;
        _data.ForEach(d => d.Reset());
        string newState = new string(_data.Select(d => d.State).ToArray());
        do
        {
            state = newState;
            step++;

            _data.Where(d => d.State == 'L' && d.AdjEye.All(n => n.State != '#')).ToList().ForEach(d => d.NewState = '#');
            _data.Where(d => d.State == '#' && d.AdjEye.Count(n => n.State == '#') >= 5).ToList().ForEach(d => d.NewState = 'L');

            _data.ForEach(d => d.State = d.NewState);
            newState = new string(_data.Select(d => d.State).ToArray());
        }
        while (newState != state);
        return newState.Count(c => c == '#');
    }

    private record SeatState
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char InitState { get; set; }
        public char State { get; set; }
        public char NewState { get; set; }
        public List<SeatState> Adj { get; set; } = new List<SeatState>();
        public List<SeatState> AdjEye { get; set; } = new List<SeatState>();
        public void Reset()
        {
            State = InitState;
            NewState = InitState;
        }
        public void GetEye(List<SeatState> _data)
        {
            List<List<SeatState>> tmp = new List<List<SeatState>>();

            tmp.Add(_data.Where(n => n.Y == Y && n.X < X).ToList());
            tmp.Add(_data.Where(n => n.Y == Y && n.X > X).ToList());
            tmp.Add(_data.Where(n => n.X == X && n.Y < Y).ToList());
            tmp.Add(_data.Where(n => n.X == X && n.Y > Y).ToList());
            tmp.Add(_data.Where(n => X - n.X == Y - n.Y && X < n.X).ToList());
            tmp.Add(_data.Where(n => X - n.X == Y - n.Y && X > n.X).ToList());
            tmp.Add(_data.Where(n => X - n.X == n.Y - Y && X < n.X).ToList());
            tmp.Add(_data.Where(n => X - n.X == n.Y - Y && X > n.X).ToList());

            AdjEye = tmp.Select(l => l.OrderBy(n => Math.Abs(n.X - X) + Math.Abs(n.Y - Y)).FirstOrDefault(n => n.InitState != '.')).ToList();

            AdjEye.RemoveAll(l => l is null);
        }
    }
}