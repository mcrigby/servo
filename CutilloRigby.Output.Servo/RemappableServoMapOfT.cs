namespace CutilloRigby.Output.Servo;

public sealed class RemappableServoMap<T> : IRemappableServoMap<T>
{
    private readonly IRemappableServoMap _servoMap;

    public RemappableServoMap(IRemappableServoMapFactory servoMapFactory)
    {
        _servoMap = servoMapFactory.GetRemappableServoMap(typeof(T).FullName.Replace('+', '.'));
    }

    public float this[byte index] { get => _servoMap[index]; }

    public bool AddMap(byte index, float[] values) => _servoMap.AddMap(index, values);

    public bool Remap(byte index) => _servoMap.Remap(index);
}
