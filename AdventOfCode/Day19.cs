using System;

namespace AdventOfCode;

public class Day19 : BaseDay
{
    Blueprint[] blueprints;
    record Blueprint(int Id, int ore, int clay, int obsidOre, int obsidClay, int geodeOre, int obsid);
    record State(int[] Robots, int[] Inventory, Blueprint Blueprint, int TimeCurrent, int TimeLimit);
    record ProductionTick(int[] robots, int[] inventory, int time);
    public Day19()
    {
        blueprints = File.ReadAllLines(InputFilePath).Select(x => x.Split(" "))
            .Select(x => new Blueprint(int.Parse(x[1][0..^1]), int.Parse(x[6]), int.Parse(x[12]),
            int.Parse(x[18]), int.Parse(x[21]), int.Parse(x[27]), int.Parse(x[30]))).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        return new(blueprints.AsParallel()
           .Select(x => x.Id * ProduceRobots(new State(new int[] { 1, 0, 0, 0 }, new int[4] { 1, 0, 0, 0 }, x, 1, 24))).Sum().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(blueprints.Take(3).AsParallel()
            .Select(x => (long)ProduceRobots( new State(new int[] { 1, 0, 0, 0 }, new int[4] { 1, 0, 0, 0 }, x, 1, 32))).Aggregate((a,b) => a*b).ToString());
    }
    int ProduceRobots(State state)
    {
        int max = 0;
        ProductionTick tick;
        if (state.TimeCurrent > state.TimeLimit) 
            return 0;
        if (state.TimeCurrent == state.TimeLimit) 
            return state.Inventory[3];

        if (state.Robots[2] > 0)
        {
            tick = AdvanceTime(state, 3, 0, state.Blueprint.geodeOre, 2, state.Blueprint.obsid);
            int tmp = state.Inventory[3] + state.Robots[3] * (state.TimeLimit - state.TimeCurrent);
            if (state.TimeCurrent + tick.time <= state.TimeLimit) tmp = ProduceRobots(new State(tick.robots, tick.inventory, state.Blueprint, state.TimeCurrent + tick.time, state.TimeLimit));
            max = Math.Max(max, tmp);
            if (tick.time == 1) 
                return max;
        }
        
        if (state.Robots[0] < 4)
        {
            tick = AdvanceTime(state, 0, 0, state.Blueprint.ore, 0, 0);
            max = Math.Max(max, ProduceRobots(new State(tick.robots, tick.inventory, state.Blueprint, state.TimeCurrent + tick.time, state.TimeLimit)));
        }
        
        if (state.Robots[1] < 8)
        {
            tick = AdvanceTime(state, 1, 0, state.Blueprint.clay, 0, 0);
            max = Math.Max(max, ProduceRobots(new State(tick.robots, tick.inventory, state.Blueprint, state.TimeCurrent + tick.time, state.TimeLimit)));
        }
        
        if (state.Robots[1] > 0 && state.Robots[2] < 8)
        {
            tick = AdvanceTime(state, 2, 0, state.Blueprint.obsidOre, 1, state.Blueprint.obsidClay);
            max = Math.Max(max, ProduceRobots(new State(tick.robots, tick.inventory, state.Blueprint, state.TimeCurrent + tick.time, state.TimeLimit)));
        }
        return max;
    }

    private ProductionTick AdvanceTime(State state, int robotIndex, int typeIndex, int oreCount, int typeIndexSecond, int oreCountSecond)
    {
        int time = Math.Max((oreCount - state.Inventory[typeIndex] + state.Robots[typeIndex] - 1) / state.Robots[typeIndex], (oreCountSecond - state.Inventory[typeIndexSecond] + state.Robots[typeIndexSecond] - 1) / state.Robots[typeIndexSecond]);
        time = time < 0 ? 1 : time + 1;
        int[] newInventory = new int[4] { state.Inventory[0] + state.Robots[0] * time, state.Inventory[1] + state.Robots[1] * time, state.Inventory[2] + state.Robots[2] * time, state.Inventory[3] + state.Robots[3] * time };
        newInventory[typeIndex] -= oreCount; 
        newInventory[typeIndexSecond] -= oreCountSecond;
        int[] newRobots = new int[4] { state.Robots[0], state.Robots[1], state.Robots[2], state.Robots[3] }; newRobots[robotIndex]++;
        return new ProductionTick(newRobots, newInventory, time);
    }
}