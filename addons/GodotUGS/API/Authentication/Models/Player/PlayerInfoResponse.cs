namespace Unity.Services.Authentication.Internal.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class PlayerInfoResponse
{
    /// <summary>
    /// Constructor
    /// </summary>
    public PlayerInfoResponse() { }

    [JsonPropertyName("disabled")]
    public bool Disabled;

    [JsonPropertyName("externalIds")]
    public List<ExternalIdentity> ExternalIds;

    [JsonPropertyName("id")]
    public string Id;

    [JsonPropertyName("createdAt")]
    public string CreatedAt;

    [JsonPropertyName("lastLoginAt")]
    public string LastLoginAt;

    [JsonPropertyName("usernamepassword")]
    public UsernameInfo UsernamePassword;
}
