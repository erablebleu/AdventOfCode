namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2015/day/13
/// </summary>
public class _2015_13 : Problem
{
    private Dictionary<string, Person> _data;

    public override void Parse()
    {
        _data = new Dictionary<string, Person>();
        foreach (string line in Inputs)
        {
            string[] el = line.ParseExact("{0} would {1} {2} happiness units by sitting next to {3}.");
            Person a = _data.GetOrAdd(el[0], () => new Person { Name = el[0] });
            Person b = _data.GetOrAdd(el[3], () => new Person { Name = el[3] });
            a.Happiness[b] = (el[1] == "gain" ? 1 : -1) * int.Parse(el[2]);
        }
    }

    public override object PartOne() => GetTotalHappiness(_data.Values.ToArray());

    public override object PartTwo()
    {
        Person me = new Person { Name = "me" };
        List<Person> list = _data.Values.ToList();
        foreach (Person p in list)
        {
            p.Happiness[me] = 0;
            me.Happiness[p] = 0;
        }
        list.Add(me);
        return GetTotalHappiness(list.ToArray());
    }

    private int GetTotalHappiness(Person[] people) => CombinatoryHelper.GetPermutations(people)
            .Max(p => Enumerable.Range(0, p.Length).Sum(i => (i < p.Length - 1 ? p[i].Happiness[p[i + 1]] : p[i].Happiness[p[0]])
                                                           + (i > 0 ? p[i].Happiness[p[i - 1]] : p[i].Happiness[p[p.Length - 1]])));

    private class Person
    {
        public Dictionary<Person, int> Happiness = new();
        public string Name;

        public override string ToString() => $"{Name}";
    }
}