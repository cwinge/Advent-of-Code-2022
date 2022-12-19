namespace AdventOfCode;

public class Day18 : BaseDay
{
    HashSet<Cube> cubes;
    record struct Cube(int x, int y, int z)
    {
        public IEnumerable<Cube> Neighbors => new[]
        {
            this with {x = x - 1 },
            this with {x = x + 1 },
            this with {y = y - 1 },
            this with {y = y + 1 },
            this with {z = z - 1 },
            this with {z = z + 1 }
        };
    }

    public Day18()
    {
        cubes = File.ReadAllLines(InputFilePath).Select(x => x.Split(",")).Select(x => new Cube(int.Parse(x.ElementAt(0)), int.Parse(x.ElementAt(1)), int.Parse(x.ElementAt(2)))).ToHashSet();
    }

    public override ValueTask<string> Solve_1()
    {
        return new(cubes.SelectMany(x => x.Neighbors).Count(x => !cubes.Contains(x)).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var min = new Cube(cubes.MinBy(c => c.x).x - 1, cubes.MinBy(c => c.y).y - 1, cubes.MinBy(c => c.z).z - 1);
        var max = new Cube(cubes.MaxBy(c => c.x).x + 1, cubes.MaxBy(c => c.y).y + 1, cubes.MaxBy(c => c.z).z + 1);
        var water = FloodFill(min, min, max);

        return new(cubes.SelectMany(c => c.Neighbors).Count(c => water.Contains(c)).ToString());
    }

    HashSet<Cube> FloodFill(Cube from, Cube min, Cube max)
    {
        var result = new HashSet<Cube>();
        var queue = new Queue<Cube>();

        queue.Enqueue(from);
        result.Add(from);
        while (queue.Any())
        {
            var water = queue.Dequeue();
            foreach (var neighbour in water.Neighbors)
            {
                if (!result.Contains(neighbour) && !cubes.Contains(neighbour) && Inside(min, max, neighbour))
                {
                    queue.Enqueue(neighbour);
                    result.Add(neighbour);
                }
            }
        }

        return result;
    }

    bool Inside(Cube min, Cube max, Cube point) =>
        min.x <= point.x && point.x <= max.x &&
        min.y <= point.y && point.y <= max.y &&
        min.z <= point.z && point.z <= max.z;
}