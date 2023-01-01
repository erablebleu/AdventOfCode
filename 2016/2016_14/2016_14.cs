using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/14
/// </summary>
public class _2016_14 : Problem
{
    private static MD5 _md5 = MD5.Create();
    private byte[] _data;

    public override void Parse()
    {
        _data = Encoding.ASCII.GetBytes(Inputs[0]);
    }

    public override object PartOne() => GetNthKey(_data, 64);

    public override object PartTwo() => GetNthKey(_data, 64, 2016);

    private static bool Contains(byte[] hash, int cnt, byte c)
    {
        for (int i = 0; i < hash.Length - cnt + 1; i++)
        {
            bool same = true;
            for (int j = 0; j < cnt && same; j++)
                if (hash[i + j] != c)
                    same = false;

            if (same)
                return true;
        }
        return false;
    }

    private static byte[] GetHash(byte[] data) => GetHexString(_md5.ComputeHash(data));

    private static byte[] GetHexString(byte[] bytes)
    {
        byte[] result = new byte[bytes.Length * 2];
        byte GetChar(int b) => b < 10 ? (byte)(48 + b) : (byte)(97 + b - 10);
        for (int i = 0; i < bytes.Length; i++)
        {
            result[2 * i] = GetChar(bytes[i] >> 4);
            result[2 * i + 1] = GetChar(bytes[i] & 0xF);
        }
        return result;
    }

    private static int GetNthKey(byte[] data, int index, int stretchCount = 0)
    {
        Dictionary<int, byte[]> hashMap = new();
        List<int> keys = new();

        for (int i = 0; keys.Count < index; i++)
        {
            byte[] hash = GetStretchHash(data, i, stretchCount);
            hashMap.Add(i, hash);

            if (i > 1000 && IsKey(hashMap[i - 1001], 3, out byte c))
            {
                for (int j = i - 1000; j <= i; j++)
                    if (Contains(hashMap[j], 5, c))
                    {
                        keys.Add(i - 1001);
                        break;
                    }
            }
        }

        return keys.Last();
    }

    private static byte[] GetStretchHash(byte[] data, int idx, int cnt)
    {
        byte[] hash = GetHash(data.Concat(Encoding.ASCII.GetBytes(idx.ToString())));
        for (int i = 0; i < cnt; i++)
            hash = GetHash(hash);

        return hash;
    }

    private static bool IsKey(byte[] hash, int cnt, out byte c)
    {
        for (int i = 0; i < hash.Length - cnt + 1; i++)
        {
            bool same = true;
            for (int j = 1; j < cnt && same; j++)
                if (hash[i] != hash[i + j])
                    same = false;
            if (!same)
                continue;

            c = hash[i];
            return true;
        }
        c = 0;
        return false;
    }
}