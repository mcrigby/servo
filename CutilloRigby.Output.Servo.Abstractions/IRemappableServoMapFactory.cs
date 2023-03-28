namespace CutilloRigby.Output.Servo;

public interface IRemappableServoMapFactory
{
    void AddRemappableServoMap(string name, IRemappableServoMap map);
    void AddRemappableServoMap<T>(IRemappableServoMap map);
    IRemappableServoMap GetRemappableServoMap(string name);
}