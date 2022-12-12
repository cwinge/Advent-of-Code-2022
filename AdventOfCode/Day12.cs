namespace AdventOfCode;

public class Day12 : BaseDay
{
    static List<Coord> neighbors = new List<Coord>() { new Coord(-1, 0), new Coord(1, 0), new Coord(0, -1), new Coord(0, 1) };
    char[][] grid;
    record struct Coord(int y, int x);
    public Day12()
    {
        grid = File.ReadAllLines(InputFilePath).Select(x => x.ToCharArray()).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        return new(BFS().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var costs = grid.Select(x => x.Select(c => c == 'S' ? 1 : c == 'E' ? 26 : c - 'a' + 1).ToArray()).ToArray();

        return new(BFS(costs).ToString());
    }

    private int BFS(int[][] costs = null)
    {
        var queue = GetCosts(costs);
        var visited = new HashSet<Coord>();

        while (queue.Any())
        {
            (Coord point, int cost) = queue.Dequeue();
            if (!visited.Add(point))
                continue;
            if (grid[point.y][point.x] == 'E')
                return cost;

            foreach (var neighbor in neighbors)
            {
                var newY = point.y + neighbor.y;
                var newX = point.x + neighbor.x;
                if ((newY >= 0 && newY < grid.Length) && (newX >= 0 && newX < grid[0].Length))
                {
                    if (costs == null)
                    {
                        if (grid[point.y][point.x] == 'S' ? grid[newY][newX] - 'a' <= 1 : grid[newY][newX] - grid[point.y][point.x] <= 1)
                            queue.Enqueue((new Coord(newY, newX), cost + 1));
                    }
                    else
                    {
                        if (costs[newY][newX] <= 1 + costs[point.y][point.x])
                            queue.Enqueue((new Coord(newY, newX), cost + 1));
                    }
                }
            }
        }
        return 0;
    }

    private Queue<(Coord, int cost)> GetCosts(int[][] costs)
    {
        var queue = new Queue<(Coord, int cost)>();
        for (int y = 0; y < grid.Length - 1; y++)
            for (int x = 0; x < grid[0].Length - 1; x++)
                if ((costs != null && costs[y][x] == 1) || (costs == null && grid[y][x] == 'S'))
                    queue.Enqueue((new Coord(y, x), 0));

        return queue;
    }
}