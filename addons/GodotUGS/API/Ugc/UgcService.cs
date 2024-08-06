namespace Unity.Services.Ugc;

using System;
using System.Collections.Generic;
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

public interface IUgcService
{
    /// <summary>
    /// Get all content from a project specific environment.
    /// Content with visibility set to Hidden or with ModerationStatus different from Approved won't be returned.
    /// Deleted contents and ones that haven't finished uploading won't be returned either.
    /// </summary>
    /// <param name="getContentsArgs">The details of the search request</param>
    /// <returns>A list of contents from the environment with pagination information</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<PagedResults<Content>> GetContentsAsync(GetContentsArgs getContentsArgs = null);

    /// <summary>
    /// Get all content created by the current signed in player
    /// </summary>
    /// <param name="getPlayerContentsArgs">The details of the search request</param>
    /// <returns>A list of contents from the environment with pagination information</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<PagedResults<Content>> GetPlayerContentsAsync(GetPlayerContentsArgs getPlayerContentsArgs = null);

    /// <summary>
    /// Get content by <see cref="ContentTrendType"/>
    /// </summary>
    /// <param name="getContentTrendsArgs">The details of the search request</param>
    /// <returns>A list of contents of this trend type, with pagination information</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<PagedResults<Content>> GetContentTrendsAsync(GetContentTrendsArgs getContentTrendsArgs = null);

    /// <summary>
    /// Get content versions of a content.
    /// </summary>
    /// <param name="getContentVersionsArgs">Contains all the parameters of the request</param>
    /// <returns>The list of versions associated with the content with pagination information</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<PagedResults<ContentVersion>> GetContentVersionsAsync(GetContentVersionsArgs getContentVersionsArgs);

    /// <summary>
    /// Get a specific content.
    /// </summary>
    /// <param name="getContentArgs">Contains all the parameters of the request</param>
    /// <returns>The requested content</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<Content> GetContentAsync(GetContentArgs getContentArgs);

    /// <summary>
    /// Create a new content in a project environment
    /// </summary>
    /// <param name="createContentArgs">Contains all the parameters of the request</param>
    /// <returns>The content that will be eventually available once the upload has been uploaded.</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<Content> CreateContentAsync(CreateContentArgs createContentArgs);

    /// <summary>
    /// Create a new version of a content.
    /// </summary>
    /// <param name="contentId">The content identifier</param>
    /// <param name="asset">The stream containing the binary payload of the content</param>
    /// <param name="thumbnail">The stream containing the image representing the content</param>
    /// <returns>The content that will be eventually available once the upload has been uploaded.</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<Content> CreateContentVersionAsync(string contentId, byte[] asset, byte[] thumbnail = null);

    /// <summary>
    /// Delete a content
    /// </summary>
    /// <param name="contentId">The content identifier</param>
    /// <returns>A task</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task DeleteContentAsync(string contentId);

    /// <summary>
    /// Get a list of tags associated with the project
    /// </summary>
    /// <returns>A list of tags</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<List<Tag>> GetTagsAsync();

    /// <summary>
    /// Get the rating of a content.
    /// </summary>
    /// <param name="contentId">The content identifier</param>
    /// <returns>The rating of the content</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<ContentUserRating> GetUserContentRatingAsync(string contentId);

    /// <summary>
    /// Submit a user rating of a content
    /// </summary>
    /// <param name="contentId">The content identifier</param>
    /// <param name="rating">The rating value</param>
    /// <returns>The rating</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<ContentUserRating> SubmitUserContentRatingAsync(string contentId, float rating);

    /// <summary>
    /// Report a content
    /// </summary>
    /// <param name="reportContentArgs">The details of the report request</param>
    /// <returns>The reported content</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<Content> ReportContentAsync(ReportContentArgs reportContentArgs);

    /// <summary>
    /// Approve content that needed moderation.
    /// </summary>
    /// <param name="contentId">The content identifier</param>
    /// <returns>The approved content</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<Content> ApproveContentAsync(string contentId);

    /// <summary>
    /// Reject content that needed moderation.
    /// </summary>
    /// <param name="reportContentArgs">The details of the report reject</param>
    /// <returns>The rejected content</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<Content> RejectContentAsync(ReportContentArgs reportContentArgs);

    /// <summary>
    /// Search for content that needs moderation in the project.
    /// </summary>
    /// <param name="searchContentModerationArgs">The details of the search request</param>
    /// <returns>A list of contents requiring moderation with pagination information</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<PagedResults<Content>> SearchContentModerationAsync(
        SearchContentModerationArgs searchContentModerationArgs
    );

    /// <summary>
    /// Update a specific content details.
    /// </summary>
    /// <param name="updateContentDetailsArgs">The details of the update request</param>
    /// <returns>The updated content</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<Content> UpdateContentDetailsAsync(UpdateContentDetailsArgs updateContentDetailsArgs);

    /// <summary>
    /// Subscribe to the content for the current user
    /// </summary>
    /// <param name="contentId">The content identifier</param>
    /// <returns>The created subscription</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<Subscription> CreateSubscriptionAsync(string contentId);

    /// <summary>
    /// Unsubscribe to the content for the current user
    /// </summary>
    /// <param name="contentId">The content identifier</param>
    /// <returns>A task</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task DeleteSubscriptionAsync(string contentId);

    /// <summary>
    /// Get all subscriptions of the current user
    /// </summary>
    /// <param name="getSubscriptionsArgs">The details of the search request</param>
    /// <returns>A list of subscriptions of the current user with pagination information</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<PagedResults<Subscription>> GetSubscriptionsAsync(GetSubscriptionsArgs getSubscriptionsArgs);

    /// <summary>
    /// Check if the current user is subscribed to the content
    /// </summary>
    /// <param name="contentId">The content identifier</param>
    /// <returns>true if the content is subscribed to, false otherwise</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<bool> IsSubscribedToAsync(string contentId);

    /// <summary>
    /// Download data and/or thumbnail of a content
    /// </summary>
    /// <param name="content">The content that will have its data downloaded</param>
    /// <param name="downloadContent">True if we want to download the content's data</param>
    /// <param name="downloadThumbnail">True if we want to download the content's thumbnail</param>
    /// <returns>The downloaded data will be put in the `content` parameter so this call returns nothing.</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task DownloadContentDataAsync(Content content, bool downloadContent, bool downloadThumbnail);

    /// <summary>
    /// Create a new representation of a content
    /// </summary>
    /// <param name="createRepresentationArgs">Contains all the parameters of the request</param>
    /// <returns>The created representation</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<Representation> CreateRepresentationAsync(CreateRepresentationArgs createRepresentationArgs);

    /// <summary>
    /// Get a specific representation of a content
    /// </summary>
    /// <param name="getRepresentationArgs">Contains all the parameters of the request</param>
    /// <returns>The representation</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<Representation> GetRepresentationAsync(GetRepresentationArgs getRepresentationArgs);

    /// <summary>
    /// Update a specific representation.
    /// </summary>
    /// <param name="updateRepresentationArgs">The details of the update request</param>
    /// <returns>The updated representation</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task<Representation> UpdateRepresentationAsync(UpdateRepresentationArgs updateRepresentationArgs);

    /// <summary>
    /// Delete representation
    /// </summary>
    /// <param name="deleteRepresentationArgs">The id of the representation to delete</param>
    /// <returns>A task</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public Task DeleteRepresentationAsync(DeleteRepresentationArgs deleteRepresentationArgs);
}

public partial class UgcService : Node, IUgcService
{
    public static UgcService Instance { get; private set; }

    private RestClient ugcClient;
    private RestClient downloadClient = new();
    private const string UgcURL = "https://ugc.services.api.unity.com";
    private static string ProjectId => UnityServices.Instance.ProjectId;
    private static string PlayerId => AuthenticationService.Instance.PlayerId;
    private static string EnvironmentId => AuthenticationService.Instance.EnvironmentId;
    private readonly MD5 md5 = MD5.Create();

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
            configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions { IncludeFields = true })
        );

        ugcClient.AddDefaultHeaders(UnityServices.Instance.DefaultHeaders);
    }

    public async Task<PagedResults<Content>> GetContentsAsync(GetContentsArgs getContentsArgs = null)
    {
        Validate();

        var request = new RestRequest($"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/search")
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

    public async Task<PagedResults<Content>> GetPlayerContentsAsync(GetPlayerContentsArgs getPlayerContentsArgs = null)
    {
        Validate();

        var request = new RestRequest($"/v1/content/search") { RequestFormat = DataFormat.Json }
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

    public async Task<PagedResults<Content>> GetContentTrendsAsync(GetContentTrendsArgs getContentTrendsArgs = null)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/trends/{getContentTrendsArgs.TrendType}"
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

    public async Task<PagedResults<ContentVersion>> GetContentVersionsAsync(
        GetContentVersionsArgs getContentVersionsArgs
    )
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{getContentVersionsArgs.ContentId}/versions"
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

    public async Task<Content> GetContentAsync(GetContentArgs getContentArgs)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{getContentArgs.ContentId}"
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

    public async Task<Content> CreateContentAsync(CreateContentArgs createContentArgs)
    {
        Validate();

        var request = new RestRequest($"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content", Method.Post)
        {
            RequestFormat = DataFormat.Json
        };

        request.AddJsonBody(
            new NewContentRequest(
                createContentArgs.Name,
                createContentArgs.Description,
                createContentArgs.CustomId,
                createContentArgs.IsPublic ? ContentVisibility.Public : ContentVisibility.Private,
                createContentArgs.TagIds,
                GetHash(createContentArgs.Asset),
                GetHash(createContentArgs.Thumbnail),
                createContentArgs.Metadata
            )
        );

        var response = await ugcClient.ExecuteAsync<UploadContentResponse>(request);
        if (response.IsSuccessful)
        {
            await UploadContentDataAsync(response.Data, createContentArgs.Asset, createContentArgs.Thumbnail);
            return new Content(response.Data.Content);
        }
        else
        {
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    public async Task<Content> CreateContentVersionAsync(string contentId, byte[] asset, byte[] thumbnail = null)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{contentId}/version",
            Method.Post
        );

        string assetHash = GetHash(asset);
        string thumbnailHash = GetHash(thumbnail);
        request.AddJsonBody(new { assetHash, thumbnailHash });

        var response = await ugcClient.ExecuteAsync<UploadContentResponse>(request);
        if (response.IsSuccessful)
        {
            await UploadContentDataAsync(response.Data, asset, thumbnail);
            return new Content(response.Data.Content);
        }
        else
        {
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    public async Task DeleteContentAsync(string contentId)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{contentId}",
            Method.Delete
        )
        {
            RequestFormat = DataFormat.Json
        };

        var response = await ugcClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task<List<Tag>> GetTagsAsync()
    {
        Validate();

        var request = new RestRequest($"/v1/projects/{ProjectId}/environments/{EnvironmentId}/tags")
        {
            RequestFormat = DataFormat.Json
        };

        var response = await ugcClient.ExecuteAsync<List<InternalTag>>(request);
        if (response.IsSuccessful)
            return response.Data.ConvertAll(x => new Tag(x));
        else
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task<ContentUserRating> GetUserContentRatingAsync(string contentId)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{contentId}/rating"
        )
        {
            RequestFormat = DataFormat.Json
        };

        var response = await ugcClient.ExecuteAsync<InternalContentUserRating>(request);
        if (response.IsSuccessful)
            return new ContentUserRating(response.Data);
        else
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task<ContentUserRating> SubmitUserContentRatingAsync(string contentId, float rating)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{contentId}/rating",
            Method.Put
        ).AddJsonBody(new { rating });

        var response = await ugcClient.ExecuteAsync<InternalContentUserRating>(request);
        if (response.IsSuccessful)
            return new ContentUserRating(response.Data);
        else
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task<Content> ReportContentAsync(ReportContentArgs reportContentArgs)
    {
        Validate();

        var request = new RestRequest(
            $"/v2/projects/{ProjectId}/environments/{EnvironmentId}/content/{reportContentArgs.ContentId}/report",
            Method.Post
        ).AddJsonBody(new { reportContentArgs.ReportReason, reportContentArgs.OtherReason });

        var response = await ugcClient.ExecuteAsync<InternalContent>(request);
        if (response.IsSuccessful)
            return new Content(response.Data);
        else
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task<Content> ApproveContentAsync(string contentId)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{contentId}/approve",
            Method.Post
        )
        {
            RequestFormat = DataFormat.Json
        };

        var response = await ugcClient.ExecuteAsync<InternalContent>(request);
        if (response.IsSuccessful)
            return new Content(response.Data);
        else
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task<Content> RejectContentAsync(ReportContentArgs reportContentArgs)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{reportContentArgs.ContentId}/reject",
            Method.Post
        ).AddJsonBody(new { reportContentArgs.ReportReason, reportContentArgs.OtherReason });

        var response = await ugcClient.ExecuteAsync<InternalContent>(request);
        if (response.IsSuccessful)
            return new Content(response.Data);
        else
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task<PagedResults<Content>> SearchContentModerationAsync(
        SearchContentModerationArgs searchContentModerationArgs
    )
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/moderation/search"
        )
            .AddQueryParameter("offset", searchContentModerationArgs?.Offset ?? 0)
            .AddQueryParameter("limit", searchContentModerationArgs?.Limit ?? 25)
            .AddQueryParameter("includeTotal", searchContentModerationArgs?.IncludeTotal ?? false);

        if (searchContentModerationArgs?.SortBys != null)
        {
            foreach (var sort in searchContentModerationArgs?.SortBys)
                request.AddQueryParameter("sortBys", sort);
        }

        if (!string.IsNullOrEmpty(searchContentModerationArgs?.Search))
            request.AddQueryParameter("search", searchContentModerationArgs.Search);

        if (searchContentModerationArgs?.Filters != null)
        {
            foreach (var filter in searchContentModerationArgs?.Filters)
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

    public async Task<Content> UpdateContentDetailsAsync(UpdateContentDetailsArgs updateContentDetailsArgs)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{updateContentDetailsArgs.ContentId}/details",
            Method.Put
        ).AddJsonBody(
            new UpdateContentRequest(
                updateContentDetailsArgs.Name,
                updateContentDetailsArgs.Description,
                updateContentDetailsArgs.CustomId,
                updateContentDetailsArgs.IsPublic ? ContentVisibility.Public : ContentVisibility.Private,
                0,
                updateContentDetailsArgs.TagsId,
                updateContentDetailsArgs.Version,
                updateContentDetailsArgs.Metadata
            )
        );

        var response = await ugcClient.ExecuteAsync<InternalContent>(request);
        if (response.IsSuccessful)
            return new Content(response.Data);
        else
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task<Subscription> CreateSubscriptionAsync(string contentId)
    {
        Validate();

        var request = new RestRequest($"/v1/subscriptions", Method.Post).AddJsonBody(
            new
            {
                projectId = ProjectId,
                environmentId = EnvironmentId,
                contentId
            }
        );

        var response = await ugcClient.ExecuteAsync<InternalSubscription>(request);
        if (response.IsSuccessful)
            return new Subscription(response.Data);
        else
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task DeleteSubscriptionAsync(string contentId)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/subscriptions/projects/{ProjectId}/environments/{EnvironmentId}/content/{contentId}",
            Method.Delete
        );

        var response = await ugcClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task<PagedResults<Subscription>> GetSubscriptionsAsync(GetSubscriptionsArgs getSubscriptionsArgs)
    {
        Validate();

        var request = new RestRequest($"/v1/subscriptions/search") { RequestFormat = DataFormat.Json }
            .AddQueryParameter("offset", getSubscriptionsArgs?.Offset ?? 0)
            .AddQueryParameter("limit", getSubscriptionsArgs?.Limit ?? 25)
            .AddQueryParameter("includeTotal", getSubscriptionsArgs?.IncludeTotal ?? false);

        if (getSubscriptionsArgs?.SortBys != null)
        {
            foreach (var sort in getSubscriptionsArgs?.SortBys)
                request.AddQueryParameter("sortBys", sort);
        }

        if (!string.IsNullOrEmpty(getSubscriptionsArgs?.Search))
            request.AddQueryParameter("search", getSubscriptionsArgs.Search);

        if (getSubscriptionsArgs?.Filters != null)
        {
            foreach (var filter in getSubscriptionsArgs?.Filters)
                request.AddQueryParameter("filters", filter);
        }

        var response = await ugcClient.ExecuteAsync<PagedResults<InternalSubscription>>(request);
        if (response.IsSuccessful)
        {
            return new PagedResults<Subscription>(
                response.Data.Offset,
                response.Data.Limit,
                response.Data.Total,
                response.Data.Results.ConvertAll(x => new Subscription(x))
            );
        }
        else
        {
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    public async Task<bool> IsSubscribedToAsync(string contentId)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/subscriptions/projects/{ProjectId}/environments/{EnvironmentId}/content/{contentId}"
        )
        {
            RequestFormat = DataFormat.Json
        };

        var response = await ugcClient.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            return true;
        }
        else
        {
            var e = new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
            if (e.Content.Code == 22018) // code for not found
                return false;
            else
                throw e;
        }
    }

    public async Task DownloadContentDataAsync(Content content, bool downloadContent, bool downloadThumbnail)
    {
        Validate();

        if (downloadContent)
        {
            var request = new RestRequest(content.DownloadUrl) { RequestFormat = DataFormat.Json };

            var response = await downloadClient.ExecuteAsync(request);
            if (response.IsSuccessful)
                content.DownloadedContent = response?.RawBytes ?? null;
            else
                throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }

        if (downloadThumbnail && !string.IsNullOrEmpty(content.ThumbnailUrl))
        {
            var request = new RestRequest(content.ThumbnailUrl) { RequestFormat = DataFormat.Json };

            var response = await downloadClient.ExecuteAsync(request);
            if (response.IsSuccessful)
                content.DownloadedThumbnail = response?.RawBytes ?? null;
            else
                throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    private async Task UploadContentDataAsync(UploadContentResponse contentResponse, byte[] asset, byte[] thumbnail)
    {
        var contentRequest = new RestRequest(contentResponse.UploadContentUrl, Method.Put)
        {
            RequestFormat = DataFormat.Json,
            AlwaysSingleFileAsContent = true
        }.AddFile("asset", asset, "", ContentType.Binary);

        if (contentResponse?.UploadContentHeaders != null)
        {
            foreach (var header in contentResponse.UploadContentHeaders)
            {
                foreach (var value in header.Value)
                    contentRequest.AddHeader(header.Key, value);
            }
        }

        var response = await ugcClient.ExecuteAsync(contentRequest);
        if (!response.IsSuccessful)
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);

        if (thumbnail != null && thumbnail.Length > 0)
        {
            var thumbnailRequest = new RestRequest(contentResponse.UploadThumbnailUrl, Method.Put)
            {
                RequestFormat = DataFormat.Json,
                AlwaysSingleFileAsContent = true
            }.AddFile("thumbnail", thumbnail, "", ContentType.Binary);

            if (contentResponse?.UploadThumbnailHeaders != null)
            {
                foreach (var header in contentResponse.UploadThumbnailHeaders)
                {
                    foreach (var value in header.Value)
                        thumbnailRequest.AddHeader(header.Key, value);
                }
            }

            response = await ugcClient.ExecuteAsync(thumbnailRequest);
            if (!response.IsSuccessful)
                throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    public async Task<Representation> CreateRepresentationAsync(CreateRepresentationArgs createRepresentationArgs)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{createRepresentationArgs.ContentId}/representations",
            Method.Post
        ).AddJsonBody(new { createRepresentationArgs.Tags, createRepresentationArgs.Metadata });

        var response = await ugcClient.ExecuteAsync<InternalRepresentation>(request);
        if (response.IsSuccessful)
            return new Representation(response.Data);
        else
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task<Representation> GetRepresentationAsync(GetRepresentationArgs getRepresentationArgs)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{getRepresentationArgs.ContentId}/representations/{getRepresentationArgs.RepresentationId}"
        )
        {
            RequestFormat = DataFormat.Json
        };

        var response = await ugcClient.ExecuteAsync<InternalRepresentation>(request);
        if (response.IsSuccessful)
            return new Representation(response.Data);
        else
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task<Representation> UpdateRepresentationAsync(UpdateRepresentationArgs updateRepresentationArgs)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{updateRepresentationArgs.ContentId}/representations/{updateRepresentationArgs.RepresentationId}",
            Method.Put
        ).AddJsonBody(
            new
            {
                updateRepresentationArgs.Tags,
                updateRepresentationArgs.Version,
                updateRepresentationArgs.Metadata
            }
        );

        var response = await ugcClient.ExecuteAsync<InternalRepresentation>(request);
        if (response.IsSuccessful)
            return new Representation(response.Data);
        else
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    /// <summary>
    /// Create a representation version.
    /// </summary>
    /// <param name="contentId">The content identifier</param>
    /// <param name="representationId">The representation identifier</param>
    /// <param name="asset">The stream containing the binary payload of the representation version</param>
    /// <returns>The representation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public async Task<RepresentationVersion> CreateRepresentationVersionAsync(
        string contentId,
        string representationId,
        byte[] asset
    )
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{contentId}/representations/{representationId}/version",
            Method.Post
        ).AddJsonBody(new { md5Hash = GetHash(asset) });

        var response = await ugcClient.ExecuteAsync<InternalRepresentationVersion>(request);
        if (response.IsSuccessful)
            return new RepresentationVersion(response.Data);
        else
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    /// <summary>
    /// Get representations of a content
    /// </summary>
    /// <param name="getRepresentationsArgs">The details of the search request</param>
    /// <returns>A list of representations of specific content with pagination information</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public async Task<PagedResults<Representation>> GetRepresentationsAsync(
        GetRepresentationsArgs getRepresentationsArgs
    )
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{getRepresentationsArgs.ContentId}/representations/search"
        )
        {
            RequestFormat = DataFormat.Json
        }
            .AddQueryParameter("offset", getRepresentationsArgs?.Offset ?? 0)
            .AddQueryParameter("limit", getRepresentationsArgs?.Limit ?? 25)
            .AddQueryParameter("includeTotal", getRepresentationsArgs?.IncludeTotal ?? false);

        if (getRepresentationsArgs?.SortBys != null)
        {
            foreach (var sort in getRepresentationsArgs?.SortBys)
                request.AddQueryParameter("sortBys", sort);
        }

        if (!string.IsNullOrEmpty(getRepresentationsArgs?.Search))
            request.AddQueryParameter("search", getRepresentationsArgs.Search);

        if (getRepresentationsArgs?.Filters != null)
        {
            foreach (var filter in getRepresentationsArgs?.Filters)
                request.AddQueryParameter("filters", filter);
        }

        var response = await ugcClient.ExecuteAsync<PagedResults<InternalRepresentation>>(request);
        if (response.IsSuccessful)
        {
            return new PagedResults<Representation>(
                response.Data.Offset,
                response.Data.Limit,
                response.Data.Total,
                response.Data.Results.ConvertAll(x => new Representation(x))
            );
        }
        else
        {
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Search representations within a project
    /// </summary>
    /// <param name="searchRepresentationsArgs">The details of the search request</param>
    /// <returns>A list of representations within a project with pagination information</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public async Task<PagedResults<Representation>> SearchRepresentationsAsync(
        SearchRepresentationsArgs searchRepresentationsArgs
    )
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/representations/search"
        )
            .AddQueryParameter("offset", searchRepresentationsArgs?.Offset ?? 0)
            .AddQueryParameter("limit", searchRepresentationsArgs?.Limit ?? 25)
            .AddQueryParameter("includeTotal", searchRepresentationsArgs?.IncludeTotal ?? false);

        if (searchRepresentationsArgs?.SortBys != null)
        {
            foreach (var sort in searchRepresentationsArgs?.SortBys)
                request.AddQueryParameter("sortBys", sort);
        }

        if (!string.IsNullOrEmpty(searchRepresentationsArgs?.Search))
            request.AddQueryParameter("search", searchRepresentationsArgs.Search);

        if (searchRepresentationsArgs?.Filters != null)
        {
            foreach (var filter in searchRepresentationsArgs?.Filters)
                request.AddQueryParameter("filters", filter);
        }

        var response = await ugcClient.ExecuteAsync<PagedResults<InternalRepresentation>>(request);
        if (response.IsSuccessful)
        {
            return new PagedResults<Representation>(
                response.Data.Offset,
                response.Data.Limit,
                response.Data.Total,
                response.Data.Results.ConvertAll(x => new Representation(x))
            );
        }
        else
        {
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Get versions of a representation
    /// </summary>
    /// <param name="getRepresentationVersionsArgs">The details of the search request</param>
    /// <returns>A list of versions of a specific representation with pagination information</returns>
    /// <exception cref="InvalidOperationException">Thrown if user is not signed in.</exception>
    /// <exception cref="UgcException">Thrown if request is unsuccessful due to UGC Service specific issues.</exception>
    public async Task<PagedResults<RepresentationVersion>> GetRepresentationVersionsAsync(
        GetRepresentationVersionsArgs getRepresentationVersionsArgs
    )
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{getRepresentationVersionsArgs.ContentId}/representations/{getRepresentationVersionsArgs.RepresentationId}/versions"
        )
            .AddQueryParameter("offset", getRepresentationVersionsArgs?.Offset ?? 0)
            .AddQueryParameter("limit", getRepresentationVersionsArgs?.Limit ?? 25)
            .AddQueryParameter("includeTotal", getRepresentationVersionsArgs?.IncludeTotal ?? false);

        if (getRepresentationVersionsArgs?.SortBys != null)
        {
            foreach (var sort in getRepresentationVersionsArgs?.SortBys)
                request.AddQueryParameter("sortBys", sort);
        }

        var response = await ugcClient.ExecuteAsync<PagedResults<InternalRepresentationVersion>>(request);
        if (response.IsSuccessful)
        {
            return new PagedResults<RepresentationVersion>(
                response.Data.Offset,
                response.Data.Limit,
                response.Data.Total,
                response.Data.Results.ConvertAll(x => new RepresentationVersion(x))
            );
        }
        else
        {
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    public async Task DeleteRepresentationAsync(DeleteRepresentationArgs deleteRepresentationArgs)
    {
        Validate();

        var request = new RestRequest(
            $"/v1/projects/{ProjectId}/environments/{EnvironmentId}/content/{deleteRepresentationArgs.ContentId}/representations/{deleteRepresentationArgs.RepresentationId}",
            Method.Delete
        );

        var response = await ugcClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw new UgcException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    private string GetHash(byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
            return null;

        var md5Bytes = md5.ComputeHash(bytes);
        return Convert.ToBase64String(md5Bytes);
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
