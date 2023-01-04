namespace AdventOfCode;

/// <summary>
/// https://adventofcode.com/2022/day/07
/// </summary>
public class _2022_07 : Problem
{
    private List<FileInfo> _directories;
    private FileInfo _root;

    public override void Parse()
    {
        _root = new FileInfo("/");
        FileInfo currentDir = _root;
        _directories = new List<FileInfo>() { _root };

        for (int i = 0; i < Inputs.Length; i++)
        {
            string line = Inputs[i];
            string cmd = line.Substring(2, 2);
            switch (cmd)
            {
                case "cd":
                    string target = line.Substring(5);
                    switch (target)
                    {
                        case "..":
                            currentDir = _directories.First(d => d.Content.Contains(currentDir));
                            break;

                        case "/":
                            currentDir = _root;
                            break;

                        default:
                            currentDir = currentDir.Content.First(d => d.Name == target);
                            break;
                    }

                    break;

                case "ls":
                    while (i + 1 < Inputs.Length && !Inputs[i + 1].StartsWith("$"))
                    {
                        i++;
                        string[] el = Inputs[i].Split(" ");
                        if (el[0] == "dir")
                        {
                            FileInfo dir = new(el[1]);
                            _directories.Add(dir);
                            currentDir.Content.Add(dir);
                        }
                        else
                        {
                            FileInfo dir = new(el[1], int.Parse(el[0]));
                            currentDir.Content.Add(dir);
                        }
                    }
                    break;
            }
        }
    }

    public override object PartOne() => _directories.Where(d => d.Size <= 100000).Sum(d => d.Size);

    public override object PartTwo()
    {
        int totalSize = _root.Size;
        int unusedSize = 70000000 - totalSize;

        return _directories.Where(d => d.Size + unusedSize >= 30000000).OrderBy(d => d.Size).First().Size;
    }

    private class FileInfo
    {
        private int _size;

        public FileInfo(string name)
        {
            Name = name;
            IsDirectory = true;
        }

        public FileInfo(string name, int size) : this(name)
        {
            Size = size;
            IsDirectory = false;
        }

        public List<FileInfo> Content { get; set; } = new();
        public bool IsDirectory { get; set; }
        public string Name { get; set; }
        public int Size { get => IsDirectory ? Content.Sum(f => f.Size) : _size; set => _size = value; }
    }
}