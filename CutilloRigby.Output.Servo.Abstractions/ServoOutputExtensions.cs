namespace CutilloRigby.Output.Servo;

public static class NewBaseType
{
    public static ServoOutputEventArgs ToEventArgs(this ServoOutput output, byte address)
    {
        return new ServoOutputEventArgs
        {
            Address = address,
            Name = output.Name,
            Value = output.Value
        };
    }
}
