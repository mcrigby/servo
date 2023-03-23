namespace CutilloRigby.Output.Servo;

public sealed class ServoChanged : IServoChanged
{
    public void Trigger(object sender, ServoChangedEventArgs eventArgs)
    {
        Changed?.Invoke(sender, eventArgs);
    }

    public event EventHandler<ServoChangedEventArgs> Changed = delegate { };
}
