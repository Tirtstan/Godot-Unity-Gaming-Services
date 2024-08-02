namespace Unity.Services.Ugc.Internal.Models;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

/// <summary>
/// InternalRepresentation model
/// </summary>
public class InternalRepresentation
{
    /// <summary>
    /// Creates an instance of InternalRepresentation.
    /// </summary>
    /// <param name="id">id param</param>
    /// <param name="contentId">contentId param</param>
    /// <param name="tags">tags param</param>
    /// <param name="createdAt">createdAt param</param>
    /// <param name="updatedAt">updatedAt param</param>
    /// <param name="currentVersion">currentVersion param</param>
    /// <param name="downloadUrl">downloadUrl param</param>
    /// <param name="md5Hash">md5Hash param</param>
    /// <param name="deletedAt">deletedAt param</param>
    /// <param name="metadata">metadata param</param>
    /// <param name="webhookEventName">webhookEventName param</param>
    public InternalRepresentation(
        string id,
        string contentId,
        List<InternalRepresentationTag> tags,
        DateTime createdAt,
        DateTime updatedAt,
        string currentVersion = default,
        string downloadUrl = default,
        string md5Hash = default,
        DateTime? deletedAt = default,
        string metadata = default,
        string webhookEventName = default
    )
    {
        Id = id;
        ContentId = contentId;
        CurrentVersion = currentVersion;
        DownloadUrl = downloadUrl;
        Md5Hash = md5Hash;
        Tags = tags;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
        Metadata = metadata;
        WebhookEventName = webhookEventName;
    }

    /// <summary>
    /// Parameter id of InternalRepresentation
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; }

    /// <summary>
    /// Parameter contentId of InternalRepresentation
    /// </summary>
    [JsonPropertyName("contentId")]
    public string ContentId { get; }

    /// <summary>
    /// Parameter currentVersion of InternalRepresentation
    /// </summary>
    [JsonPropertyName("currentVersion")]
    public string CurrentVersion { get; }

    /// <summary>
    /// Parameter downloadUrl of InternalRepresentation
    /// </summary>
    [JsonPropertyName("downloadUrl")]
    public string DownloadUrl { get; }

    /// <summary>
    /// Parameter md5Hash of InternalRepresentation
    /// </summary>
    [JsonPropertyName("md5Hash")]
    public string Md5Hash { get; }

    /// <summary>
    /// Parameter tags of InternalRepresentation
    /// </summary>
    [JsonPropertyName("tags")]
    public List<InternalRepresentationTag> Tags { get; }

    /// <summary>
    /// Parameter createdAt of InternalRepresentation
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Parameter updatedAt of InternalRepresentation
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; }

    /// <summary>
    /// Parameter deletedAt of InternalRepresentation
    /// </summary>
    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; }

    /// <summary>
    /// Parameter metadata of InternalRepresentation
    /// </summary>
    [JsonPropertyName("metadata")]
    public string Metadata { get; }

    /// <summary>
    /// Parameter webhookEventName of InternalRepresentation
    /// </summary>
    [JsonPropertyName("webhookEventName")]
    public string WebhookEventName { get; }
}
