namespace Unity.Services.CloudSave.Internal.Models;

using System;
using System.Text.Json.Serialization;

// sourced from Unity

public class ModifiedMetadata
{
    /// <summary>
    /// Timestamp for when the object was modified.
    /// </summary>
    /// <param name="date">Date time in ISO 8601 format. Null if there is no associated value.</param>
    public ModifiedMetadata(DateTime? date)
    {
        Date = date;
    }

    /// <summary>
    /// Date time in ISO 8601 format. Null if there is no associated value.
    /// </summary>
    [JsonPropertyName("date")]
    public DateTime? Date { get; }
}
