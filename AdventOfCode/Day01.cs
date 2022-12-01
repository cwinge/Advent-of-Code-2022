namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string[] _input;
    private readonly record struct Elf(int TotalCalories);
    private readonly List<Elf> _elfs = new List<Elf>();

    public Day01()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() 
    {        
        int calories = 0;
       foreach(var input in _input) 
       {
            var isNumeric = int.TryParse(input, out int row);
            if(isNumeric)
            {
                calories += row;
            }
            else
            {
                _elfs.Add(new Elf(calories));
                calories = 0;
            }
       }
        
        return new(_elfs.Max(e => e.TotalCalories).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(_elfs.OrderByDescending(e => e.TotalCalories).Take(3).Sum(x => x.TotalCalories).ToString());
    }
}
