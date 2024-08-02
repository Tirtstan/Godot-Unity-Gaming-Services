namespace Unity.Services.Ugc.Internal.Models;

// sourced from Unity

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Unity.Services.Ugc.Models;

public class InternalContent
{
    /// <summary>
    /// Response for getting content info
    /// </summary>
    /// <param name="id">Content item guid</param>
    /// <param name="projectId">Owning project id</param>
    /// <param name="environmentId">Owning environment id</param>
    /// <param name="creatorAccountId">Account id that uploaded content</param>
    /// <param name="name">Display name</param>
    /// <param name="customId">Client supplied id for content</param>
    /// <param name="description">Description</param>
    /// <param name="visibility">visibility param <see cref="ContentVisibility"/></param>
    /// <param name="moderationStatus">moderationStatus param</param>
    /// <param name="version">Current live version of content</param>
    /// <param name="createdAt">Date content was created</param>
    /// <param name="updatedAt">Date content was last updated</param>
    /// <param name="deletedAt">Date content was soft deleted</param>
    /// <param name="thumbnailUrl">Image url for thumbnail</param>
    /// <param name="downloadUrl">Download url for raw content</param>
    /// <param name="portalUrl">Content portal url</param>
    /// <param name="contentMd5Hash">Md5 hash of the content binary</param>
    /// <param name="thumbnailMd5Hash">Md5 hash of the content thumbnail</param>
    /// <param name="metadata">Content metadata</param>
    /// <param name="tags">Tag Ids</param>
    /// <param name="discoveryTags">Content Discovery Tags</param>
    /// <param name="averageRating">Average user rating</param>
    /// <param name="ratingCount">Number of user ratings</param>
    /// <param name="subscriptionCount">Number of subscriptions</param>
    /// <param name="statistics">statistics param</param>
    /// <param name="isUserSubscribed">User is subscribed</param>
    /// <param name="assetUploadStatus">assetUploadStatus param</param>
    /// <param name="thumbnailUploadStatus">thumbnailUploadStatus param</param>
    /// <param name="webhookEventName">webhookEventName param</param>
    public InternalContent(
        string id,
        string projectId,
        string environmentId,
        string creatorAccountId,
        string name = default,
        string customId = default,
        string description = default,
        string visibility = default,
        string moderationStatus = default,
        string version = default,
        DateTime createdAt = default,
        DateTime updatedAt = default,
        DateTime? deletedAt = default,
        string thumbnailUrl = default,
        string downloadUrl = default,
        string portalUrl = default,
        string contentMd5Hash = default,
        string thumbnailMd5Hash = default,
        string metadata = default,
        List<InternalTag> tags = default,
        List<InternalTag> discoveryTags = default,
        float? averageRating = default,
        int? ratingCount = default,
        int? subscriptionCount = default,
        ContentStatistics statistics = default,
        bool isUserSubscribed = default,
        string assetUploadStatus = default,
        string thumbnailUploadStatus = default,
        string webhookEventName = default
    )
    {
        Id = id;
        Name = name;
        CustomId = customId;
        Description = description;
        Visibility = visibility;
        ModerationStatus = moderationStatus;
        Version = version;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
        ProjectId = projectId;
        EnvironmentId = environmentId;
        CreatorAccountId = creatorAccountId;
        ThumbnailUrl = thumbnailUrl;
        DownloadUrl = downloadUrl;
        PortalUrl = portalUrl;
        ContentMd5Hash = contentMd5Hash;
        ThumbnailMd5Hash = thumbnailMd5Hash;
        Metadata = metadata;
        Tags = tags;
        DiscoveryTags = discoveryTags;
        AverageRating = averageRating;
        RatingCount = ratingCount;
        SubscriptionCount = subscriptionCount;
        Statistics = statistics;
        IsUserSubscribed = isUserSubscribed;
        AssetUploadStatus = assetUploadStatus;
        ThumbnailUploadStatus = thumbnailUploadStatus;
        WebhookEventName = webhookEventName;
    }

    /// <summary>
    /// Content item guid
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; }

    /// <summary>
    /// Display name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    /// Client supplied id for content
    /// </summary>
    [JsonPropertyName("customId")]
    public string CustomId { get; }

    /// <summary>
    /// Description
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; }

    /// <summary>
    /// Parameter visibility of ContentDTO, see <see cref="ContentVisibility"/>
    /// </summary>
    [JsonPropertyName("visibility")]
    public string Visibility { get; }

    /// <summary>
    /// Parameter moderationStatus of ContentDTO, see <see cref="ModerationStatus"/>
    /// </summary>
    [JsonPropertyName("moderationStatus")]
    public string ModerationStatus { get; }

    /// <summary>
    /// Current live version of content
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; }

    /// <summary>
    /// Date content was created
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Date content was last updated
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; }

    /// <summary>
    /// Date content was soft deleted
    /// </summary>
    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; }

    /// <summary>
    /// Owning project id
    /// </summary>
    [JsonPropertyName("projectId")]
    public string ProjectId { get; }

    /// <summary>
    /// Owning environment id
    /// </summary>
    [JsonPropertyName("environmentId")]
    public string EnvironmentId { get; }

    /// <summary>
    /// Account id that uploaded content
    /// </summary>
    [JsonPropertyName("creatorAccountId")]
    public string CreatorAccountId { get; }

    /// <summary>
    /// Image url for thumbnail
    /// </summary>
    [JsonPropertyName("thumbnailUrl")]
    public string ThumbnailUrl { get; }

    /// <summary>
    /// Download url for raw content
    /// </summary>
    [JsonPropertyName("downloadUrl")]
    public string DownloadUrl { get; }

    /// <summary>
    /// Content portal url
    /// </summary>
    [JsonPropertyName("portalUrl")]
    public string PortalUrl { get; }

    /// <summary>
    /// Md5 hash of the content binary
    /// </summary>
    [JsonPropertyName("contentMd5Hash")]
    public string ContentMd5Hash { get; }

    /// <summary>
    /// Md5 hash of the content thumbnail
    /// </summary>
    [JsonPropertyName("thumbnailMd5Hash")]
    public string ThumbnailMd5Hash { get; }

    /// <summary>
    /// Content metadata
    /// </summary>
    [JsonPropertyName("metadata")]
    public string Metadata { get; }

    /// <summary>
    /// Tag Ids
    /// </summary>
    [JsonPropertyName("tags")]
    public List<InternalTag> Tags { get; }

    /// <summary>
    /// Content Discovery Tags
    /// </summary>
    [JsonPropertyName("discoveryTags")]
    public List<InternalTag> DiscoveryTags { get; }

    /// <summary>
    /// Average user rating
    /// </summary>
    [JsonPropertyName("averageRating")]
    public float? AverageRating { get; }

    /// <summary>
    /// Number of user ratings
    /// </summary>
    [JsonPropertyName("ratingCount")]
    public int? RatingCount { get; }

    /// <summary>
    /// Number of subscriptions
    /// </summary>
    [JsonPropertyName("subscriptionCount")]
    public int? SubscriptionCount { get; }

    /// <summary>
    /// Parameter statistics of ContentDTO
    /// </summary>
    [JsonPropertyName("statistics")]
    public ContentStatistics Statistics { get; }

    /// <summary>
    /// User is subscribed
    /// </summary>
    [JsonPropertyName("isUserSubscribed")]
    public bool IsUserSubscribed { get; }

    /// <summary>
    /// Parameter assetUploadStatus of ContentDTO, see <see cref="ContentUploadStatus"/>
    /// </summary>
    [JsonPropertyName("assetUploadStatus")]
    public string AssetUploadStatus { get; }

    /// <summary>
    /// Parameter thumbnailUploadStatus of ContentDTO, see <see cref="ContentUploadStatus"/>
    /// </summary>
    [JsonPropertyName("thumbnailUploadStatus")]
    public string ThumbnailUploadStatus { get; }

    /// <summary>
    /// Parameter webhookEventName of ContentDTO
    /// </summary>
    [JsonPropertyName("webhookEventName")]
    public string WebhookEventName { get; }
}
