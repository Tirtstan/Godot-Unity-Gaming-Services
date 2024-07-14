namespace Unity.Services;

public class PaginationOptions
{
    public int? Offset { get; set; }
    public int? Limit { get; set; }
    public bool? IncludeMetadata { get; set; }
}

public class IncludeMetadataOptions
{
    public bool? IncludeMetadata { get; set; }
}
