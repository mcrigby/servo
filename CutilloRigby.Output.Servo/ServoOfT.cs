using Microsoft.Extensions.Logging;

namespace CutilloRigby.Output.Servo;

public sealed class Servo<T> : IServo<T>
{
    private readonly IServo _servo;

    public Servo(IServoConfiguration<T> configuration, IServoMap<T> map, IServoChanged servoChanged, ILogger<Servo> logger)
    {
        _servo = new Servo(configuration, map, servoChanged, logger);
    }

    public string Name => _servo.Name;

    public byte Value => _servo.Value;

    public void SetValue(byte value) => _servo.SetValue(value);

    public void OverwriteValue(byte value) => _servo.OverwriteValue(value);
    
    public void Reset() => _servo.Reset();

    public void Start() => _servo.Start();
    public void Stop() => _servo.Stop();
}
