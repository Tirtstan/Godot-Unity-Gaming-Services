namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;

public class UserSession
{
    [JsonPropertyName("expiresIn")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("idToken")]
    public string IdToken { get; set; } = "";

    [JsonPropertyName("sessionToken")]
    public string SessionToken { get; set; } = "";

    [JsonPropertyName("lastNotificationDate")]
    public double LastNotificationDate { get; set; }

    [JsonPropertyName("user")]
    public User User { get; set; } = new();

    [JsonPropertyName("userId")]
    public string UserId { get; set; } = "";
}
