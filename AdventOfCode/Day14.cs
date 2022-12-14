namespace AdventOfCode;

public class Day14 : BaseDay
{
    enum Type { Rock, Sand}
    record struct Point(int x, int y);
    List<Point[]> input;
    Dictionary<Point, Type> grid = new Dictionary<Point, Type>();
    int minX = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
    public Day14()
    {
        input = File.ReadAllLines(InputFilePath).Select(x => x.Split(" -> ")
            .Select(c => c.Split(",")).Select(n => new Point(int.Parse(n.First()), int.Parse(n.Last()))).ToArray()).ToList();        
    }

    public override ValueTask<string> Solve_1()
    {
        input.ForEach(ParseRockLine);
        while (DropOneSandUnit()) { }

        return new(grid.Values.Where(x => x == Type.Sand).Count().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        grid.Clear();
        input.ForEach(ParseRockLine);
        maxX = int.MaxValue;
        minX = int.MinValue;
        while (DropOneSandUnit(true)) { }

        return new(grid.Values.Where(x => x == Type.Sand).Count().ToString());
    }

    private bool DropOneSandUnit(bool partTwo = false)
    {
        int x = 500, y = 0; // sand origin
        while (Move(x, y, out x, out y))
            if (x < minX || x > maxX || y > maxY)
            {
                if(!partTwo)
                    return false;
                break;
            }
                
        grid.Add(new Point(x, y), Type.Sand);
        
        return y != 0 || x != 500;
    }

    private bool Move(int x, int y, out int newX, out int newY)
    {
        if(!grid.ContainsKey(new Point(x, y + 1))) // Below
        {
            newX = x; 
            newY = y + 1;
            return true;
        }
        if (!grid.ContainsKey(new Point(x - 1, y + 1))) // Left
        {
            newX = x - 1;
            newY = y + 1 ;
            return true;
        }
        if (!grid.ContainsKey(new Point(x + 1, y + 1))) // Right
        {
            newX = x + 1;
            newY = y + 1;
            return true;
        }
        newX = x;
        newY = y;
        return false;
    }

    private void ParseRockLine(Point[] points)
    {
        for(int i = 0; i < points.Length-1; i++)
            CreateRockLine(points[i], points[i+1]);
    }

    private void CreateRockLine(Point from, Point to)
    {
        minX = Math.Min(minX, Math.Min(from.x, to.x)); // update bordersize
        maxX = Math.Max(maxX, Math.Max(from.x, to.x));
        maxY = Math.Max(maxY, Math.Max(from.y, to.y));
        for (int x = Math.Min(from.x, to.x); x <= Math.Max(from.x, to.x); x++)
            for(int y = Math.Min(from.y, to.y); y <= Math.Max(from.y, to.y); y++)
                grid.TryAdd(new Point(x, y), Type.Rock);
    }
}