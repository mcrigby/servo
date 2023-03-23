using CutilloRigby.Output.Servo;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServoDIExtensions
{
    public static IServiceCollection AddServos(this IServiceCollection services,
        Action<IServoFactory>? configure = null)
    {
        var servoFactory = new ServoFactory();
        configure?.Invoke(servoFactory);

        services.AddSingleton<IServoFactory>(servoFactory);
        services.AddSingleton(typeof(IServo<>), typeof(Servo<>));

        services.AddSingleton<IServoChanged, ServoChanged>();

        return services;
    }

    public static IServiceCollection AddServoConfiguration(this IServiceCollection services, 
        Action<IServoConfigurationFactory>? configure = null)
    {
        var servoConfigurationFactory = new ServoConfigurationFactory();
        configure?.Invoke(servoConfigurationFactory);

        services.AddSingleton<IServoConfigurationFactory>(servoConfigurationFactory);
        services.AddSingleton(typeof(IServoConfiguration<>), typeof(ServoConfiguration<>));

        return services;
    }

    public static IServiceCollection AddServoMap(this IServiceCollection services, 
        Action<IServoMapFactory>? configure = null)
    {
        var servoMapFactory = new ServoMapFactory();
        configure?.Invoke(servoMapFactory);

        services.AddSingleton<IServoMapFactory>(servoMapFactory);
        services.AddSingleton(typeof(IServoMap<>), typeof(ServoMap<>));

        return services;
    }
}
