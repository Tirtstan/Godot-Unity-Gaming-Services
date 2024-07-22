namespace Unity.Services.Ugc.Models;

using System;
using Unity.Services.Ugc.Internal.Models;

/// <summary>
/// Contains content version metadata
/// </summary>
public class ContentVersion
{
    internal ContentVersion(InternalContentVersion contentVersionDto)
    {
        Id = contentVersionDto.Id;
        ContentMd5Hash = contentVersionDto.ContentMd5Hash;
        ThumbnailMd5Hash = contentVersionDto.ThumbnailMd5Hash;
        ContentId = contentVersionDto.ContentId;
        CreatedAt = contentVersionDto.CreatedAt;
        UpdatedAt = contentVersionDto.UpdatedAt;
        AssetUploadStatus = contentVersionDto.AssetUploadStatus;
        ThumbnailUploadStatus = contentVersionDto.ThumbnailUploadStatus;
        Size = contentVersionDto.Size;
    }

    /// <summary>
    ///     Content version id
    /// </summary>
    public string Id { get; }

    /// <summary>
    ///     Md5 hash of the content binary
    /// </summary>
    public string ContentMd5Hash { get; }

    /// <summary>
    ///     Md5 hash of the content thumbnail
    /// </summary>
    public string ThumbnailMd5Hash { get; }

    /// <summary>
    ///     Content id
    /// </summary>
    public string ContentId { get; }

    /// <summary>
    ///     Date content was created
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    ///     Date content was last updated
    /// </summary>
    public DateTime UpdatedAt { get; }

    /// <summary>
    ///     Asset upload status
    /// </summary>
    public string AssetUploadStatus { get; }

    /// <summary>
    ///     Thumbnail upload status
    /// </summary>
    public string ThumbnailUploadStatus { get; }

    /// <summary>
    ///     Size of the asset
    /// </summary>
    public long? Size { get; }
}
