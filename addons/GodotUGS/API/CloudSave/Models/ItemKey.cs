namespace Unity.Services.CloudSave.Models;

using System;

public class ItemKey
{
    /// <summary>
    /// Response type for a Data Item stored in the Cloud Save service.
    /// </summary>
    /// <param name="key">The data key</param>
    /// <param name="writeLock">Write lock value for the key, to be used for enforcing conflict checking when updating an existing data item</param>
    /// <param name="modified">The date time when the value was last modified</param>
    public ItemKey(string key, string writeLock, DateTime? modified)
    {
        Key = key;
        WriteLock = writeLock;
        Modified = modified;
    }

    /// <summary>
    /// The data key
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// The write lock value for the data, used for enforcing conflict checking when updating an existing data item
    /// </summary>
    public string WriteLock { get; }

    /// <summary>
    /// The date time when the value was last modified
    /// </summary>
    public DateTime? Modified { get; }
}
