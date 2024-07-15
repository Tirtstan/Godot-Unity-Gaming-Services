namespace Unity.Services.CloudSave.Models;

// sourced from Unity

using System.Collections.Generic;
using System.Text.Json.Serialization;

/// <summary>
/// QueryResult model
/// </summary>
public class EntityData
{
    /// <summary>
    /// Creates an instance of QueryResult.
    /// </summary>
    /// <param name="id">id param</param>
    /// <param name="data">The list of data key-value pairs for the entity</param>
    public EntityData(string id = default, List<Item> data = default)
    {
        Id = id;
        Data = data;
    }

    /// <summary>
    /// Parameter id of QueryResult
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; }

    /// <summary>
    /// The list of data key-value pairs for the entity
    /// </summary>
    [JsonPropertyName("data")]
    public List<Item> Data { get; }
}
