namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly (char direction, int steps)[] _input;
    private List<(int y, int x)> T = new List<(int y, int x)> {(0,0) };
    private (int y, int x) H = (0, 0);
    HashSet<(int y, int x)> visited = new HashSet<(int, int)> { (0, 0) };

    public Day09()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.Split(" ")).Select(x => (x[0][0], int.Parse(x[1]))).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        /*foreach(var move in _input)
        {
            MoveHead(move.direction, move.steps);
        }*/

        return new(visited.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        T = new List<(int y, int x)> { (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0) };
        H = (0, 0);
        visited.Clear();

        foreach (var move in _input)
        {
            MoveHead(move.direction, move.steps);
        }

        return new(visited.Count.ToString());
    }

    private void MoveHead(char direction, int steps)
    {
        if(steps == 0) return;

        switch(direction)
        {
            case 'R':
                H.x++;
                MoveTail(0, 1, 0);
                break;
            case 'L':
                H.x--;
                MoveTail(0, -1, 0);
                break;
            case 'U':
                H.y++;
                MoveTail(1, 0, 0);
                break;
            case 'D':
                H.y--;
                MoveTail(-1, 0, 0);
                break;
        }
        Console.Write("H: " + H);
        for(int i = 0; i < T.Count; i++)
        {
            Console.Write($" {i+1}: {T[i]}");
        }
        Console.WriteLine($" visited: {visited.Count}");
        MoveHead(direction, steps - 1);
    }

    private bool NeedToMove(int y, int x, (int y, int x) k) => (Math.Abs(k.y - y) > 1 || Math.Abs(k.x - x) > 1);

    private void MoveTail(int y, int x, int knotIndex)
    {
        if(knotIndex == T.Count) return;
        if(H.x == 5 && H.y == 2)
        {
            int a = 2;
        }

        if (!NeedToMove(T[knotIndex].y, T[knotIndex].x, knotIndex == 0 ? H : T[knotIndex - 1])) return;
        var t = T[knotIndex];
        int oldX = t.x, oldY = t.y;
        t.x = x == 0 ? H.x : t.x + x;
        t.y = y == 0 ? H.y : t.y + y;
        T[knotIndex] = t;

        if (knotIndex == T.Count - 1)
        {
            visited.Add(T[T.Count - 1]);
        }

        if (knotIndex != T.Count-1)
            MoveTail(t.y - oldY, t.x - oldX, knotIndex + 1);       
    }
}