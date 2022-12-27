namespace AdventOfCode;

public class _2015_10 : Problem
{
    private byte[] _data;

    public override void Parse()
    {
        _data = Inputs[0].Select(c => (byte)(c - 48)).ToArray();
    }

    public override object PartOne() => Emulate(_data, 40);

    public override object PartTwo() => Emulate(_data, 50);

    private static long Emulate(byte[] array, int count)
    {
        for (int c = 0; c < count; c++)
        {
            List<byte> next = new();
            for (int i = 0; i < array.Length; i++)
            {
                int cnt;
                for (cnt = 1; i + cnt < array.Length && array[i] == array[i + cnt]; cnt++) ;
                for (int j = cnt.ToString().Length - 1; j >= 0; j--)
                    next.Add((byte)(cnt / Math.Pow(10, j)));
                next.Add(array[i]);
                i += cnt - 1;
            }
            array = next.ToArray();
        }
        return array.Length;
    }
}