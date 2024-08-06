namespace Unity.Services.Authentication.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;
using Unity.Services.Authentication.Internal.Models;

public class User
{
    /// <summary>
    /// Constructor
    /// </summary>
    public User() { }

    /// <summary>
    /// Player Id
    /// </summary>

    [JsonPropertyName("id")]
    public string Id;

    /// <summary>
    /// Player Creation date
    /// </summary>
    [JsonPropertyName("createdAt")]
    public string CreatedAt;

    /// <summary>
    /// Player External Ids
    /// </summary>
    [JsonPropertyName("externalIds")]
    public List<ExternalIdentity> ExternalIds;

    /// <summary>
    /// Username and Password information
    /// </summary>
    [JsonPropertyName("username")]
    public string Username;

    /// <summary>
    /// Username Information
    /// </summary>
    [JsonPropertyName("UsernameInfo")]
    public UsernameInfo UsernameInfo;
}
