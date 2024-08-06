namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;

public class SignInWithAppleGameCenterRequest : SignInWithExternalTokenRequest
{
    internal SignInWithAppleGameCenterRequest() { }

    /// <summary>
    /// Parameters to add an AppleGameCenter config
    /// </summary>
    [JsonPropertyName("appleGameCenterConfig")]
    public AppleGameCenterConfig AppleGameCenterConfig;
}
