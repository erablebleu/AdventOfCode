namespace AdventOfCode.Tools;

public static class IntExtension
{
    public static int Loop(this int value, int min, int max)
    {
        if (value < min) return max + (value - min + 1) % (max - min + 1);
        if (value > max) return min + (value - max - 1) % (max - min + 1);
        return value;
    }

    public static int Normalize(this int value, int min, int max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
}