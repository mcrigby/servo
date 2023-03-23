namespace CutilloRigby.Output.Servo;

public sealed class ServoConfiguration<T> : IServoConfiguration<T>
{
    private readonly IServoConfiguration _servoConfiguration;

    public ServoConfiguration(IServoConfigurationFactory servoConfigurationFactory)
    {
        _servoConfiguration = servoConfigurationFactory.GetServoConfiguration(typeof(T).FullName.Replace('+', '.'));
    }

    public byte Chip { get => _servoConfiguration.Chip; set => _servoConfiguration.Chip = value; }
    public byte Channel { get => _servoConfiguration.Channel; set => _servoConfiguration.Channel = value; }
    public string? Name { get => _servoConfiguration.Name; set => _servoConfiguration.Name = value; }
    public bool Enabled { get => _servoConfiguration.Enabled; set => _servoConfiguration.Enabled = value; }
    public byte DefaultValue { get => _servoConfiguration.DefaultValue; set => _servoConfiguration.DefaultValue = value; }
}
