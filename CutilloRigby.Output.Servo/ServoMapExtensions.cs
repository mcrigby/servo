namespace CutilloRigby.Output.Servo;

public static class ServoMapExtensions
{
    public static ServoMap Reverse(this ServoMap source)
    {
        var values = (float[])source;
        return new ServoMap(values.Reverse().ToArray());
    }
}