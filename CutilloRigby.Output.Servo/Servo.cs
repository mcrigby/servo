using System.Device.Pwm;
using Microsoft.Extensions.Logging;

namespace CutilloRigby.Output.Servo;

public sealed class Servo : IServo
{
    private readonly IServoConfiguration _configuration;
    private readonly IServoMap _map;
    private readonly PwmChannel _channel;

    private readonly IServoChanged _servoChanged;
    private readonly ServoChangedEventArgs _eventArgs;

    private byte _value;

    public Servo(IServoConfiguration configuration, IServoMap map, IServoChanged servoChanged, ILogger<Servo> logger)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _map = map ?? throw new ArgumentNullException(nameof(map));
        SetLogHandlers(logger ?? throw new ArgumentNullException(nameof(logger)));

        _value = _configuration.DefaultValue;
        _channel = PwmChannel.Create(_configuration.Chip, _configuration.Channel, 50, _map[_configuration.DefaultValue]);

        _servoChanged = servoChanged ?? throw new ArgumentNullException(nameof(servoChanged));
        _eventArgs = new ServoChangedEventArgs
        {
            Chip = _configuration.Chip,
            Channel = _configuration.Channel,
            Name = _configuration.Name,
            Value = _configuration.DefaultValue
        };

        _servoChanged.Trigger(this, _eventArgs);
    }

    public string Name => _configuration.Name ?? "Unnamed";

    public byte Value => _value;

    public void SetValue(byte value)
    {
        if (_value != value)
        {
            setInformation_ValueChanged(_configuration.Name, _value, value);
            _value = value;

            var dutyCycle = _map[_value];
            _channel.DutyCycle = dutyCycle;
            setInformation_DutyCycleChanged(_configuration.Name, dutyCycle);

            _eventArgs.Value = _value;
            _servoChanged.Trigger(this, _eventArgs);
        }
    }

    public void Reset()
    {
        SetValue(_configuration.DefaultValue);
    }

    public void Start()
    {
        _channel.Start();
    }

    public void Stop()
    {
        _channel.Stop();
    }

    private void SetLogHandlers(ILogger logger)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            setInformation_ValueChanged = (name, oldValue, newValue) => 
                logger.LogInformation("Channel {name} value changed from {oldValue} to {newValue}.", 
                        name, oldValue, newValue);
            setInformation_DutyCycleChanged = (name, dutyCycle) => 
                logger.LogInformation("Channel {name} duty cycle set to {dutyCycle}.", 
                        name, dutyCycle);
        }
    }

    private Action<string?, object?, object?> setInformation_ValueChanged = (name, oldValue, newValue) => { };
    private Action<string?, float?> setInformation_DutyCycleChanged = (name, dutyCycle) => { };
}
