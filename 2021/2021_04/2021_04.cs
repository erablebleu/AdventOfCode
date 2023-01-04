namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2021/day/04
/// </summary>
public class _2021_04 : Problem
{
    private const int BingoBoardSize = 5;
    private List<BingoBoard> _data;

    public override void Parse()
    {
        List<BingoBoard> boards = Inputs.Skip(2).Chunk(BingoBoardSize + 1).Select(data => new BingoBoard(data)).ToList();

        foreach (int value in Inputs.First().Split(",").Select(v => int.Parse(v)))
            boards.ForEach(b => b.Play(value));

        _data = boards.Where(b => b.Win).OrderBy(b => b.WinPlayCount).ToList();
    }

    public override object PartOne() => _data.First().Score;

    public override object PartTwo() => _data.Last().Score;

    private class BingoBoard
    {
        private int[,] _grid = new int[BingoBoardSize, BingoBoardSize];
        private bool[,] _marks = new bool[BingoBoardSize, BingoBoardSize];
        private int _playCount = 0;

        public BingoBoard(string[] lines)
        {
            for (int line = 0; line < BingoBoardSize; line++)
            {
                int[] el = lines[line].Split(" ").Where(v => !string.IsNullOrWhiteSpace(v)).Select(v => int.Parse(v)).ToArray();
                for (int column = 0; column < BingoBoardSize; column++)
                    _grid[line, column] = el[column];
            }
        }

        public int Score { get; private set; }
        public bool Win { get; private set; }
        public int WinPlayCount { get; private set; }

        public bool Play(int value)
        {
            if (Win)
                return false;

            _playCount++;

            for (int line = 0; line < BingoBoardSize; line++)
                for (int column = 0; column < BingoBoardSize; column++)
                    if (_grid[line, column] == value)
                    {
                        _marks[line, column] = true;

                        if (Enumerable.Range(0, BingoBoardSize).All(i => _marks[line, i])
                            || Enumerable.Range(0, BingoBoardSize).All(i => _marks[i, column]))
                        {
                            WinPlayCount = _playCount;
                            Win = true;
                            Score = Enumerable.Range(0, BingoBoardSize * BingoBoardSize)
                                              .Where(i => !_marks[i / BingoBoardSize, i % BingoBoardSize])
                                              .Sum(i => _grid[i / BingoBoardSize, i % BingoBoardSize])
                                    * value;
                        }

                        return true;
                    }

            return false;
        }
    }
}