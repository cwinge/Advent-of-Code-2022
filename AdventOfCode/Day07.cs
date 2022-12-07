namespace AdventOfCode;

public class Day07 : BaseDay
{
    private readonly string[] _input;
    static Directory rootDirectory = new Directory("/", null);
    Directory currentDirectory = rootDirectory;

    public Day07()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        ReadConsoleLog();

        return new(rootDirectory.AddSizesWithLimit(100_000).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        long requiredExtraSpace = 30_000_000 - (70_000_000 - rootDirectory.Size); // required - current
        var list = rootDirectory.GetAllSubDirectorySizes();

        return new(list.Order().First(x => x >= requiredExtraSpace).ToString());
    }

    private void ReadConsoleLog()
    {
        foreach (var line in _input)
        {
            if (line[0] == '$') // If command
                ExecuteCommand(line.Split(" "));
            else
                AddInfo(line.Split(" "));
        }
    }

    private void AddInfo(string[] info)
    {
        switch (info[0])
        {
            case "dir":
                currentDirectory.SubDirectories.TryAdd(info[1], new Directory(info[1], currentDirectory));
                break;
            default:
                currentDirectory.Files.TryAdd(info[1], long.Parse(info[0]));
                break;
        }
    }

    private void ExecuteCommand(string[] command)
    {
        if (command[1] == "cd")
            ChangeDirectory(command[2]);
    }

    private void ChangeDirectory(string directory)
    {
        switch (directory)
        {
            case "/":
                currentDirectory = rootDirectory;
                break;
            case "..":
                currentDirectory = currentDirectory.Parent;
                break;
            default:
                currentDirectory = currentDirectory.SubDirectories[directory];
                break;
        }
    }

    private record Directory(string FolderName, Directory? Parent)
    {
        public Dictionary<string, long> Files = new();
        public Dictionary<string, Directory> SubDirectories = new();

        public long Size => SubDirectories.Values.Select(x => x.Size).Sum() + Files.Values.Sum();

        public long AddSizesWithLimit(long limit)
        {
            long ownSize = this.Size;
            long sum = ownSize <= limit ? ownSize : 0;

            foreach (Directory directory in SubDirectories.Values)
            {
                sum += directory.AddSizesWithLimit(limit);
            }
            return sum;
        }

        public List<long> GetAllSubDirectorySizes()
        {
            var list = new List<long>();

            foreach (var dir in SubDirectories.Values)
            {
                list.Add(dir.Size);
                list.AddRange(dir.GetAllSubDirectorySizes());
            }

            return list;
        }
    }
}