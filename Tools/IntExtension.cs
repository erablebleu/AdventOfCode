﻿using System.Runtime.CompilerServices;

namespace AdventOfCode.Tools;

public static class IntExtension
{
    public static int Loop(this int value, int min, int size)
    {
        if (value < min) return min + size - 1 + (value - min + 1) % size;
        if (value >= min + size) return min + (value - min - size) % size;
        return value;
    }

    public static int Normalize(this int value, int min, int size)
    {
        if (value < min) return min;
        if (value >= min + size) return min + size - 1;
        return value;
    }

    public static bool GetBit(this int value, int bitIdx) => (value & 1 << bitIdx) != 0;
    public static int SetBit(this int value, int bitIdx, bool state) => state ? SetBit(value, bitIdx) : ResetBit(value, bitIdx);
    public static int SetBit(this int value, int bitIdx) => value | (1 << bitIdx);
    public static int SetBits(this int value, params int[] bitIdx)
    {
        foreach(int idx in bitIdx)
            value = value.SetBit(idx);
        return value;
    }
    public static int ResetBit(this int value, int bitIdx) => value & ~(1 << bitIdx);
    public static bool IsInRange(this int value, int min, int size) => value >= min && value < min + size;
}

public static class LongExtension
{
    public static bool GetBit(this long value, int bitIdx) => (value & 1L << bitIdx) != 0;
    public static long SetBit(this long value, int bitIdx, bool state) => state ? SetBit(value, bitIdx) : ResetBit(value, bitIdx);
    public static long SetBit(this long value, int bitIdx) => value | (1L << bitIdx);
    public static long SetBits(this long value, params int[] bitIdx)
    {
        foreach (int idx in bitIdx)
            value = value.SetBit(idx);
        return value;
    }
    public static long ResetBit(this long value, int bitIdx) => value & ~(1L << bitIdx);
}