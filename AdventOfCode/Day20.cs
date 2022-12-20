using System.Collections.ObjectModel;

namespace AdventOfCode;

public class Day20 : BaseDay
{
    List<TrackableLong> input; // Keep order
    ObservableCollection<TrackableLong> decrypt; // Rearrange
    public Day20()
    {
        input = File.ReadAllLines(InputFilePath).Select(x => new TrackableLong(int.Parse(x))).ToList();
        decrypt = new ObservableCollection<TrackableLong>(input);
    }
    public class TrackableLong { public long Value { get; set; } public TrackableLong(long value) { Value = value; } }

    public override ValueTask<string> Solve_1()
    {
        Decrypt();
        var indexOf0 = decrypt.IndexOf(input.FirstOrDefault(x => x.Value == 0));

        return new((GetElement(indexOf0, 1000) + GetElement(indexOf0, 2000) + GetElement(indexOf0, 3000)).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        input.ForEach(x => x.Value *= 811589153);
        decrypt = new ObservableCollection<TrackableLong>(input);
        Enumerable.Range(0, 10).ToList().ForEach(x => Decrypt());
        var indexOf0 = decrypt.IndexOf(input.FirstOrDefault(x => x.Value == 0));

        return new((GetElement(indexOf0, 1000) + GetElement(indexOf0, 2000) + GetElement(indexOf0, 3000)).ToString());
    }

    long GetElement(int start, int add) => decrypt.ElementAt((start + add) % decrypt.Count).Value;
    private void Decrypt()
    {
        foreach (var i in input)
        {
            var oldIndex = decrypt.IndexOf(i);
            var newIndex = (oldIndex + i.Value) % (decrypt.Count - 1);
            newIndex = newIndex <= 0 ? (decrypt.Count - 1) + newIndex : newIndex;
            decrypt.Move(oldIndex, (int)newIndex);
        }
    }
}