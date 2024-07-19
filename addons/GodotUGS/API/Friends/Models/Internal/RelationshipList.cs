namespace Unity.Services.Friends.Internal.Models;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Unity.Services.Friends.Models;

public class RelationshipList
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("expires")]
    public DateTime Expires { get; set; }

    [JsonPropertyName("type")]
    public RelationshipType Type { get; set; }

    [JsonPropertyName("members")]
    public List<Member> Members { get; set; }
}
