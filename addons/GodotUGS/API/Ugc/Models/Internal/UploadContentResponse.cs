namespace Unity.Services.Ugc.Internal.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class UploadContentResponse
{
    public UploadContentResponse(
        string uploadThumbnailUrl = default,
        string uploadContentUrl = default,
        Dictionary<string, List<string>> uploadContentHeaders = default,
        Dictionary<string, List<string>> uploadThumbnailHeaders = default,
        string version = default,
        InternalContent content = default
    )
    {
        UploadThumbnailUrl = uploadThumbnailUrl;
        UploadContentUrl = uploadContentUrl;
        UploadContentHeaders = uploadContentHeaders;
        UploadThumbnailHeaders = uploadThumbnailHeaders;
        Version = version;
        Content = content;
    }

    [JsonPropertyName("uploadThumbnailUrl")]
    public string UploadThumbnailUrl { get; }

    [JsonPropertyName("uploadContentUrl")]
    public string UploadContentUrl { get; }

    [JsonPropertyName("uploadContentHeaders")]
    public Dictionary<string, List<string>> UploadContentHeaders { get; }

    [JsonPropertyName("uploadThumbnailHeaders")]
    public Dictionary<string, List<string>> UploadThumbnailHeaders { get; }

    [JsonPropertyName("version")]
    public string Version { get; }

    [JsonPropertyName("content")]
    public InternalContent Content { get; }

    internal Dictionary<string, string> GetContentHeadersAsQueryParam()
    {
        var dictionary = new Dictionary<string, string>();

        if (UploadContentHeaders != null)
        {
            var uploadContentHeadersStringValue = UploadContentHeaders.ToString();
            dictionary.Add("uploadContentHeaders", uploadContentHeadersStringValue);
        }

        return dictionary;
    }

    internal Dictionary<string, string> GetThumbnailHeadersAsQueryParam()
    {
        var dictionary = new Dictionary<string, string>();

        if (UploadThumbnailHeaders != null)
        {
            var uploadThumbnailHeadersStringValue = UploadThumbnailHeaders.ToString();
            dictionary.Add("uploadThumbnailHeaders", uploadThumbnailHeadersStringValue);
        }

        return dictionary;
    }
}
