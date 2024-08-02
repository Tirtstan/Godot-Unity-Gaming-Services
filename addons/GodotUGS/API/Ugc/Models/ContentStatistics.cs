namespace Unity.Services.Ugc.Models;

using System.Text.Json.Serialization;

/// <summary>
/// ContentStatistics model
/// </summary>
public class ContentStatistics
{
    /// <summary>
    /// Creates an instance of ContentStatistics.
    /// </summary>
    /// <param name="ratingsAverage">ratingsAverage param</param>
    /// <param name="ratingsCount">ratingsCount param</param>
    /// <param name="downloadsCount">downloadsCount param</param>
    /// <param name="subscriptionsCount">subscriptionsCount param</param>
    /// <param name="reportsCount">reportsCount param</param>
    public ContentStatistics(
        AverageStat ratingsAverage = default,
        CountStat ratingsCount = default,
        CountStat downloadsCount = default,
        CountStat subscriptionsCount = default,
        CountStat reportsCount = default
    )
    {
        RatingsAverage = ratingsAverage;
        RatingsCount = ratingsCount;
        DownloadsCount = downloadsCount;
        SubscriptionsCount = subscriptionsCount;
        ReportsCount = reportsCount;
    }

    /// <summary>
    /// Parameter ratingsAverage of ContentStatistics
    /// </summary>
    [JsonPropertyName("ratingsAverage")]
    public AverageStat RatingsAverage { get; }

    /// <summary>
    /// Parameter ratingsCount of ContentStatistics
    /// </summary>
    [JsonPropertyName("ratingsCount")]
    public CountStat RatingsCount { get; }

    /// <summary>
    /// Parameter downloadsCount of ContentStatistics
    /// </summary>
    [JsonPropertyName("downloadsCount")]
    public CountStat DownloadsCount { get; }

    /// <summary>
    /// Parameter subscriptionsCount of ContentStatistics
    /// </summary>
    [JsonPropertyName("subscriptionsCount")]
    public CountStat SubscriptionsCount { get; }

    /// <summary>
    /// Parameter reportsCount of ContentStatistics
    /// </summary>
    [JsonPropertyName("reportsCount")]
    public CountStat ReportsCount { get; }
}
