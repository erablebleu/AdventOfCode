namespace AdventOfCode;

public class _2015_09 : Problem
{
    private Dictionary<string, City> _cities;

    public override void Parse()
    {
        _cities = new Dictionary<string, City>();
        foreach (string line in Inputs)
        {
            string[] el = line.ParseExact("{0} to {1} = {2}");
            if (!_cities.ContainsKey(el[0]))
                _cities[el[0]] = new City { Name = el[0] };
            if (!_cities.ContainsKey(el[1]))
                _cities[el[1]] = new City { Name = el[1] };
            _cities[el[0]].AccessibleCities[_cities[el[1]]] = int.Parse(el[2]);
            _cities[el[1]].AccessibleCities[_cities[el[0]]] = int.Parse(el[2]);
        }
    }

    public override object PartOne() => _cities.Values.SelectMany(c => c.VisitAll(_cities.Values.ToList())).Min();

    public override object PartTwo() => _cities.Values.SelectMany(c => c.VisitAll(_cities.Values.ToList())).Max();

    internal class City
    {
        public Dictionary<City, int> AccessibleCities = new();
        public string Name;

        public override string ToString() => $"{Name}";

        public IEnumerable<int> VisitAll(List<City> cities)
        {
            cities.Remove(this);

            if (cities.Count == 0)
                yield return 0;
            else
            {
                foreach (City city in AccessibleCities.Keys.Where(c => cities.Contains(c)))
                    foreach (int dist in city.VisitAll(cities.ToList()))
                        yield return dist + AccessibleCities[city];
            }

            yield break;
        }
    }
}