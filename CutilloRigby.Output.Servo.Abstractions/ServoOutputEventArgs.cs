namespace CutilloRigby.Output.Servo;

public sealed class ServoOutputEventArgs
{
    public byte Address { get; set; }
    public string? Name { get; set; }
    public byte Value { get; set; }
}
