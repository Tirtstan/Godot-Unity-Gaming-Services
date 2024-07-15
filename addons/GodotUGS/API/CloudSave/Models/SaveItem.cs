namespace Unity.Services.CloudSave.Models;

// sourced from Unity

/// <summary>
/// Request type for posting a Data Item to the Cloud Save service.
/// </summary>
public class SaveItem
{
    /// <summary>
    /// Request type for posting a Data Item to the Cloud Save service.
    /// </summary>
    /// <param name="value">Any data object</param>
    /// <param name="writeLock">Enforces conflict checking when updating an existing data item. This field should be omitted when creating a new data item. When updating an existing item, omitting this field ignores write conflicts. When present, an error response will be returned if the writeLock in the request does not match the stored writeLock.</param>
    public SaveItem(object value, string writeLock)
    {
        Value = value;
        WriteLock = writeLock;
    }

    /// <summary>
    /// Any data object
    /// </summary>
    public object Value { get; }

    /// <summary>
    /// Enforces conflict checking when updating an existing data item. This field should be omitted when creating a new data item. When updating an existing item, omitting this field ignores write conflicts. When present, an error response will be returned if the writeLock in the request does not match the stored writeLock.
    /// </summary>
    public string WriteLock { get; }
}
