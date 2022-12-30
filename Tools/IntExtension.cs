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
}