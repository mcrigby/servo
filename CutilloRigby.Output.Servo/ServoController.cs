using Microsoft.Extensions.Logging;
using System.Device.Pwm;

namespace CutilloRigby.Output.Servo;

public sealed class ServoController : IDisposable
{
    private readonly byte _address;
    private readonly IServoOutputChanged _outputChanged;
    private readonly IServoMap _map;

    private readonly PwmChannel _pwmChannel;

    public ServoController(byte address, IServoOutputChanged outputChanged, IServoMap map, ILogger<ServoController> logger)
    {
        _address = address;
        _outputChanged = outputChanged ?? throw new ArgumentNullException(nameof(outputChanged));
        _map = map ?? throw new ArgumentNullException(nameof(map));
        CreateLogHandlers(logger ?? throw new ArgumentNullException(nameof(logger)));

        _pwmChannel = PwmChannel.Create(0, _address, 50, _map[0]);
        _pwmChannel.Start();

        _outputChanged.Changed += OutputChangedHandler;
    }

    private void OutputChangedHandler(object? sender, ServoOutputEventArgs eventArgs)
    {
        if (_address != eventArgs.Address)
            return;
        
        var dutyCycle = _map[eventArgs.Value];
        _pwmChannel.DutyCycle = dutyCycle;

        _setInformation_DutyCycle(eventArgs.Address, eventArgs.Name ?? "Unknown", eventArgs.Value, dutyCycle);
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

    public void Dispose()
    {
        _pwmChannel.Stop();
    }

    private Action<byte, string, byte, float> _setInformation_DutyCycle = (address, buttonName, value, dutyCycle) => { };
}
