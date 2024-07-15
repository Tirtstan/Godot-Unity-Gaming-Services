namespace Unity.Services.CloudSave.Models.Data.Player;

// sourced from Unity

public class DeleteOptions : WriteLockOptions, IAccessControlOptions
{
    public IAccessClassOptions AccessClassOptions { get; }

    public DeleteOptions()
    {
        AccessClassOptions = new DefaultWriteAccessClassOptions();
    }

    public DeleteOptions(WriteAccessClassOptions accessClassOptions)
    {
        AccessClassOptions = accessClassOptions ?? new DefaultWriteAccessClassOptions();
    }
}

public class DeleteAllOptions : IAccessControlOptions
{
    public IAccessClassOptions AccessClassOptions { get; }

    public DeleteAllOptions()
    {
        AccessClassOptions = new DefaultWriteAccessClassOptions();
    }

    public DeleteAllOptions(WriteAccessClassOptions accessClassOptions)
    {
        AccessClassOptions = accessClassOptions ?? new DefaultWriteAccessClassOptions();
    }
}

public class SaveOptions : IAccessControlOptions
{
    public IAccessClassOptions AccessClassOptions { get; }

    public SaveOptions()
    {
        AccessClassOptions = new DefaultWriteAccessClassOptions();
    }

    public SaveOptions(WriteAccessClassOptions accessClassOptions)
    {
        AccessClassOptions = accessClassOptions ?? new DefaultWriteAccessClassOptions();
    }
}

public class ListAllKeysOptions : IAccessControlOptions
{
    public IAccessClassOptions AccessClassOptions { get; }

    public ListAllKeysOptions()
    {
        AccessClassOptions = new DefaultReadAccessClassOptions();
    }

    public ListAllKeysOptions(ReadAccessClassOptions accessClassOptions)
    {
        AccessClassOptions = accessClassOptions ?? new DefaultReadAccessClassOptions();
    }
}

public class LoadOptions : IAccessControlOptions
{
    public IAccessClassOptions AccessClassOptions { get; }

    public LoadOptions()
    {
        AccessClassOptions = new DefaultReadAccessClassOptions();
    }

    public LoadOptions(ReadAccessClassOptions accessClassOptions)
    {
        AccessClassOptions = accessClassOptions ?? new DefaultReadAccessClassOptions();
    }
}

public class LoadAllOptions : IAccessControlOptions
{
    public IAccessClassOptions AccessClassOptions { get; }

    public LoadAllOptions()
    {
        AccessClassOptions = new DefaultReadAccessClassOptions();
    }

    public LoadAllOptions(ReadAccessClassOptions accessClassOptions)
    {
        AccessClassOptions = accessClassOptions ?? new DefaultReadAccessClassOptions();
    }
}

public class QueryOptions : IAccessControlOptions
{
    public IAccessClassOptions AccessClassOptions { get; }

    public QueryOptions()
    {
        AccessClassOptions = new PublicReadAccessClassOptions();
    }
}

public abstract class WriteAccessClassOptions : IAccessClassOptions
{
    public abstract AccessClass AccessClass { get; }
    public string PlayerId => null;
}

public class DefaultWriteAccessClassOptions : WriteAccessClassOptions
{
    public override AccessClass AccessClass => AccessClass.Default;
}

public class PublicWriteAccessClassOptions : WriteAccessClassOptions
{
    public override AccessClass AccessClass => AccessClass.Public;
}

public abstract class ReadAccessClassOptions : IAccessClassOptions
{
    public abstract AccessClass AccessClass { get; }
    public abstract string PlayerId { get; }
}

public class DefaultReadAccessClassOptions : ReadAccessClassOptions
{
    public override AccessClass AccessClass => AccessClass.Default;
    public override string PlayerId => null;
}

public class ProtectedReadAccessClassOptions : ReadAccessClassOptions
{
    public override AccessClass AccessClass => AccessClass.Protected;
    public override string PlayerId => null;
}

public class PublicReadAccessClassOptions : ReadAccessClassOptions
{
    public override AccessClass AccessClass => AccessClass.Public;
    public override string PlayerId { get; }

    public PublicReadAccessClassOptions()
    {
        PlayerId = null;
    }

    public PublicReadAccessClassOptions(string playerId)
    {
        PlayerId = playerId;
    }
}
