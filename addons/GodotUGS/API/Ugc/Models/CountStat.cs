namespace Unity.Services.Ugc.Models;

using System.Text.Json.Serialization;

/// <summary>
/// CountStat model
/// </summary>
public class CountStat
{
    /// <summary>
    /// Creates an instance of CountStat.
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
    public CountStat(
        int allTime = default,
        int past365Days = default,
        int past180Days = default,
        int past90Days = default,
        int past60Days = default,
        int past30Days = default,
        int past14Days = default,
        int past7Days = default,
        int pastDay = default
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
    /// Parameter allTime of CountStat
    /// </summary>
    [JsonPropertyName("allTime")]
    public int AllTime { get; }

    /// <summary>
    /// Parameter past365Days of CountStat
    /// </summary>
    [JsonPropertyName("past365Days")]
    public int Past365Days { get; }

    /// <summary>
    /// Parameter past180Days of CountStat
    /// </summary>
    [JsonPropertyName("past180Days")]
    public int Past180Days { get; }

    /// <summary>
    /// Parameter past90Days of CountStat
    /// </summary>
    [JsonPropertyName("past90Days")]
    public int Past90Days { get; }

    /// <summary>
    /// Parameter past60Days of CountStat
    /// </summary>
    [JsonPropertyName("past60Days")]
    public int Past60Days { get; }

    /// <summary>
    /// Parameter past30Days of CountStat
    /// </summary>
    [JsonPropertyName("past30Days")]
    public int Past30Days { get; }

    /// <summary>
    /// Parameter past14Days of CountStat
    /// </summary>
    [JsonPropertyName("past14Days")]
    public int Past14Days { get; }

    /// <summary>
    /// Parameter past7Days of CountStat
    /// </summary>
    [JsonPropertyName("past7Days")]
    public int Past7Days { get; }

    /// <summary>
    /// Parameter pastDay of CountStat
    /// </summary>
    [JsonPropertyName("pastDay")]
    public int PastDay { get; }
}
