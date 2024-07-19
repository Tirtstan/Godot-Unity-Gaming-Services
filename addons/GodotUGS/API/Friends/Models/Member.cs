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
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// The member's role
    /// </summary>
    /// <see cref="MemberRole"/>
    [JsonPropertyName("role")]
    public string Role { get; set; }

    /// <summary>
    /// The presence data of the member
    /// </summary>
    [JsonPropertyName("presence")]
    public Presence Presence { get; set; }

    /// <summary>
    /// The profile data of the member
    /// </summary>
    [JsonPropertyName("profile")]
    public Profile Profile { get; set; }
}

public class MemberRole
{
    [JsonPropertyName("TARGET")]
    public static readonly string Target = "TARGET";

    [JsonPropertyName("SOURCE")]
    public static readonly string Source = "SOURCE";

    [JsonPropertyName("NONE")]
    public static readonly string None = "NONE";
}

/// <summary>
/// The role of a member in a relationship
/// </summary>
// public enum MemberRole
// {
//     /// <summary>
//     /// Enum Target for value: TARGET
//     /// </summary>
//     [JsonPropertyName("TARGET")]
//     Target = 1,

//     /// <summary>
//     /// Enum Source for value: SOURCE
//     /// </summary>
//     [JsonPropertyName("SOURCE")]
//     Source = 2,

//     /// <summary>
//     /// Enum None for value: NONE
//     /// </summary>
//     [JsonPropertyName("NONE")]
//     None = 3,
// }
