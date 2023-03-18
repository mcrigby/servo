namespace CutilloRigby.Output.Servo;

public sealed class ServoOutput
{
        public string? Name { get; set; }
        public bool Enabled { get; set; }
        public byte Value { get; set; }
        public byte DefaultValue { get; set; }
}
