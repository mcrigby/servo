using CutilloRigby.Output.Servo;

namespace Harness;

internal sealed class ServoSettingsConfiguration
{
    public IDictionary<string, ServoOutput>? Channels { get; set; }
}
