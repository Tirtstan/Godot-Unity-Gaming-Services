namespace Unity.Services.Ugc.Internal.Models;

using System;
using System.Text.Json.Serialization;
using Unity.Services.Ugc.Models;

/// <summary>
/// InternalContentVersion model
/// </summary>
public class InternalContentVersion
{
    /// <summary>
    /// Creates an instance of InternalContentVersion.
    /// </summary>
    /// <param name="id">id param</param>
    /// <param name="contentId">contentId param</param>
    /// <param name="createdAt">createdAt param</param>
    /// <param name="updatedAt">updatedAt param</param>
    /// <param name="assetUploadStatus">assetUploadStatus param</param>
    /// <param name="thumbnailUploadStatus">thumbnailUploadStatus param</param>
    /// <param name="contentMd5Hash">contentMd5Hash param</param>
    /// <param name="thumbnailMd5Hash">thumbnailMd5Hash param</param>
    /// <param name="size">size param</param>
    public InternalContentVersion(
        string id,
        string contentId,
        DateTime createdAt,
        DateTime updatedAt,
        string assetUploadStatus,
        string thumbnailUploadStatus,
        string contentMd5Hash = default,
        string thumbnailMd5Hash = default,
        long? size = default
    )
    {
        Id = id;
        ContentMd5Hash = contentMd5Hash;
        ThumbnailMd5Hash = thumbnailMd5Hash;
        Size = size;
        ContentId = contentId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        AssetUploadStatus = assetUploadStatus;
        ThumbnailUploadStatus = thumbnailUploadStatus;
    }

    /// <summary>
    /// Parameter id of ContentVersionDTO
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; }

    /// <summary>
    /// Parameter contentMd5Hash of ContentVersionDTO
    /// </summary>
    [JsonPropertyName("contentMd5Hash")]
    public string ContentMd5Hash { get; }

    /// <summary>
    /// Parameter thumbnailMd5Hash of ContentVersionDTO
    /// </summary>
    [JsonPropertyName("thumbnailMd5Hash")]
    public string ThumbnailMd5Hash { get; }

    /// <summary>
    /// Parameter size of ContentVersionDTO
    /// </summary>
    [JsonPropertyName("size")]
    public long? Size { get; }

    /// <summary>
    /// Parameter contentId of ContentVersionDTO
    /// </summary>
    [JsonPropertyName("contentId")]
    public string ContentId { get; }

    /// <summary>
    /// Parameter createdAt of ContentVersionDTO
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Parameter updatedAt of ContentVersionDTO
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; }

    /// <summary>
    /// Parameter assetUploadStatus of ContentVersionDTO, see <see cref="ContentUploadStatus"/>
    /// </summary>
    [JsonPropertyName("assetUploadStatus")]
    public string AssetUploadStatus { get; }

    /// <summary>
    /// Parameter thumbnailUploadStatus of ContentVersionDTO, see <see cref="ContentUploadStatus"/>
    /// </summary>
    [JsonPropertyName("thumbnailUploadStatus")]
    public string ThumbnailUploadStatus { get; }
}
