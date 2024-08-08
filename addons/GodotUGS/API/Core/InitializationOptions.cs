namespace Unity.Services.Core;

public class InitializationOptions
{
    public InitializationOptions() { }

    public InitializationOptions(string environment)
    {
        Environment = environment;
    }

    public string Environment { get; private set; } = "production";

    public void SetEnvironmentName(string environment) => Environment = environment;
}
