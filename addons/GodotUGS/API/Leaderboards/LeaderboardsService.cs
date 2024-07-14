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
    public async Task<LeaderboardScoresPage> GetScoresAsync(string leaderboardId, GetScoresOptions options = null)
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
        GetScoresOptions options = null
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
        GetScoresOptions options = null
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

    public override void _ExitTree() { }
}

public class GetScoresOptions : PaginationOptions { }
