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

    public static ServoMap LinearServoMap(string? name = null) => CustomServoMap(name: name ?? "Linear");
    public static ServoMap SignedServoMap(string? name = null) => CustomServoMap(-128, name: name ?? "Signed");
    public static ServoMap CustomServoMap(int rangeStart, Func<int, float> dutyCycleCalculation, Func<int, int>? outputOrder = null, string? name = null)
    {

        var values = Enumerable.Range(rangeStart, 256)
            .OrderBy(x => outputOrder?.Invoke(x) ?? x)
            .Select(dutyCycleCalculation)
            .ToArray();

        return new ServoMap(values, name ?? "Calulated Custom");
    }
    public static ServoMap CustomServoMap(short rangeStart = 0, short rangeLength = 256, float dutyCycleMin = 0.05f, float dutyCycleMax = 0.10f, string? name = null)
    {
        var rangeOffset = 0 - rangeStart;
        var dutyCycleRange = dutyCycleMax - dutyCycleMin;
        var stepFactor = (rangeLength - 1) / dutyCycleRange;

        var values = Enumerable.Range(rangeStart, rangeLength)
            .OrderBy(x => x < 0 ? rangeLength + x : x)
            .Select(x => ((x + rangeOffset) / stepFactor) + dutyCycleMin)
            .ToArray();

        return new ServoMap(values, name ?? "Standard Custom");
    }

    public static implicit operator ServoMap(float[] value) => new ServoMap(value, "Implicit Conversion");
    public static implicit operator float[](ServoMap map) => map.Values;
}
