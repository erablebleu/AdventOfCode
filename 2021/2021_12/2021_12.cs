namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2021/day/12
/// </summary>
public class _2021_12 : Problem
{
    private Node _end;
    private Node[] _nodes;
    private Path[] _pathes;
    private Node _start;

    public override void Parse()
    {
        _nodes = Inputs.SelectMany(l => l.Split('-')).Distinct().Select(d => new Node(d, d.ToUpper() == d)).ToArray();
        _pathes = Inputs.Select(l => GetPath(l)).ToArray();
        _start = _nodes.First(n => n.Name == "start");
        _end = _nodes.First(n => n.Name == "end");
    }

    public override object PartOne() => CountRoutes(new List<Node>() { _start }, _end, (n1, n2, last, list) => n1 == last && (n2.IsBig || !list.Contains(n2)));

    public override object PartTwo() => CountRoutes(new List<Node>() { _start }, _end, (n1, n2, last, list) => n1 == last && (n2.IsBig || !list.Contains(n2) || n2 != _start && n2 != _end && list.Count(n => n == n2) == 1 && list.Where(n => !n.IsBig).GroupBy(n => n).All(g => g.Count() == 1)));

    private record Node(string Name, bool IsBig);
    private record Path(Node From, Node To);

    private int CountRoutes(List<Node> travelledNodes, Node target, Func<Node, Node, Node, List<Node>, bool> predicate)
    {
        int cnt = 0;

        Node start = travelledNodes.Last();

        foreach (Node node in _pathes.Where(p => predicate.Invoke(p.From, p.To, start, travelledNodes)).Select(p => p.To))
        {
            if (node == target) cnt++;
            else
            {
                List<Node> tmpList = travelledNodes.ToList();
                tmpList.Add(node);
                cnt += CountRoutes(tmpList, target, predicate);
            }
        }
        foreach (Node node in _pathes.Where(p => predicate.Invoke(p.To, p.From, start, travelledNodes)).Select(p => p.From))
        {
            if (node == target) cnt++;
            else
            {
                List<Node> tmpList = travelledNodes.ToList();
                tmpList.Add(node);
                cnt += CountRoutes(tmpList, target, predicate);
            }
        }
        return cnt;
    }

    private Path GetPath(string line)
    {
        string[] el = line.Split('-');
        return new Path(_nodes.First(n => n.Name == el[0]), _nodes.First(n => n.Name == el[1]));
    }
}