using CutilloRigby.Output.Servo;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServoDIExtensions
{
    public static IServiceCollection AddServo<T>(this IServiceCollection services)
    {
        services.AddSingleton<IServo<T>, Servo<T>>();
        services.TryAddSingleton<IServoChanged, ServoChanged>();

        return services;
    }

    public static IServiceCollection AddServoConfiguration(this IServiceCollection services, 
        IDictionary<string, IServoConfiguration>? source = null, Action<IServoConfigurationFactory>? configure = null)
    {
        var servoConfigurationFactory = new ServoConfigurationFactory(source ?? new Dictionary<string, IServoConfiguration>());
        configure?.Invoke(servoConfigurationFactory);

        services.AddSingleton<IServoConfigurationFactory>(servoConfigurationFactory);
        services.AddSingleton(typeof(IServoConfiguration<>), typeof(ServoConfiguration<>));

        return services;
    }

    public static IServiceCollection AddServoMap(this IServiceCollection services, 
        IDictionary<string, IServoMap>? source = null, Action<IServoMapFactory>? configure = null)
    {
        services.AddSingleton<IServoMapFactory>(provider => {
            var servoMapFactory = new ServoMapFactory(source ?? new Dictionary<string, IServoMap>());
            configure?.Invoke(servoMapFactory);

            return servoMapFactory;
        });
        services.AddSingleton(typeof(IServoMap<>), typeof(ServoMap<>));

        return services;
    }
}
