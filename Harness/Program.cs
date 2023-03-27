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

                services.AddServoMap(servoMapDictionary, factory =>
                {
                    factory.AddServoMap("Harness.Steering_Servo", 
                        //ServoMap.SignedServoMap());
                        ServoMap.CustomServoMap(rangeStart: -128, dutyCycleMin: 0.056f, dutyCycleMax: 0.094f));
                });
                services.AddServoConfiguration(servoConfigurationDictionary);
                services.AddServoRequirements();
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

        while (!cancellationToken.IsCancellationRequested)
        {
            var read = Console.ReadKey(true);

            switch (read.Key)
            {
                case ConsoleKey.DownArrow: 
                    //servoState.SetChannel(0, DecrementServoValue(servoState.GetChannel(0)));
                    break;
                case ConsoleKey.UpArrow: 
                    //servoState.SetChannel(0, IncrementServoValue(servoState.GetChannel(0)));
                    break;
                case ConsoleKey.LeftArrow: 
                    steeringServo.SetValue(DecrementServoValue(steeringServo.Value));
                    break;
                case ConsoleKey.RightArrow:
                    steeringServo.SetValue(IncrementServoValue(steeringServo.Value));
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
