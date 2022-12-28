namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/19
/// </summary>
public class _2015_19 : Problem
{
    private string[][] _data;
    private string _mol;

    public override void Parse()
    {
        _data = Inputs.Take(Inputs.Length - 2).Select(l => l.Split(" => ").ToArray()).ToArray();
        _mol = Inputs.Last();
    }

    public override object PartOne()
    {
        HashSet<string> set = new();

        foreach (string[] item in _data)
            foreach (int idx in _mol.AllIndexesOf(item[0]))
                set.Add(_mol.Remove(idx, item[0].Length).Insert(idx, item[1]));

        return set.Count;
    }

    public override object PartTwo()
    {/*
        Random rd = new();
        string[][] data = _data.OrderBy(c => rd.Next()).ToArray();
        string str = _mol;
        int count = 0;

        while (str != "e")
        {
            int cnt = 0;
            foreach (string[] item in data)
            {
                int idx = str.IndexOf(item[1]);
                if (idx < 0)
                    continue;
                str = str.Remove(idx, item[1].Length).Insert(idx, item[0]);
                cnt++;
            }
            if(cnt == 0)
            {
                data = data.OrderBy(c => rd.Next()).ToArray();
                str = _mol;
                count = 0;
            }
            count += cnt;
        }
        return count;*/

        List<string> set = new() { _mol };
        int count = 0;

        while (set.Count > 0)
        {
            List<string> next = new();
            count++;
            foreach (string str in set)
            {
                foreach (string[] item in _data)
                    foreach (int idx in str.AllIndexesOf(item[1]))
                    {
                        string tmpstr = str.Remove(idx, item[1].Length).Insert(idx, item[0]);
                        if (tmpstr.Length > 1)
                            next.Add(tmpstr);
                        if (tmpstr == "e")
                            return count;
                    }
            }
            set = next.OrderBy(l => l.Length).Take(100).ToList();
        }
        return null;
    }
}