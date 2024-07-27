namespace Unity.Services.Ugc.Models;

// sourced from Unity

using System;
using Unity.Services.Ugc.Internal.Models;

/// <summary>
/// Contains content user rating metadata
/// </summary>
public class ContentUserRating
{
    internal ContentUserRating(InternalContentUserRating contentUserRatingDTO)
    {
        Id = contentUserRatingDTO.Id;
        UserId = contentUserRatingDTO.UserId;
        ContentId = contentUserRatingDTO.ContentId;
        CreatedAt = contentUserRatingDTO.CreatedAt;
        UpdatedAt = contentUserRatingDTO.UpdatedAt;
        Rating = contentUserRatingDTO.Rating;
    }

    /// <summary>
    /// Id of content rating
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Id of user providing rating
    /// </summary>
    public string UserId { get; }

    /// <summary>
    /// Content the rating applies to
    /// </summary>
    public string ContentId { get; }

    /// <summary>
    /// Date rating was first created
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Date rating was updated
    /// </summary>
    public DateTime UpdatedAt { get; }

    /// <summary>
    /// User rating
    /// </summary>
    public float Rating { get; }
}
