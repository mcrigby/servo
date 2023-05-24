using CutilloRigby.Output.Servo;
using CutilloRigby.Output.Servo.Remappable;

namespace Microsoft.Extensions.DependencyInjection;

public static class RemappableServoMapDIExtensions
{
    public static IServiceCollection AddRemappableServoMap(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IRemappableServoMap<>), typeof(RemappableServoMap<>));
        
        return services;
    }
}
