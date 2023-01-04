namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2020/day/25
/// </summary>
public class _2020_25 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne()
    {
        long cardPublicKey = long.Parse(Inputs[0]);
        long doorPublicKey = long.Parse(Inputs[1]);

        //cardPublicKey = 5764801;
        //doorPublicKey = 17807724;

        int doorLoopSize = GetLoopSize(doorPublicKey);

        return GetTransformedKey(cardPublicKey, doorLoopSize);
    }

    public override object PartTwo() => "Merry Christmas!";

    private static int GetLoopSize(long value, int subjectNumber = 7)
    {
        long res = 1;
        int i;

        for (i = 0; res != value; i++)
            res = res * subjectNumber % 20201227;

        return i;
    }

    private static long GetTransformedKey(long subjectNumber, long loopSize)
    {
        long res = 1;

        for (int i = 0; i < loopSize; i++)
            res = res * subjectNumber % 20201227;

        return res;
    }
}