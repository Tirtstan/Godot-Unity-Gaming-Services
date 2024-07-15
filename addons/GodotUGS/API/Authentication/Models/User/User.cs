namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;

public class User
{
    [JsonPropertyName("disabled")]
    public bool Disabled { get; set; }

    [JsonPropertyName("externalIds")]
    public Identity[] ExternalIds { get; set; } = { };

    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("username")]
    public string Username { get; set; } = "";
}
