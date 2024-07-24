namespace Unity.Services.Ugc;

// sourced from Unity

using System.Collections.Generic;
using System.IO;
using Unity.Services.Ugc.Models;

/// <summary>
/// Base class for supporting other SearchRequest class with pagination information
/// </summary>
/// <typeparam name="T">Enum of the fields that can be sorted for that request</typeparam>
public class BaseSearchArgs<T>
    where T : struct, System.Enum
{
    /// <summary>
    /// Optional results offset used for pagination
    /// </summary>
    public int Offset { get; set; } = 0;

    /// <summary>
    /// Optional maximum number of results that will be retrieved
    /// </summary>
    public int Limit { get; set; } = 100;

    internal List<string> SortBys { get; private set; }

    /// <summary>
    /// Optional search term used to retrieve specific results
    /// </summary>
    public string Search { get; set; }

    /// <summary>
    /// Optional list of filters
    /// </summary>
    public List<string> Filters { get; set; }

    /// <summary>
    /// Optional flag to includes the total in the search result
    /// </summary>
    public bool IncludeTotal { get; set; }

    /// <summary>
    /// Add a new sort rule to the request. Sorts will be done in the order of which they were added.
    /// </summary>
    /// <param name="sortBy">The field by which the request should be sorted.</param>
    /// <param name="isDescending">Set to true if the sort should be descending, false otherwise</param>
    public void AddSortBy(T sortBy, bool isDescending)
    {
        if (SortBys == null)
            SortBys = new List<string>();
        var sortByTerm = sortBy.ToString();
        SortBys.Add(isDescending ? "-" + sortByTerm : sortByTerm);
    }
}

/// <summary>
/// Field by which results can be sorted.
/// </summary>
public enum BaseSearchSortBy
{
    /// <summary>
    /// By created date
    /// </summary>
    CreatedAt,

    /// <summary>
    /// By last updated date
    /// </summary>
    UpdatedAt,
}

/// <summary>
/// Fields by which results can be sorted while searching contents.
/// </summary>
public enum SearchContentSortBy
{
    /// <summary>
    /// By name
    /// </summary>
    Name,

    /// <summary>
    /// By project id
    /// </summary>
    ProjectId,

    /// <summary>
    /// By content version
    /// </summary>
    Version,

    /// <summary>
    /// By content visibility
    /// </summary>
    Visibility,

    /// <summary>
    /// By created date
    /// </summary>
    CreatedAt,

    /// <summary>
    /// By last updated date
    /// </summary>
    UpdatedAt,

    /// <summary>
    /// By ratings count
    /// </summary>
    ContentEnvironmentStatistics_Data_RatingsCount_AllTime,

    /// <summary>
    /// By average rating
    /// </summary>
    ContentEnvironmentStatistics_Data_RatingsAverage_AllTime,

    /// <summary>
    /// By subscriptions count
    /// </summary>
    ContentEnvironmentStatistics_Data_SubscriptionsCount_AllTime,

    /// <summary>
    /// By downloads count
    /// </summary>
    ContentEnvironmentStatistics_Data_DownloadsCount_AllTime
}

/// <summary>
/// Field by which results can be sorted while searching content versions.
/// </summary>
public enum SearchContentVersionsSortBy
{
    /// <summary>
    /// By size
    /// </summary>
    Size,

    /// <summary>
    /// By created date
    /// </summary>
    CreatedAt,

    /// <summary>
    /// By last updated date
    /// </summary>
    UpdatedAt
}

/// <summary>
/// Fields by which results can be sorted while searching subscriptions.
/// </summary>
public enum SearchSubscriptionSortBy
{
    /// <summary>
    /// By created date
    /// </summary>
    CreatedAt,

    /// <summary>
    /// By last updated date
    /// </summary>
    UpdatedAt,
}

/// <summary>
/// Field by which results can be sorted while searching RepresentationVersions.
/// </summary>
public enum SearchRepresentationVersionSortBy
{
    /// <summary>
    /// By size
    /// </summary>
    Size,

    /// <summary>
    /// By created date
    /// </summary>
    CreatedAt,

    /// <summary>
    /// By last updated date
    /// </summary>
    UpdatedAt,
}

/// <summary>
/// Trend type used to retrieve trending contents
/// </summary>
public enum ContentTrendType
{
    /// <summary>
    /// By top-rated
    /// </summary>
    TopRated,

    /// <summary>
    /// By most downloaded
    /// </summary>
    MostDownloaded,

    /// <summary>
    /// By newest
    /// </summary>
    Newest,

    /// <summary>
    /// By popularity
    /// </summary>
    Popular,

    /// <summary>
    /// By trending
    /// </summary>
    Trending,

    /// <summary>
    /// By engaging
    /// </summary>
    Engaging
}

/// <summary>
/// Support class to make a content request with <see cref="UgcService.GetContentsAsync"/>
/// Contains all the required and optional parameters of the request
/// </summary>
public class GetContentsArgs : BaseSearchArgs<SearchContentSortBy>
{
    /// <summary>
    /// Optional list of tag ids used to retrieve content with corresponding tags
    /// </summary>
    public List<string> Tags { get; set; }

    /// <summary>
    /// True if the content should include the statistics
    /// </summary>
    public bool IncludeStatistics { get; set; }
}

/// <summary>
/// Support class to make a content request with <see cref="UgcService.GetPlayerContentsAsync"/>
/// Contains all the required and optional parameters of the request
/// </summary>
public class GetPlayerContentsArgs : BaseSearchArgs<SearchContentSortBy>
{
    /// <summary>
    /// True if the content should include the statistics
    /// </summary>
    public bool IncludeStatistics { get; set; }
}

/// <summary>
/// Support class to make a content request with <see cref="UgcService.GetContentTrendsAsync"/>
/// Contains all the required and optional parameters of the request
/// </summary>
public class GetContentTrendsArgs
{
    /// <summary>
    /// Construct a new GetContentTrendsArgs object.
    /// </summary>
    /// <param name="trendType">Trend type used to retrieve content of this trend</param>
    public GetContentTrendsArgs(ContentTrendType trendType)
    {
        TrendType = trendType;
    }

    /// <summary>
    /// Trend type used to retrieve content of this trend
    /// </summary>
    public ContentTrendType TrendType { get; set; }

    /// <summary>
    /// Optional results offset used for pagination
    /// </summary>
    public int Offset { get; set; } = 0;

    /// <summary>
    /// Optional maximum number of results that will be retrieved
    /// </summary>
    public int Limit { get; set; } = 100;

    /// <summary>
    /// Optional sorting order of the results. Defaults is true.
    /// </summary>
    public bool IsSortByDescending { get; set; } = true;

    internal string GetSortBy()
    {
        return IsSortByDescending ? $"-{TrendType}" : TrendType.ToString();
    }

    /// <summary>
    /// Optional flag to includes the total in the search result
    /// </summary>
    public bool IncludeTotal { get; set; }
}

/// <summary>
/// Support class to update details about a content item <see cref="UgcService.UpdateContentDetailsAsync"/>
/// Contains all the required and optional parameters of the request
/// </summary>
public class UpdateContentDetailsArgs
{
    /// <summary>
    /// Construct a new GetContentArgs object.
    /// </summary>
    /// <param name="contentId">The content identifier of the content to update</param>
    /// <param name="name">The title of the content</param>
    /// <param name="description">The text describing the content</param>
    /// <param name="isPublic">True if the content needs to be public, false otherwise</param>
    /// <param name="tagsId">The list of tag ids selected for the content</param>
    public UpdateContentDetailsArgs(
        string contentId,
        string name,
        string description,
        bool isPublic,
        List<string> tagsId
    )
    {
        ContentId = contentId;
        Name = name;
        Description = description;
        IsPublic = isPublic;
        TagsId = tagsId;
    }

    /// <summary>
    /// The content identifier of the content to update
    /// </summary>
    public string ContentId { get; set; }

    /// <summary>
    /// The new name of the content
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The new text describing the content
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Custom Id for content
    /// </summary>
    public string CustomId { get; set; }

    /// <summary>
    /// Custom metadata of the content. It is a string that a developer can use to store any information about the content.
    /// 4000 characters max.
    /// </summary>
    public string Metadata { get; set; }

    /// <summary>
    /// True if the content needs to be public, false otherwise
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// The new list of tag ids selected for the content
    /// </summary>
    public List<string> TagsId { get; set; }

    /// <summary>
    /// The current version id of the content
    /// </summary>
    public string Version { get; set; }
}

/// <summary>
/// Support class to make a subscription request with <see cref="UgcService.GetSubscriptionsAsync"/>
/// Contains all the required and optional parameters of the request
/// </summary>
public class GetSubscriptionsArgs : BaseSearchArgs<SearchSubscriptionSortBy> { }

/// <summary>
/// Support class to make a content request with <see cref="UgcService.GetContentAsync"/>
/// Contains all the parameters of the request
/// </summary>
public class GetContentArgs
{
    /// <summary>
    /// Construct a new GetContentArgs object.
    /// </summary>
    /// <param name="contentId">The content identifier of the content to update</param>
    public GetContentArgs(string contentId)
    {
        ContentId = contentId;
    }

    /// <summary>
    /// The content identifier of the content to update
    /// </summary>
    public string ContentId { get; set; }

    /// <summary>
    /// True if the content should be downloaded after Content is fetched. Default is false.
    /// </summary>
    public bool DownloadContent { get; set; }

    /// <summary>
    /// True if the thumbnail should be downloaded after Content is fetched. Default is false.
    /// </summary>
    public bool DownloadThumbnail { get; set; }

    /// <summary>
    /// True if the content should include the statistics
    /// </summary>
    public bool IncludeStatistics { get; set; }
}

/// <summary>
/// Support class to make a content request with <see cref="UgcService.CreateContentAsync"/>
/// Contains all the parameters of the request
/// </summary>
public class CreateContentArgs
{
    /// <summary>
    /// Construct a new CreateContentArgs object.
    /// </summary>
    /// <param name="name">The title of the content</param>
    /// <param name="description">The text describing the content</param>
    /// <param name="asset">The stream containing the binary payload of the content</param>
    public CreateContentArgs(string name, string description, Stream asset)
    {
        Name = name;
        Description = description;
        Asset = asset;
    }

    /// <summary>
    /// The name of the content
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The text describing the content
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Custom Id for content
    /// </summary>
    public string CustomId { get; set; }

    /// <summary>
    /// The stream containing the binary payload of the content
    /// </summary>
    public Stream Asset { get; set; }

    /// <summary>
    /// The stream containing the image representing the content
    /// </summary>
    public Stream Thumbnail { get; set; }

    /// <summary>
    /// Custom metadata of the content. It is a string that a developer can use to store any information about the content.
    /// 4000 characters max.
    /// </summary>
    public string Metadata { get; set; }

    /// <summary>
    /// True if the content needs to be public, false otherwise
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// The list of tag ids selected for the content
    /// </summary>
    public List<string> TagIds { get; set; }
}

/// <summary>
/// Support class to make a representation request with <see cref="UgcService.CreateRepresentationAsync"/>
/// Contains all the parameters of the request
/// </summary>
public class CreateRepresentationArgs
{
    /// <summary>
    /// Construct a new CreateRepresentationArgs object.
    /// </summary>
    /// <param name="contentId">The id of the content to create representation for.</param>
    /// <param name="tags">The list of tags for the representation</param>
    public CreateRepresentationArgs(string contentId, List<string> tags)
    {
        ContentId = contentId;
        Tags = tags;
    }

    /// <summary>
    /// The id the content
    /// </summary>
    public string ContentId { get; set; }

    /// <summary>
    /// The list of tags for the representation
    /// </summary>
    public List<string> Tags { get; set; }

    /// <summary>
    /// Custom metadata of the representation. It is a string that a developer can use to store any information about the representation.
    /// 4000 characters max.
    /// </summary>
    public string Metadata { get; set; }
}

/// <summary>
/// Support class to make a representation request with <see cref="UgcService.GetRepresentationAsync"/>
/// Contains all the parameters of the request
/// </summary>
public class GetRepresentationArgs
{
    /// <summary>
    /// Construct a new GetRepresentationArgs object.
    /// </summary>
    /// <param name="contentId">The content identifier of the representation</param>
    /// <param name="representationId">The representation identifier</param>
    public GetRepresentationArgs(string contentId, string representationId)
    {
        RepresentationId = representationId;
        ContentId = contentId;
    }

    /// <summary>
    /// The content identifier of the representation
    /// </summary>
    public string ContentId { get; set; }

    /// <summary>
    /// The representation identifier of the representation
    /// </summary>
    public string RepresentationId { get; set; }

    /// <summary>
    /// True if the representation output should be downloaded after Representation is fetched. Default is false.
    /// </summary>
    public bool DownloadRepresentation { get; set; }
}

/// <summary>
/// Support class to update details about a representation item <see cref="UgcService.UpdateRepresentationAsync"/>
/// Contains all the required and optional parameters of the request
/// </summary>
public class UpdateRepresentationArgs
{
    /// <summary>
    /// Construct a new UpdateRepresentationArgs object.
    /// </summary>
    /// <param name="contentId">The content identifier of the content to update</param>
    /// <param name="representationId">The title of the content</param>
    /// <param name="tags">The list of tag ids selected for the content</param>
    /// <param name="version">Set the current version of the representation</param>
    public UpdateRepresentationArgs(string contentId, string representationId, List<string> tags, string version)
    {
        RepresentationId = representationId;
        ContentId = contentId;
        Version = version;
        Tags = tags;
    }

    /// <summary>
    /// The id of the content
    /// </summary>
    public string ContentId { get; set; }

    /// <summary>
    /// The representation identifier of the representation
    /// </summary>
    public string RepresentationId { get; set; }

    /// <summary>
    /// The list of tags for the representation
    /// </summary>
    public List<string> Tags { get; set; }

    /// <summary>
    /// The current version of the representation
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// Custom metadata of the representation. It is a string that a developer can use to store any information about the representation.
    /// 4000 characters max.
    /// </summary>
    public string Metadata { get; set; }
}

/// <summary>
/// Support class to make a representation request with <see cref="UgcService.GetRepresentationsAsync"/>
/// Contains all the required and optional parameters of the request
/// </summary>
public class GetRepresentationsArgs : BaseSearchArgs<BaseSearchSortBy>
{
    /// <summary>
    /// Construct a new GetRepresentationsArgs object.
    /// </summary>
    /// <param name="contentId">The content identifier of the representation</param>
    public GetRepresentationsArgs(string contentId)
    {
        ContentId = contentId;
    }

    /// <summary>
    /// The id of the content
    /// </summary>
    public string ContentId { get; set; }

    /// <summary>
    /// Optional list of tags used to retrieve representations with corresponding tags
    /// </summary>
    public List<string> Tags { get; set; }
}

/// <summary>
/// Support class to make a representation request with <see cref="UgcService.SearchRepresentationsAsync"/>
/// Contains all the required and optional parameters of the request
/// </summary>
public class SearchRepresentationsArgs : BaseSearchArgs<BaseSearchSortBy>
{
    /// <summary>
    /// Construct a new SearchRepresentationsArgs object.
    /// </summary>
    /// <param name="searchTerm">The search term used to retrieve specific representations</param>
    public SearchRepresentationsArgs(string searchTerm)
    {
        Search = searchTerm;
    }

    /// <summary>
    /// Optional list of tags used to retrieve representations with corresponding tags
    /// </summary>
    public List<string> Tags { get; set; }
}

/// <summary>
/// Support class to make a representation version request with <see cref="UgcService.GetRepresentationVersionsAsync"/>
/// Contains all the required and optional parameters of the request
/// </summary>
public class GetRepresentationVersionsArgs : BaseSearchArgs<SearchRepresentationVersionSortBy>
{
    /// <summary>
    /// Construct a new GetRepresentationVersionsArgs object.
    /// </summary>
    /// <param name="contentId">The content identifier of the representation</param>
    /// <param name="representationId">The representation identifier</param>
    public GetRepresentationVersionsArgs(string contentId, string representationId)
    {
        ContentId = contentId;
        RepresentationId = representationId;
    }

    /// <summary>
    /// The id of the content
    /// </summary>
    public string ContentId { get; set; }

    /// <summary>
    /// The id of the content representation
    /// </summary>
    public string RepresentationId { get; set; }
}

/// <summary>
/// Support class to make a content version request with <see cref="UgcService.GetRepresentationVersionsAsync"/>
/// Contains all the required and optional parameters of the request
/// </summary>
public class GetContentVersionsArgs : BaseSearchArgs<SearchContentVersionsSortBy>
{
    /// <summary>
    /// Construct a new GetContentVersionsArgs object.
    /// </summary>
    /// <param name="contentId">The content identifier of the ContentVersions</param>
    public GetContentVersionsArgs(string contentId)
    {
        ContentId = contentId;
    }

    /// <summary>
    /// The id of the content
    /// </summary>
    public string ContentId { get; set; }
}

/// <summary>
/// Support class to make a representation request with <see cref="UgcService.DeleteRepresentationAsync"/>
/// Contains all the parameters of the request
/// </summary>
public class DeleteRepresentationArgs
{
    /// <summary>
    /// Construct a new DeleteRepresentationArgs object.
    /// </summary>
    /// <param name="contentId">The content identifier of the representation</param>
    /// <param name="representationId">The representation identifier</param>
    public DeleteRepresentationArgs(string contentId, string representationId)
    {
        RepresentationId = representationId;
        ContentId = contentId;
    }

    /// <summary>
    /// The content identifier of the representation
    /// </summary>
    public string ContentId { get; set; }

    /// <summary>
    /// The representation identifier of the representation
    /// </summary>
    public string RepresentationId { get; set; }
}

/// <summary>
/// Support class to make a content request with <see cref="UgcService.SearchContentModerationAsync"/>
/// Contains all the required and optional parameters of the request
/// </summary>
public class SearchContentModerationArgs : BaseSearchArgs<SearchContentSortBy> { }

/// <summary>
/// Support class to make a content request with <see cref="UgcService.ReportContentAsync"/>
/// Contains all the required and optional parameters of the request
/// </summary>
public class ReportContentArgs
{
    /// <summary>
    /// Construct a new ReportContentArgs object.
    /// </summary>
    /// <param name="contentId">The content identifier of the representation</param>
    /// <param name="reportReason">The representation identifier</param>
    public ReportContentArgs(string contentId, Reason reportReason)
    {
        ContentId = contentId;
        ReportReason = reportReason;
    }

    /// <summary>
    /// The content identifier of the reported content
    /// </summary>
    public string ContentId { get; set; }

    /// <summary>
    /// The report reason enum for the request
    /// </summary>
    public Reason ReportReason { get; set; }

    /// <summary>
    /// Optional: if Reason was set to Other, provide an alternate reason here
    /// </summary>
    public string OtherReason { get; set; }
}
