namespace Unity.Services.Ugc.Models;

using System.Text.Json.Serialization;

/// <summary>
/// ModerationStatus static class.
/// </summary>
/// <value></value>
public class ModerationStatus
{
    [JsonPropertyName("approved")]
    public static readonly string Approved = "approved";

    [JsonPropertyName("rejected")]
    public static readonly string Rejected = "rejected";

    [JsonPropertyName("needsModeration")]
    public static readonly string NeedsModeration = "needsModeration";
}


// public enum ModerationStatus
// {
//     /// <summary>
//     /// Enum Approved for value: approved
//     /// </summary>
//     [EnumMember(Value = "approved")]
//     Approved = 1,

//     /// <summary>
//     /// Enum Rejected for value: rejected
//     /// </summary>
//     [EnumMember(Value = "rejected")]
//     Rejected = 2,

//     /// <summary>
//     /// Enum NeedsModeration for value: needsModeration
//     /// </summary>
//     [EnumMember(Value = "needsModeration")]
//     NeedsModeration = 3
// }
