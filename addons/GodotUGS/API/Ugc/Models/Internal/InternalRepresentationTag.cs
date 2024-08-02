namespace Unity.Services.Ugc.Internal.Models;

using System.Text.Json.Serialization;

/// <summary>
/// InternalRepresentationTag model
/// </summary>
public class InternalRepresentationTag
{
    /// <summary>
    /// Creates an instance of InternalRepresentationTag.
    /// </summary>
    /// <param name="id">id param</param>
    /// <param name="name">name param</param>
    public InternalRepresentationTag(string id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Parameter id of InternalRepresentationTag
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; }

    /// <summary>
    /// Parameter name of InternalRepresentationTag
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }
}
