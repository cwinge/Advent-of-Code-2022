using System.Text;

namespace AdventOfCode;

public class Day10 : BaseDay
{
    enum Operation { NOOP, ADDX}
    private readonly (Operation op, int value)[] _input;
    private Dictionary<int, int> values = new();
    private int reg = 1, cycle = 0, row = 0, pixel = 0;
    private char[,] CRT = new char[6, 40];

    public Day10()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.Split(" ")).Select(x => (x[0][0] == 'n' ? Operation.NOOP : Operation.ADDX, x[0][0] == 'n' ? 0 : int.Parse(x[1]))).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        RunOperations();

        return new(values.Keys.Where(key => key <= 220).Select(k => values[k] * k).Sum().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(PrintCRT());
    }

    private void RunOperations()
    {        
        foreach (var (op, value) in _input)
        {
            if (op == Operation.NOOP)
            {
                UpdateCycle(out row, out pixel);
            }
            else
            {
                for (int i = 0; i < 2; i++)
                    UpdateCycle(out row, out pixel);
                reg += value;
            }
        }
    }

    private void SaveValueAtCycle(int cycle)
    {
        if (cycle % 40 == 20)
            values[cycle] = reg;
    }

    private void UpdateCycle(out int row, out int pixel)
    {
        row = cycle / 40;
        pixel = cycle % 40;
        cycle++;
        SaveValueAtCycle(cycle);
        UpdatePixel(row, pixel);
    }

    private void UpdatePixel( int row, int pixel)
    {
        CRT[row, pixel] = Math.Abs(reg - pixel) <= 1 ? '#' : '.';
    }

    private string PrintCRT()
    {
        StringBuilder sb = new();
        for(int r = 0; r < 6; r++)
        {
            for(int p = 0; p < 40; p++)
                sb.Append(CRT[r, p]);
            sb.AppendLine();
        }

        return sb.ToString();
    }
}