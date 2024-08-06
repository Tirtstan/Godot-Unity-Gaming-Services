namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;

public class SignInWithSteamRequest : SignInWithExternalTokenRequest
{
    internal SignInWithSteamRequest() { }

    /// <summary>
    /// Option to add Steam configuration
    /// </summary>
    [JsonPropertyName("steamConfig")]
    public SteamConfig SteamConfig;
}
