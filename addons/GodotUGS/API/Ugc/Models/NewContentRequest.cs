namespace Unity.Services.Ugc.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;

/// <summary>
/// Create a new content item
/// </summary>
internal class NewContentRequest
{
    /// <summary>
    /// Create a new content item
    /// </summary>
    /// <param name="name">Display name of the content</param>
    /// <param name="description">Description of the content</param>
    /// <param name="customId">customId param</param>
    /// <param name="visibility">visibility param, see <see cref="ContentVisibility"/></param>
    /// <param name="tagIds">Tag Ids of the content</param>
    /// <param name="contentMd5Hash">A base64 encoded representation of the Content binary  that will be uploaded. Used as a checksum.</param>
    /// <param name="thumbnailMd5Hash">A base64 encoded representation of the Thumbnail file  that will be uploaded. Used as a checksum.</param>
    /// <param name="metadata">metadata param</param>
    public NewContentRequest(
        string name,
        string description,
        string customId = default,
        string visibility = default,
        List<string> tagIds = default,
        string contentMd5Hash = default,
        string thumbnailMd5Hash = default,
        string metadata = default
    )
    {
        Name = name;
        Description = description;
        CustomId = customId;
        Visibility = visibility;
        TagIds = tagIds;
        ContentMd5Hash = contentMd5Hash;
        ThumbnailMd5Hash = thumbnailMd5Hash;
        Metadata = metadata;
    }

    /// <summary>
    /// Display name of the content
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    /// Description of the content
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; }

    /// <summary>
    /// Parameter customId of NewContentRequest
    /// </summary>
    [JsonPropertyName("customId")]
    public string CustomId { get; }

    /// <summary>
    /// Parameter visibility of NewContentRequest, see <see cref="ContentVisibility"/>
    /// </summary>
    [JsonPropertyName("visibility")]
    public string Visibility { get; }

    /// <summary>
    /// Tag Ids of the content
    /// </summary>
    [JsonPropertyName("tagIds")]
    public List<string> TagIds { get; }

    /// <summary>
    /// A base64 encoded representation of the Content binary  that will be uploaded. Used as a checksum.
    /// </summary>
    [JsonPropertyName("contentMd5Hash")]
    public string ContentMd5Hash { get; }

    /// <summary>
    /// A base64 encoded representation of the Thumbnail file  that will be uploaded. Used as a checksum.
    /// </summary>
    [JsonPropertyName("thumbnailMd5Hash")]
    public string ThumbnailMd5Hash { get; }

    /// <summary>
    /// Parameter metadata of NewContentRequest
    /// </summary>
    [JsonPropertyName("metadata")]
    public string Metadata { get; }
}
