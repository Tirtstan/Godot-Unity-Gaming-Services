namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;

public class SignInResponse
{
    public SignInResponse() { }

    [JsonPropertyName("expiresIn")]
    public int ExpiresIn;

    [JsonPropertyName("idToken")]
    public string IdToken;

    [JsonPropertyName("sessionToken")]
    public string SessionToken;

    [JsonPropertyName("lastNotificationDate")]
    public double LastNotificationDate;

    [JsonPropertyName("user")]
    public User User;

    [JsonPropertyName("userId")]
    public string UserId;
}
