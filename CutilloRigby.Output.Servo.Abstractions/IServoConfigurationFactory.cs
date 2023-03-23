namespace CutilloRigby.Output.Servo;

public interface IServoConfigurationFactory
{
    void AddServoConfiguration(string name, IServoConfiguration configuration);
    IServoConfiguration GetServoConfiguration(string name);
}