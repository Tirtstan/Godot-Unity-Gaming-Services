namespace Unity.Services.Ugc;

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
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
    private static string EnvironmentId => AuthenticationService.Instance.EnvironmentId;

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
        Validate();

        var request = new RestRequest(
            $"/projects/{ProjectId}/environments/{EnvironmentId}/content/{getContentVersionsArgs.ContentId}/versions"
        )
        {
            RequestFormat = DataFormat.Json
        }
            .AddQueryParameter("offset", getContentVersionsArgs?.Offset ?? 0)
            .AddQueryParameter("limit", getContentVersionsArgs?.Limit ?? 25)
            .AddQueryParameter("includeTotal", getContentVersionsArgs?.IncludeTotal ?? false);

        if (getContentVersionsArgs?.SortBys != null)
        {
            foreach (var sort in getContentVersionsArgs?.SortBys)
                request.AddQueryParameter("sortBys", sort);
        }

        var response = await ugcClient.ExecuteAsync<PagedResults<InternalContentVersion>>(request);
        if (response.IsSuccessful)
        {
            return new PagedResults<ContentVersion>(
                response.Data.Offset,
                response.Data.Limit,
                response.Data.Total,
                response.Data.Results.ConvertAll(x => new ContentVersion(x))
            );
        }
        else
        {
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Get a specific content.
    /// </summary>
    /// <param name="getContentArgs">Contains all the parameters of the request</param>
    /// <returns>The requested content</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public async Task<Content> GetContentAsync(GetContentArgs getContentArgs)
    {
        Validate();

        var request = new RestRequest(
            $"/projects/{ProjectId}/environments/{EnvironmentId}/content/{getContentArgs.ContentId}"
        )
        {
            RequestFormat = DataFormat.Json
        }.AddQueryParameter("includeStatistics", getContentArgs.IncludeStatistics);

        var response = await ugcClient.ExecuteAsync<InternalContent>(request);
        if (response.IsSuccessful)
        {
            var content = new Content(response.Data);
            await DownloadContentDataAsync(content, getContentArgs.DownloadContent, getContentArgs.DownloadThumbnail);
            return content;
        }
        else
        {
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Download data and/or thumbnail of a content
    /// </summary>
    /// <param name="content">The content that will have its data downloaded</param>
    /// <param name="downloadContent">True if we want to download the content's data</param>
    /// <param name="downloadThumbnail">True if we want to download the content's thumbnail</param>
    /// <returns>The downloaded data will be put in the `content` parameter so this call returns nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public async Task DownloadContentDataAsync(Content content, bool downloadContent, bool downloadThumbnail)
    {
        Validate();

        if (downloadContent)
        {
            var request = new RestRequest(content.DownloadUrl) { RequestFormat = DataFormat.Json };

            var response = await ugcClient.ExecuteAsync(request);
            if (response.IsSuccessful)
                content.DownloadedContent = response?.RawBytes ?? null;
            else
                throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }

        if (downloadThumbnail && !string.IsNullOrEmpty(content.ThumbnailUrl))
        {
            var request = new RestRequest(content.ThumbnailUrl) { RequestFormat = DataFormat.Json };

            var response = await ugcClient.ExecuteAsync(request);
            if (response.IsSuccessful)
                content.DownloadedThumbnail = response?.RawBytes ?? null;
            else
                throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Create a new content in a project environment
    /// </summary>
    /// <param name="createContentArgs">Contains all the parameters of the request</param>
    /// <returns>The content that will be eventually available once the upload has been uploaded.</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public async Task<Content> CreateContentAsync(CreateContentArgs createContentArgs)
    {
        Validate();

        var request = new RestRequest($"/projects/{ProjectId}/environments/{EnvironmentId}/content", Method.Post)
        {
            RequestFormat = DataFormat.Json
        };

        var md5 = MD5.Create();
        string assetMD5Hash = GetHash(md5, createContentArgs.Asset);

        string thumbnailMd5Hash = null;
        if (createContentArgs.Thumbnail != null)
            thumbnailMd5Hash = GetHash(md5, createContentArgs.Thumbnail);

        request.AddJsonBody(
            new NewContentRequest(
                createContentArgs.Name,
                createContentArgs.Description,
                createContentArgs.CustomId,
                createContentArgs.IsPublic ? ContentVisibility.Public : ContentVisibility.Private,
                createContentArgs.TagIds,
                assetMD5Hash,
                thumbnailMd5Hash,
                createContentArgs.Metadata
            )
        );

        var response = await ugcClient.ExecuteAsync<UploadContentResponse>(request);
        if (response.IsSuccessful)
        {
            var content = new Content(response.Data.Content);
            await UploadContentDataAsync(response.Data, createContentArgs.Asset, createContentArgs.Thumbnail);
            return content;
        }
        else
        {
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    private async Task UploadContentDataAsync(UploadContentResponse contentResponse, Stream asset, Stream thumbnail)
    {
        var contentRequest = new RestRequest(contentResponse.UploadContentUrl, Method.Put)
        {
            RequestFormat = DataFormat.Json,
        }.AddFile("asset", GetBytes(asset), "", ContentType.Binary);

        foreach (var header in contentResponse.UploadContentHeaders)
        {
            foreach (var value in header.Value)
                contentRequest.AddHeader(header.Key, value);
        }

        var response = await ugcClient.ExecuteAsync(contentRequest);
        GD.Print(response.Content);
        if (!response.IsSuccessful)
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);

        if (thumbnail != null)
        {
            var thumbnailRequest = new RestRequest(contentResponse.UploadThumbnailUrl, Method.Put)
            {
                RequestFormat = DataFormat.Json
            }.AddFile("thumbnail", GetBytes(thumbnail), "", ContentType.Binary);

            foreach (var header in contentResponse.UploadThumbnailHeaders)
            {
                foreach (var value in header.Value)
                    thumbnailRequest.AddHeader(header.Key, value);
            }

            response = await ugcClient.ExecuteAsync(thumbnailRequest);
            if (!response.IsSuccessful)
                throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    private static string GetHash(MD5 md5, Stream stream)
    {
        stream.Position = 0;
        byte[] hash = md5.ComputeHash(stream);
        return Convert.ToBase64String(hash);
    }

    private static byte[] GetBytes(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
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
