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
    
    public static ServoMap StandardServoMap(short rangeMin = 0, short rangeMax = 255,
        float dutyCycleMin = 0.05f, float dutyCycleNeutral = 0.075f, float dutyCycleMax = 0.10f, string? name = null) =>
        SplitDualRangeServoMap(rangeMin: rangeMin, rangeNeutral: rangeMin, rangeMax: rangeMax,
            lowRangeDutyCycleMin: dutyCycleMin, lowRangeDutyCycleMax: dutyCycleMin, dutyCycleNeutral: dutyCycleMin,
            highRangeDutyCycleMin: dutyCycleMin, highRangeDutyCycleMax: dutyCycleMax, name: name ?? "Standard");
    
    public static ServoMap DualRangeServoMap(short rangeMin = 0, short rangeNeutral = 128, short rangeMax = 255,
        float dutyCycleMin = 0.05f, float dutyCycleNeutral = 0.075f, float dutyCycleMax = 0.10f, string? name = null) =>
        SplitDualRangeServoMap(rangeMin: rangeMin, rangeNeutral: rangeNeutral, rangeMax: rangeMax,
            lowRangeDutyCycleMin: dutyCycleMin, lowRangeDutyCycleMax: dutyCycleNeutral, dutyCycleNeutral: dutyCycleNeutral,
            highRangeDutyCycleMin: dutyCycleNeutral, highRangeDutyCycleMax: dutyCycleMax,
            name: name ?? "Dual Range");
    
    public static ServoMap SplitDualRangeServoMap(short rangeMin = 0, short rangeNeutral = 128, short rangeMax = 255,
        float lowRangeDutyCycleMin = 0.05f, float lowRangeDutyCycleMax = 0.075f, float dutyCycleNeutral = 0.075f,
        float highRangeDutyCycleMin = 0.075f, float highRangeDutyCycleMax = 0.10f, string? name = null) =>
        CustomServoMap(rangeMin: rangeMin, rangeNeutral: rangeNeutral, rangeMax: rangeMax, 
            lowRangeDutyCycleCalculation: GetDutyCycleCalculation(lowRangeDutyCycleMin, lowRangeDutyCycleMax, 0 - rangeMin, rangeNeutral - rangeMin),
            neutralDutyCycleCalculation: _ => dutyCycleNeutral,
            highRangeDutyCycleCalculation: GetDutyCycleCalculation(highRangeDutyCycleMin, highRangeDutyCycleMax, 0 - rangeNeutral, rangeMax - rangeNeutral),
            name: name ?? "Split Dual Range");

    public static ServoMap CustomServoMap(Func<int, float> lowRangeDutyCycleCalculation, Func<int, float> highRangeDutyCycleCalculation,
        Func<int, float> neutralDutyCycleCalculation, short rangeMin = 0, short rangeNeutral = 128, short rangeMax = 255,
        Func<int, int>? outputOrder = null, string? name = null)
    {
        if (!(rangeMin <= rangeNeutral && rangeNeutral <= rangeMax))
            throw new ArgumentException("Invalid Range. rangeMin <= rangeNeutral <= rangeMax");
        
        var rangeLength = (rangeMax + 1) - rangeMin;
        if (outputOrder == null)
            outputOrder = x => (x < 0 ? rangeLength + x : x);
        if (neutralDutyCycleCalculation == null)
            neutralDutyCycleCalculation = _ => 0f;
        
        var values = Enumerable.Range(rangeMin, rangeLength)
            .OrderBy(outputOrder)
            .Select(x => {
                if (x > rangeNeutral)
                    return highRangeDutyCycleCalculation(x);
                if (x < rangeNeutral)
                    return lowRangeDutyCycleCalculation(x);
                return neutralDutyCycleCalculation(x);
            })
            .ToArray();

        return new ServoMap(values, name ?? "Calulated Custom");
    }

    private static Func<int, float> GetDutyCycleCalculation(float min, float max, int offset, int steps)
    {
        var dutyCycleRange = max - min;
        var stepFactor = steps / dutyCycleRange;

        return x => ((x + offset) / stepFactor) + min;
    }

    public static implicit operator ServoMap(float[] value) => new ServoMap(value, "Converted from Values");
    public static implicit operator float[](ServoMap map) => map.Values;
}
