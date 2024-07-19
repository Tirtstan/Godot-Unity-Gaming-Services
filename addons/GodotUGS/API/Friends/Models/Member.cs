namespace Unity.Services.Friends.Models;

using System.Text.Json.Serialization;

/// <summary>
/// The representation of a user in a relationship
/// </summary>
public class Member
{
    /// <summary>
    /// The ID of the member
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The member's role
    /// </summary>
    public MemberRole Role { get; set; }

    /// <summary>
    /// The presence data of the member
    /// </summary>
    public Presence Presence { get; set; }

    /// <summary>
    /// The profile data of the member
    /// </summary>
    public Profile Profile { get; set; }
}

/// <summary>
/// The role of a member in a relationship
/// </summary>
public enum MemberRole
{
    /// <summary>
    /// Enum Target for value: TARGET
    /// </summary>
    [JsonPropertyName("TARGET")]
    Target = 1,

    /// <summary>
    /// Enum Source for value: SOURCE
    /// </summary>
    [JsonPropertyName("SOURCE")]
    Source = 2,

    /// <summary>
    /// Enum None for value: NONE
    /// </summary>
    [JsonPropertyName("NONE")]
    None = 3,
}
