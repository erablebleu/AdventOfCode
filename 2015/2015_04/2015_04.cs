using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode;

public class _2015_04 : Problem
{
    private byte[] _data;

    public override void Parse()
    {
        _data = Encoding.ASCII.GetBytes(Inputs[0]);
    }

    public override object PartOne() => Find(b => b[0] == 0 && b[1] == 0 && (b[2] & 0xF0) == 0);
    public override object PartTwo() => Find(b => b[0] == 0 && b[1] == 0 && b[2] == 0);

    private int Find(Func<byte[], bool> testFunction)
    {
        using MD5 md5 = MD5.Create();

        for (int i = 0; ; i++)
        {
            byte[] num = Encoding.ASCII.GetBytes(i.ToString());
            byte[] bytes = new byte[_data.Length + num.Length];
            Array.Copy(_data, bytes, _data.Length);
            Array.Copy(num, 0, bytes, _data.Length, num.Length);
            if (testFunction(md5.ComputeHash(bytes)))
                return i;
        }
    }
}