namespace Unity.Services.CloudSave.Models;

// sourced from Unity

using System;
using System.Text.Json;

public class Item
{
    /// <summary>
    /// Response type for a Data Item stored in the Cloud Save service.
    /// </summary>
    internal Item(Internal.Models.InternalItem item)
    {
        Key = item.Key;
        Value = item.Value;
        Created = item.Created?.Date;
        Modified = item.Modified?.Date;
        WriteLock = item.WriteLock;
    }

    /// <summary>
    /// The key against which the data is stored
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Any JSON serializable structure
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// The write lock value for the data, used for enforcing conflict checking when updating an existing data item
    /// </summary>
    public string WriteLock { get; }

    /// <summary>
    /// The date time when the value was last modified
    /// </summary>
    public DateTime? Modified { get; }

    /// <summary>
    /// The date time when the value was initially created
    /// </summary>
    public DateTime? Created { get; }

    public T GetValueAs<T>() => JsonSerializer.Deserialize<T>(Value.ToString());
}
