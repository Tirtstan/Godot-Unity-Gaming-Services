using System.Text.Json.Serialization;

namespace Unity.Services.Ugc.Models;

/// <summary>
/// AverageStat model
/// </summary>
public class AverageStat
{
    /// <summary>
    /// Creates an instance of AverageStat.
    /// </summary>
    /// <param name="allTime">allTime param</param>
    /// <param name="past365Days">past365Days param</param>
    /// <param name="past180Days">past180Days param</param>
    /// <param name="past90Days">past90Days param</param>
    /// <param name="past60Days">past60Days param</param>
    /// <param name="past30Days">past30Days param</param>
    /// <param name="past14Days">past14Days param</param>
    /// <param name="past7Days">past7Days param</param>
    /// <param name="pastDay">pastDay param</param>
    public AverageStat(
        double allTime = default,
        double past365Days = default,
        double past180Days = default,
        double past90Days = default,
        double past60Days = default,
        double past30Days = default,
        double past14Days = default,
        double past7Days = default,
        double pastDay = default
    )
    {
        AllTime = allTime;
        Past365Days = past365Days;
        Past180Days = past180Days;
        Past90Days = past90Days;
        Past60Days = past60Days;
        Past30Days = past30Days;
        Past14Days = past14Days;
        Past7Days = past7Days;
        PastDay = pastDay;
    }

    /// <summary>
    /// Parameter allTime of AverageStat
    /// </summary>
    [JsonPropertyName("allTime")]
    public double AllTime { get; }

    /// <summary>
    /// Parameter past365Days of AverageStat
    /// </summary>
    [JsonPropertyName("past365Days")]
    public double Past365Days { get; }

    /// <summary>
    /// Parameter past180Days of AverageStat
    /// </summary>
    [JsonPropertyName("past180Days")]
    public double Past180Days { get; }

    /// <summary>
    /// Parameter past90Days of AverageStat
    /// </summary>
    [JsonPropertyName("past90Days")]
    public double Past90Days { get; }

    /// <summary>
    /// Parameter past60Days of AverageStat
    /// </summary>
    [JsonPropertyName("past60Days")]
    public double Past60Days { get; }

    /// <summary>
    /// Parameter past30Days of AverageStat
    /// </summary>
    [JsonPropertyName("past30Days")]
    public double Past30Days { get; }

    /// <summary>
    /// Parameter past14Days of AverageStat
    /// </summary>
    [JsonPropertyName("past14Days")]
    public double Past14Days { get; }

    /// <summary>
    /// Parameter past7Days of AverageStat
    /// </summary>
    [JsonPropertyName("past7Days")]
    public double Past7Days { get; }

    /// <summary>
    /// Parameter pastDay of AverageStat
    /// </summary>
    [JsonPropertyName("pastDay")]
    public double PastDay { get; }
}
