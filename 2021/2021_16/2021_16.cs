using System.Collections;

namespace AdventOfCode;

public static class BitArrayExtension
{
    public static BigInteger GetBigInteger(this BitArray array, int pos, int cnt)
    {
        BitArray tmpArray = new(cnt);

        for (int i = 0; i < cnt; i++)
            tmpArray[cnt - i - 1] = array[pos + i];

        byte[] byteArr = new byte[tmpArray.Length / 8 + (tmpArray.Length % 8 > 0 ? 1 : 0)];
        tmpArray.CopyTo(byteArr, 0);

        return new BigInteger(byteArr, true);
    }

    public static int GetInt(this BitArray array, int pos, int cnt)
    {
        BitArray tmpArray = new BitArray(cnt);
        for (int i = 0; i < cnt; i++)
            tmpArray[cnt - i - 1] = array[pos + i];

        int[] intArray = new int[1];
        tmpArray.CopyTo(intArray, 0);
        return intArray[0];
    }
}

internal class Packet
{
    public List<Packet> SubPackets { get; private set; } = new();
    public int Type { get; set; }
    public BigInteger Value { get; private set; }
    public int Version { get; set; }

    public static Packet Get(BitArray data)
    {
        int pos = 0;
        return Get(data, ref pos);
    }

    public static Packet Get(BitArray data, ref int pos)
    {
        int startPos = pos;
        Packet result = new();
        result.Version = data.GetInt(pos, 3);
        pos += 3;
        result.Type = data.GetInt(pos, 3);
        pos += 3;

        switch (result.Type)
        {
            case 4:
                List<bool> ls = new();
                while (data[pos])
                {
                    pos++;
                    for (int i = 0; i < 4; i++)
                        ls.Add(data[pos + i]);
                    pos += 4;
                }
                pos++;
                for (int i = 0; i < 4; i++)
                    ls.Add(data[pos + i]);
                pos += 4;

                result.Value = new BitArray(ls.ToArray()).GetBigInteger(0, ls.Count);

                //if (pos - startPos % 4 > 0)
                //    pos += 4 - (pos - startPos) % 4;
                break;

            default:
                pos++;
                if (data[pos - 1])
                {
                    int cnt = data.GetInt(pos, 11);
                    pos += 11;
                    for (int i = 0; i < cnt; i++)
                        result.SubPackets.Add(Packet.Get(data, ref pos));
                }
                else
                {
                    int size = data.GetInt(pos, 15);
                    pos += 15;
                    int tmpPos = pos;
                    while (pos < tmpPos + size)
                        result.SubPackets.Add(Packet.Get(data, ref pos));
                }
                break;
        }

        return result;
    }

    public BigInteger ComputeValue()
    {
        BigInteger result = 0;
        switch (Type)
        {
            case 0:
                foreach (Packet packet in SubPackets)
                    result += packet.ComputeValue();
                break;

            case 1:
                result = 1;
                foreach (Packet packet in SubPackets)
                    result *= packet.ComputeValue();
                break;

            case 2:
                result = SubPackets.First().ComputeValue();
                foreach (Packet packet in SubPackets.Skip(1))
                {
                    BigInteger value = packet.ComputeValue();
                    if (value < result)
                        result = value;
                }
                break;

            case 3:
                result = SubPackets.First().ComputeValue();
                foreach (Packet packet in SubPackets.Skip(1))
                {
                    BigInteger value = packet.ComputeValue();
                    if (value > result)
                        result = value;
                }
                break;

            case 4: return Value;
            case 5: return SubPackets[0].ComputeValue() > SubPackets[1].ComputeValue() ? 1 : 0;
            case 6: return SubPackets[0].ComputeValue() < SubPackets[1].ComputeValue() ? 1 : 0;
            case 7: return SubPackets[0].ComputeValue() == SubPackets[1].ComputeValue() ? 1 : 0;
        }

        return result;
    }

    public void Log(string prefix = "")
    {
        Console.WriteLine(prefix + this.ToString());
        foreach (Packet packet in SubPackets)
            packet.Log("   " + prefix);
    }

    public int RecursiveSum(Func<Packet, int> predicate) => predicate.Invoke(this) + SubPackets.Sum(p => p.RecursiveSum(predicate));

    public override string ToString() => $"V={Version:D1} Type={Type:D1} Value={Value}";
}

/* FORMAT
 *
 * VVVTTT
 *
 * VVV : 3 bits - integer - Version
 * TTT : 3 bits - integer - Type
 *
 *
 * Type 4 : literal value of 4 bits groups
 * group format : GXXXX
 * G = 1 -> not the last group
 * G = 0 -> last group
 * XXXX : value
 *
 * Type != 4 : operator
 * format : ILLLLLLLLLLLLLLLAAAAAAAAAAABBBBBBBBBBBBBBBB
 * I = 0 -> next 15 bits L are size au rest of packet
 * I = 1 -> next 11 bits L are count of 11 size sub-packet
 */

/// <summary>
/// https://adventofcode.com/2021/day/16
/// </summary>
public class _2021_16 : Problem
{
    private Packet _data;

    public override void Parse()
    {
        BitArray data = GetBitArray(Inputs.First());
        //BitArray data = GetBitArray("04005AC33890");
        _data = Packet.Get(data);
        //_data.Log("|- ");
    }

    public override object PartOne() => _data.RecursiveSum(p => p.Version);

    public override object PartTwo() => _data.ComputeValue();

    private static BitArray GetBitArray(string hexValue)
    {
        byte[] bytes = new byte[hexValue.Length / 2];

        for (int i = 0; i < hexValue.Length; i += 2)
            bytes[i / 2] = byte.Parse(hexValue.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);

        BitArray array = new(bytes.Reverse().ToArray());
        bool[] boolArray = new bool[array.Length];
        array.CopyTo(boolArray, 0);
        return new BitArray(boolArray.Reverse().ToArray());
    }
}