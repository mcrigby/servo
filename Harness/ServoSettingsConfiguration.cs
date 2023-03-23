using CutilloRigby.Output.Servo;

namespace Harness;

internal sealed class ServoSettingsConfiguration
{
    public byte Chip { get; set; }
    public string? Name { get; set; }
    public IDictionary<string, ServoConfiguration>? Channels { get; set; }
}
