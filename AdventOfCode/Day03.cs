namespace AdventOfCode;

public class Day03 : BaseDay
{
    private readonly string[] _input;

    public Day03()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        int priority = _input.
            Select(b => b.Chunk(b.Length/2)).
            Select(x => x.First().Intersect(x.Last()).
            Select(c => char.IsUpper(c) ? (c - 38) : (c - 96))).
            SelectMany(x => x).
            Sum();  

        return new(priority.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int priority = _input.
            Chunk(3).
            Select(x => x.First().Intersect(x.Last()).Intersect(x.ElementAt(1)).
            Select(c => char.IsUpper(c) ? (c - 38) : (c - 96))).
            SelectMany(x => x).
            Sum();

        return new(priority.ToString()); 
    }

}
