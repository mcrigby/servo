namespace CutilloRigby.Output.Servo;

public interface IServoChanged
{
    void Trigger(object sender, ServoChangedEventArgs eventArgs);
    event EventHandler<ServoChangedEventArgs> Changed;
}
