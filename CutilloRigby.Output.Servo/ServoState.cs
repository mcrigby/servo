using Microsoft.Extensions.Logging;

namespace CutilloRigby.Output.Servo;

public sealed class ServoState : IServoState, IServoOutputChanged
{
    public ServoState(ILogger<ServoState> logger)
    {
        SetLogHandlers(logger ?? throw new ArgumentNullException(nameof(logger)));

        Channels = new Dictionary<byte, ServoOutput>();
    }

    public byte Chip { get; set; }
    
    public string? Name { get; set; }

    public IDictionary<byte, ServoOutput> Channels { get; set; }

    public byte[] AvailableChannels => Channels.Keys.ToArray();

    public event EventHandler<ServoOutputEventArgs> Changed = delegate { };

    public const string Default_Name = "Unknown";

    public bool HasChannel(byte address)
    {
        lock (Channels)
        {
            return Channels.ContainsKey(address);
        }
    }

    public void SetChannel(byte address, byte value)
    {
        lock (Channels)
        {
            if (!Channels.ContainsKey(address) || !Channels[address].Enabled)
                return;
        }

        if (GetChannel(address) != value)
        {
            ServoOutputEventArgs eventArgs;

            lock (Channels)
            {
                var axis = Channels[address];
                    
                setInformation_ValueChanged(axis.Name, axis.Value, value);

                axis.Value = value;
                Channels[address] = axis;

                eventArgs = Channels[address].ToEventArgs(address);
            }

            Changed?.Invoke(this, eventArgs);
        }
    }

    public byte GetChannel(byte address)
    {
        if (!Channels.ContainsKey(address))
            return 0;

        lock (Channels)
        {
            return Channels[address].Value;
        }
    }

    public string GetChannelName(byte address)
    {
        if (!Channels.ContainsKey(address))
            return Default_Name;

        lock (Channels)
        {
            return Channels[address]?.Name ?? Default_Name;
        }
    }

    public void ResetChannels()
    {
        lock (Channels)
        {
            foreach (var address in Channels.Keys)
                Channels[address].Value = Channels[address].DefaultValue;
        }
    }

    private void SetLogHandlers(ILogger logger)
    {
        if (logger.IsEnabled(LogLevel.Information))
        {
            setInformation_ValueChanged = (name, oldValue, newValue) => 
                logger.LogInformation("Channel {name} value changed from {oldValue} to {newValue}.", 
                        name, oldValue, newValue);;
        }
    }

    private Action<string?, object?, object?> setInformation_ValueChanged = (name, oldValue, newValue) => { };
}
