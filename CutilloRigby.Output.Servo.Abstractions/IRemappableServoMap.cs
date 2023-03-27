namespace CutilloRigby.Output.Servo;

public interface IRemappableServoMap : IServoMap
{
    bool AddMap(byte index, float[] values);
    bool Remap(byte index);
}
