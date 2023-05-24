namespace CutilloRigby.Output.Servo.Remappable;

public interface IRemappableServoMap : IServoMap
{
    bool AddMap(byte index, IServoMap values);
    bool Remap(byte index);
}
