using CutilloRigby.Output.Servo;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServoDIExtensions
{
    public static IServiceCollection AddServo<T>(this IServiceCollection services)
    {
        services.AddSingleton<Servo<T>>();
        services.AddSingleton<IServo<T>>(provider => provider.GetRequiredService<Servo<T>>());
        services.AddHostedService<Servo<T>>(provider => provider.GetRequiredService<Servo<T>>());

        return services;
    }

    public static IServiceCollection AddServoRequirements(this IServiceCollection services)
    {
        services.AddSingleton<IServoChanged, ServoChanged>();

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
        var servoMapFactory = new ServoMapFactory(source ?? new Dictionary<string, IServoMap>());
        configure?.Invoke(servoMapFactory);

        services.AddSingleton<IServoMapFactory>(servoMapFactory);
        services.AddSingleton(typeof(IServoMap<>), typeof(ServoMap<>));

        return services;
    }
}
