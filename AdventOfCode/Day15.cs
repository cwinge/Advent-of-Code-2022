namespace AdventOfCode;

public class Day15 : BaseDay
{
    SensorBeacon[] pairs;
    record struct SensorBeacon(int xSensor, int ySensor, int xBeacon, int yBeacon, int distance);
    record struct PointDistance(int x, int y, int xDistance);
    record struct PointRange(PointDistance point)
    {
        public int minY = point.y - point.xDistance;
        public int maxY = point.y + point.xDistance;
    }
    public Day15()
    {
        pairs = File.ReadAllLines(InputFilePath).Select(s => new string(s.Where(x => char.IsNumber(x) || x == ',' || x == ':').ToArray()))
            .Select(x => x.Split(new char[] { ',', ':' }))
            .Select(x => (int.Parse(x.ElementAt(0)), int.Parse(x.ElementAt(1)), int.Parse(x.ElementAt(2)), int.Parse(x.ElementAt(3))))
            .Select(x => new SensorBeacon(x.Item1, x.Item2, x.Item3, x.Item4, Dist(x.Item1, x.Item2, x.Item3, x.Item4))).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        int y = 2_000_000;
        var minX = pairs.Select(p => p.xSensor - (p.distance - Math.Abs(y - p.ySensor))).Min();
        var maxX = pairs.Select(p => p.xSensor + (p.distance - Math.Abs(y - p.ySensor))).Max();

        return new(Enumerable.Range(minX, maxX - minX).Where(x => pairs.Any(p => Dist(x, y, p.xSensor, p.ySensor) <= p.distance)).Count().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int x = 0,  y = 0;
        foreach (var sensor in pairs)
        {
            if (LocateBeacon(sensor, out x, out y))
                break;
        }

        return new((x * (long)4_000_000 + y).ToString());
    }

    private bool LocateBeacon(SensorBeacon sensor, out int x, out int y)
    {
        for (x = Math.Max(0, sensor.xSensor - sensor.distance - 1); x <= Math.Min(4_000_000, sensor.xSensor + sensor.distance + 1); x++)
        {
            var maxY = Math.Min(sensor.ySensor + sensor.distance + 1 - Math.Abs(sensor.xSensor - x), 4_000_000);
            var minY = Math.Max(sensor.ySensor - (sensor.distance + 1 - Math.Abs(sensor.xSensor - x)), 0);
            int tempX = x; // capture x so it can be used in function
            if (pairs.All(s => Dist(s.xSensor, s.ySensor, tempX, maxY) > s.distance))
            {
                y = maxY;
                return true;
            }                
            if (pairs.All(s => Dist(s.xSensor, s.ySensor, tempX, minY) > s.distance))
            {
                y = minY;
                return true;
            }                
        }
        y = 0;
        return false;
    }

    private int Dist(int xA, int yA, int xB, int yB) => Math.Abs(xA - xB) + Math.Abs(yA - yB);
}