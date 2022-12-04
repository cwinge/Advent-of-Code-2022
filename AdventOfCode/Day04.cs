namespace AdventOfCode;

public class Day04 : BaseDay
{
    private readonly string[] _input;
    private readonly record struct Range(string[] range)
    {
        public readonly int lower = int.Parse(range[0]);
        public readonly int upper = int.Parse(range[1]);
    }

    public Day04()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var ranges = _input.
            Select(x => x.Split(",")
                .Select(r => r.Split("-"))
                .Select(e => new Range(e))).
            Where(m => FullyContained(m.First(), m.Last())).
            Count();

        return new(ranges.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var ranges = _input.
            Select(x => x.Split(",")
                .Select(r => r.Split("-"))
                .Select(e => new Range(e))).
            Where(m => PartiallyContained(m.First(), m.Last())).
            Count();

        return new(ranges.ToString());
    }

    private bool FullyContained(Range f, Range s)
    {
        if (f.lower >= s.lower)
            if (f.upper <= s.upper)
                return true;

        if (s.lower >= f.lower)
            if (s.upper <= f.upper)
                return true;

        return false;
    }

    private bool PartiallyContained(Range f, Range s)
    {
        if (f.lower >= s.lower && f.lower <= s.upper)
            return true;

        if (f.upper >= s.lower && f.upper <= s.upper)
            return true;

        if (s.lower >= f.lower && s.lower <= f.upper)
            return true;

        if (s.upper >= f.lower && s.upper <= f.upper)
            return true;

        return false;
    }

}
