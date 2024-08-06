namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;

public class LinkWithOculusRequest : LinkWithExternalTokenRequest
{
    internal LinkWithOculusRequest() { }

    /// <summary>
    /// Option to add oculus config
    /// </summary>
    [JsonPropertyName("oculusConfig")]
    public OculusConfig OculusConfig;
}
