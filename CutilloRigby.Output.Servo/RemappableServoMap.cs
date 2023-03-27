namespace CutilloRigby.Output.Servo;

public sealed class RemappableServoMap : IRemappableServoMap
{
    private readonly IDictionary<byte, IServoMap> _maps;
    private IServoMap _activeMap;

    public RemappableServoMap(IDictionary<byte, IServoMap>? maps = null)
    {
        _maps = maps ?? new Dictionary<byte, IServoMap>();
        
        _activeMap = _maps.Values.FirstOrDefault();
    }

    public float this[byte index]
    {
        get
        {
            if (0 <= index && index < _activeMap.Values.Length)
                return _activeMap[index];
            return 0;
        }
    }

    public string Name => _activeMap.Name;

    public float[] Values => _activeMap.Values;

    public bool AddMap(byte index, IServoMap map)
    {
        if (_maps.ContainsKey(index))
            return false;

        _maps.Add(index, map);
        return true;
    }

    public bool Remap(byte index)
    {
        if (!_maps.ContainsKey(index))
            return false;

        _activeMap = _maps[index];
        return true;
    }

    public static readonly RemappableServoMap Default = new RemappableServoMap(new Dictionary<byte, IServoMap>{
        {0, ServoMap.LinearServoMap() }
    });
}
