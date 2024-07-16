namespace Unity.Services.CloudSave.Models;

// sourced from Unity

public abstract class WriteLockOptions
{
    public string WriteLock { get; set; }
}

public interface IAccessControlOptions
{
    public IAccessClassOptions AccessClassOptions { get; }
}

public interface IAccessClassOptions
{
    public AccessClass AccessClass { get; }
    public string PlayerId { get; }
}

public enum AccessClass
{
    Default,
    Private,
    Protected,
    Public
}
