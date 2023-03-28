namespace CutilloRigby.Output.Servo;

public interface IServo
{
    string Name { get; }

    byte Value { get; }
    
    void SetValue(byte value);

    void Reset();

    void Start();
    
    void Stop();
}