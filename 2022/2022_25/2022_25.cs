namespace AdventOfCode;

public class _2022_25 : Problem
{
    public override void Solve()
    {
        BigInteger result = 0;
        foreach (string line in Inputs)
            result += SNAFUNumber.GetDecimal(line);
        AddSolution(SNAFUNumber.GetSNAFU(result));
    }

    public class SNAFUNumber
    {
        public static BigInteger GetDecimal(string value)
        {
            BigInteger result = 0;
            for(int i = 0; i < value.Length; i++)
            {
                result += (BigInteger)Math.Pow(5, value.Length - i - 1) * value[i] switch
                {
                    '2' => 2,
                    '1' => 1,
                    '-' => -1,
                    '=' => -2,
                    _ => 0,
                };
            }

            return result;
        }

        public static string GetSNAFU(BigInteger value)
        {
            List<char> result = new();
            while(value > 0)
            {
                BigInteger digit = value % 5;
                if (digit < 3)
                    result.Insert(0, (char)(48 + digit));
                else
                {
                    result.Insert(0, digit == 3 ? '=' : '-'); 
                    value += 5;
                }
                value /= 5;
            }
            return new string(result.ToArray());
        }
    }
}