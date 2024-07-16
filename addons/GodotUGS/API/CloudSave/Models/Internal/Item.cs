namespace Unity.Services.CloudSave.Internal.Models;

// sourced from Unity

using System.Text.Json.Serialization;

/// <summary>
/// Response type for a Data Item stored in the Cloud Save service.
/// </summary>
public class Item
{
    /// <summary>
    /// Response type for a Data Item stored in the Cloud Save service.
    /// </summary>
    /// <param name="key">Item key</param>
    /// <param name="value">Any JSON serializable structure with a maximum size of 5 MB.</param>
    /// <param name="writeLock">Enforces conflict checking when updating an existing data item. This field should be omitted when creating a new data item. When updating an existing item, omitting this field ignores write conflicts. When present, an error response will be returned if the writeLock in the request does not match the stored writeLock.</param>
    /// <param name="modified">modified param</param>
    /// <param name="created">created param</param>
    public Item(string key, object value, string writeLock, ModifiedMetadata modified, ModifiedMetadata created)
    {
        Key = key;
        Value = value;
        WriteLock = writeLock;
        Modified = modified;
        Created = created;
    }

    /// <summary>
    /// Item key
    /// </summary>
    [JsonPropertyName("key")]
    public string Key { get; }

    /// <summary>
    /// Any JSON serializable structure with a maximum size of 5 MB.
    /// </summary>
    [JsonPropertyName("value")]
    public object Value { get; }

    /// <summary>
    /// Enforces conflict checking when updating an existing data item. This field should be omitted when creating a new data item. When updating an existing item, omitting this field ignores write conflicts. When present, an error response will be returned if the writeLock in the request does not match the stored writeLock.
    /// </summary>
    [JsonPropertyName("writeLock")]
    public string WriteLock { get; }

    /// <summary>
    /// Parameter modified of Item
    /// </summary>
    [JsonPropertyName("modified")]
    public ModifiedMetadata Modified { get; }

    /// <summary>
    /// Parameter created of Item
    /// </summary>
    [JsonPropertyName("created")]
    public ModifiedMetadata Created { get; }
}
