namespace AdventOfCode;

public class Day17 : BaseDay
{
    string movements;
    HashSet<(int x, long y)> cave = new HashSet<(int x, long y)>();
    enum Direction { RIGHT, LEFT, UP, DOWN }
    public Day17()
    {
        movements = File.ReadAllText(InputFilePath);
        Enumerable.Range(0, 7).ToList().ForEach(x => cave.Add((x, 0))); // Add floor
    }

    public override ValueTask<string> Solve_1()
    {
        return new(DropRocks(2022).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        cave.Clear();
        Enumerable.Range(0, 7).ToList().ForEach(x => cave.Add((x, 0))); // Add floor
        return new(DropRocks(1000000000000, true).ToString());
    }

    private long DropRocks(long max, bool partTwo = false)
    {
        long top = 0, added = 0;
        int moveIndex = 0;
        var seen = new Dictionary<(int i, long turn), (long turn, long y)>();

        for (long turn = 0; turn < max; turn++)
        {
            var rock = GetRock(turn % 5, top + 4);
            while (true)
            {
                rock = MoveRockHorizontal(moveIndex, rock);
                moveIndex = (moveIndex + 1) % movements.Length;
                rock = MoveRock(rock, Direction.DOWN);
                if (rock.Overlaps(cave))
                {
                    rock = MoveRock(rock, Direction.UP);
                    cave.UnionWith(rock);
                    top = cave.Max(y => y.y);

                    if (partTwo)
                    {
                        var pos = (moveIndex, turn % 5);
                        if (seen.ContainsKey(pos) && turn >= 2022)
                        {
                            var turns = turn - seen[pos].turn;
                            var size = (max - turn) / turns;
                            added += size * top - seen[pos].y;
                            turn += size * turns;
                        }

                        seen[pos] = (turn, top);
                    }
                    break;
                }
            }
        }
        return top + added;
    }

    private HashSet<(int x, long y)> MoveRockHorizontal(int moveIndex, HashSet<(int x, long y)> rock)
    {
        if (movements[moveIndex] == '>')
        {
            rock = MoveRock(rock, Direction.RIGHT);
            if (rock.Overlaps(cave))
                rock = MoveRock(rock, Direction.LEFT);
        }
        else
        {
            rock = MoveRock(rock, Direction.LEFT);
            if (rock.Overlaps(cave))
                rock = MoveRock(rock, Direction.RIGHT);
        }

        return rock;
    }

    private HashSet<(int x, long y)> MoveRock(HashSet<(int x, long y)> rock, Direction direction)
    {
        if ((direction == Direction.RIGHT && rock.Any(x => x.x == 6)) || (direction == Direction.LEFT && rock.Any(x => x.x == 0)))
            return rock;

        var newRockPostition = new HashSet<(int x, long y)>();
        foreach (var (x, y) in rock)
        {
            int newX = direction == Direction.RIGHT ? x + 1 : (direction == Direction.LEFT ? x - 1 : x);
            long newY = direction == Direction.UP ? y + 1 : (direction == Direction.DOWN ? y - 1 : y);
            newRockPostition.Add((newX, newY));
        }
        return newRockPostition;
    }

    private HashSet<(int x, long y)> GetRock(long rockIndex, long y)
    {
        return rockIndex switch
        {
            0 => new HashSet<(int x, long y)> { (2, y), (3, y), (4, y), (5, y) },
            1 => new HashSet<(int x, long y)> { (3, y + 2), (2, y + 1), (3, y + 1), (4, y + 1), (3, y) },
            2 => new HashSet<(int x, long y)> { (2, y), (3, y), (4, y), (4, y + 1), (4, y + 2) },
            3 => new HashSet<(int x, long y)> { (2, y), (2, y + 1), (2, y + 2), (2, y + 3) },
            4 => new HashSet<(int x, long y)> { (2, y + 1), (2, y), (3, y + 1), (3, y) },
            _ => new HashSet<(int x, long y)>(),
        };
    }


}