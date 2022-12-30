namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/04
/// </summary>
public class _2016_04 : Problem
{
    private (string Encrypted, int SectorId, string Checksum)[] _data;

    public override void Parse()
    {
        _data = Inputs.Select(l => Parse(l)).ToArray();
    }

    public override object PartOne() => _data.Where(r => GetChecksum(r.Encrypted) == r.Checksum).Sum(r => r.SectorId);

    public override object PartTwo() => _data.FirstOrDefault(r => Decrypt(r).Contains("northpole")).SectorId;

    private static string Decrypt((string Encrypted, int SectorId, string Checksum) room) => Decrypt(room.Encrypted, room.SectorId);

    private static string Decrypt(string input, int value)
    {
        value %= 26;
        return new string(input.Select(c => c == '-' ? ' ' : (char)(c + value).Loop(97, 26)).ToArray());
    }

    private static string GetChecksum(string data)
        => new(data.Replace("-", "").Distinct().Select(c => (c, data.Count(a => a == c))).OrderByDescending(k => k.Item2).ThenBy(k => k.c).Take(5).Select(k => k.c).ToArray());

    private static (string encrypted, int sectorId, string checksum) Parse(string input)
    {
        string[] el = input.ParseExact("{0}[{1}]");
        return (el[0].Substring(0, el[0].Length - 4), int.Parse(el[0].Substring(el[0].Length - 3)), el[1]);
    }
}