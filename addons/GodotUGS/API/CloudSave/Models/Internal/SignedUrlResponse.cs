namespace Unity.Services.CloudSave.Internal.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;

/// <summary>
/// An object containing a signed URL for a resource and the necessary information needed to access it.
/// </summary>
public class SignedUrlResponse
{
    /// <summary>
    /// An object containing a signed URL for a resource and the necessary information needed to access it.
    /// </summary>
    /// <param name="signedUrl">The signed URL used to access the resource.</param>
    /// <param name="httpMethod">The HTTP method that must be used on the signedUrl.</param>
    /// <param name="requiredHeaders">The set of HTTP headers that must be sent with the  request for it to succeed.</param>
    public SignedUrlResponse(
        string signedUrl = default,
        string httpMethod = default,
        Dictionary<string, string> requiredHeaders = default
    )
    {
        SignedUrl = signedUrl;
        HttpMethod = httpMethod;
        RequiredHeaders = requiredHeaders;
    }

    /// <summary>
    /// The signed URL used to access the resource.
    /// </summary>
    [JsonPropertyName("signedUrl")]
    public string SignedUrl { get; }

    /// <summary>
    /// The HTTP method that must be used on the signedUrl.
    /// </summary>
    [JsonPropertyName("httpMethod")]
    public string HttpMethod { get; }

    /// <summary>
    /// The set of HTTP headers that must be sent with the  request for it to succeed.
    /// </summary>
    [JsonPropertyName("requiredHeaders")]
    public Dictionary<string, string> RequiredHeaders { get; }
}
