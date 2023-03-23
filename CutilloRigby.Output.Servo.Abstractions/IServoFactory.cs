namespace CutilloRigby.Output.Servo;

public interface IServoFactory
{
    void AddServo(string name, IServo servo);
    IServo GetServo(string name);
}