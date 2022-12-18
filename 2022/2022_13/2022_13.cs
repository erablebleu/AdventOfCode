namespace AdventOfCode;

public class _2022_13 : Problem
{
    public class PacketData : IComparable<PacketData>
    {
        public int Value { get; set; }
        public List<PacketData> Datas { get; set; } = new();
        public bool IsInteger { get; set; }
        public PacketData(int value) 
        {
            Value = value;
            IsInteger = true;
        }
        public PacketData(string value)
        {
            if (value.StartsWith("["))
            {
                int openCount = 0;
                int subStart = 1;
                for (int i = 1; i < value.Length; i++)
                {
                    switch (value[i])
                    {
                        case '[':
                            openCount++;
                            break;
                        case ']' when openCount > 0:
                            openCount--;
                            break;
                        case ']' when openCount == 0:
                        case ',' when openCount == 0:
                            Datas.Add(new PacketData(value.Substring(subStart, i - subStart)));
                            subStart = i + 1;
                            break;
                    }
                }
            }
            else if (value.Length > 0)
            {
                IsInteger= true;
                Value = int.Parse(value);
            }
        }
        public int CompareTo(PacketData other)
        {
            if(IsInteger && other.IsInteger)
                return Compare(Value, other.Value);

            List<PacketData> l = IsInteger ? new List<PacketData> { new PacketData(Value) } : Datas;
            List<PacketData> r = other.IsInteger ? new List<PacketData> { new PacketData(other.Value) } : other.Datas;

            for(int i = 0; i < Math.Max(l.Count, r.Count); i++)
            {
                if (i >= l.Count) return -1;
                if (i >= r.Count) return 1;

                int res = l[i].CompareTo(r[i]);
                if(res != 0) return res;
            }

            return 0;
        }
        public override string ToString() => IsInteger ? Value.ToString() : $"[{string.Join(",", Datas.Select(d => d.ToString()))}]";

        public static int Compare(int l, int r) => l - r;
    }
    public override void Solve()
    {
        int idxSum = 0;

        for(int i = 0; i < Inputs.Length; i+=3)
        {
            int idx = i / 3 + 1;
            PacketData left = new(Inputs[i]);
            PacketData right = new(Inputs[i + 1]);

            if (left.CompareTo(right) < 0)
                idxSum += idx;
        }

        Solutions.Add($"{idxSum}");

        List<PacketData> data = Inputs.Where(l => !string.IsNullOrEmpty(l)).Select(l => new PacketData(l)).ToList();
        PacketData dk2 = new PacketData("[[2]]");
        PacketData dk6 = new PacketData("[[6]]");
        data.Add(dk2);
        data.Add(dk6);
        data.Sort();

        Solutions.Add($"{(data.IndexOf(dk2) + 1) * (data.IndexOf(dk6) + 1)}");
    }
}