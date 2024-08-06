namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Configuration for an Oculus Id provider.
/// </summary>
public class OculusConfig
{
    /// <summary>
    /// Constructor
    /// </summary>

    internal OculusConfig() { }

    /// <summary>
    /// Oculus account userId
    /// </summary>
    [JsonPropertyName("userId")]
    public string UserId;
}
