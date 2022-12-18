namespace AdventOfCode;

public class _2021_04 : Problem
{
    private const int BingoBoardSize = 5;
    private class BingoBoard
    {
        private int _playCount = 0;
        private bool[,] _marks = new bool[BingoBoardSize, BingoBoardSize];
        private int [,] _grid = new int[BingoBoardSize, BingoBoardSize];
        public bool Win { get; private set; }
        public int Score { get; private set; }
        public int WinPlayCount { get; private set; }

        public BingoBoard(string[] lines)
        {
            for(int line = 0; line < BingoBoardSize; line++)
            {
                int[] el = lines[line].Split(" ").Where(v => !string.IsNullOrWhiteSpace(v)).Select(v => int.Parse(v)).ToArray();
                for(int column=0; column< BingoBoardSize; column++)
                    _grid[line, column] = el[column];
            }
        }

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
    public override void Solve()
    {
        List<BingoBoard> boards = Inputs.Skip(2).Chunk(BingoBoardSize + 1).Select(data => new BingoBoard(data)).ToList();

        foreach(int value in Inputs.First().Split(",").Select(v => int.Parse(v)))
            boards.ForEach(b => b.Play(value));

        List<BingoBoard> winners = boards.Where(b => b.Win).OrderBy(b => b.WinPlayCount).ToList();

        Solutions.Add($"{winners.First().Score}"); 
        Solutions.Add($"{winners.Last().Score}");
    }
}