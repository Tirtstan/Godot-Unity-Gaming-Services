namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;

public class SignInWithOculusRequest : SignInWithExternalTokenRequest
{
    internal SignInWithOculusRequest() { }

    /// <summary>
    /// Option to add oculus config
    /// </summary>
    [JsonPropertyName("oculusConfig")]
    public OculusConfig OculusConfig;
}
