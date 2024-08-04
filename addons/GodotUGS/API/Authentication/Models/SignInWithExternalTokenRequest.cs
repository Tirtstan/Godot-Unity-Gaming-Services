namespace Unity.Services.Authentication;

using System.Text.Json.Serialization;

/// <summary>
/// Contains an external provider authentication information.
/// </summary>
public class SignInWithExternalTokenRequest
{
    /// <summary>
    /// Constructor
    /// </summary>
    internal SignInWithExternalTokenRequest() { }

    /// <summary>
    /// The external provider type id.
    /// To be removed when we drop legacy support.
    /// </summary>
    [JsonPropertyName("idProvider")]
    public string IdProvider;

    /// <summary>
    /// The external provider authentication token.
    /// </summary>
    [JsonPropertyName("token")]
    public string Token;

    /// <summary>
    /// Option to sign in only if an account exists.
    /// </summary>
    [JsonPropertyName("signInOnly")]
    public bool SignInOnly;
}
