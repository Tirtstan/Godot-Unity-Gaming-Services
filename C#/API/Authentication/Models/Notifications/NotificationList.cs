using System.Collections.Generic;

namespace Unity.Services.Authentication.Models;

/// <summary>
/// A list of notifications used for serialization.
/// </summary>
public class NotificationList
{
    public List<Notification> notifications { get; set; }
}
