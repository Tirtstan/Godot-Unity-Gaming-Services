namespace Unity.Services.Ugc.Internal.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Response for getting tag info
/// </summary>
public class InternalTag
{
    /// <summary>
    /// Response for getting tag info
    /// </summary>
    /// <param name="projectId">The project that the tag belongs to. If \&quot;global\&quot; then tag is un-editable/un-deletable and is shared  among all projects.</param>
    /// <param name="id">Tag item guid</param>
    /// <param name="name">Display name</param>
    /// <param name="environmentId">The environment that the tag belongs to.</param>
    public InternalTag(string projectId, string id = default, string name = default, string environmentId = default)
    {
        Id = id;
        Name = name;
        ProjectId = projectId;
        EnvironmentId = environmentId;
    }

    /// <summary>
    /// Tag item guid
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; }

    /// <summary>
    /// Display name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    /// The project that the tag belongs to. If "global" then tag is un-editable/un-deletable and is shared among all projects.
    /// </summary>
    [JsonPropertyName("projectId")]
    public string ProjectId { get; }

    /// <summary>
    /// The environment that the tag belongs to.
    /// </summary>
    [JsonPropertyName("environmentId")]
    public string EnvironmentId { get; }
}
