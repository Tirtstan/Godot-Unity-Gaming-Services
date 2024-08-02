namespace Unity.Services.Ugc.Internal.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;

/// <summary>
/// Update the meta data about a content item
/// </summary>
public class UpdateContentRequest
{
    /// <summary>
    /// Update the meta data about a content item
    /// </summary>
    /// <param name="name">New name or null to ignore</param>
    /// <param name="description">New description or null to ignore</param>
    /// <param name="customId">customId param</param>
    /// <param name="visibility">visibility param, see <see cref="Ugc.Models.ContentVisibility"/></param>
    /// <param name="contentTypeId">New Content Type id or zero to ignore</param>
    /// <param name="tagsId">tagsId param</param>
    /// <param name="version">Version id of content</param>
    /// <param name="metadata">metadata param</param>
    public UpdateContentRequest(
        string name,
        string description,
        string customId = default,
        string visibility = default,
        int contentTypeId = default,
        List<string> tagsId = default,
        string version = default,
        string metadata = default
    )
    {
        Name = name;
        Description = description;
        CustomId = customId;
        Visibility = visibility;
        ContentTypeId = contentTypeId;
        TagsId = tagsId;
        Version = version;
        Metadata = metadata;
    }

    /// <summary>
    /// New name or null to ignore
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    /// New description or null to ignore
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; }

    /// <summary>
    /// Parameter customId of UpdateContentRequest
    /// </summary>
    [JsonPropertyName("customId")]
    public string CustomId { get; }

    /// <summary>
    /// Parameter visibility of UpdateContentRequest, see <see cref="Ugc.Models.ContentVisibility"/>
    /// </summary>
    [JsonPropertyName("visibility")]
    public string Visibility { get; }

    /// <summary>
    /// New Content Type id or zero to ignore
    /// </summary>
    [JsonPropertyName("contentTypeId")]
    public int ContentTypeId { get; }

    /// <summary>
    /// Parameter tagsId of UpdateContentRequest
    /// </summary>
    [JsonPropertyName("tagsId")]
    public List<string> TagsId { get; }

    /// <summary>
    /// Version id of content
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; }

    /// <summary>
    /// Parameter metadata of UpdateContentRequest
    /// </summary>
    [JsonPropertyName("metadata")]
    public string Metadata { get; }
}
