namespace AdventOfCode.Tools;

public static class MathHelper
{
    public static BigInteger[] Fibonacci(long n)
    {
        BigInteger[] result = new BigInteger[n + 1];
        result[0] = 0;
        result[1] = 1;
        for (int j = 2; j <= n; j++)
            result[j] = result[j - 1] + result[j - 2];

        return result;
    }

    public static BigInteger Factorial(int n)
    {
        if (n == 0) return 1;
        BigInteger result = 1;
        for (int i = 1; i <= n; i++)
            result *= i;
        return result;
    }

    public static long GetHeptagonalNumber(long n) => n * (5 * n - 3) / 2;

    public static long GetHexagonalNumber(long n) => n * (2 * n - 1);

    public static long GetNextPrime(long value)
    {
        value++;
        value += value % 2 == 0 ? 1 : 0;
        while (!IsPrime(value))
            value++;
        return value;
    }

    public static long GetOctogonalNumber(long n) => n * (3 * n - 2);

    public static long GetPentagonalNumber(long n) => n * (3 * n - 1) / 2;

    public static long GetPolygonalNumber(int degree, long n) => degree switch
    {
        3 => GetTriangleNumber(n),
        4 => GetSquareNumber(n),
        5 => GetPentagonalNumber(n),
        6 => GetHexagonalNumber(n),
        7 => GetHeptagonalNumber(n),
        8 => GetOctogonalNumber(n),
        _ => throw new NotImplementedException(),
    };

    public static long GetSquareNumber(long n) => n * n;

    public static long GetTriangleNumber(long n) => n * (n + 1) / 2;

    public static bool IsPrime(long number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        var boundary = (long)Math.Floor(Math.Sqrt(number));

        for (long i = 3; i <= boundary; i += 2)
            if (number % i == 0)
                return false;

        return true;
    }

    public static bool IsSquare(long value) => IsSquare((decimal)value);

    public static bool IsSquare(decimal value)
    {
        decimal s = Sqrt(value);
        return s == ((int)s);
    }

    public static decimal Sqrt(decimal square)
    {
        if (square < 0) return 0;

        decimal root = square / 3;
        int i;
        for (i = 0; i < 32; i++)
            root = (root + square / root) / 2;
        return root;
    }

    public static decimal Square(decimal value) => value * value;
}