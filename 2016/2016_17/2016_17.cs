using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/17
/// </summary>
public class _2016_17 : Problem
{
    private static string[] DirectionKeys = new string[] { "U", "D", "L", "R" };
    private string _data;

    public override void Parse()
    {
        _data = Inputs[0];
    }

    public override object PartOne() => Emulate(_data);

    public override object PartTwo() => Emulate(_data, false);

    private static object Emulate(string data, bool findShortest = true)
    {
        using MD5 md5 = MD5.Create();
        HashSet<string> states = new();
        List<(string, IPoint2D)> list = new() { (data, new IPoint2D()) };
        int max = 0;
        int cnt = 0;

        while (list.Any())
        {
            cnt++;
            List<(string, IPoint2D)> next = new();
            foreach ((string passcode, IPoint2D position) in list)
            {
                string hash = BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(passcode))).Replace("-", string.Empty);
                for (int i = 0; i < 4; i++)
                {
                    IPoint2D p = position + IVector2D.DirectionNSEW[i];
                    if (!p.X.IsInRange(0, 4)
                        || !p.Y.IsInRange(0, 4)
                        || hash[i] < 'B'
                        || hash[i] > 'F')
                        continue;

                    string nextPasscode = passcode + DirectionKeys[i];

                    if (p.X == 3 && p.Y == 3)
                    {
                        max = cnt;
                        if (findShortest)
                            return nextPasscode.Substring(data.Length);
                    }
                    else
                        next.Add((nextPasscode, p));
                }
            }

            list = next;
        }

        return max;
    }
}