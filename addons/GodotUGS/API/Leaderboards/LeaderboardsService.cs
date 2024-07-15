namespace Unity.Services.Leaderboards;

using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards.Models;

public partial class LeaderboardsService : Node
{
    public static LeaderboardsService Instance { get; private set; }

    private RestClient leaderboardsClient;
    private const string LeaderboardsURL = "https://leaderboards.services.api.unity.com/v1";
    private static string ProjectId => UnityServices.Instance.ProjectId;
    private static string PlayerId => AuthenticationService.Instance.PlayerId;

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        var options = new RestClientOptions(LeaderboardsURL)
        {
            Authenticator = new JwtAuthenticator(AuthenticationService.Instance.AccessToken)
        };
        leaderboardsClient = new RestClient(
            options,
            configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions { })
        );

        leaderboardsClient.AddDefaultHeaders(
            new Dictionary<string, string>
            {
                { "ProjectId", UnityServices.Instance.ProjectId },
                { "UnityEnvironment", UnityServices.Instance.Environment }
            }
        );
    }

    /// <summary>
    /// Gets a paginated list of entries for the specified leaderboard.
    /// </summary>
    /// <param name="leaderboardId">ID string of the leaderboard</param>
    /// <param name="options">Options object with "Offset" and "Limit" pagination options, and "IncludeMetadata" options.
    /// "Offset" is the number of entries to skip when retrieving the leaderboard scores, defaults to 0.
    /// "Limit" is the number of leaderboard scores to return, defaults to 10.
    /// "IncludeMetadata" is whether to return stored metadata, defaults to false.
    /// </param>
    /// <returns>Task for a Response object containing status code, headers, and Models.LeaderboardScoresPage object containing the paginated list of retrieved entries.</returns>
    public async Task<LeaderboardScoresPage> GetScoresAsync(string leaderboardId, PaginationOptions options = null)
    {
        var request = new RestRequest($"/projects/{ProjectId}/leaderboards/{leaderboardId}/scores")
            .AddQueryParameter("offset", options?.Offset ?? 0)
            .AddQueryParameter("limit", options?.Limit ?? 10)
            .AddQueryParameter("includeMetadata", options?.IncludeMetadata ?? false);
        request.RequestFormat = DataFormat.Json;

        var response = await leaderboardsClient.ExecuteAsync<LeaderboardScoresPage>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw response.ErrorException;
    }

    /// <summary>
    /// Gets a paginated list of entries for the specified leaderboard within the specified tier.
    /// </summary>
    /// <param name="leaderboardId">ID string of the leaderboard</param>
    /// <param name="tierId">ID string of the tier</param>
    /// <param name="options">Options object with "Offset" and "Limit" pagination options, and "IncludeMetadata" options.
    /// "Offset" is the number of entries to skip when retrieving the leaderboard scores, defaults to 0.
    /// "Limit" is the number of leaderboard scores to return, defaults to 10.
    /// "IncludeMetadata" is whether to return stored metadata, defaults to false.
    /// </param>
    /// <returns>Task for a Response object containing status code, headers, and Models.LeaderboardTierScoresPage object containing the paginated list of retrieved entries.</returns>
    public async Task<LeaderboardTierScoresPage> GetScoresByTierAsync(
        string leaderboardId,
        string tierId,
        PaginationOptions options = null
    )
    {
        var request = new RestRequest($"/projects/{ProjectId}/leaderboards/{leaderboardId}/tiers/{tierId}/scores")
            .AddQueryParameter("offset", options?.Offset ?? 0)
            .AddQueryParameter("limit", options?.Limit ?? 10)
            .AddQueryParameter("includeMetadata", options?.IncludeMetadata ?? false);
        request.RequestFormat = DataFormat.Json;

        var response = await leaderboardsClient.ExecuteAsync<LeaderboardTierScoresPage>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw response.ErrorException;
    }

    /// <summary>
    /// Gets a list of entries from the specified leaderboard for the specified player IDs.
    /// </summary>
    /// <param name="leaderboardId">ID string of the leaderboard</param>
    /// <param name="playerIds">List of player IDs to get entries for</param>
    /// <param name="options">Options object with "IncludeMetadata", whether to retrieve stored metadata (defaults to false).</param>
    /// <returns>Task for a Response object containing status code, headers, and Models.LeaderboardScores object containing the list of retrieved entries.</returns>
    public async Task<LeaderboardScoresWithNotFoundPlayerIds> GetScoresByPlayerIdsAsync(
        string leaderboardId,
        List<string> playerIds,
        IncludeMetadataOptions options = null
    )
    {
        var request = new RestRequest($"/projects/{ProjectId}/leaderboards/{leaderboardId}/scores/players", Method.Post)
            .AddQueryParameter("includeMetadata", options?.IncludeMetadata ?? false)
            .AddJsonBody(new { playerIds });

        var response = await leaderboardsClient.ExecuteAsync<LeaderboardScoresWithNotFoundPlayerIds>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw response.ErrorException;
    }

    /// <summary>
    /// Gets a list of entries for the specified players by player ID from the specified leaderboard archive version.
    /// </summary>
    /// <param name="leaderboardId">ID string of the leaderboard</param>
    /// <param name="versionId">ID string of the leaderboard archive version</param>
    /// <param name="playerIds">List of player IDs to get scores for</param>
    /// <param name="options">Options object with "IncludeMetadata", whether to retrieve stored metadata (defaults to false).</param>
    /// <returns>Task for a Response object containing status code, headers, and Models.LeaderboardVersionScoresByPlayerIds object containing the list of retrieved entries.</returns>
    public async Task<LeaderboardVersionScoresWithNotFoundPlayerIds> GetVersionScoresByPlayerIdsAsync(
        string leaderboardId,
        string versionId,
        List<string> playerIds,
        IncludeMetadataOptions options = null
    )
    {
        var request = new RestRequest(
            $"/projects/{ProjectId}/leaderboards/{leaderboardId}/versions/{versionId}/scores/players",
            Method.Post
        )
            .AddQueryParameter("includeMetadata", options?.IncludeMetadata ?? false)
            .AddJsonBody(new { playerIds });

        var response = await leaderboardsClient.ExecuteAsync<LeaderboardVersionScoresWithNotFoundPlayerIds>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw response.ErrorException;
    }

    /// <summary>
    /// Gets the entries of the current player as well as the specified number of neighboring players ranked either side of the player.
    /// </summary>
    /// <param name="leaderboardId">ID string of the leaderboard</param>
    /// <param name="options">Options object with "RangeLimit", the number of entries either side of the player to retrieve (defaults to 5), and IncludeMetadata, whether to retrieve stored metadata (defaults to false).</param>
    /// <returns>Task for a Response object containing status code, headers, and Models.LeaderboardScores object containing the list of retrieved entries.</returns>
    public async Task<LeaderboardScores> GetPlayerRangeAsync(string leaderboardId, RangeOptions options = null)
    {
        var request = new RestRequest(
            $"/projects/{ProjectId}/leaderboards/{leaderboardId}/scores/players/{PlayerId}/range"
        )
            .AddQueryParameter("rangeLimit", options?.RangeLimit ?? 5)
            .AddQueryParameter("includeMetadata", options?.IncludeMetadata ?? false);
        request.RequestFormat = DataFormat.Json;

        var response = await leaderboardsClient.ExecuteAsync<LeaderboardScores>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw response.ErrorException;
    }

    /// <summary>
    /// Gets the entries of the current player as well as the specified number of neighboring players ranked
    /// either side of the player in the specified leaderboard archive version.
    /// </summary>
    /// <param name="leaderboardId">ID string of the leaderboard</param>
    /// <param name="versionId">ID string of the leaderboard archive version</param>
    /// <param name="options">Options object with "RangeLimit", the number of entries either side of the player to retrieve (defaults to 5), and IncludeMetadata, whether to retrieve stored metadata (defaults to false).</param>
    /// <returns>Task for a Response object containing status code, headers, and Models.LeaderboardVersionScores object containing the list of retrieved entries.</returns>
    public async Task<LeaderboardVersionScores> GetVersionPlayerRangeAsync(
        string leaderboardId,
        string versionId,
        RangeOptions options = null
    )
    {
        var request = new RestRequest(
            $"/projects/{ProjectId}/leaderboards/{leaderboardId}/versions/{versionId}/scores/players/{PlayerId}/range"
        )
            .AddQueryParameter("rangeLimit", options?.RangeLimit ?? 5)
            .AddQueryParameter("includeMetadata", options?.IncludeMetadata ?? false);
        request.RequestFormat = DataFormat.Json;

        var response = await leaderboardsClient.ExecuteAsync<LeaderboardVersionScores>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw response.ErrorException;
    }

    /// <summary>
    /// Gets the entry for the current player in the specified leaderboard.
    /// </summary>
    /// <param name="leaderboardId">ID string of the leaderboard</param>
    /// <param name="options">Options object with "IncludeMetadata", whether to retrieve stored metadata (defaults to false).</param>
    /// <returns>Task for a Response object containing status code, headers, and Models.LeaderboardEntry object containing the retrieved entry.</returns>
    public async Task<LeaderboardEntry> GetPlayerScoreAsync(string leaderboardId, IncludeMetadataOptions options = null)
    {
        var request = new RestRequest(
            $"/projects/{ProjectId}/leaderboards/{leaderboardId}/scores/players/{PlayerId}"
        ).AddQueryParameter("includeMetadata", options?.IncludeMetadata ?? false);
        request.RequestFormat = DataFormat.Json;

        var response = await leaderboardsClient.ExecuteAsync<LeaderboardEntry>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw response.ErrorException;
    }

    /// <summary>
    /// Adds or updates an entry for the current player in the specified leaderboard.
    /// </summary>
    /// <param name="leaderboardId">ID string of the leaderboard</param>
    /// <param name="score">Score value to be submitted</param>
    /// <param name="options">Options object with "Metadata", an object containing metadata to be stored alongside the score (defaults to null).</param>
    /// <returns>Task for a Response object containing status code, headers, and Models.LeaderboardEntry object containing the added or updated entry.</returns>
    public async Task<LeaderboardEntry> AddPlayerScoreAsync(
        string leaderboardId,
        double score,
        AddPlayerScoreOptions options = null
    )
    {
        var request = new RestRequest(
            $"/projects/{ProjectId}/leaderboards/{leaderboardId}/scores/players/{PlayerId}",
            Method.Post
        ).AddJsonBody(
            new
            {
                score,
                metadata = options?.Metadata,
                versionId = options?.VersionId
            }
        );

        var response = await leaderboardsClient.ExecuteAsync<LeaderboardEntry>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw response.ErrorException;
    }

    /// <summary>
    /// Gets the list of archived leaderboard versions for the specified leaderboard.
    /// </summary>
    /// <param name="leaderboardId">ID string of the leaderboard</param>
    /// <param name="options">Options object with "Limit", the number of entries to return (starting with the most recent). Defaults to all entries</param>
    /// <returns>Task for a Response object containing status code, headers, and Models.LeaderboardVersions object containing the list of retrieved versions.</returns>
    public async Task<LeaderboardVersions> GetVersionsAsync(string leaderboardId, GetVersionsOptions options = null)
    {
        var request = new RestRequest($"/projects/{ProjectId}/leaderboards/{leaderboardId}/versions").AddQueryParameter(
            "limit",
            options?.Limit ?? 10
        );
        request.RequestFormat = DataFormat.Json;

        var response = await leaderboardsClient.ExecuteAsync<LeaderboardVersions>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw response.ErrorException;
    }

    /// <summary>
    /// Gets a paginated list of entries for the specified leaderboard archive version.
    /// </summary>
    /// <param name="leaderboardId">ID string of the leaderboard</param>
    /// <param name="versionId">ID string of the leaderboard archive version</param>
    /// <param name="options">Options object with "Offset" and "Limit" pagination options, and "IncludeMetadata" options.
    /// "Offset" is the number of entries to skip when retrieving the leaderboard scores, defaults to 0.
    /// "Limit" is the number of leaderboard scores to return, defaults to 10.
    /// "IncludeMetadata" is whether to return stored metadata, defaults to false.
    /// </param>
    /// <returns>Task for a Response object containing status code, headers, and Models.LeaderboardVersionScoresPage object containing the paginated list of retrieved entries.</returns>
    public async Task<LeaderboardVersionScoresPage> GetVersionScoresAsync(
        string leaderboardId,
        string versionId,
        PaginationOptions options = null
    )
    {
        var request = new RestRequest($"/projects/{ProjectId}/leaderboards/{leaderboardId}/versions/{versionId}/scores")
            .AddQueryParameter("offset", options?.Offset ?? 0)
            .AddQueryParameter("limit", options?.Limit ?? 10)
            .AddQueryParameter("includeMetadata", options?.IncludeMetadata ?? false);
        request.RequestFormat = DataFormat.Json;

        var response = await leaderboardsClient.ExecuteAsync<LeaderboardVersionScoresPage>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw response.ErrorException;
    }

    /// <summary>
    /// Gets a paginated list of entries from the specified leaderboard archive version and within the specified tier.
    /// </summary>
    /// <param name="leaderboardId">ID string of the leaderboard</param>
    /// <param name="versionId">ID string of the leaderboard archive version</param>
    /// <param name="tierId">ID string of the tier</param>
    /// <param name="options">Options object with "Offset" and "Limit" pagination options, and "IncludeMetadata" options.
    /// "Offset" is the number of entries to skip when retrieving the leaderboard scores, defaults to 0.
    /// "Limit" is the number of leaderboard scores to return, defaults to 10.
    /// "IncludeMetadata" is whether to return stored metadata, defaults to false.
    /// </param>
    /// <returns>Task for a Response object containing status code, headers, and Models.LeaderboardVersionTierScoresPage object containing the paginated list of retrieved entries.</returns>
    public async Task<LeaderboardVersionTierScoresPage> GetVersionScoresByTierAsync(
        string leaderboardId,
        string versionId,
        string tierId,
        PaginationOptions options = null
    )
    {
        var request = new RestRequest(
            $"/projects/{ProjectId}/leaderboards/{leaderboardId}/versions/{versionId}/tiers/{tierId}/scores"
        )
            .AddQueryParameter("offset", options?.Offset ?? 0)
            .AddQueryParameter("limit", options?.Limit ?? 10)
            .AddQueryParameter("includeMetadata", options?.IncludeMetadata ?? false);
        request.RequestFormat = DataFormat.Json;

        var response = await leaderboardsClient.ExecuteAsync<LeaderboardVersionTierScoresPage>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw response.ErrorException;
    }

    /// <summary>
    /// Gets the entry for the current player in the specified leaderboard archive version.
    /// </summary>
    /// <param name="leaderboardId">ID string of the leaderboard</param>
    /// <param name="versionId">ID string of the leaderboard archive version</param>
    /// <param name="options">Options object with "IncludeMetadata", whether to retrieve stored metadata (defaults to false).</param>
    /// <returns>Task for a Response object containing status code, headers, and Models.LeaderboardVersionEntry object containing the retrieved entry.</returns>
    public async Task<LeaderboardVersionEntry> GetVersionPlayerScoreAsync(
        string leaderboardId,
        string versionId,
        IncludeMetadataOptions options = null
    )
    {
        var request = new RestRequest(
            $"/projects/{ProjectId}/leaderboards/{leaderboardId}/versions/{versionId}/scores/players/{PlayerId}"
        ).AddQueryParameter("includeMetadata", options?.IncludeMetadata ?? false);
        request.RequestFormat = DataFormat.Json;

        var response = await leaderboardsClient.ExecuteAsync<LeaderboardVersionEntry>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw response.ErrorException;
    }
}
