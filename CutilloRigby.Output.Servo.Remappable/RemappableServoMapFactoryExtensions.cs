using CutilloRigby.Output.Servo.Remappable;

namespace CutilloRigby.Output.Servo;

public static class RemappableServoMapFactoryExtensions
{
    public static void AddRemappableServoMap(this IServoMapFactory servoMapFactory, string name, IRemappableServoMap map)
    {
        servoMapFactory.AddServoMap(name, map);
    }

    public static void AddRemappableServoMap<T>(this IServoMapFactory servoMapFactory, IRemappableServoMap map)
    {
        servoMapFactory.AddServoMap<T>(map);
    }

    public static IRemappableServoMap GetRemappableServoMap(this IServoMapFactory servoMapFactory, string name)
    {
        var map = servoMapFactory.GetServoMap(name);
        if (map is IRemappableServoMap remappable)
            return remappable;

        return RemappableServoMap.Default;
    }

    public static IRemappableServoMap GetRemappableServoMap<T>(this IServoMapFactory servoMapFactory)
    {
        var map = servoMapFactory.GetServoMap<T>();
        if (map is IRemappableServoMap remappable)
            return remappable;

        return RemappableServoMap.Default;
    }
}
