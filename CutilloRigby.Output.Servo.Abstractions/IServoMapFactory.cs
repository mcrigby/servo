namespace CutilloRigby.Output.Servo;

public interface IServoMapFactory
{
    void AddServoMap(string name, IServoMap map);
    IServoMap GetServoMap(string name);
}