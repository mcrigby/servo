namespace CutilloRigby.Output.Servo;

public sealed class ServoConfigurationFactory : IServoConfigurationFactory
{
    private readonly IDictionary<string, IServoConfiguration> _source;

    public ServoConfigurationFactory(IDictionary<string, IServoConfiguration> source)
    {
        _source = source ?? throw new ArgumentNullException(nameof(source));
    }

    public void AddServoConfiguration(string name, IServoConfiguration configuration)
    {
        if (!_source.ContainsKey(name))
            _source.Add(name, configuration);
        else
            _source[name] = configuration;
    }

    public void AddServoConfiguration<T>(IServoConfiguration configuration)
    {
        AddServoConfiguration(typeof(T).FactoryName(), configuration);
    }

    public IServoConfiguration GetServoConfiguration(string name)
    {
        if (!_source.ContainsKey(name))
            return ServoConfiguration.None;
        else
            return _source[name];
    }

    public IServoConfiguration GetServoConfiguration<T>()
    {
        return GetServoConfiguration(typeof(T).FactoryName());
    }
}
