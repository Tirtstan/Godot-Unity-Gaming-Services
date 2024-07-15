namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;

public class UsernameInfo
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = "";

    [JsonPropertyName("createdAt")]
    public string CreatedAt { get; set; } = "";

    [JsonPropertyName("lastLoginAt")]
    public string LastLoginAt { get; set; } = "";

    [JsonPropertyName("passwordUpdatedAt")]
    public string PasswordUpdatedAt { get; set; } = "";
}
