namespace AdventOfCode
{
    public class _2020_25 : Problem
    {
        #region Methods

        public override void Solve()
        {
            long cardPublicKey = long.Parse(Inputs[0]);
            long doorPublicKey = long.Parse(Inputs[1]);

            //cardPublicKey = 5764801;
            //doorPublicKey = 17807724;

            int cardLoopSize = GetLoopSize(cardPublicKey);
            int doorLoopSize = GetLoopSize(doorPublicKey);

            long cardEncryptionKey = GetTransformedKey(cardPublicKey, doorLoopSize);
            long doorEncryptionKey = GetTransformedKey(doorPublicKey, cardLoopSize);

            Solutions.Add($"{cardEncryptionKey}");
        }

        public long GetTransformedKey(long subjectNumber, long loopSize)
        {
            long res = 1;

            for(int i =0; i < loopSize; i++)
                res = (res * subjectNumber) % 20201227;

            return res;
        }
        public int GetLoopSize(long value, int subjectNumber = 7)
        {
            long res = 1;
            int i;

            for (i = 0; res != value; i++)
                res = (res * subjectNumber) % 20201227;

            return i;
        }

        #endregion
    }
}