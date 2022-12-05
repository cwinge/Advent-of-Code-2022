using System.Text;

namespace AdventOfCode;

public class Day05 : BaseDay
{
    private readonly string[] _input;
    private List<Stack<char>> _stacks = new();

    public Day05()
    {
        _input = File.ReadAllLines(InputFilePath);
        ReadStacks();        
    }

    public override ValueTask<string> Solve_1()
    {
        _input.
            Skip(10).
            Select(x => x.Split(" ")).
            ToList().
            ForEach(x => CraneMoveOperation9000(int.Parse(x.ElementAt(1)), int.Parse(x.ElementAt(3)) - 1, int.Parse(x.ElementAt(5)) - 1));

        var solution = _stacks.Aggregate(
            new StringBuilder(),
            (sb, s) => sb.Append(s.Peek())).ToString();

        return new(solution);
    }

    public override ValueTask<string> Solve_2()
    {
        ReadStacks();
        var stacks = _stacks.Select(x => x.ToList()).ToList();
        stacks.ForEach(x => x.Reverse());

        _input.
            Skip(10).
            Select(x => x.Split(" ")).
            ToList().
            ForEach(x => CraneMoveOperation9001(stacks, int.Parse(x.ElementAt(1)), int.Parse(x.ElementAt(3)) - 1, int.Parse(x.ElementAt(5)) - 1));

        var solution = stacks.Aggregate(
            new StringBuilder(),
            (sb, s) => sb.Append(s.Last())).ToString();

        return new(solution);
    }

    private void CraneMoveOperation9000(int amount, int moveFrom, int moveTo)
    {
        for(int i = 0; i < amount; i++) 
        {
            _stacks[moveTo].Push(_stacks[moveFrom].Pop());
        }
    }

    private void CraneMoveOperation9001(List<List<char>> stacks, int amount, int moveFrom, int moveTo)
    {
        stacks[moveTo].AddRange(stacks[moveFrom].TakeLast(amount));
        stacks[moveFrom].RemoveRange(stacks[moveFrom].Count > amount ? stacks[moveFrom].Count - amount : 0, Math.Min(stacks[moveFrom].Count, amount));    
    }

    private void ReadStacks()
    {
        _stacks.Clear();
        var stacks = _input.Take(9).Reverse();

        for(int i = 0; i < 9; i++)
        {
            _stacks.Add(new Stack<char>());
        }

        foreach (var row in stacks)
        {
            int s = 1;
            for(int i = 0; i < 9; i++)
            {
                if (char.IsLetter(row[s]))
                    _stacks[i].Push(row[s]);                  
                s += 4;
            }
        }
    }
}