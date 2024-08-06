namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Configuration for an AppleGameCenter Id provider.
/// </summary>
public class AppleGameCenterConfig
{
    internal AppleGameCenterConfig() { }

    /// <summary>
    /// AppleGameCenter teamPlayerId
    /// </summary>
    [JsonPropertyName("teamPlayerId")]
    public string TeamPlayerId;

    /// <summary>
    /// AppleGameCenter publicKeyURL
    /// </summary>
    [JsonPropertyName("publicKeyUrl")]
    public string PublicKeyURL;

    /// <summary>
    /// AppleGameCenter salt
    /// </summary>
    [JsonPropertyName("salt")]
    public string Salt;

    /// <summary>
    /// AppleGameCenter timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public ulong Timestamp;
}
