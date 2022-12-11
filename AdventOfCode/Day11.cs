namespace AdventOfCode;

public class Day11 : BaseDay
{
    private Monkey[] _input, _input2;

    private record Monkey(int id, List<long> items, Operation op, OpVal opVal, int test, int trueMonkey, int falseMonkey)
    {
        public long inspecations = 0;

        public void InspectItem(Monkey[] monkies, Func<long, long> worry)
        {
            if (items.Count == 0) return;
            inspecations++;
            var item = items.First();
            item = worry(op == Operation.ADD ? item + (opVal.old ? item : opVal.value) : item * (opVal.old ? item : opVal.value));

            if (item % test == 0)
                monkies[trueMonkey].items.Add(item);
            else
                monkies[falseMonkey].items.Add(item);

            items.RemoveAt(0);
        }
    }

    public Day11()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.Split(" ")).Chunk(7).Select(x => CreateMonkeyFromInput(x)).ToArray();
        _input2 = File.ReadAllLines(InputFilePath).Select(x => x.Split(" ")).Chunk(7).Select(x => CreateMonkeyFromInput(x)).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        return new(RunRounds(_input, 20, (x) => x / 3));
    }

    public override ValueTask<string> Solve_2()
    {
        long modulo = _input2.Select(x => x.test).Aggregate((a, x) => a * x);

        return new(RunRounds(_input2, 10_000, (x) => x % modulo));
    }

    private string RunRounds(Monkey[] monkies, int rounds, Func<long, long> worry)
    {
        for (int i = 0; i < rounds; i++)
        {
            foreach (var m in monkies)
            {
                int items = m.items.Count;
                for (int c = 0; c < items; c++)
                {
                    m.InspectItem(monkies, worry);
                }
            }
        }

        return monkies.Select(x => x.inspecations).OrderDescending().Take(2).Aggregate((a, x) => a * x).ToString();
    }

    enum Operation { ADD, MUL }
    record OpVal(int value, bool old);
    private Monkey CreateMonkeyFromInput(string[][] input)
    {
        return new Monkey(
            input[0][1][0] - 48,
            input[1].Skip(2).Where(n => char.IsNumber(n[0])).Select(n => long.Parse(n.Substring(0, 2))).ToList(),
            input[2][input[2].Length - 2] == "*" ? Operation.MUL : Operation.ADD,
            int.TryParse(input[2][input[2].Length - 1], out int res) ? new OpVal(res, false) : new OpVal(0, true),
            int.Parse(input[3][input[3].Length - 1]),
            int.Parse(input[4][input[4].Length - 1]),
            int.Parse(input[5][input[5].Length - 1]));
    }
}