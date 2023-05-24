namespace CutilloRigby.Output.Servo;

public interface IServoConfigurationFactory
{
    void AddServoConfiguration(string name, IServoConfiguration configuration);
    void AddServoConfiguration<T>(IServoConfiguration configuration);
    IServoConfiguration GetServoConfiguration(string name);
    IServoConfiguration GetServoConfiguration<T>();
}