using Microsoft.Extensions.Logging;

namespace CutilloRigby.Output.Servo;

public sealed class ServoControllerFactory
{
    private readonly IServoState _servoState;
    private readonly IServoOutputChanged _outputChanged;
    private readonly IServoMap _map;
    private readonly ILoggerFactory _loggerFactory;

    private readonly IDictionary<byte, ServoController> _controllers;

    public ServoControllerFactory(IServoState servoState, IServoOutputChanged outputChanged, IServoMap map, ILoggerFactory loggerFactory)
    {
        _servoState = servoState ?? throw new ArgumentNullException(nameof(servoState));
        _outputChanged = outputChanged ?? throw new ArgumentNullException(nameof(outputChanged));
        _map = map ?? throw new ArgumentNullException(nameof(map));
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

        CreateLogHandlers(_loggerFactory.CreateLogger<ServoControllerFactory>());

        _controllers = new Dictionary<byte, ServoController>();
    }

    public ServoController? GetController(byte address)
    {
        if (!_servoState.HasChannel(address))
        {
            _getError_NotHandled(address);
            return null;
        }

        if (!_controllers.ContainsKey(address))
        {
            _controllers.Add(address, new ServoController(address, _outputChanged, _map, 
                _loggerFactory.CreateLogger<ServoController>()));
            _getInformation_Create(address);
        }

        return _controllers[address];
    }

    private void CreateLogHandlers(ILogger logger)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            _getInformation_Create = (address) =>
                logger.LogInformation("Controller added for Channel {address}.", address);
        }

        if (logger.IsEnabled(LogLevel.Error))
        {
            _getError_NotHandled = (address) =>
                logger.LogInformation("Channel {address} is not handled.", address);
        }
    }

    private Action<byte> _getInformation_Create = (address) => { };
    private Action<byte> _getError_NotHandled = (address) => { };
}
