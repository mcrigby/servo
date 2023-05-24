namespace CutilloRigby.Output.Servo;

public sealed class ServoMapFactory : IServoMapFactory
{
    private readonly IDictionary<string, IServoMap> _source;

    public ServoMapFactory(IDictionary<string, IServoMap> source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
    }

    public void AddServoMap(string name, IServoMap map)
    {
        if (!_source.ContainsKey(name))
            _source.Add(name, map);
        else
            _source[name] = map;
    }

    public void AddServoMap<T>(IServoMap map)
    {
        AddServoMap(typeof(T).FactoryName(), map);
    }

    public IServoMap GetServoMap(string name)
    {
        if (!_source.ContainsKey(name))
            return EmptyServoMap.Instance;
        else
            return _source[name];
    }

    public IServoMap GetServoMap<T>()
    {
        return GetServoMap(typeof(T).FactoryName());
    }
}
