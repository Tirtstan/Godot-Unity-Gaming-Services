namespace Unity.Services.Friends.Options;

/// <summary>
/// Defines the options desired when initializing the Friends service.
/// </summary>
public class InitializeOptions
{
    public MemberOptions MemberOptions { get; } = new MemberOptions();

    public bool UseEvents { get; set; } = true;

    /// <summary>
    /// Defines whether or not to attach presence data to the member object
    /// </summary>
    /// <param name="withPresence">Whether or not to attach presence data</param>
    /// <returns>The <see cref="InitializeOptions"/> with the updated presence flag</returns>
    public InitializeOptions WithMemberPresence(bool withPresence)
    {
        MemberOptions.IncludePresence = withPresence;
        return this;
    }

    /// <summary>
    /// Defines whether or not to attach profile data to the member object
    /// </summary>
    /// <param name="withProfile">Whether or not to attach profile data</param>
    /// <returns>The <see cref="InitializeOptions"/> with the updated profile flag</returns>
    public InitializeOptions WithMemberProfile(bool withProfile)
    {
        MemberOptions.IncludeProfile = withProfile;
        return this;
    }

    /// <summary>
    /// Defines whether or not to attach profile data to the member object
    /// </summary>
    /// <param name="withEvents">whether or not to use events to update lists</param>
    /// <returns>The <see cref="InitializeOptions"/> with the updated events flag</returns>
    public InitializeOptions WithEvents(bool withEvents)
    {
        UseEvents = withEvents;
        return this;
    }
}
