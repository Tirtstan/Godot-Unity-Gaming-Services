namespace Unity.Services.Friends.Internal.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;
using Unity.Services.Friends.Models;

public class InternalRelationship
{
    /// <see cref="RelationshipType"/>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("members")]
    public List<InternalMember> Members { get; set; }
}

public class InternalMember
{
    [JsonPropertyName("profileName")]
    public string ProfileName { get; set; } = "";

    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <see cref="MemberRole"/>
    [JsonPropertyName("role")]
    public string Role { get; set; }
}
