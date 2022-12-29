namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/22
/// </summary>
public class _2015_22 : Problem
{
    /// <summary>
    /// [0]: cost
    /// [1]: duration
    /// [2]: damage
    /// [3]: heal
    /// [4]: armor
    /// [5]: mana regen
    /// </summary>
    private static readonly int[][] _spells = new int[][]
    {
        new int[] { 53, 0, 4, 0, 0, 0 },        // Magic Missile
        new int[] { 73, 0, 2, 2, 0, 0 },        // Drain
        new int[] { 113, 6, 0, 0, 7, 0 },       // Shield
        new int[] { 173, 6, 3, 0, 0, 0 },       // Poison
        new int[] { 229, 5, 0, 0, 0, 101 },     // Recharge
    };

    private int[] _boss;

    public override void Parse()
    {
        _boss = Inputs.Select(l => int.Parse(l.Split(": ")[1])).ToArray();
    }

    public override object PartOne() => Emulate(true, 0, 50, 500, _boss[0], _boss[1], new int[6, 5]);

    public override object PartTwo() => Emulate(true, 1, 50, 500, _boss[0], _boss[1], new int[6, 5]);

    /// <summary>
    /// </summary>
    /// <param name="myTurn"></param>
    /// <param name="lifeLoss"></param>
    /// <param name="life"></param>
    /// <param name="mana"></param>
    /// <param name="bosslife"></param>
    /// <param name="bossdamage"></param>
    /// <param name="effects">
    /// Current effects
    /// [0]: damage
    /// [1]: heal
    /// [2]: armor
    /// [3]: mana regen
    /// [4]: effect key
    /// </param>
    /// <returns></returns>
    private static int? Emulate(bool myTurn, int lifeLoss, int life, int mana, int bosslife, int bossdamage, int[,] effects)
    {
        bosslife -= effects[0, 0];
        life += effects[0, 1];
        mana += effects[0, 3];

        if (bosslife <= 0)
            return 0;

        if (!myTurn)
        {
            life -= Math.Max(1, bossdamage - effects[0, 2]);

            if (life <= 0)
                return null;

            return Emulate(!myTurn, lifeLoss, life, mana, bosslife, bossdamage, GetNext(effects));
        }

        int? min = null;
        life -= lifeLoss;

        if (life <= effects[0, 1])
            return null;

        for (int i = 0; i < _spells.Length; i++)
        {
            int[] spell = _spells[i];
            int cost = spell[0];
            if (cost > mana || (effects[lifeLoss, 4] & 1 << i) != 0) // lifeLoss shouldn't be used here. But PartOne is too slow if not
                continue;

            int dL = 0;
            int dBL = 0;
            if (spell[1] == 0)
            {
                dBL = -spell[2];
                dL = spell[3];

                if (bosslife + dBL <= 0)
                    return cost;
            }

            int? res = Emulate(!myTurn, lifeLoss, life + dL, mana - cost, bosslife + dBL, bossdamage, GetNext(effects, i));

            if (res.HasValue)
                min = min is null ? cost + res.Value : Math.Min(min.Value, cost + res.Value);
        }

        return min;
    }

    private static int[,] GetNext(int[,] effects)
    {
        int[,] result = new int[6, 5];

        for (int i = 0; i < 5; i++)
            for (int j = 0; j < 5; j++)
                result[i, j] = effects[i + 1, j];

        return result;
    }

    private static int[,] GetNext(int[,] effects, int spellIdx)
    {
        int[,] result = GetNext(effects);

        int[] spell = _spells[spellIdx];
        int key = 1 << spellIdx;

        for (int i = 0; i < spell[1]; i++)
        {
            result[i, 4] |= key;
            for (int j = 0; j < 4; j++)
                result[i, j] += spell[j + 2];
        }

        return result;
    }
}