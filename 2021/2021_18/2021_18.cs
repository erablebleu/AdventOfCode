namespace AdventOfCode;

internal class SnailfishNumber
{
    public SnailfishNumber(SnailfishNumber left, SnailfishNumber right)
    {
        Left = new SnailfishNumberPart(left);
        Right = new SnailfishNumberPart(right);
    }

    public SnailfishNumber(SnailfishNumber left, int right)
    {
        Left = new SnailfishNumberPart(left);
        Right = new SnailfishNumberPart(right);
    }

    public SnailfishNumber(int left, SnailfishNumber right)
    {
        Left = new SnailfishNumberPart(left);
        Right = new SnailfishNumberPart(right);
    }

    public SnailfishNumber(int left, int right)
    {
        Left = new SnailfishNumberPart(left);
        Right = new SnailfishNumberPart(right);
    }

    public SnailfishNumber(string line)
    {
        int openCnt = 0;
        int splitterIdx = 0;
        // search splitter ','
        for (int i = 1; i < line.Length; i++)
        {
            switch (line[i])
            {
                case '[': openCnt++; break;
                case ']': openCnt--; break;
                case ',' when openCnt == 0:
                    splitterIdx = i;
                    i = line.Length;
                    break;
            }
        }
        Left = new SnailfishNumberPart(line.Substring(1, splitterIdx - 1));
        Right = new SnailfishNumberPart(line.Substring(splitterIdx + 1, line.Length - splitterIdx - 2));
    }

    public SnailfishNumberPart Left { get; set; }
    public SnailfishNumberPart Right { get; set; }

    public static SnailfishNumber operator +(SnailfishNumber a, SnailfishNumber b) => new(a, b);

    public static SnailfishNumber operator +(int a, SnailfishNumber b) => new(a, b);

    public static SnailfishNumber operator +(SnailfishNumber a, int b) => new(a, b);

    public void Explode(int depth, ref ReduceResult result)
    {
        if (result.ExplodePart is not null)
        {
            if (result.NextExplode is not null) return;
            if (Left.IsInt)
                result.NextExplode = Left;
            else
                Left.Number.Explode(depth + 1, ref result);
            return;
        }

        if (Left.CanExplode(depth + 1))
        {
            result.ExplodePart = Left;
            if (Right.IsInt)
                result.NextExplode = Right;
            else
                Right.Number.Explode(depth + 1, ref result);
            return;
        }
        if (Left.IsInt)
            result.PrevExplode = Left;
        else
        {
            Left.Number.Explode(depth + 1, ref result);
            if (result.ExplodePart is not null)
            {
                if (result.NextExplode is not null) return;
                if (Right.IsInt)
                    result.NextExplode = Right;
                else
                    Right.Number.Explode(depth + 1, ref result);
                return;
            }
        }

        if (Right.CanExplode(depth + 1))
        {
            result.ExplodePart = Right;
            return;
        }
        if (Right.IsInt)
            result.PrevExplode = Right;
        else
            Right.Number.Explode(depth + 1, ref result);
    }

    public int GetMagnitude() => 3 * Left.GetMagnitude() + 2 * Right.GetMagnitude();

    public SnailfishNumber Reduce()
    {
        while (Reduce(0)) ;
        return this;
    }

    public bool Reduce(int depth)
    {
        ReduceResult result = new();
        Explode(depth, ref result);
        if (result.ExplodePart is not null)
        {
            if (result.PrevExplode is not null) result.PrevExplode.Value += result.ExplodePart.Number.Left.Value;
            if (result.NextExplode is not null) result.NextExplode.Value += result.ExplodePart.Number.Right.Value;
            result.ExplodePart.Explode();
            return true;
        }

        if (Split(out SnailfishNumberPart part))
        {
            part.Split();
            return true;
        }

        return false;
    }

    public bool Split(out SnailfishNumberPart part)
    {
        part = null;
        if (Left.CanSplit())
        {
            part = Left;
            return true;
        }
        if (!Left.IsInt && Left.Number.Split(out part))
        {
            return true;
        }
        if (Right.CanSplit())
        {
            part = Right;
            return true;
        }
        if (!Right.IsInt
           && Right.Number.Split(out part))
        {
            return true;
        }
        return false;
    }

    public override string ToString() => $"[{Left},{Right}]";

    internal class ReduceResult
    {
        public SnailfishNumberPart ExplodePart { get; set; }
        public SnailfishNumberPart NextExplode { get; set; }
        public SnailfishNumberPart PrevExplode { get; set; }
    }
}

internal class SnailfishNumberPart
{
    public SnailfishNumberPart(string line)
    {
        if (int.TryParse(line, out int value))
        {
            IsInt = true;
            Value = value;
        }
        else
            Number = new SnailfishNumber(line);
    }

    public SnailfishNumberPart(SnailfishNumber number)
    {
        Number = number;
    }

    public SnailfishNumberPart(int value)
    {
        IsInt = true;
        Value = value;
    }

    public bool IsInt { get; set; }
    public SnailfishNumber Number { get; set; }
    public int Value { get; set; }

    public bool CanExplode(int depth) => !IsInt && depth >= 4 && Number.Left.IsInt && Number.Right.IsInt;

    public bool CanSplit() => IsInt && Value >= 10;

    public void Explode()
    {
        if (IsInt)
            throw new InvalidOperationException("Can't explod a regular number");
        Number = null;
        IsInt = true;
        Value = 0;
    }

    public int GetMagnitude() => IsInt ? Value : Number.GetMagnitude();

    public void Split()
    {
        if (!IsInt)
            throw new InvalidOperationException("Can't spit a non regular number");
        if (Value < 10)
            throw new InvalidOperationException("Regular number must be 10 or greater to split");

        IsInt = false;
        Number = new SnailfishNumber(Value / 2, (Value + 1) / 2);
        Value = 0;
    }

    public override string ToString() => IsInt ? Value.ToString() : Number.ToString();
}

/// <summary>
/// https://adventofcode.com/2021/day/18
/// </summary>
public class _2021_18 : Problem
{
    public override void Parse()
    {
    }

    public override object PartOne()
    {
        SnailfishNumber[] numbers = Inputs.Select(l => new SnailfishNumber(l)).ToArray();
        SnailfishNumber sum = numbers.First();

        for (int i = 1; i < numbers.Length; i++)
        {
            sum += numbers[i];
            sum.Reduce();
        }

        return sum.GetMagnitude();
    }

    public override object PartTwo()
    {
        SnailfishNumber[] numbers = Inputs.Select(l => new SnailfishNumber(l)).ToArray();
        int maxMagn = 0;

        for (int i = 0; i < Inputs.Length - 1; i++)
        {
            for (int j = i + 1; j < Inputs.Length; j++)
            {
                maxMagn = Math.Max(maxMagn, (new SnailfishNumber(Inputs[i]) + new SnailfishNumber(Inputs[j])).Reduce().GetMagnitude());
                maxMagn = Math.Max(maxMagn, (new SnailfishNumber(Inputs[j]) + new SnailfishNumber(Inputs[i])).Reduce().GetMagnitude());
            }
        }

        return maxMagn;
    }
}