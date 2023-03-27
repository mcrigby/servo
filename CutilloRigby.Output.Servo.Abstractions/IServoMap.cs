namespace CutilloRigby.Output.Servo;

public interface IServoMap
{
    string Name { get; }
    float[] Values { get; }

    float this[byte index] { get; }
}
