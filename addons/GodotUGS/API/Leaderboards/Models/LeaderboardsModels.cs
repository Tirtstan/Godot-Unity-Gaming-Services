using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

// all classes provided by Unity

namespace Unity.Services.Leaderboards.Models;

/// <summary>
/// LeaderboardEntry model
/// </summary>
public class LeaderboardEntry
{
    /// <summary>
    /// Creates an instance of LeaderboardEntry.
    /// </summary>
    /// <param name="playerId">playerId param</param>
    /// <param name="playerName">playerName param</param>
    /// <param name="rank">rank param</param>
    /// <param name="score">score param</param>
    /// <param name="tier">tier param</param>
    /// <param name="updatedTime">updatedTime param</param>
    /// <param name="metadata">metadata param</param>
    public LeaderboardEntry(
        string playerId,
        string playerName,
        int rank,
        double score,
        string tier = default,
        DateTime updatedTime = default,
        string metadata = null
    )
    {
        PlayerId = playerId;
        PlayerName = playerName;
        Rank = rank;
        Score = score;
        Tier = tier;
        UpdatedTime = updatedTime;
        Metadata = metadata;
    }

    /// <summary>
    /// Parameter playerId of LeaderboardEntry
    /// </summary>
    public string PlayerId { get; }

    /// <summary>
    /// Parameter playerName of LeaderboardEntry
    /// </summary>
    public string PlayerName { get; }

    /// <summary>
    /// Parameter rank of LeaderboardEntry
    /// </summary>
    public int Rank { get; }

    /// <summary>
    /// Parameter score of LeaderboardEntry
    /// </summary>
    public double Score { get; }

    /// <summary>
    /// Parameter tier of LeaderboardEntry
    /// </summary>
    public string Tier { get; }

    /// <summary>
    /// Parameter updatedTime of LeaderboardEntry
    /// </summary>
    public DateTime UpdatedTime { get; }

    /// <summary>
    /// Parameter metadata of LeaderboardEntry
    /// </summary>
    public string Metadata { get; }
}

/// <summary>
/// LeaderboardScoresPage model
/// </summary>
public class LeaderboardScoresPage
{
    /// <summary>
    /// Creates an instance of LeaderboardScoresPage.
    /// </summary>
    /// <param name="offset">offset param</param>
    /// <param name="limit">limit param</param>
    /// <param name="total">total param</param>
    /// <param name="results">results param</param>
    public LeaderboardScoresPage(
        int offset = default,
        int limit = default,
        int total = default,
        List<LeaderboardEntry> results = default
    )
    {
        Offset = offset;
        Limit = limit;
        Total = total;
        Results = results;
    }

    /// <summary>
    /// Parameter offset of LeaderboardScoresPage
    /// </summary>
    public int Offset { get; }

    /// <summary>
    /// Parameter limit of LeaderboardScoresPage
    /// </summary>
    public int Limit { get; }

    /// <summary>
    /// Parameter total of LeaderboardScoresPage
    /// </summary>
    public int Total { get; }

    /// <summary>
    /// Parameter results of LeaderboardScoresPage
    /// </summary>
    public List<LeaderboardEntry> Results { get; }
}

/// <summary>
/// LeaderboardScores model
/// </summary>
public class LeaderboardScores
{
    /// <summary>
    /// Creates an instance of LeaderboardScores.
    /// </summary>
    /// <param name="results">results param</param>
    public LeaderboardScores(List<LeaderboardEntry> results = default)
    {
        Results = results;
    }

    /// <summary>
    /// Parameter results of LeaderboardScores
    /// </summary>
    public List<LeaderboardEntry> Results { get; }
}

/// <summary>
/// LeaderboardTierScoresPage model
/// </summary>
public class LeaderboardTierScoresPage
{
    /// <summary>
    /// Creates an instance of LeaderboardTierScoresPage.
    /// </summary>
    /// <param name="tier">tier param</param>
    /// <param name="offset">offset param</param>
    /// <param name="limit">limit param</param>
    /// <param name="total">total param</param>
    /// <param name="results">results param</param>
    public LeaderboardTierScoresPage(
        string tier = default,
        int offset = default,
        int limit = default,
        int total = default,
        List<LeaderboardEntry> results = default
    )
    {
        Tier = tier;
        Offset = offset;
        Limit = limit;
        Total = total;
        Results = results;
    }

    /// <summary>
    /// Parameter tier of LeaderboardTierScoresPage
    /// </summary>
    public string Tier { get; }

    /// <summary>
    /// Parameter offset of LeaderboardTierScoresPage
    /// </summary>
    public int Offset { get; }

    /// <summary>
    /// Parameter limit of LeaderboardTierScoresPage
    /// </summary>
    public int Limit { get; }

    /// <summary>
    /// Parameter total of LeaderboardTierScoresPage
    /// </summary>
    public int Total { get; }

    /// <summary>
    /// Parameter results of LeaderboardTierScoresPage
    /// </summary>
    public List<LeaderboardEntry> Results { get; }
}

/// <summary>
/// LeaderboardScoresWithNotFoundPlayerIds model
/// </summary>
public class LeaderboardScoresWithNotFoundPlayerIds
{
    /// <summary>
    /// Creates an instance of LeaderboardScoresWithNotFoundPlayerIds.
    /// </summary>
    /// <param name="results">results param</param>
    /// <param name="playerIds">entriesNotFoundForPlayerIds param</param>
    public LeaderboardScoresWithNotFoundPlayerIds(
        List<LeaderboardEntry> results = default,
        List<string> entriesNotFoundForPlayerIds = default
    )
    {
        Results = results;
        EntriesNotFoundForPlayerIds = entriesNotFoundForPlayerIds;
    }

    /// <summary>
    /// Parameter results of LeaderboardScoresWithNotFoundPlayerIds
    /// </summary>
    public List<LeaderboardEntry> Results { get; }

    /// <summary>
    /// Parameter entriesNotFoundForPlayerIds of LeaderboardScoresWithNotFoundPlayerIds
    /// </summary>
    [JsonPropertyName("entriesNotFoundForPlayerIds")]
    public List<string> EntriesNotFoundForPlayerIds { get; }
}

/// <summary>
/// LeaderboardVersionScores model
/// </summary>
public class LeaderboardVersionScores
{
    /// <summary>
    /// Creates an instance of LeaderboardVersionScores.
    /// </summary>
    /// <param name="version">version param</param>
    /// <param name="results">results param</param>
    public LeaderboardVersionScores(LeaderboardVersion version = default, List<LeaderboardEntry> results = default)
    {
        Version = version;
        Results = results;
    }

    /// <summary>
    /// Parameter version of LeaderboardVersionScores
    /// </summary>
    public LeaderboardVersion Version { get; }

    /// <summary>
    /// Parameter results of LeaderboardVersionScores
    /// </summary>
    public List<LeaderboardEntry> Results { get; }
}

/// <summary>
/// LeaderboardVersionScoresWithNotFoundPlayerIds model
/// </summary>
public class LeaderboardVersionScoresWithNotFoundPlayerIds
{
    /// <summary>
    /// Creates an instance of LeaderboardVersionScoresWithNotFoundPlayerIds.
    /// </summary>
    /// <param name="version">version param</param>
    /// <param name="results">results param</param>
    /// <param name="playerIds">entriesNotFoundForPlayerIds param</param>
    public LeaderboardVersionScoresWithNotFoundPlayerIds(
        LeaderboardVersion version = default,
        List<LeaderboardEntry> results = default,
        List<string> playerIds = default
    )
    {
        Version = version;
        Results = results;
        EntriesNotFoundForPlayerIds = playerIds;
    }

    /// <summary>
    /// Parameter version of LeaderboardVersionScoresWithNotFoundPlayerIds
    /// </summary>
    public LeaderboardVersion Version { get; }

    /// <summary>
    /// Parameter results of LeaderboardVersionScoresWithNotFoundPlayerIds
    /// </summary>
    public List<LeaderboardEntry> Results { get; }

    /// <summary>
    /// Parameter entriesNotFoundForPlayerIds of LeaderboardVersionScoresWithNotFoundPlayerIds
    /// </summary>
    public List<string> EntriesNotFoundForPlayerIds { get; }
}

/// <summary>
/// LeaderboardVersionEntry model
/// </summary>
public class LeaderboardVersionEntry
{
    /// <summary>
    /// Creates an instance of LeaderboardVersionEntry.
    /// </summary>
    /// <param name="playerId">playerId param</param>
    /// <param name="playerName">playerName param</param>
    /// <param name="rank">rank param</param>
    /// <param name="score">score param</param>
    /// <param name="version">version param</param>
    public LeaderboardVersionEntry(
        string playerId,
        string playerName,
        int rank,
        double score,
        LeaderboardVersion version = default
    )
    {
        Version = version;
        PlayerId = playerId;
        PlayerName = playerName;
        Rank = rank;
        Score = score;
    }

    /// <summary>
    /// Parameter version of LeaderboardVersionEntry
    /// </summary>
    public LeaderboardVersion Version { get; }

    /// <summary>
    /// Parameter playerId of LeaderboardVersionEntry
    /// </summary>
    public string PlayerId { get; }

    /// <summary>
    /// Parameter playerName of LeaderboardVersionEntry
    /// </summary>
    public string PlayerName { get; }

    /// <summary>
    /// Parameter rank of LeaderboardVersionEntry
    /// </summary>
    public int Rank { get; }

    /// <summary>
    /// Parameter score of LeaderboardVersionEntry
    /// </summary>
    public double Score { get; }
}

/// <summary>
/// LeaderboardVersion model
/// </summary>
public class LeaderboardVersion
{
    /// <summary>
    /// Creates an instance of LeaderboardVersion.
    /// </summary>
    /// <param name="id">id param</param>
    /// <param name="start">start param</param>
    /// <param name="end">end param</param>
    public LeaderboardVersion(string id = default, DateTime start = default, DateTime end = default)
    {
        Id = id;
        Start = start;
        End = end;
    }

    /// <summary>
    /// Parameter id of LeaderboardVersion
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Parameter start of LeaderboardVersion
    /// </summary>
    public DateTime Start { get; }

    /// <summary>
    /// Parameter end of LeaderboardVersion
    /// </summary>
    public DateTime End { get; }
}

/// <summary>
/// LeaderboardVersionScoresPage model
/// </summary>

public class LeaderboardVersionScoresPage
{
    /// <summary>
    /// Creates an instance of LeaderboardVersionScoresPage.
    /// </summary>
    /// <param name="version">version param</param>
    /// <param name="offset">offset param</param>
    /// <param name="limit">limit param</param>
    /// <param name="total">total param</param>
    /// <param name="results">results param</param>
    public LeaderboardVersionScoresPage(
        LeaderboardVersion version = default,
        int offset = default,
        int limit = default,
        int total = default,
        List<LeaderboardEntry> results = default
    )
    {
        Version = version;
        Offset = offset;
        Limit = limit;
        Total = total;
        Results = results;
    }

    /// <summary>
    /// Parameter version of LeaderboardVersionScoresPage
    /// </summary>
    public LeaderboardVersion Version { get; }

    /// <summary>
    /// Parameter offset of LeaderboardVersionScoresPage
    /// </summary>
    public int Offset { get; }

    /// <summary>
    /// Parameter limit of LeaderboardVersionScoresPage
    /// </summary>
    public int Limit { get; }

    /// <summary>
    /// Parameter total of LeaderboardVersionScoresPage
    /// </summary>
    public int Total { get; }

    /// <summary>
    /// Parameter results of LeaderboardVersionScoresPage
    /// </summary>
    public List<LeaderboardEntry> Results { get; }
}

/// <summary>
/// LeaderboardVersionTierScoresPage model
/// </summary>
public class LeaderboardVersionTierScoresPage
{
    /// <summary>
    /// Creates an instance of LeaderboardVersionTierScoresPage.
    /// </summary>
    /// <param name="version">version param</param>
    /// <param name="tier">tier param</param>
    /// <param name="offset">offset param</param>
    /// <param name="limit">limit param</param>
    /// <param name="total">total param</param>
    /// <param name="results">results param</param>
    public LeaderboardVersionTierScoresPage(
        LeaderboardVersion version = default,
        string tier = default,
        int offset = default,
        int limit = default,
        int total = default,
        List<LeaderboardEntry> results = default
    )
    {
        Version = version;
        Tier = tier;
        Offset = offset;
        Limit = limit;
        Total = total;
        Results = results;
    }

    /// <summary>
    /// Parameter version of LeaderboardVersionTierScoresPage
    /// </summary>
    public LeaderboardVersion Version { get; }

    /// <summary>
    /// Parameter tier of LeaderboardVersionTierScoresPage
    /// </summary>
    public string Tier { get; }

    /// <summary>
    /// Parameter offset of LeaderboardVersionTierScoresPage
    /// </summary>
    public int Offset { get; }

    /// <summary>
    /// Parameter limit of LeaderboardVersionTierScoresPage
    /// </summary>
    public int Limit { get; }

    /// <summary>
    /// Parameter total of LeaderboardVersionTierScoresPage
    /// </summary>
    public int Total { get; }

    /// <summary>
    /// Parameter results of LeaderboardVersionTierScoresPage
    /// </summary>
    public List<LeaderboardEntry> Results { get; }
}

/// <summary>
/// LeaderboardVersions model
/// </summary>
public class LeaderboardVersions
{
    /// <summary>
    /// Creates an instance of LeaderboardVersions.
    /// </summary>
    /// <param name="leaderboardId">leaderboardId param</param>
    /// <param name="results">results param</param>
    /// <param name="nextReset">nextReset param</param>
    /// <param name="versionId">versionId param</param>
    /// <param name="totalArchivedVersions">totalArchivedVersions param</param>
    public LeaderboardVersions(
        string leaderboardId = default,
        List<LeaderboardVersion> results = default,
        DateTime nextReset = default,
        string versionId = default,
        int totalArchivedVersions = default
    )
    {
        LeaderboardId = leaderboardId;
        Results = results;
        NextReset = nextReset;
        VersionId = versionId;
        TotalArchivedVersions = totalArchivedVersions;
    }

    /// <summary>
    /// Parameter leaderboardId of LeaderboardVersions
    /// </summary>
    public string LeaderboardId { get; }

    /// <summary>
    /// Parameter results of LeaderboardVersions
    /// </summary>
    public List<LeaderboardVersion> Results { get; }

    /// <summary>
    /// Parameter nextReset of LeaderboardVersions
    /// </summary>
    public DateTime NextReset { get; }

    /// <summary>
    /// Parameter versionId of LeaderboardVersions
    /// </summary>
    public string VersionId { get; }

    /// <summary>
    /// Parameter totalArchivedVersions of LeaderboardVersions
    /// </summary>
    public int TotalArchivedVersions { get; }
}
