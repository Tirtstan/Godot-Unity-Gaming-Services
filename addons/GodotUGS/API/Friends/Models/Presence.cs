namespace Unity.Services.Friends.Models;

using System;
using System.Text.Json.Serialization;

/// <summary>
/// Presence model
/// </summary>
public class Presence
{
    /// <summary>
    /// The availability of the user
    /// </summary>
    /// see <see cref="Availability"/>
    public string Availability { get; set; }

    /// <summary>
    /// The last seen time of the user in UTC
    /// </summary>
    public DateTime? LastSeen { get; set; }

    public object Activity { get; set; }
}

/// <summary>
/// The current availability of the player.
/// </summary>
public class Availability
{
    [JsonPropertyName("UNKNOWN")]
    public static readonly string Unknown = "UNKNOWN";

    [JsonPropertyName("ONLINE")]
    public static readonly string Online = "ONLINE";

    [JsonPropertyName("BUSY")]
    public static readonly string Busy = "BUSY";

    [JsonPropertyName("AWAY")]
    public static readonly string Away = "AWAY";

    [JsonPropertyName("INVISIBLE")]
    public static readonly string Invisible = "INVISIBLE";

    [JsonPropertyName("OFFLINE")]
    public static readonly string Offline = "OFFLINE";
}

// public enum Availability
// {
//     /// <summary>
//     /// Enum Unknown for value: UNKNOWN
//     /// </summary>
//     [JsonPropertyName("UNKNOWN")]
//     Unknown = 0,

//     /// <summary>
//     /// Enum Online for value: ONLINE
//     /// </summary>
//     [JsonPropertyName("ONLINE")]
//     Online = 1,

//     /// <summary>
//     /// Enum Busy for value: BUSY
//     /// </summary>
//     [JsonPropertyName("BUSY")]
//     Busy = 2,

//     /// <summary>
//     /// Enum Away for value: AWAY
//     /// </summary>
//     [JsonPropertyName("AWAY")]
//     Away = 3,

//     /// <summary>
//     /// Enum Invisible for value: INVISIBLE
//     /// </summary>
//     [JsonPropertyName("INVISIBLE")]
//     Invisible = 4,

//     /// <summary>
//     /// Enum Offline for value: OFFLINE
//     /// </summary>
//     [JsonPropertyName("OFFLINE")]
//     Offline = 5
// }
