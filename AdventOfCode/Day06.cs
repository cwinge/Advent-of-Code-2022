using System.Text;

namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly string _input;
    private readonly int markerSize = 4;
    private readonly int messageSize = 14;

    public Day06()
    {
        _input = File.ReadAllText(InputFilePath);  
    }

    public override ValueTask<string> Solve_1()
    {

        return new(FindDistinctWithSize(markerSize).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(FindDistinctWithSize(messageSize).ToString());
    }

    private int FindDistinctWithSize(int size)
    {
        int i = 0;
        for (; i < _input.Length - size; i++)
        {
            if (size == _input.Substring(i, size).Distinct().Count())
                break;
        }

        return i + size;
    }    
}