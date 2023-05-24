namespace CutilloRigby.Output.Servo;

public sealed class EmptyServoMap : IServoMap
{
    private EmptyServoMap() { }

    public float this[byte index] => 0;

    public string Name => "Empty Servo Map";

    private readonly float[] values = new float[0];
    public float[] Values => values;

    private static IServoMap? _instance = null;

    public static IServoMap Instance
    {
        get{
            if (_instance == null)
                _instance = new EmptyServoMap();
            return _instance;
        }
    }
}
