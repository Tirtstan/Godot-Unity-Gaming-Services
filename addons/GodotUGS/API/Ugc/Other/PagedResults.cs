namespace Unity.Services.Ugc;

using System.Collections.Generic;
using System.Text.Json.Serialization;

/// <summary>
/// Container for paginated results used in search requests
/// </summary>
/// <typeparam name="T">One of the UGC model class</typeparam>
public class PagedResults<T>
{
    /// <summary>
    /// Construct a new PagedResults object.
    /// </summary>
    /// <param name="offset">offset param</param>
    /// <param name="limit">limit param</param>
    /// <param name="total">total param</param>
    /// <param name="results">results param</param>
    public PagedResults(int offset, int limit, int total, List<T> results)
    {
        Offset = offset;
        Limit = limit;
        Total = total;
        Results = results;
    }

    /// <summary>
    /// Pagination offset of the results
    /// </summary>
    public int Offset { get; }

    /// <summary>
    /// Limit on the number of results fetched
    /// </summary>
    public int Limit { get; }

    /// <summary>
    /// Total number of results of the whole search request.
    /// Filled only if `IncludeTotal` was set to `true` in the search request
    /// </summary>
    public int Total { get; }

    /// <summary>
    /// List of results fetched
    /// </summary>
    [JsonPropertyName("results")]
    public List<T> Results { get; }
}
