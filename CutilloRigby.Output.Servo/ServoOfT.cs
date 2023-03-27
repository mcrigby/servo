using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CutilloRigby.Output.Servo;

public sealed class Servo<T> : IServo<T>, IHostedService
{
    private readonly IServo _servo;

    public Servo(IServoConfiguration<T> configuration, IServoMap<T> map, IServoChanged servoChanged, ILogger<Servo> logger)
    {
        _servo = new Servo(configuration, map, servoChanged, logger);
    }

    public string? Name { get => _servo.Name; }

    public byte Value { get => _servo.Value; }

    public void SetValue(byte value) => _servo.SetValue(value);

    public void Reset() => _servo.Reset();

    public void Start() => _servo.Start();
    public void Stop() => _servo.Stop();

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
}
