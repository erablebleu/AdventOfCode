namespace AdventOfCode.Tools;

public static class IntExtension
{
    public static int Loop(this int value, int min, int max)
    {
        if (value < min) return max;
        if (value > max) return min;
        return value;
    }

    public static int Normalize(this int value, int min, int max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
}