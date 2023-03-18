namespace CutilloRigby.Output.Servo;

public interface IServoOutputChanged
{
    event EventHandler<ServoOutputEventArgs> Changed;
}
