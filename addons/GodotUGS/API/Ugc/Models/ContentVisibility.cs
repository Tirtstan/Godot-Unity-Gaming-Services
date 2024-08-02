namespace Unity.Services.Ugc.Models;

using System.Text.Json.Serialization;

/// <summary>
/// ContentVisibility static class.
/// </summary>
/// <value>Controls the visibility of content across projects<p>Possible values:</p> <ul> <li><b>Private</b>: Content is only visible to current user in current game</li> <li><b>Hidden</b>: Content is hidden because Reports exceeds project threshold</li> <li><b>Public</b>: Content is visible to any users</li> <li><b>Unlisted</b>: Content is not searchable but still accessible directly</li>  </ul> </value>
public class ContentVisibility
{
    [JsonPropertyName("private")]
    public static readonly string Private = "private";

    [JsonPropertyName("hidden")]
    public static readonly string Hidden = "hidden";

    [JsonPropertyName("public")]
    public static readonly string Public = "public";

    [JsonPropertyName("unlisted")]
    public static readonly string Unlisted = "unlisted";
}


// public enum ContentVisibility
// {
//     /// <summary>
//     /// Enum Private for value: private
//     /// </summary>
//     [EnumMember(Value = "private")]
//     Private = 1,

//     /// <summary>
//     /// Enum Hidden for value: hidden
//     /// </summary>
//     [EnumMember(Value = "hidden")]
//     Hidden = 4,

//     /// <summary>
//     /// Enum Public for value: public
//     /// </summary>
//     [EnumMember(Value = "public")]
//     Public = 5,

//     /// <summary>
//     /// Enum Unlisted for value: unlisted
//     /// </summary>
//     [EnumMember(Value = "unlisted")]
//     Unlisted = 6
// }
