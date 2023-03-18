using CutilloRigby.Output.Servo;

namespace Harness;

internal sealed class ServoSettingsConfiguration
{
    IDictionary<string, ServoOutput>? Channels { get; set; }
}
