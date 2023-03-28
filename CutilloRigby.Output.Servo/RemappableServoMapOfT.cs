namespace CutilloRigby.Output.Servo;

public sealed class RemappableServoMap<T> : IRemappableServoMap<T>
{
    private readonly IRemappableServoMap _servoMap;

    public RemappableServoMap(IRemappableServoMapFactory servoMapFactory)
    {
        _servoMap = servoMapFactory.GetRemappableServoMap(typeof(T).FactoryName());
    }

    public string Name => _servoMap.Name;

    public float[] Values => _servoMap.Values;

    public float this[byte index] => _servoMap[index];

    public bool AddMap(byte index, IServoMap map) => _servoMap.AddMap(index, map);

    public bool Remap(byte index) => _servoMap.Remap(index);
}
