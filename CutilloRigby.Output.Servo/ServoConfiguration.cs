namespace CutilloRigby.Output.Servo;

public sealed class ServoConfiguration : IServoConfiguration
{
    public byte Chip { get; set; }
    public byte Channel { get; set; }
    public string? Name { get; set; }
    public bool Enabled { get; set; }
    public byte DefaultValue { get; set; }

    public static readonly IServoConfiguration None = new ServoConfiguration {
        Chip = 255,
        Channel = 255,
        Name = "None",
        Enabled = false,
        DefaultValue = 0
    };
}
