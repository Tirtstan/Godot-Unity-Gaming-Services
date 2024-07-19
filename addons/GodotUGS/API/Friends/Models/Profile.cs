namespace Unity.Services.Friends.Models;

/// <summary>
/// The representation of a user's profile information
/// </summary>
public class Profile
{
    /// <summary>
    /// The name of the user.
    /// The ability to set/get each individual user's name is detailed <seealso href="https://docs.unity.com/authentication/en/manual/player-name-management">here</seealso>
    /// </summary>
    public string Name { get; set; }
}
