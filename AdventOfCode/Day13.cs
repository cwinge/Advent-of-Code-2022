using System.Text.Json.Nodes;

namespace AdventOfCode;

public class Day13 : BaseDay
{
    (JsonNode, JsonNode)[] pairs;
    public Day13()
    {
        pairs = File.ReadAllLines(InputFilePath).Chunk(3).Select(x => (JsonNode.Parse(x.ElementAt(0)), JsonNode.Parse(x.ElementAt(1)))).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        int correct = 0;
        for (int i = 0; i < pairs.Length; i++)
        {
            if (IsCorrect(pairs[i].Item1, pairs[i].Item2) == true)
                correct += i + 1;
        }
        return new(correct.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var packets = pairs.SelectMany(x => new List<JsonNode> { x.Item1, x.Item2 }).ToList();
        var dividers = new List<JsonNode> { JsonNode.Parse("[[2]]"), JsonNode.Parse("[[6]]") };
        packets.AddRange(dividers);
        packets.Sort((l, r) => IsCorrect(l, r) == true ? -1 : 1);

        return new(((packets.IndexOf(dividers[0]) + 1) * (packets.IndexOf(dividers[1]) + 1)).ToString());
    }

    bool? IsCorrect(JsonNode left, JsonNode right)
    {
        if (left is JsonValue && right is JsonValue)
            return OutcomeValue(left.AsValue().GetValue<int>(), right.AsValue().GetValue<int>());

        return OutcomeArray(left, right);
    }

    private static bool? OutcomeValue(int left, int right) => left == right ? null : left < right;

    private bool? OutcomeArray(JsonNode left, JsonNode right)
    {
        JsonArray leftArray = left is JsonArray ? left.AsArray() : new JsonArray(left.GetValue<int>());
        JsonArray rightArray = right is JsonArray ? right.AsArray() : new JsonArray(right.GetValue<int>());

        foreach (var i in Enumerable.Range(0, Math.Min(leftArray.Count, rightArray.Count)))
        {
            var res = IsCorrect(leftArray[i], rightArray[i]);
            if (res.HasValue)
                return res.Value;
        }

        return leftArray.Count == rightArray.Count ? null : leftArray.Count < rightArray.Count;
    }
}