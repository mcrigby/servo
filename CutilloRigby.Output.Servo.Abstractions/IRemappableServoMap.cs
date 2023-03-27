namespace CutilloRigby.Output.Servo;

public interface IRemappableServoMap : IServoMap
{
    bool AddMap(byte index, IServoMap values);
    bool Remap(byte index);
}
