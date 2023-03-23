namespace CutilloRigby.Output.Servo;

public sealed class ServoConfiguration : IServoConfiguration
{
    public byte Chip { get; set; }
    public byte Channel { get; set; }
    public string? Name { get; set; }
    public bool Enabled { get; set; }
    public byte DefaultValue { get; set; }
}