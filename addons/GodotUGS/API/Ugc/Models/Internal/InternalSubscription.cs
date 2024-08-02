namespace Unity.Services.Ugc.Internal.Models;

using System;
using System.Text.Json.Serialization;

/// <summary>
/// Response for Subscription
/// </summary>
internal class InternalSubscription
{
    /// <summary>
    /// Response for Subscription
    /// </summary>
    /// <param name="id">Id of the subscription</param>
    /// <param name="playerId">Id of the player</param>
    /// <param name="content">content param</param>
    /// <param name="createdAt">Date subscription was created</param>
    /// <param name="updatedAt">Date subscription was last updated</param>
    /// <param name="deletedAt">Date subscription was soft deleted</param>
    public InternalSubscription(
        string id,
        string playerId,
        InternalContent content = default,
        DateTime createdAt = default,
        DateTime updatedAt = default,
        DateTime? deletedAt = default
    )
    {
        Id = id;
        PlayerId = playerId;
        Content = content;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        DeletedAt = deletedAt;
    }

    /// <summary>
    /// Id of the subscription
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; }

    /// <summary>
    /// Id of the player
    /// </summary>
    [JsonPropertyName("playerId")]
    public string PlayerId { get; }

    /// <summary>
    /// Parameter content of SubscriptionDTO
    /// </summary>
    [JsonPropertyName("content")]
    public InternalContent Content { get; }

    /// <summary>
    /// Date subscription was created
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Date subscription was last updated
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; }

    /// <summary>
    /// Date subscription was soft deleted
    /// </summary>
    [JsonPropertyName("deletedAt")]
    public DateTime? DeletedAt { get; }
}
