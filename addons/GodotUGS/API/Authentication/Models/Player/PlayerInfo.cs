namespace Unity.Services.Authentication;

using System.Text.Json.Serialization;
using Unity.Services.Authentication.Models;

public class PlayerInfo
{
    public string Username => UsernameInfo.Username;

    [JsonPropertyName("disabled")]
    public bool Disabled { get; set; }

    [JsonPropertyName("externalIds")]
    public Identity[] ExternalIds { get; set; } = { };

    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("createdAt")]
    public string CreatedAt { get; set; } = "";

    [JsonPropertyName("lastLoginAt")]
    public string LastLoginAt { get; set; } = "";

    [JsonPropertyName("usernamepassword")]
    public UsernameInfo UsernameInfo { get; set; } = new();
}
