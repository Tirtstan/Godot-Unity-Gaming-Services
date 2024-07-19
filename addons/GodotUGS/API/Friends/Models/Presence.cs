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
    public Availability Availability { get; set; }

    /// <summary>
    /// The last seen time of the user in UTC
    /// </summary>
    public DateTime LastSeen { get; set; }

    public object Activity { get; set; }
}

/// <summary>
/// The current availability of the player.
/// </summary>
public enum Availability
{
    /// <summary>
    /// Enum Unknown for value: UNKNOWN
    /// </summary>
    [JsonPropertyName("UNKNOWN")]
    Unknown = 0,

    /// <summary>
    /// Enum Online for value: ONLINE
    /// </summary>
    [JsonPropertyName("ONLINE")]
    Online = 1,

    /// <summary>
    /// Enum Busy for value: BUSY
    /// </summary>
    [JsonPropertyName("BUSY")]
    Busy = 2,

    /// <summary>
    /// Enum Away for value: AWAY
    /// </summary>
    [JsonPropertyName("AWAY")]
    Away = 3,

    /// <summary>
    /// Enum Invisible for value: INVISIBLE
    /// </summary>
    [JsonPropertyName("INVISIBLE")]
    Invisible = 4,

    /// <summary>
    /// Enum Offline for value: OFFLINE
    /// </summary>
    [JsonPropertyName("OFFLINE")]
    Offline = 5
}
