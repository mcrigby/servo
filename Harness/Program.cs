using System.Device.Gpio;
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
                var servoSettingsSection = hostBuilder.Configuration.GetSection("ServoSettings");
                var servoSettingsConfiguration = servoSettingsSection.Get<ServoSettingsConfiguration>(options => options.ErrorOnUnknownConfiguration = true);

                services.AddSingleton<IServoMap>(ServoMap.SignedServoMap());
                services.AddServoState(servoSettingsConfiguration.Chip, servoSettingsConfiguration.Name,
                    servoSettingsConfiguration.Channels);
                services.AddServoControllers();

                services.AddSingleton<GpioController>();
                services.AddSingleton<StatusLed>();
            })
            .Build();

        var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStopping.Register(async () => await host.StopAsync());

        await host.StartAsync(lifetime.ApplicationStopping);
        Run(host, lifetime.ApplicationStopped);

        Stop(host);
    }

    private static void Run(IHost? host, CancellationToken cancellationToken)
    {
        if (host == null)
            return;

        var statusLed = host.Services.GetRequiredService<StatusLed>();
        var servoState = host.Services.GetRequiredService<IServoState>();

        while (!cancellationToken.IsCancellationRequested)
        {
            var read = Console.ReadKey(true);

            switch (read.Key)
            {
                case ConsoleKey.DownArrow: 
                    servoState.SetChannel(0, DecrementServoValue(servoState.GetChannel(0)));
                    break;
                case ConsoleKey.UpArrow: 
                    servoState.SetChannel(0, IncrementServoValue(servoState.GetChannel(0)));
                    break;
                case ConsoleKey.LeftArrow: 
                    servoState.SetChannel(1, DecrementServoValue(servoState.GetChannel(1)));
                    break;
                case ConsoleKey.RightArrow:
                    servoState.SetChannel(1, IncrementServoValue(servoState.GetChannel(1)));
                    break;
                default:
                    break;
            };
        }

        servoState.SetChannel(0, 0);
        servoState.SetChannel(1, 0);
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

    private static void Stop(IHost? host)
    {
        if (host == null)
            return;

        var statusLed = host.Services.GetRequiredService<StatusLed>();

        statusLed.SetRedLed(false);
        statusLed.SetGreenLed(false);
        statusLed.SetBlueLed(false);
    }
}
