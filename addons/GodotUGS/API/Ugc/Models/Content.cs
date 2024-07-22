namespace Unity.Services.Ugc.Models;

using System;
using System.Collections.Generic;
using Unity.Services.Ugc.Internal.Models;

/// <summary>
/// Contains content metadata
/// </summary>
public class Content
{
    public Content(InternalContent contentDTO, string addVersionId = null)
    {
        Id = contentDTO.Id;
        Name = contentDTO.Name;
        Description = contentDTO.Description;
        Visibility = contentDTO.Visibility;
        Version = contentDTO.Version;
        CreatedAt = contentDTO.CreatedAt;
        UpdatedAt = contentDTO.UpdatedAt;
        DeletedAt = contentDTO.DeletedAt;
        ProjectId = contentDTO.ProjectId;
        EnvironmentId = contentDTO.EnvironmentId;
        CreatorAccountId = contentDTO.CreatorAccountId;
        ThumbnailUrl = contentDTO.ThumbnailUrl;
        DownloadUrl = contentDTO.DownloadUrl;
        ContentMd5Hash = contentDTO.ContentMd5Hash;
        ThumbnailMd5Hash = contentDTO.ThumbnailMd5Hash;
        Tags = contentDTO.Tags?.ConvertAll(x => new Tag(x));
        DiscoveryTags = contentDTO.DiscoveryTags?.ConvertAll(x => new Tag(x));
        AverageRating = contentDTO.AverageRating;
        RatingCount = contentDTO.RatingCount;
        SubscriptionCount = contentDTO.SubscriptionCount;
        Statistics = contentDTO.Statistics;
        IsUserSubscribed = contentDTO.IsUserSubscribed;
        AssetUploadStatus = contentDTO.AssetUploadStatus;
        ThumbnailUploadStatus = contentDTO.ThumbnailUploadStatus;
        AddVersionId = addVersionId;
        CustomId = contentDTO.CustomId;
        Metadata = contentDTO.Metadata;
    }

    /// <summary>
    /// Content item guid
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Display name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Global visibility, see <see cref="ContentVisibility"/>
    /// </summary>
    public string Visibility { get; }

    /// <summary>
    /// Current live version of content
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Metadata of the content
    /// </summary>
    public string Metadata { get; }

    /// <summary>
    /// Date content was created
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Date content was last updated
    /// </summary>
    public DateTime UpdatedAt { get; }

    /// <summary>
    /// Date content was soft deleted
    /// </summary>
    public DateTime? DeletedAt { get; }

    /// <summary>
    /// Owning project id
    /// </summary>
    public string ProjectId { get; }

    /// <summary>
    /// Owning environment id
    /// </summary>
    public string EnvironmentId { get; }

    /// <summary>
    /// Creator id that uploaded content
    /// </summary>
    public string CreatorAccountId { get; }

    internal string ThumbnailUrl { get; }

    internal string DownloadUrl { get; }

    /// <summary>
    /// Md5 hash of the content binary
    /// </summary>
    public string ContentMd5Hash { get; }

    /// <summary>
    /// Md5 hash of the content thumbnail
    /// </summary>
    public string ThumbnailMd5Hash { get; }

    /// <summary>
    /// Tag Ids
    /// </summary>
    public List<Tag> Tags { get; }

    /// <summary>
    /// Content discovery tags
    /// </summary>
    public List<Tag> DiscoveryTags { get; }

    /// <summary>
    /// Average user rating
    /// </summary>
    public float? AverageRating { get; }

    /// <summary>
    /// Number of user ratings
    /// </summary>
    public int? RatingCount { get; }

    /// <summary>
    /// Number of subscriptions
    /// </summary>
    public int? SubscriptionCount { get; }

    /// <summary>
    /// Content statistics
    /// </summary>
    public ContentStatistics Statistics { get; }

    /// <summary>
    /// User is subscribed
    /// </summary>
    public bool IsUserSubscribed { get; }

    /// <summary>
    /// Asset upload status, see <see cref="ContentUploadStatus"/>
    /// </summary>
    public string AssetUploadStatus { get; }

    /// <summary>
    /// Thumbnail upload status, see <see cref="ContentUploadStatus"/>
    /// </summary>
    public string ThumbnailUploadStatus { get; }

    /// <summary>
    /// The downloaded asset. This value is only set with `GetContentAsync` or `DownloadContentDataAsync`
    /// </summary>
    public byte[] DownloadedContent { get; internal set; }

    /// <summary>
    /// The downloaded thumbnail. This value is only set with `GetContentAsync` or `DownloadContentDataAsync`
    /// </summary>
    public byte[] DownloadedThumbnail { get; internal set; }

    /// <summary>
    /// Custom Id for content
    /// </summary>
    public string CustomId { get; internal set; }

    /// <summary>
    /// Id returned when adding content version
    /// </summary>
    public string AddVersionId { get; internal set; }
}
