namespace Unity.Services.CloudSave.Models;

// sourced from Unity

using System.Collections.Generic;
using System.Text.Json.Serialization;

/// <summary>
/// The request body for querying an index
/// </summary>
public class Query
{
    /// <summary>
    /// The request body for querying an index
    /// </summary>
    /// <param name="fields">fields param</param>
    /// <param name="returnKeys">The keys to return in the response. This can include keys not on the index. If not specified or empty, the data on the results will be empty for any returned entities.</param>
    /// <param name="offset">The number of results to skip. Defaults to 0.</param>
    /// <param name="limit">The maximum number of results to return. Defaults to 10. Specifying 0 will return the default number of results.</param>
    public Query(
        List<FieldFilter> fields,
        HashSet<string> returnKeys = default,
        int offset = default,
        int limit = default
    )
    {
        Fields = fields;
        ReturnKeys = returnKeys;
        Offset = offset;
        Limit = limit;
    }

    /// <summary>
    /// The request body for querying an index
    /// </summary>
    /// <param name="fields">fields param</param>
    /// <param name="returnKeys">The keys to return in the response. This can include keys not on the index. If not specified or empty, the data on the results will be empty for any returned entities.</param>
    /// <param name="offset">The number of results to skip. Defaults to 0.</param>
    /// <param name="limit">The maximum number of results to return. Defaults to 10. Specifying 0 will return the default number of results.</param>
    /// <param name="sampleSize">If set, the given number of random items will be chosen from the total query results and returned as a sample. Defaults to null.</param>
    public Query(List<FieldFilter> fields, HashSet<string> returnKeys, int offset, int limit, int? sampleSize)
    {
        Fields = fields;
        ReturnKeys = returnKeys;
        Offset = offset;
        Limit = limit;
        SampleSize = sampleSize;
    }

    /// <summary>
    /// Parameter fields of Query
    /// </summary>
    [JsonPropertyName("fields")]
    public List<FieldFilter> Fields { get; }

    /// <summary>
    /// The keys to return in the response. This can include keys not on the index. If not specified or empty, the data on the results will be empty for any returned entities.
    /// </summary>
    [JsonPropertyName("returnKeys")]
    public HashSet<string> ReturnKeys { get; }

    /// <summary>
    /// The number of results to skip. Defaults to 0.
    /// </summary>
    [JsonPropertyName("offset")]
    public int Offset { get; }

    /// <summary>
    /// The maximum number of results to return. Defaults to 10. Specifying 0 will return the default number of results.
    /// </summary>
    [JsonPropertyName("limit")]
    public int Limit { get; }

    /// <summary>
    /// If set, the given number of random items will be chosen from the total query results and returned as a sample. Defaults to null.
    /// </summary>
    [JsonPropertyName("sampleSize")]
    public int? SampleSize { get; }
}
