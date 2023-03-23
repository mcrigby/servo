namespace CutilloRigby.Output.Servo;

public sealed class ServoMap<T> : IServoMap<T>
{
    private readonly IServoMap _servoMap;

    public ServoMap(IServoMapFactory servoMapFactory)
    {
        _servoMap = servoMapFactory.GetServoMap(typeof(T).FullName.Replace('+', '.'));
    }

    public float this[byte index] => _servoMap[index];
}
