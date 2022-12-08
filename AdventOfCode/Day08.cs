namespace AdventOfCode;

public class Day08 : BaseDay
{
    private readonly int[][] _input;


    public Day08()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.ToCharArray().Select(c => (int)Char.GetNumericValue(c)).ToArray()).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        int visible = 0;
        for(int r = 0; r < _input.Length;r++)
        {
            for(int c = 0; c < _input[0].Length; c++)
            {
                if(IsVisible(r, c))
                    visible++;
            }
        }
        return new(visible.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int max = 0;

        for (int r = 0; r < _input.Length; r++)
        {
            for (int c = 0; c < _input[0].Length; c++)
            {
                int locScore = SceneScoreRow(r, c, _input[r][c]) * SceneScoreColumn(r, c, _input[r][c]);
                if( locScore > max) max = locScore;
            }
        }

        return new(max.ToString());
    }

    public bool IsVisible(int row, int col)
    {
        if ((row == 0 || row == _input.Length - 1) || (col == 0 || col == _input[0].Length - 1))
            return true;

        if( VisibleRow(0, col, row, _input[row][col]) || 
            VisibleRow(col + 1, _input[0].Length, row, _input[row][col]) ||
            VisibleColumn(0, row, col, _input[row][col]) ||
            VisibleColumn(row + 1, _input.Length, col,  _input[row][col]))
            return true;

        return false;
    }

    public bool VisibleRow(int start, int stop, int row, int value)
    {
        for(int col = start; col < stop; col++)
        {
            if (_input[row][col] >= value)
                return false;
        }

        return true;
    }

    public bool VisibleColumn(int start, int stop, int col, int value)
    {
        for (int row = start; row < stop; row++)
        {
            if (_input[row][col] >= value)
                return false;
        }

        return true;
    }

    public int SceneScoreRow(int row, int col, int value)
    {
        int scoreLeft = 0;
        for (int c = col-1; c >= 0; c--)
        {
            scoreLeft++;
            if (_input[row][c] >= value)
                break;
        }

        int scoreRight = 0;
        for (int c = col + 1; c < _input[0].Length; c++)
        {
            scoreRight++;
            if (_input[row][c] >= value)
                break;
        }

        return scoreLeft * scoreRight;
    }

    public int SceneScoreColumn(int row, int col, int value)
    {
        int scoreAbove = 0;
        for (int r = row - 1; r >= 0; r--)
        {
            scoreAbove++;
            if (_input[r][col] >= value)
                break;
        }

        int scoreBelow = 0;
        for (int r = row + 1; r < _input.Length; r++)
        {
            scoreBelow++;
            if (_input[r][col] >= value)
                break;
        }

        return scoreAbove * scoreBelow;
    }
}