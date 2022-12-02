using System.Runtime.CompilerServices;

namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly string[] _input;
    private readonly record struct Round(char OpposingHand, char OwnHand);
    private List<Round> _rounds = new();

    public Day02()
    {
        _input = File.ReadAllLines(InputFilePath);
        _rounds.AddRange(_input.Select(x => x.Split(" ")).Select( r => new Round(char.Parse(r[0]), char.Parse(r[1]))));
    }

    public override ValueTask<string> Solve_1()
    {
        int score = 0;
        foreach (var round in _rounds)
        {
            score += OutcomeScore(round) + SignScore(round.OwnHand);
        }

        return new(score.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int score = 0;
        foreach (var round in _rounds)
        {
            switch (round.OwnHand)
            {
                case 'X': // loss
                    score += 0 + (round.OpposingHand == 'A' ? SignScore('Z') : SignScore((char)(round.OpposingHand + 23 - 1)));
                    break;
                case 'Y': // draw
                    score += 3 + (SignScore((char)(round.OpposingHand + 23)));
                    break;
                case 'Z': // win
                    score += 6 + (round.OpposingHand == 'C' ? SignScore('X') : SignScore((char)(round.OpposingHand + 23 + 1)));
                    break;
                default:
                    Console.WriteLine("Error with round: " + round.ToString());
                    break;
            }
        }

        return new(score.ToString());
    }

    private static int SignScore(char hand)
    {
        return hand == 'X' ? 1 : hand == 'Y' ? 2 : 3;
    }

    private static int OutcomeScore(Round round)
    {
        switch (round.OwnHand)
        {
            case 'X':
                return round.OpposingHand == 'A' ? 3 : round.OpposingHand == 'B' ? 0 : 6;
            case 'Y':
                return round.OpposingHand == 'B' ? 3 : round.OpposingHand == 'C' ? 0 : 6;
            case 'Z':
                return round.OpposingHand == 'C' ? 3 : round.OpposingHand == 'A' ? 0 : 6;
            default:
                Console.WriteLine("Error with round: " + round.ToString());
                return -1;
        }
    }
}
