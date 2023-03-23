namespace CutilloRigby.Output.Servo;

public interface IServoConfiguration
{
    byte Chip { get; set; }
    byte Channel { get; set; }
    string? Name { get; set; }
    bool Enabled { get; set; }
    byte DefaultValue { get; set; }
}
