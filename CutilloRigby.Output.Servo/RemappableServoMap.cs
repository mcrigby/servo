namespace CutilloRigby.Output.Servo;

public sealed class RemappableServoMap : IRemappableServoMap
{
    private readonly IDictionary<byte, float[]> _values;
    private float[] _activeValues;

    public RemappableServoMap(IDictionary<byte, float[]>? values = null)
    {
        _values = values ?? new Dictionary<byte, float[]>();
        
        _activeValues = _values.Values.FirstOrDefault();
    }

    public float this[byte index]
    {
        get
        {
            if (0 <= index && index < _activeValues.Length)
                return _activeValues[index];
            return 0;
        }
    }

    public bool AddMap(byte index, float[] values)
    {
        if (_values.ContainsKey(index))
            return false;

        _values.Add(index, values);
        return true;
    }

    public bool Remap(byte index)
    {
        if (!_values.ContainsKey(index))
            return false;

        _activeValues = _values[index];
        return true;
    }

    public static implicit operator RemappableServoMap(float[] value) => new RemappableServoMap(new Dictionary<byte, float[]> { {0, value} } );
    public static implicit operator float[](RemappableServoMap map) => map._activeValues;
}
