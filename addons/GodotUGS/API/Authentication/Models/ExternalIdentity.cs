namespace Unity.Services.Authentication.Internal.Models;

// sourced from Unity

using System.Text.Json.Serialization;

/// <summary>
/// Contains elements for ExternalId
/// </summary>
public class ExternalIdentity
{
    /// <summary>
    /// Constructor
    /// </summary>
    public ExternalIdentity() { }

    /// <summary>
    /// The external provider type id.
    /// </summary>
    [JsonPropertyName("providerId")]
    public string ProviderId;

    /// <summary>
    /// The external id
    /// </summary>
    [JsonPropertyName("externalId")]
    public string ExternalId;
}
