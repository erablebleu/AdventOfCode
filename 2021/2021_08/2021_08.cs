namespace AdventOfCode;

public class _2021_08 : Problem
{
    [Flags]
    internal enum Segments
    {
        a = 0x0001,
        b = 0x0002,
        c = 0x0004,
        d = 0x0008,
        e = 0x0010,
        f = 0x0020,
        g = 0x0040,

        N0 = a | b | c | e |f | g,
        N1 = c | f,
        N2 = a | c | d | e | g,
        N3 = a | c | d | f | g,
        N4 = b | c | d | f,
        N5 = a | b | d | f | g,
        N6 = a | b | d | e | f | g,
        N7 = a | c | f,
        N8 = a | b | c | d | e | f | g,
        N9 = a | b | c | d | f | g,
    }

    internal static Dictionary<Segments, int> Numbers = new()
    {
        { Segments.N0, 0 },
        { Segments.N1, 1 },
        { Segments.N2, 2 },
        { Segments.N3, 3 },
        { Segments.N4, 4 },
        { Segments.N5, 5 },
        { Segments.N6, 6 },
        { Segments.N7, 7 },
        { Segments.N8, 8 },
        { Segments.N9, 9 },
    };

    public override void Solve()
    {
        SignalPattern[] signalPatterns = Inputs.Select(i => new SignalPattern(i)).ToArray();
        Solutions.Add($"{signalPatterns.Sum(sp => sp.Value.Count(v => v.Length == 2 || v.Length == 4 || v.Length == 3 || v.Length == 7))}");
        Solutions.Add($"{signalPatterns.Sum(sp => sp.Result)}");
    }

    internal class SignalPattern
    {
        private static Dictionary<char, char>[] _maps;
        private Dictionary<char, char> _map;

        static SignalPattern()
        {
            char[] values = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
            List<string> combinations = new() { string.Empty };

            for(int i = 0; i < values.Length; i++)
            {
                List<string> next = new();
                foreach (string val in combinations)
                {
                    foreach (char c in values)
                    {
                        if (val.Contains(c))
                            continue;
                        next.Add($"{val}{c}");
                    }
                }
                combinations = next;
            }

            _maps = combinations.Select(c => Enumerable.Range(0, values.Length).ToDictionary(i => values[i], i => c[i])).ToArray();
        }

        public SignalPattern(string line)
        {
            Entry = line;
            string[] el = line.Split(" | ");
            Patterns = el[0].Split(" ").ToArray();
            Value = el[1].Split(" ").ToArray();

            Decode();
        }

        public string Entry { get; private set; }

        public string[] Patterns { get; private set; }

        public int Result { get; private set; }

        public string[] Value { get; private set; }

        public void Decode()
        {
            _map = _maps.First(m => Patterns.All(p => Numbers.ContainsKey(GetSegments(Transpose(p, m)))));
            Result = int.Parse(string.Concat(Value.Select(p => GetSegments(Transpose(p, _map))).Select(s => $"{Numbers[s]}")));
        }

        private static Segments GetSegments(string pattern) => (Segments)Enum.Parse(typeof(Segments), string.Join<char>(",", pattern));
        private static string Transpose(string value, Dictionary<char, char> map)
        {
            string result = string.Empty;
            foreach (char c in value)
                result += map[c];
            return result;
        }
    }
}