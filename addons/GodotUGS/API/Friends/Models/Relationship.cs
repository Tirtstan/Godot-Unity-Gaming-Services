using System.Text.Json.Serialization;

namespace Unity.Services.Friends.Models;

/// <summary>
/// The representation of a relationship between members
/// </summary>
public class Relationship
{
    public Relationship() { }

    public Relationship(string id, string type, Member member)
    {
        Id = id;
        Type = type;
        Member = member;
    }

    /// <summary>
    /// The id of the relationship
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The type of relationship
    /// </summary>
    /// <see cref="RelationshipType"/>
    public string Type { get; set; }

    /// <summary>
    /// The member with whom the current user has the relationship
    /// </summary>
    public Member Member { get; set; }
}

/// <summary>
/// The relationship type between members
/// </summary>
public class RelationshipType
{
    [JsonPropertyName("FRIEND")]
    public static readonly string Friend = "FRIEND";

    [JsonPropertyName("BLOCK")]
    public static readonly string Block = "BLOCK";

    [JsonPropertyName("FRIEND_REQUEST")]
    public static readonly string FriendRequest = "FRIEND_REQUEST";
}


/// <summary>
/// The relationship type between members
/// </summary>
// public enum RelationshipType
// {
//     /// <summary>
//     /// Enum Friend for value: FRIEND
//     /// </summary>
//     [JsonPropertyName("FRIEND")]
//     Friend = 1,

//     /// <summary>
//     /// Enum Block for value: BLOCK
//     /// </summary>
//     [JsonPropertyName("BLOCK")]
//     Block = 2,

//     /// <summary>
//     /// Enum FriendRequest for value: FRIEND_REQUEST
//     /// </summary>
//     [JsonPropertyName("FRIEND_REQUEST")]
//     FriendRequest = 3,
// }
