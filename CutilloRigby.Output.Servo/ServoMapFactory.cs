namespace CutilloRigby.Output.Servo;

public sealed class ServoMapFactory : IServoMapFactory, IRemappableServoMapFactory
{
    private readonly IDictionary<string, IServoMap> _source;

    public ServoMapFactory(IDictionary<string, IServoMap> source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
    }

    public void AddRemappableServoMap(string name, IRemappableServoMap map)
    {
        if (!_source.ContainsKey(name))
            _source.Add(name, map);
        else
            _source[name] = map;
    }

    public void AddServoMap(string name, IServoMap map)
    {
        if (!_source.ContainsKey(name))
            _source.Add(name, map);
        else
            _source[name] = map;
    }

    public IRemappableServoMap GetRemappableServoMap(string name)
    {
        if (_source.ContainsKey(name) 
            && (_source[name] is IRemappableServoMap remappable))
            return remappable;

        return RemappableServoMap.Default;
    }

    public IServoMap GetServoMap(string name)
    {
        if (!_source.ContainsKey(name))
            return ServoMap.LinearServoMap();
        else
            return _source[name];
    }
}
