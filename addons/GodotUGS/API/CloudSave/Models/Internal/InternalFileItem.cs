namespace Unity.Services.CloudSave.Internal.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class InternalFileItem
{
    /// <summary>
    /// Creates an instance of FileItem.
    /// </summary>
    /// <param name="size">size param</param>
    /// <param name="created">created param</param>
    /// <param name="modified">modified param</param>
    /// <param name="writeLock">writeLock param</param>
    /// <param name="contentType">contentType param</param>
    /// <param name="key">key param</param>
    public InternalFileItem(
        long size,
        ModifiedMetadata created,
        ModifiedMetadata modified,
        string writeLock,
        string contentType,
        string key = default
    )
    {
        Size = size;
        Created = created;
        Modified = modified;
        WriteLock = writeLock;
        ContentType = contentType;
        Key = key;
    }

    /// <summary>
    /// Parameter size of FileItem
    /// </summary>
    [JsonPropertyName("size")]
    public long Size { get; }

    /// <summary>
    /// Parameter created of FileItem
    /// </summary>
    [JsonPropertyName("created")]
    public ModifiedMetadata Created { get; }

    /// <summary>
    /// Parameter modified of FileItem
    /// </summary>
    [JsonPropertyName("modified")]
    public ModifiedMetadata Modified { get; }

    /// <summary>
    /// Parameter writeLock of FileItem
    /// </summary>
    [JsonPropertyName("writeLock")]
    public string WriteLock { get; }

    /// <summary>
    /// Parameter contentType of FileItem
    /// </summary>
    [JsonPropertyName("contentType")]
    public string ContentType { get; }

    /// <summary>
    /// Parameter key of FileItem
    /// </summary>
    [JsonPropertyName("key")]
    public string Key { get; }
}

public class FileItemList
{
    [JsonPropertyName("results")]
    public List<InternalFileItem> Results { get; set; }

    [JsonPropertyName("links")]
    public Link Links { get; set; }
}

public class Link
{
    [JsonPropertyName("next")]
    public string Next { get; set; }
}
