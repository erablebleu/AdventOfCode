using System.Security;

namespace AdventOfCode;

public class _2022_07 : Problem
{
    public class FileInfo
    {
        private int _size;
        public string Name { get; set; }
        public int Size { get => IsDirectory ? Content.Sum(f => f.Size) : _size; set => _size = value; }
        public bool IsDirectory { get; set; }
        public List<FileInfo> Content { get; set; } = new();
        public FileInfo(string name) { 
            Name = name;
            IsDirectory = true;
        }
        public FileInfo(string name, int size) : this(name)
        {
            Size = size;
            IsDirectory = false;
        }

    }
    public override void Solve()
    {
        FileInfo root = new("/");
        FileInfo currentDir = root;
        List<FileInfo> directories = new() { root };

        for (int i = 0; i < Inputs.Length; i++)
        {
            string line = Inputs[i];
            string cmd = line.Substring(2, 2);
            switch(cmd)
            {
                case "cd":
                    string target = line.Substring(5);
                    switch(target)
                    {
                        case "..":
                            currentDir = directories.First(d => d.Content.Contains(currentDir));
                            break;
                        case "/":
                            currentDir = root;
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
                            directories.Add(dir);
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

        Solutions.Add($"{directories.Where(d => d.Size <= 100000).Sum(d => d.Size)}");

        int totalSize = root.Size;
        int unusedSize = 70000000 - totalSize;

        Solutions.Add($"{directories.Where(d => d.Size + unusedSize >= 30000000).OrderBy(d => d.Size).First().Size}");
    }
}