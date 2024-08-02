namespace Unity.Services.Ugc.Models;

using Unity.Services.Ugc.Internal.Models;

/// <summary>
/// Contains tag metadata
/// </summary>
public class Tag
{
    public Tag(InternalTag tagDTO)
    {
        Id = tagDTO.Id;
        Name = tagDTO.Name;
    }

    /// <summary>
    /// Tag item guid
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Display name
    /// </summary>
    public string Name { get; }
}
