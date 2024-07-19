namespace Unity.Services.Friends.Options;

/// <summary>
/// Defines options to select the desired data when retrieving member information.
/// </summary>
public class MemberOptions
{
    public bool IncludePresence { get; set; } = true;
    public bool IncludeProfile { get; set; } = true;
}
