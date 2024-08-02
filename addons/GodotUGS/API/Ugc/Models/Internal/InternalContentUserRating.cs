namespace Unity.Services.Ugc.Internal.Models;

// sourced from Unity

using System;
using System.Text.Json.Serialization;

/// <summary>
/// Response for getting content user rating info
/// </summary>
internal class InternalContentUserRating
{
    /// <summary>
    /// Response for getting content user rating info
    /// </summary>
    /// <param name="id">Id of content rating</param>
    /// <param name="userId">Id of user providing rating</param>
    /// <param name="contentId">Content the rating applies to</param>
    /// <param name="createdAt">Date rating was first created</param>
    /// <param name="updatedAt">Date rating was updated</param>
    /// <param name="rating">User rating</param>
    public InternalContentUserRating(
        string id,
        string userId,
        string contentId,
        DateTime createdAt = default,
        DateTime updatedAt = default,
        float rating = default
    )
    {
        Id = id;
        UserId = userId;
        ContentId = contentId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Rating = rating;
    }

    /// <summary>
    /// Id of content rating
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; }

    /// <summary>
    /// Id of user providing rating
    /// </summary>
    [JsonPropertyName("userId")]
    public string UserId { get; }

    /// <summary>
    /// Content the rating applies to
    /// </summary>
    [JsonPropertyName("contentId")]
    public string ContentId { get; }

    /// <summary>
    /// Date rating was first created
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Date rating was updated
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; }

    /// <summary>
    /// User rating
    /// </summary>
    [JsonPropertyName("rating")]
    public float Rating { get; }
}
