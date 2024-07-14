namespace Unity.Services.Authentication.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;

/// <summary>
/// A list of notifications used for serialization.
/// </summary>
public class NotificationList
{
    [JsonPropertyName("notifications")]
    public List<Notification> Notifications { get; set; }
}
