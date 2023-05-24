namespace CutilloRigby.Output.Servo;

public interface IServoMapFactory
{
    void AddServoMap(string name, IServoMap map);
    void AddServoMap<T>(IServoMap map);
    IServoMap GetServoMap(string name);
    IServoMap GetServoMap<T>();
}