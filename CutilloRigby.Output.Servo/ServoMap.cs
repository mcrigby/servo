namespace CutilloRigby.Output.Servo;

public sealed class ServoMap : IServoMap
{
    private readonly float[] _value;

    public ServoMap(float[] value)
    {
        _value = value;
    }

    public float this[byte index] 
    {
        get
        {
            if (0 <= index && index < _value.Length)
                return _value[index];
            return 0;
        }
    }

    public static IServoMap LinearServoMap() => CustomServoMap(0, x => (float)((x / 5120) + 0.05));
    public static IServoMap SignedServoMap() => CustomServoMap(-128, x => (float)(((x + 128) / 5120) + 0.05), x => x < 0 ? 256 + x : x);
    public static IServoMap CustomServoMap(int rangeStart, Func<float, float> dutyCycleCalculation, Func<int, int>? outputOrder = null)
    {

        var values = Enumerable.Range(rangeStart, 256)
            .OrderBy(x => outputOrder?.Invoke(x) ?? x)
            .Select(x => (float)x)
            .Select(dutyCycleCalculation)
            .ToArray();

        return new ServoMap(values);
    }
}
