namespace Unity.Services.Ugc.Internal.Models;

using System;
using System.Text.Json.Serialization;
using Unity.Services.Ugc.Models;

/// <summary>
/// InternalRepresentationVersion model
/// </summary>
public class InternalRepresentationVersion
{
    /// <summary>
    /// Creates an instance of InternalRepresentationVersion.
    /// </summary>
    /// <param name="id">id param</param>
    /// <param name="md5Hash">md5Hash param</param>
    /// <param name="representationId">representationId param</param>
    /// <param name="createdAt">createdAt param</param>
    /// <param name="updatedAt">updatedAt param</param>
    /// <param name="uploadStatus">uploadStatus param, see <see cref="ContentUploadStatus"/></param>
    /// <param name="size">size param</param>
    /// <param name="deletedAt">deletedAt param</param>
    public InternalRepresentationVersion(
        string id,
        string md5Hash,
        string representationId,
        DateTime createdAt,
        DateTime updatedAt,
        string uploadStatus,
        long? size = default,
        DateTime? deletedAt = default
    )
    {
        Id = id;
        Md5Hash = md5Hash;
        Size = size;
        RepresentationId = representationId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
        UploadStatus = uploadStatus;
    }

    /// <summary>
    /// Parameter id of InternalRepresentationVersion
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; }

    /// <summary>
    /// Parameter md5Hash of InternalRepresentationVersion
    /// </summary>
    [JsonPropertyName("md5Hash")]
    public string Md5Hash { get; }

    /// <summary>
    /// Parameter size of InternalRepresentationVersion
    /// </summary>
    [JsonPropertyName("size")]
    public long? Size { get; }

    /// <summary>
    /// Parameter representationId of InternalRepresentationVersion
    /// </summary>
    [JsonPropertyName("representationId")]
    public string RepresentationId { get; }

    /// <summary>
    /// Parameter createdAt of InternalRepresentationVersion
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Parameter updatedAt of InternalRepresentationVersion
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; }

    /// <summary>
    /// Parameter deletedAt of InternalRepresentationVersion
    /// </summary>
    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; }

    /// <summary>
    /// Parameter uploadStatus of InternalRepresentationVersion, see <see cref="ContentUploadStatus"/>
    /// </summary>
    [JsonPropertyName("uploadStatus")]
    public string UploadStatus { get; }
}
