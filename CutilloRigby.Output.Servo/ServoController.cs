using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Device.Pwm;

namespace CutilloRigby.Output.Servo;

public sealed class ServoController : IHostedService
{
    private readonly IServoState _servoState;
    private readonly IServoOutputChanged _outputChanged;
    private readonly IServoMap _map;

    private readonly IDictionary<byte, PwmChannel> _pwmChannel;

    public ServoController(IServoState servoState, IServoOutputChanged outputChanged, IServoMap map, 
        ILogger<ServoController> logger)
    {
        _servoState = servoState ?? throw new ArgumentNullException(nameof(servoState));
        _outputChanged = outputChanged ?? throw new ArgumentNullException(nameof(outputChanged));
        _map = map ?? throw new ArgumentNullException(nameof(map));
        CreateLogHandlers(logger ?? throw new ArgumentNullException(nameof(logger)));

        _pwmChannel = _servoState.AvailableChannels
            .ToDictionary(x => x, x => PwmChannel.Create(_servoState.Chip, x, 50, _map[0]));

        _outputChanged.Changed += OutputChangedHandler;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var channel in _pwmChannel.Values)
            channel.Start();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var channel in _pwmChannel.Values)
            channel.Stop();

        return Task.CompletedTask;
    }

    private void OutputChangedHandler(object? sender, ServoOutputEventArgs eventArgs)
    {
        var dutyCycle = _map[eventArgs.Value];
        _pwmChannel[eventArgs.Address].DutyCycle = dutyCycle;

        _setInformation_DutyCycle(eventArgs.Address, eventArgs.Name ?? ServoState.Default_Name, eventArgs.Value, dutyCycle);
    }

    private void CreateLogHandlers(ILogger logger)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            _setInformation_DutyCycle = (address, name, value, dutyCycle) =>
                logger.LogInformation("Channel {name} ({address}) with Value {value}. Set Duty Cycle to {dutyCycle:n3}.", 
                    address, name, value, dutyCycle);
        }
    }

    private Action<byte, string, byte, float> _setInformation_DutyCycle = (address, buttonName, value, dutyCycle) => { };
}
