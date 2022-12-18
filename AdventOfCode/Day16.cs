namespace AdventOfCode;

public class Day16 : BaseDay
{
    record Valve (string Name, int Flow, string[] Tunnels, Dictionary<string, int> ShortestPath);
    Dictionary<string, Valve> valves;
    public Day16()
    {
        valves = File.ReadAllLines(InputFilePath).Select(x => x.Split(" "))
            .Select(x => new Valve(x.ElementAt(1), int.Parse(x.ElementAt(4).Split("=")[1][0..^1]), x.Skip(9).Select(t => t[0..2]).ToArray(), new Dictionary<string, int>()))
            .ToDictionary(v => v.Name);
    }

    public override ValueTask<string> Solve_1()
    {
        FindShortestPaths();
        return new(GetMaxReleased(new string[] { "AA", "" }, valves.Values.Where(v => v.Flow > 0).ToArray(), new int[] { 30, 0 }).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(GetMaxReleased(new string[] { "AA", "AA" }, valves.Values.Where(v => v.Flow > 0).ToArray(), new int[] { 26, 26 }).ToString());
    }

    long GetMaxReleased(string[] current, Valve[] targets, int[] remainingTime)
    {
        long best = 0;
        var op = remainingTime[0] < remainingTime[1] ? 1 : 0;
        var currentValve = current[op];

        foreach (var v in targets)
        {
            int newRemainingTime = remainingTime[op] - valves[currentValve].ShortestPath[v.Name] - 1;
            if (newRemainingTime > 0)
            {
                long release = newRemainingTime * v.Flow + GetMaxReleased(new string[] {v.Name, current[1-op] }, targets.Where(x => x != v).ToArray(), new int[] { newRemainingTime, remainingTime[1-op] });
                best = release > best ? release : best;
            }
        }
        return best;
    }

    private void FindShortestPaths()
    {
        foreach(var v in valves.Values) 
        {
            v.ShortestPath[v.Name] = 0;
            FindShortestPath(v, v.Name);
        }
    }

    void FindShortestPath(Valve from, string to)
    {
        var visited = new HashSet<string>();

        while (from is not null && visited.Count < valves.Count)
        {
            visited.Add(from.Name);
            var distance = from.ShortestPath[to] + 1;
            foreach (var t in from.Tunnels)
            {
                if (!visited.Contains(t))
                    valves[t].ShortestPath[to] = valves[t].ShortestPath.TryGetValue(to, out int oldistance) ? (distance < oldistance ? distance : oldistance) : distance;                        
            }
            from = valves.Values.Where(c => !visited.Contains(c.Name) && c.ShortestPath.ContainsKey(to)).OrderBy(c => c.ShortestPath[to]).FirstOrDefault();
        }
    }
}