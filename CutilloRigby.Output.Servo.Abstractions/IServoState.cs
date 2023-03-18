namespace CutilloRigby.Output.Servo;

public interface IServoState
{
    bool HasChannel(byte address);

    void SetChannel(byte address, byte value);

    byte GetChannel(byte address);
}