using CutilloRigby.Output.Servo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Harness;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .ConfigureHostOptions(options =>
            {
                options.ShutdownTimeout = TimeSpan.FromSeconds(30);
            })
            .ConfigureLogging(builder => 
                builder.AddConsole()
            )
            .ConfigureHostConfiguration(configurationBuilder => {
                configurationBuilder
                    .AddJsonFile("./appsettings.json");
            })
            .ConfigureServices((hostBuilder, services) =>
            {
                var servoConfigurationSection = hostBuilder.Configuration.GetSection("Servo");
                var servoConfigurationDictionary = servoConfigurationSection
                    .Get<Dictionary<string, ServoConfiguration>>(options => options.ErrorOnUnknownConfiguration = true)
                    .ToDictionary(x => x.Key, x => (IServoConfiguration)x.Value);
                var servoMapSection = hostBuilder.Configuration.GetSection("ServoMap");
                var servoMapDictionary = servoMapSection
                    .Get<Dictionary<string, float[]>>(options => options.ErrorOnUnknownConfiguration = true)
                    .ToDictionary(x => x.Key, x => (IServoMap)(ServoMap)x.Value);

                services.AddServo<Steering_Servo>();
                services.AddServoConfiguration(servoConfigurationDictionary);
                services.AddServoMap(servoMapDictionary, factory =>
                {
                    var steeringServoMap = ServoMap.StandardServoMap(
                        rangeStart: -128, rangeEnd: 127, 
                        dutyCycleMin: 0.056f, dutyCycleMax: 0.094f,
                        name: "Steering Servo");

                    factory.AddServoMap("Harness.Steering_Servo", 
                        new RemappableServoMap(new Dictionary<byte, IServoMap>{
                            {0, steeringServoMap},
                            {1, steeringServoMap.Reverse()}
                        }));
                });
            })
            .Build();

        var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStopping.Register(async () => await host.StopAsync());

        await host.StartAsync(lifetime.ApplicationStopping);
        Run(host, lifetime.ApplicationStopped);
    }

    private static void Run(IHost? host, CancellationToken cancellationToken)
    {
        if (host == null)
            return;

        var steeringServo = host.Services.GetRequiredService<IServo<Steering_Servo>>();
        var steeringServoMap = host.Services.GetRequiredService<IRemappableServoMap<Steering_Servo>>();
        var mapIndex = (byte)0;

        while (!cancellationToken.IsCancellationRequested)
        {
            var read = Console.ReadKey(true);

            switch (read.Key)
            {
                case ConsoleKey.S: 
                    //servoState.SetChannel(0, DecrementServoValue(servoState.GetChannel(0)));
                    break;
                case ConsoleKey.W: 
                    //servoState.SetChannel(0, IncrementServoValue(servoState.GetChannel(0)));
                    break;
                case ConsoleKey.A: 
                    steeringServo.SetValue(DecrementServoValue(steeringServo.Value));
                    break;
                case ConsoleKey.D:
                    steeringServo.SetValue(IncrementServoValue(steeringServo.Value));
                    break;
                case ConsoleKey.Y:
                    mapIndex = (byte)(mapIndex == 0 ? 1 : 0);
                    steeringServoMap.Remap(mapIndex);
                    Console.WriteLine($"Steering Servo Map Change: {steeringServoMap.Name}");
                    break;
                default:
                    break;
            };
        }

        steeringServo.SetValue(0);
    }

    private static byte IncrementServoValue(byte value)
    {
        if (value == 127)
            return value;
        if (value == 255)
            return 0;
        return (byte)(value + 1);
    }
    private static byte DecrementServoValue(byte value)
    {
        if (value == 128)
            return value;
        if (value == 0)
            return 255;
        return (byte)(value - 1);
    }
}
