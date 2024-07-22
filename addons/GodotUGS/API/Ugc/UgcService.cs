namespace Unity.Services.Ugc;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Ugc.Internal.Models;
using Unity.Services.Ugc.Models;

public partial class UgcService : Node
{
    public static UgcService Instance { get; private set; }

    private RestClient ugcClient;
    private const string UgcURL = "https://ugc.services.api.unity.com/v1";
    private static string ProjectId => UnityServices.Instance.ProjectId;
    private static string PlayerId => AuthenticationService.Instance.PlayerId;
    private static string EnvironmentId => UnityServices.Instance.Environment;

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        AuthenticationService.Instance.SignedIn += OnSignIn;
    }

    private void OnSignIn()
    {
        var options = new RestClientOptions(UgcURL)
        {
            Authenticator = new JwtAuthenticator(AuthenticationService.Instance.AccessToken)
        };
        ugcClient = new RestClient(
            options,
            configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions { })
        );

        ugcClient.AddDefaultHeaders(UnityServices.Instance.DefaultHeaders);
    }

    /// <summary>
    /// Get all content from a project specific environment.
    /// Content with visibility set to Hidden or with ModerationStatus different from Approved won't be returned.
    /// Deleted contents and ones that haven't finished uploading won't be returned either.
    /// </summary>
    /// <param name="getContentsArgs">The details of the search request</param>
    /// <returns>A list of contents from the environment with pagination information</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public async Task<PagedResults<Content>> GetContentsAsync(GetContentsArgs getContentsArgs = null)
    {
        Validate();

        var request = new RestRequest($"/projects/{ProjectId}/environments/{EnvironmentId}/content/search")
        {
            RequestFormat = DataFormat.Json
        }
            .AddQueryParameter("offset", getContentsArgs?.Offset ?? 0)
            .AddQueryParameter("limit", getContentsArgs?.Limit ?? 25)
            .AddQueryParameter("includeTotal", getContentsArgs?.IncludeTotal ?? false)
            .AddQueryParameter("includeStatistics", getContentsArgs?.IncludeStatistics ?? false);

        if (getContentsArgs?.SortBys != null)
        {
            foreach (var sort in getContentsArgs?.SortBys)
                request.AddQueryParameter("sortBys", sort);
        }

        if (!string.IsNullOrEmpty(getContentsArgs?.Search))
            request.AddQueryParameter("search", getContentsArgs?.Search);

        if (getContentsArgs?.Tags != null)
        {
            foreach (var tag in getContentsArgs?.Tags)
                request.AddQueryParameter("tags", tag);
        }

        if (getContentsArgs?.Filters != null)
        {
            foreach (var filter in getContentsArgs?.Filters)
                request.AddQueryParameter("filters", filter);
        }

        var response = await ugcClient.ExecuteAsync<PagedResults<InternalContent>>(request);
        if (response.IsSuccessful)
        {
            return new PagedResults<Content>(
                response.Data.Offset,
                response.Data.Limit,
                response.Data.Total,
                response.Data.Results.ConvertAll(x => new Content(x))
            );
        }
        else
        {
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Get all content created by the current signed in player
    /// </summary>
    /// <param name="getPlayerContentsArgs">The details of the search request</param>
    /// <returns>A list of contents from the environment with pagination information</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>

    public async Task<PagedResults<Content>> GetPlayerContentsAsync(GetPlayerContentsArgs getPlayerContentsArgs = null)
    {
        Validate();

        var request = new RestRequest($"/content/search") { RequestFormat = DataFormat.Json }
            .AddQueryParameter("offset", getPlayerContentsArgs?.Offset ?? 0)
            .AddQueryParameter("limit", getPlayerContentsArgs?.Limit ?? 25)
            .AddQueryParameter("includeTotal", getPlayerContentsArgs?.IncludeTotal ?? false)
            .AddQueryParameter("includeStatistics", getPlayerContentsArgs?.IncludeStatistics ?? false);

        if (getPlayerContentsArgs?.SortBys != null)
        {
            foreach (var sort in getPlayerContentsArgs?.SortBys)
                request.AddQueryParameter("sortBys", sort);
        }

        if (!string.IsNullOrEmpty(getPlayerContentsArgs?.Search))
            request.AddQueryParameter("search", getPlayerContentsArgs.Search);

        if (getPlayerContentsArgs?.Filters != null)
        {
            foreach (var filter in getPlayerContentsArgs?.Filters)
                request.AddQueryParameter("filters", filter);
        }

        var response = await ugcClient.ExecuteAsync<PagedResults<InternalContent>>(request);
        if (response.IsSuccessful)
        {
            return new PagedResults<Content>(
                response.Data.Offset,
                response.Data.Limit,
                response.Data.Total,
                response.Data.Results.ConvertAll(x => new Content(x))
            );
        }
        else
        {
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Get content by <see cref="ContentTrendType"/>
    /// </summary>
    /// <param name="getContentTrendsArgs">The details of the search request</param>
    /// <returns>A list of contents of this trend type, with pagination information</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public async Task<PagedResults<Content>> GetContentTrendsAsync(GetContentTrendsArgs getContentTrendsArgs = null)
    {
        Validate();

        var request = new RestRequest(
            $"/projects/{ProjectId}/environments/{EnvironmentId}/content/trends/{getContentTrendsArgs.TrendType}"
        )
        {
            RequestFormat = DataFormat.Json
        }
            .AddQueryParameter("offset", getContentTrendsArgs?.Offset ?? 0)
            .AddQueryParameter("limit", getContentTrendsArgs?.Limit ?? 25)
            .AddQueryParameter("includeTotal", getContentTrendsArgs?.IncludeTotal ?? false);

        if (!string.IsNullOrEmpty(getContentTrendsArgs?.GetSortBy()))
            request.AddQueryParameter("sortBys", getContentTrendsArgs?.GetSortBy());

        var response = await ugcClient.ExecuteAsync<PagedResults<InternalContent>>(request);
        if (response.IsSuccessful)
        {
            return new PagedResults<Content>(
                response.Data.Offset,
                response.Data.Limit,
                response.Data.Total,
                response.Data.Results.ConvertAll(x => new Content(x))
            );
        }
        else
        {
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Get content versions of a content.
    /// </summary>
    /// <param name="getContentVersionsArgs">Contains all the parameters of the request</param>
    /// <returns>The list of versions associated with the content with pagination information</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public async Task<PagedResults<ContentVersion>> GetContentVersionsAsync(
        GetContentVersionsArgs getContentVersionsArgs
    )
    {
        return null;
    }

    private static void Validate()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
            throw new InvalidOperationException("User must be signed in to use the UGC service.");
    }

    public override void _ExitTree()
    {
        AuthenticationService.Instance.SignedIn -= OnSignIn;
    }
}
