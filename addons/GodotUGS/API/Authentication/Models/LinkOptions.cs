namespace Unity.Services.Authentication;

// sourced from Unity

/// <summary>
/// Options for account linking operations
/// </summary>
public sealed class LinkOptions
{
    /// <summary>
    /// Option to force a link if one already exists.
    /// Using this option will remove the initial link if there is one.
    /// </summary>
    public bool ForceLink { get; set; }
}
