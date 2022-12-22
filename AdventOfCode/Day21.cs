using Spectre.Console.Rendering;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Numerics;

namespace AdventOfCode;

public class Day21 : BaseDay
{
    List<Monkey> monkeys;
    record Monkey(string Name, long? num, char? Op, (string first, string second)? Pair) { public long? Num { get; set; } = num; };
    public Day21()
    {
        monkeys = File.ReadAllLines(InputFilePath).Select(x => x.Split(" "))
            .Select(x => x.Length == 2 ? new Monkey(x[0][..^1], long.Parse(x[1]), null, null) : new Monkey(x[0][..^1], null, x[2][0], (x[1], x[3]))).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        return new($"{Riddle(new List<Monkey>(monkeys), new Dictionary<string, long>(), true)}");
    }

    public override ValueTask<string> Solve_2()
    {        
        return new(RiddleExtended().ToString());
    }

    private long Riddle(List<Monkey> monkeys, Dictionary<string, long> numbers, bool partOne = false)
    {
        int remaining = int.MaxValue;
        var root = monkeys.FirstOrDefault(x => x.Name == "root");

        while(remaining > 0 && remaining != monkeys.Count)
        {
            remaining = monkeys.Count; // prevent deadlock if unused monkeys
            int i = 0;
            while (i < monkeys.Count)
            {
                if (monkeys[i].Num.HasValue) // num monkey
                {
                    numbers[monkeys[i].Name] = monkeys[i].Num.Value;
                    monkeys.RemoveAt(i);
                    i--;
                }
                else
                {
                    if (numbers.TryGetValue(monkeys[i].Pair.Value.first, out long first) && numbers.TryGetValue(monkeys[i].Pair.Value.second, out long second))
                    {
                        numbers[monkeys[i].Name] = OpResult(first, second, monkeys[i].Op);
                        monkeys.RemoveAt(i);
                        i--;
                    }
                }
                i++;
            }
        }

        return partOne ? numbers["root"] : numbers[root.Pair.Value.first] - numbers[root.Pair.Value.second];
    }

    private long RiddleExtended()
    {
        var human = monkeys.FirstOrDefault(x => x.Name == "humn");
        human.Num = 0;
        while (true)
        {
            long diff = Riddle(new List<Monkey>(monkeys), new Dictionary<string, long>());
            if (diff == 0)
                break;
            if (diff < 100)
                human.Num++;
            else
                human.Num += diff / 100;
        }

        return human.Num.Value;
    }
    public long OpResult(long first, long second, char? op)
    {
        return op switch
        {
            '*' => first * second,
            '/' => first / second,
            '-' => first - second,
            '+' => first + second,
            _ => throw new UnreachableException(),
        };
    }
}