namespace Unity.Services.Authentication;

using System.Text.Json.Serialization;

/// <summary>
/// Contains an external provider authentication information.
/// </summary>
public class LinkWithExternalTokenRequest
{
    /// <summary>
    /// Constructor
    /// </summary>
    internal LinkWithExternalTokenRequest() { }

    /// <summary>
    /// The external provider type id.
    /// </summary>
    [JsonPropertyName("idProvider")]
    public string IdProvider;

    /// <summary>
    /// The external provider authentication token.
    /// </summary>
    [JsonPropertyName("token")]
    public string Token;

    /// <summary>
    /// Option to force the link in case the account is already linked to another player.
    /// </summary>
    [JsonPropertyName("forceLink")]
    public bool ForceLink;
}
