namespace CutilloRigby.Output.Servo;

public sealed class ServoMap : IServoMap
{
    private float[] _value;

    public ServoMap() : this(defaultInitialSize) { }

    public ServoMap(short initialSize)
    {
        _value = Array.Empty<float>();
        Array.Resize<float>(ref _value, initialSize);
    }

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
        set
        {
            if (index >= _value.Length)
                Array.Resize<float>(ref _value, index + 1);
            
            _value[index] = value;
        }
    }

    public float[] Value
    {
        get => _value;
        set
        {
            if (value != null)
            {
                _value = value;
                return;
            }

            _value = Array.Empty<float>();
            Array.Resize<float>(ref _value, defaultInitialSize);   
        }
    }

    private const short defaultInitialSize = 256;

    public static IServoMap LinearServoMap() => CustomServoMap();
    public static IServoMap SignedServoMap() => CustomServoMap(-128);
    public static IServoMap CustomServoMap(int rangeStart, Func<int, float> dutyCycleCalculation, Func<int, int>? outputOrder = null)
    {

        var values = Enumerable.Range(rangeStart, 256)
            .OrderBy(x => outputOrder?.Invoke(x) ?? x)
            .Select(dutyCycleCalculation)
            .ToArray();

        return new ServoMap(values);
    }
    public static IServoMap CustomServoMap(short rangeStart = 0, short rangeLength = 256, float dutyCycleMin = 0.05f, float dutyCycleMax = 0.10f)
    {
        var rangeOffset = 0 - rangeStart;
        var dutyCycleRange = dutyCycleMax - dutyCycleMin;
        var stepFactor = (rangeLength - 1) / dutyCycleRange;

        var values = Enumerable.Range(rangeStart, rangeLength)
            .OrderBy(x => x < 0 ? rangeLength + x : x)
            .Select(x => ((x + rangeOffset) / stepFactor) + dutyCycleMin)
            .ToArray();

        return new ServoMap(values);
    }

    public static implicit operator ServoMap(float[] value) => new ServoMap(value);
    public static implicit operator float[](ServoMap map) => map.Value;
}
