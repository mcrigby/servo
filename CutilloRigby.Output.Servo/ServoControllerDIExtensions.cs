using CutilloRigby.Output.Servo;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class GamepadControllerDIExtensions
{
    public static IServiceCollection AddServoControllers(this IServiceCollection services)
    {
        services.AddHostedService<ServoController>();

        return services;
    }

    public static IServiceCollection AddServoState(this IServiceCollection services,
        byte chip, string? Name, IDictionary<byte, ServoOutput>? channels)
    {
        services.AddSingleton<ServoState>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<ServoState>>();

            var result = new ServoState(logger);

            result.Chip = chip;
            result.Name = Name ?? ServoState.Default_Name;
            result.Channels = channels ?? new Dictionary<byte, ServoOutput>();

            return result;
        });

        services.AddServoStateInterfaces();

        return services;
    }

    public static IServiceCollection AddServoState(this IServiceCollection services,
        byte chip, string? Name, IDictionary<string, ServoOutput>? channels)
    {
        if (channels != null && channels.Keys.Any(x => !byte.TryParse(x, out _)))
            throw new ArgumentException("Keys for Channels must be parsable to byte.");

        services.AddSingleton<ServoState>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<ServoState>>();

            var result = new ServoState(logger);

            result.Chip = chip;
            result.Name = Name ?? ServoState.Default_Name;
            result.Channels = channels?
                .ToDictionary(x => byte.Parse(x.Key), x => x.Value)
                ?? new Dictionary<byte, ServoOutput>();

            return result;
        });

        services.AddServoStateInterfaces();

        return services;
    }

    private static IServiceCollection AddServoStateInterfaces(this IServiceCollection services)
    {
        services.AddSingleton<IServoState>(provider =>
            provider.GetRequiredService<ServoState>()
        );
        services.AddSingleton<IServoOutputChanged>(provider =>
            provider.GetRequiredService<ServoState>()
        );

        return services;
    }
}
