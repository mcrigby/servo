using Microsoft.Extensions.Logging;

namespace CutilloRigby.Output.Servo;

public sealed class ServoController
{
    private readonly IServoState _state;
    private readonly IServoMap _map;

    public ServoController(IServoState state, IServoMap map, ILogger<ServoController> logger)
    {
        _state = state ?? throw new ArgumentNullException(nameof(state));
        _map = map ?? throw new ArgumentNullException(nameof(map));
        CreateLogHandlers(logger ?? throw new ArgumentNullException(nameof(logger)));
    }

    private void CreateLogHandlers(ILogger logger)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            _setInformation_DutyCycle = (address, buttonName, value, dutyCycle) =>
                logger.LogInformation("Channel {name} ({address}) with Value {value}. Set Duty Cycle to {dutyCycle:n3}.", 
                    address, buttonName, value, dutyCycle);
        }

        if (logger.IsEnabled(LogLevel.Warning))
        {
        }

        if (logger.IsEnabled(LogLevel.Error))
        {
        }
    }

    private Action<byte, string, byte, float> _setInformation_DutyCycle = (address, buttonName, value, dutyCycle) => { };
}
