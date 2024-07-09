namespace Unity.Services.Core;

public class InitializationOptions
{
    public string Environment { get; private set; } = "production";

    public void SetEnvironmentName(string environment)
    {
        Environment = environment;
    }
}
