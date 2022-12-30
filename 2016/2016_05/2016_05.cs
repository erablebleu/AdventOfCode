using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/05
/// </summary>
public class _2016_05 : Problem
{
    private byte[] _data;

    public override void Parse()
    {
        _data = Encoding.ASCII.GetBytes(Inputs[0]);
    }

    public override object PartOne()
    {
        using MD5 md5 = MD5.Create();
        char[] result = new char[8];

        for (int i = 0, idx = 0; idx < result.Length; i++)
        {
            byte[] hash = md5.ComputeHash(ConCat(_data, Encoding.ASCII.GetBytes(i.ToString())));

            if (hash[0] != 0 || hash[1] != 0 || (hash[2] & 0xF0) != 0)
                continue;

            result[idx] = hash[2].ToString("X")[0];
            idx++;
        }

        return new string(result);
    }

    public override object PartTwo()
    {
        using MD5 md5 = MD5.Create();
        char[] result = new char[8];

        for (int i = 0, idx = 0; idx < result.Length; i++)
        {
            byte[] hash = md5.ComputeHash(ConCat(_data, Encoding.ASCII.GetBytes(i.ToString())));

            if (hash[0] != 0 || hash[1] != 0 || hash[2] >= 8 || result[hash[2]] != 0)
                continue;

            result[hash[2]] = (hash[3] >> 4).ToString("X")[0];
            idx++;
        }

        return new string(result);
    }

    private static byte[] ConCat(byte[] arrA, byte[] arrB)
    {
        byte[] result = new byte[arrA.Length + arrB.Length];
        Array.Copy(arrA, result, arrA.Length);
        Array.Copy(arrB, 0, result, arrA.Length, arrB.Length);
        return result;
    }
}