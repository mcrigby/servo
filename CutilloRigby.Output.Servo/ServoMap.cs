namespace CutilloRigby.Output.Servo;

public sealed class ServoMap : IServoMap
{
    public ServoMap(float[] value, string? name = null)
    {
        Values = value;
        Name = name ?? "Unknown";
    }

    public string Name { get; private set; }

    public float[] Values { get; private set; }

    public float this[byte index] 
    {
        get
        {
            if (0 <= index && index < Values.Length)
                return Values[index];
            return 0;
        }
    }

    public static ServoMap LinearServoMap(string? name = null) => StandardServoMap(name: name ?? "Linear");
    public static ServoMap SignedServoMap(string? name = null) => StandardServoMap(-128, 127, name: name ?? "Signed");
    
    public static ServoMap StandardServoMap(short rangeStart = 0, short rangeEnd = 255,
        float dutyCycleMin = 0.05f, float dutyCycleMax = 0.10f, string? name = null) =>
        SplitDualRangeServoMap(rangeStart: rangeStart, rangeMid: rangeStart, rangeEnd: rangeEnd,
            lowRangeDutyCycleMin: dutyCycleMin, lowRangeDutyCycleMax: dutyCycleMin,
            highRangeDutyCycleMin: dutyCycleMin, highRangeDutyCycleMax: dutyCycleMax, name: name ?? "Standard");
    
    public static ServoMap DualRangeServoMap(short rangeStart = 0, short rangeMid = 128, short rangeEnd = 255,
        float dutyCycleMin = 0.05f, float dutyCycleMid = 0.075f, float dutyCycleMax = 0.10f, string? name = null) =>
        SplitDualRangeServoMap(rangeStart: rangeStart, rangeMid: rangeMid, rangeEnd: rangeEnd,
            lowRangeDutyCycleMin: dutyCycleMin, lowRangeDutyCycleMax: dutyCycleMid,
            highRangeDutyCycleMin: dutyCycleMid, highRangeDutyCycleMax: dutyCycleMax,
            name: name ?? "Dual Range");
    
    public static ServoMap SplitDualRangeServoMap(short rangeStart = 0, short rangeMid = 128, short rangeEnd = 255,
        float lowRangeDutyCycleMin = 0.05f, float lowRangeDutyCycleMax = 0.075f, 
        float highRangeDutyCycleMin = 0.075f, float highRangeDutyCycleMax = 0.10f, string? name = null) =>
        CustomServoMap(rangeStart: rangeStart, rangeMid: rangeMid, rangeEnd: rangeEnd, 
            lowRangeDutyCycleCalculation: GetDutyCycleCalculation(lowRangeDutyCycleMin, lowRangeDutyCycleMax, 0 - rangeStart, rangeMid - rangeStart),
            highRangeDutyCycleCalculation: GetDutyCycleCalculation(highRangeDutyCycleMin, highRangeDutyCycleMax, 0 - rangeMid, rangeEnd - rangeMid),
            name: name ?? "Split Dual Range");

    public static ServoMap CustomServoMap(short rangeStart = 0, short rangeMid = 128, short rangeEnd = 255,
        Func<int, float>? lowRangeDutyCycleCalculation = null, Func<int, float>? highRangeDutyCycleCalculation = null, 
        Func<int, int>? outputOrder = null, string? name = null)
    {
        if (!(rangeStart <= rangeMid && rangeMid <= rangeEnd))
            throw new ArgumentException("Invalid Range. rangeStart <= rangeMid <= rangeEnd");
        
        var rangeOffset = 0 - rangeStart;
        var rangeLength = (rangeEnd + 1) - rangeStart;

        var values = Enumerable.Range(rangeStart, rangeLength)
            .OrderBy(x => outputOrder?.Invoke(x) ?? (x < 0 ? rangeLength + x : x))
            .Select(x => 
                (x >= rangeMid 
                    ? highRangeDutyCycleCalculation?.Invoke(x) 
                    : lowRangeDutyCycleCalculation?.Invoke(x)) 
                ?? 0f)
            .ToArray();

        return new ServoMap(values, name ?? "Calulated Custom");
    }

    private static Func<int, float> GetDutyCycleCalculation(float min, float max, int offset, int steps)
    {
        var dutyCycleRange = max - min;
        var stepFactor = steps / dutyCycleRange;

        return x => ((x + offset) / stepFactor) + min;
    }

    public static implicit operator ServoMap(float[] value) => new ServoMap(value, "Implicit Conversion");
    public static implicit operator float[](ServoMap map) => map.Values;
}
