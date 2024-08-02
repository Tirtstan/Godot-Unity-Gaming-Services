namespace Unity.Services.Ugc.Models;

using System.Text.Json.Serialization;

/// <summary>
/// ContentUploadStatus enum.
/// </summary>
/// <value></value>
public class ContentUploadStatus
{
    [JsonPropertyName("pending")]
    public static readonly string Pending = "pending";

    [JsonPropertyName("success")]
    public static readonly string Success = "success";

    [JsonPropertyName("failed")]
    public static readonly string Failed = "failed";

    [JsonPropertyName("none")]
    public static readonly string None = "none";
}


// public enum ContentUploadStatus
// {
//     /// <summary>
//     /// Enum Pending for value: pending
//     /// </summary>
//     [EnumMember(Value = "pending")]
//     Pending = 1,

//     /// <summary>
//     /// Enum Success for value: success
//     /// </summary>
//     [EnumMember(Value = "success")]
//     Success = 2,

//     /// <summary>
//     /// Enum Failed for value: failed
//     /// </summary>
//     [EnumMember(Value = "failed")]
//     Failed = 3,

//     /// <summary>
//     /// Enum None for value: none
//     /// </summary>
//     [EnumMember(Value = "none")]
//     None = 4
// }
