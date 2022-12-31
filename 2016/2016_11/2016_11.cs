namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2016/day/11
/// </summary>
public class _2016_11 : Problem
{
    private int _count;
    private int[] _floors;
    private Dictionary<string, int> _indexes;
    private int _target;

    public override void Parse()
    {
        _floors = new int[Inputs.Length];
        _indexes = new Dictionary<string, int>();
        _count = 0;
        for (int i = 0; i < Inputs.Length; i++)
        {
            if (Inputs[i].Contains("nothing"))
                continue;

            foreach (string element in Inputs[i].Split("contains ")[1].Remove("and ", "a ", ".", "-compatible").Split(", "))
            {
                string[] el = element.Split(' ');
                AddElement(i, el[0], el[1]);
            }
        }
    }

    public override object PartOne() => Emulate(_floors);

    public override object PartTwo()
    {
        AddElement(0, "elerium", "generator");
        AddElement(0, "elerium", "microchip");
        AddElement(0, "dilithium", "generator");
        AddElement(0, "dilithium", "microchip");

        return Emulate(_floors);
    }

    private void AddElement(int idx, string nature, string type)
    {
        int bitIdx = 2 * _indexes.GetOrAdd(nature.ToUpper(), () => _count++) + (type == "generator" ? 0 : 1);
        _floors[idx] = _floors[idx].SetBit(bitIdx);
        _target = _target.SetBit(bitIdx);
    }

    private IEnumerable<int> AvailableFloors(int idx, int[] floors)
    {
        if (idx > 0 && Enumerable.Range(0, idx).Any(i => i < idx && floors[i] != 0)) // move to previous floor only if items present
            yield return idx - 1;
        if (idx < _floors.Length - 1)
            yield return idx + 1;

        yield break;
    }

    private IEnumerable<int> AvailableItems(int floor)
    {
        int[] indexes = Enumerable.Range(0, 2 * _count).Where(i => floor.GetBit(i)).ToArray();

        foreach (int idx in indexes)
            yield return 0.SetBit(idx);

        if (indexes.Length < 2)
            yield break;

        for (int i = 0; i < indexes.Length; i++)
            for (int j = i + 1; j < indexes.Length; j++)
                yield return 0.SetBits(indexes[i], indexes[j]);

        yield break;
    }

    private int? Emulate(int[] initialState)
    {
        List<(int, int[])> list = new() { (0, initialState) };
        HashSet<long> states = new() { GetHash(0, initialState) };
        int count = 0;

        while (list.Any())
        {
            List<(int, int[])> next = new();

            foreach ((int idx, int[] floors) in list)
            {
                if (floors[_floors.Length - 1] == _target) // Success
                    return count;

                // A chip can't be in presence of a generator if its associated generator is not present
                // Elevator conditions :
                // - Must contain at least one item
                // - Must contain at most 2 items

                int[] items = AvailableItems(floors[idx]).ToArray();
                foreach (int nextIdx in AvailableFloors(idx, floors))
                {
                    foreach (int item in items)
                    {
                        int[] nFloors = floors.ToArray();
                        nFloors[idx] &= ~item;
                        nFloors[nextIdx] |= item;

                        long hash = GetHash(nextIdx, nFloors);
                        if (states.Contains(hash)) // Already visited state
                            continue;

                        states.Add(hash);

                        bool allowed = true;
                        for (int i = 0; i < floors.Length; i++)
                        {
                            bool protect = false; // Do I have a chip connected to a generator protecting me
                            bool exposed = false; // Am I exposed to an alone generator
                            bool singleChip = false; // chip without gen
                            for (int j = 0; j < _count; j++) // Check for Generator/Chip incompatibility
                            {
                                bool gen = nFloors[i].GetBit(2 * j);
                                bool chip = nFloors[i].GetBit(2 * j + 1);
                                if (gen && chip)
                                    protect = true;
                                if (gen && i == nextIdx)
                                    exposed = true;
                                if (chip && !gen)
                                    singleChip = true;
                            }
                            if (singleChip && exposed || exposed && !protect)
                            {
                                allowed = false;
                                break;
                            }
                        }

                        if (!allowed)
                            continue;

                        //Console.WriteLine($"Step {count}");
                        //Log(idx, floors, nextIdx, nFloors);

                        next.Add((nextIdx, nFloors));
                    }
                }
            }

            count++;
            list = next;
        }

        return null;
    }

    private long GetHash(int idx, int[] floors)
    {
        Dictionary<int, int> tmpIdx = new(); // all pairs are permutable, so use a temporary index given by apparition order
        int cnt = 0;
        long hash = 0;
        for (int i = 0; i < floors.Length; i++)
        {
            int floorHash = 0;
            for (int j = 0; j < 2 * _count; j++)
                if (floors[i].GetBit(j))
                    floorHash = floorHash.SetBit(2 * tmpIdx.GetOrAdd(j / 2, () => cnt++) + (j % 2));

            hash = (hash << (_count * 2)) | floorHash;
        }
        hash = (hash << 3) | idx;
        return hash;
    }

    private void Log(int idx, int[] floors)
    {
        for (int f = floors.Length - 1; f >= 0; f--)
        {
            int floor = floors[f];
            char e = idx == f ? 'E' : '.';
            Console.Write($"F{f + 1} {e}  ");
            for (int i = 0; i < 2 * _count; i++)
            {
                if (floor.GetBit(i))
                    Console.Write((_indexes.First(kv => kv.Value == i / 2).Key[0] + (i % 2 == 0 ? "G" : "M")).PadRight(3));
                else
                    Console.Write(".".PadRight(3));
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private void Log(int idx, int[] floors, int nIdx, int[] nFloors)
    {
        for (int f = floors.Length - 1; f >= 0; f--)
        {
            int floor = floors[f];
            Console.Write($"F{f + 1} ");
            string src = idx == f ? "#  " : "   ";
            string dst = nIdx == f ? "#  " : "   ";
            for (int i = 0; i < 2 * _count; i++)
            {
                string el = _indexes.First(kv => kv.Value == i / 2).Key[0] + (i % 2 == 0 ? "G" : "M");
                src += (floors[f].GetBit(i) ? el : ".").PadRight(3);
                dst += (nFloors[f].GetBit(i) ? el : ".").PadRight(3);
            }
            Console.Write(src);
            Console.Write("   =>   ");
            Console.Write(dst);
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}