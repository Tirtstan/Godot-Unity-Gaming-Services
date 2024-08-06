namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;

public class LinkWithSteamRequest : LinkWithExternalTokenRequest
{
    internal LinkWithSteamRequest() { }

    /// <summary>
    /// Option to add steam configuration
    /// </summary>
    [JsonPropertyName("steamConfig")]
    public SteamConfig SteamConfig;
}
