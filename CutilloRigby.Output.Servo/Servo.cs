using System.Device.Pwm;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CutilloRigby.Output.Servo;

public sealed class Servo : IServo, IHostedService
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

    public string? Name { get => _configuration.Name; }

    public byte Value { get => _value; }

    public void SetValue(byte value)
    {

        if (_value != value)
        {
            setInformation_ValueChanged(_configuration.Name, _value, value);
            _value = value;

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
                        name, oldValue, newValue);;
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Start();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Stop();

        return Task.CompletedTask;
    }

    private Action<string?, object?, object?> setInformation_ValueChanged = (name, oldValue, newValue) => { };
}
