namespace CutilloRigby.Output.Servo;

public sealed class ServoChangedEventArgs
{
    public byte Chip { get; set; }
    public byte Channel { get; set; }
    public string? Name { get; set; }
    public byte Value { get; set; }
}
