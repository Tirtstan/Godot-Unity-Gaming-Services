namespace Unity.Services.Ugc.Models;

using System;
using Unity.Services.Ugc.Internal.Models;

/// <summary>
/// Contains source content representation metadata
/// </summary>
public class RepresentationVersion
{
    internal RepresentationVersion(InternalRepresentationVersion versionDto)
    {
        Id = versionDto.Id;
        Md5Hash = versionDto.Md5Hash;
        CreatedAt = versionDto.CreatedAt;
        UpdatedAt = versionDto.UpdatedAt;
        DeletedAt = versionDto.DeletedAt;
        Size = versionDto.Size;
    }

    /// <summary>
    /// Version Id of representation
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Md5 hash of the representation version binary
    /// </summary>
    public string Md5Hash { get; }

    /// <summary>
    /// Size of the representation binary
    /// </summary>
    public long? Size { get; }

    /// <summary>
    /// Date representation version was first created
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Date representation version was updated
    /// </summary>
    public DateTime UpdatedAt { get; }

    /// <summary>
    /// Date representation version was deleted
    /// </summary>
    public DateTime? DeletedAt { get; }
}
