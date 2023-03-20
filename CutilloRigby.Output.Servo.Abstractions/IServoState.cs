namespace CutilloRigby.Output.Servo;

public interface IServoState
{
    byte Chip { get; set; }
    
    string? Name { get; set; }

    byte[] AvailableChannels { get; }

    bool HasChannel(byte address);

    void SetChannel(byte address, byte value);

    byte GetChannel(byte address);

    string GetChannelName(byte address);
    
    void ResetChannels();
}