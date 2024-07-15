namespace Unity.Services.Leaderboards;

public class GetVersionsOptions
{
    public int? Limit { get; set; }
}

public class AddPlayerScoreOptions
{
    public object Metadata { get; set; }
    public string VersionId { get; set; }
}

public class RangeOptions
{
    public int? RangeLimit { get; set; }
    public bool? IncludeMetadata { get; set; }
}
