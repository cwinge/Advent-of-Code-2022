namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly (char direction, int steps)[] _input;
    List<(int y, int x)> knots = new List<(int y, int x)> {(0,0), (0, 0) };
    HashSet<(int y, int x)> visited = new HashSet<(int, int)> { (0, 0) };

    public Day09()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.Split(" ")).Select(x => (x[0][0], int.Parse(x[1]))).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        RunMovements();

        return new(visited.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        knots = new List<(int y, int x)> { (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0), (0, 0) };
        visited.Clear();
        RunMovements();

        return new(visited.Count.ToString());
    }

    private void RunMovements()
    {
        foreach (var move in _input)
            for (var i = 0; i < move.steps; i++)
                MoveHead(move.direction, move.steps);
    }

    private void MoveHead(char direction, int steps)
    {
        knots[0] = direction switch
        {
            'R' => knots[0] with { x = knots[0].x + 1 },
            'L' => knots[0] with { x = knots[0].x - 1 },
            'U' => knots[0] with { y = knots[0].y + 1 },
            'D' => knots[0] with { y = knots[0].y - 1 },
            _ => throw new System.Diagnostics.UnreachableException($"{direction} {steps}")
        }; ;
        MoveTail();
    }

    private void MoveTail()
    {
        for (var i = 1; i < knots.Count; i++)
        {
            var dy = knots[i - 1].y - knots[i].y;
            var dx = knots[i - 1].x - knots[i].x;

            if (Math.Abs(dy) > 1 || Math.Abs(dx) > 1)
                knots[i] = (knots[i].y + Math.Sign(dy), knots[i].x + Math.Sign(dx));                
        }

        visited.Add(knots[knots.Count-1]);
    }
}