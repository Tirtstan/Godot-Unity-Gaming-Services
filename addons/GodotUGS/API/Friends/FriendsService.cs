namespace Unity.Services.Friends;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Friends.Internal.Models;
using Unity.Services.Friends.Models;
using Unity.Services.Friends.Options;

public partial class FriendsService : Node
{
    public static FriendsService Instance { get; private set; }

    /// <summary>
    /// The full list of the user's relationships.
    /// </summary>
    /// <exception cref="System.InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    public IReadOnlyList<Relationship> Relationships
    {
        get => relationships;
    }

    /// <summary>
    /// The list of the user's incoming friend requests.
    /// </summary>
    /// <exception cref="System.InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    public IReadOnlyList<Relationship> IncomingFriendRequests
    {
        get =>
            relationships
                .Where(r =>
                    r.Type == RelationshipType.FriendRequest
                    && r.Member.Role == MemberRole.Source
                    && !Blocks.Any(b => b.Member.Id == r.Member.Id)
                )
                .ToList();
    }

    /// <summary>
    /// The list of the user's outgoing friend requests.
    /// </summary>
    /// <exception cref="System.InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    public IReadOnlyList<Relationship> OutgoingFriendRequests
    {
        get =>
            relationships
                .Where(r =>
                    r.Type == RelationshipType.FriendRequest
                    && r.Member.Role == MemberRole.Target
                    && !Blocks.Any(b => b.Member.Id == r.Member.Id)
                )
                .ToList();
    }

    /// <summary>
    /// The list of the user's friends.
    /// </summary>
    /// <exception cref="System.InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    public IReadOnlyList<Relationship> Friends
    {
        get =>
            relationships
                .Where(r => r.Type == RelationshipType.Friend && !Blocks.Any(b => b.Member.Id == r.Member.Id))
                .ToList();
    }

    /// <summary>
    /// The list of the user's blocks.
    /// </summary>
    /// <exception cref="System.InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    public IReadOnlyList<Relationship> Blocks
    {
        get => relationships.Where(r => r.Type == RelationshipType.Block).ToList();
    }

    private RestClient friendsClient;
    private const string FriendsURL = "https://social.services.api.unity.com/v1";
    private InitializeOptions initializeOptions;
    private bool isInitialized;
    private List<Relationship> relationships = new();

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        AuthenticationService.Instance.SignedIn += OnSignIn;
    }

    private void OnSignIn()
    {
        var options = new RestClientOptions(FriendsURL)
        {
            Authenticator = new JwtAuthenticator(AuthenticationService.Instance.AccessToken)
        };
        friendsClient = new RestClient(
            options,
            configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions { })
        );

        friendsClient.AddDefaultHeaders(UnityServices.Instance.DefaultHeaders);
    }

    /// <summary>
    /// Initialize the Friends service API.
    /// This must be called before using any other functionality of the Friends service.
    /// This can only be called when a user is signed in.
    /// </summary>
    /// <param name="initializeOptions">Options to initialize the Friends service</param>
    /// <exception cref="ArgumentException">Represents an error that occurs when an argument is incorrectly setup.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the user is not signed in.</exception>
    /// <exception cref="FriendsServiceException">An exception containing the HttpClientResponse with headers, response code, and string of error.</exception>
    public async Task InitializeAsync(InitializeOptions initializeOptions = null)
    {
        if (isInitialized)
            return;

        if (!AuthenticationService.Instance.IsSignedIn)
            throw new InvalidOperationException("User must be signed in to initialize the Friends service.");

        this.initializeOptions = initializeOptions ?? new InitializeOptions();
        isInitialized = true;
        await ForceRelationshipsRefreshAsync();
    }

    /// <summary>
    /// Creates a friend request, or automatically creates a friendship if the user already has an incoming friend request from the
    /// targeted user.
    /// </summary>
    /// <param name="memberId">The ID of the target user.</param>
    /// <returns>The friendship created</returns>
    /// <exception cref="ArgumentException">Represents an error that occurs when an argument is incorrectly setup.</exception>
    /// <exception cref="InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    /// <exception cref="FriendsServiceException">An exception containing the HttpClientResponse with headers, response code, and string of error.</exception>
    public async Task<Relationship> AddFriendAsync(string memberId) =>
        await AddRelationshipAsync(memberId, RelationshipType.Friend);

    /// <summary>
    /// Creates a friend request, or automatically creates a friendship if the user already has an incoming friend request from the
    /// targeted user by their user name.
    /// </summary>
    /// <param name="name">The name of the target user</param>
    /// <returns>The friendship created</returns>
    /// <exception cref="ArgumentException">Represents an error that occurs when an argument is incorrectly setup.</exception>
    /// <exception cref="FriendsServiceException">An exception containing the HttpClientResponse with headers, response code, and string of error.</exception>
    /// <exception cref="InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    public async Task<Relationship> AddFriendByNameAsync(string name)
    {
        Validate();

        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be null or empty.");

        var request = new RestRequest("/relationships", Method.Post)
            .AddQueryParameter("withPresence", initializeOptions.MemberOptions.IncludePresence)
            .AddQueryParameter("withProfile", initializeOptions.MemberOptions.IncludeProfile)
            .AddJsonBody(
                new InternalRelationship
                {
                    Type = RelationshipType.FriendRequest,
                    Members = new List<InternalMember>()
                    {
                        new() { ProfileName = name, Role = MemberRole.Target }
                    }
                }
            );

        var response = await friendsClient.ExecuteAsync<RelationshipList>(request);

        if (response.IsSuccessful)
        {
            var newRelationship = new Relationship(response.Data.Id, response.Data.Type, response.Data.Members[0]);
            relationships.Remove(
                Relationships.FirstOrDefault(r =>
                    r.Type == RelationshipType.FriendRequest && r.Member.Profile.Name == name
                )
            );

            relationships.Add(newRelationship);
            return newRelationship;
        }
        else
        {
            throw new FriendsServiceException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Creates a block towards the targeted user.
    /// </summary>
    /// <param name="memberId">The ID of the target user.</param>
    /// <returns>The block created</returns>
    /// <exception cref="ArgumentException">Represents an error that occurs when an argument is incorrectly setup.</exception>
    /// <exception cref="FriendsServiceException">An exception containing the HttpClientResponse with headers, response code, and string of error.</exception>
    /// <exception cref="InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    public async Task<Relationship> AddBlockAsync(string memberId) =>
        await AddRelationshipAsync(memberId, RelationshipType.Block);

    private async Task<Relationship> AddRelationshipAsync(string memberId, RelationshipType type)
    {
        Validate();

        if (string.IsNullOrEmpty(memberId))
            throw new ArgumentException("Member ID cannot be null or empty.");

        var request = new RestRequest("/relationships", Method.Post)
            .AddQueryParameter("withPresence", initializeOptions.MemberOptions.IncludePresence)
            .AddQueryParameter("withProfile", initializeOptions.MemberOptions.IncludeProfile)
            .AddJsonBody(
                new InternalRelationship
                {
                    Type = type,
                    Members = new List<InternalMember>()
                    {
                        new() { Id = memberId, Role = MemberRole.Target }
                    }
                }
            );

        var response = await friendsClient.ExecuteAsync<RelationshipList>(request);
        if (response.IsSuccessful)
        {
            var newRelationship = new Relationship(response.Data.Id, response.Data.Type, response.Data.Members[0]);
            if (type == RelationshipType.FriendRequest || type == RelationshipType.Friend)
            {
                relationships.Remove(
                    Relationships.FirstOrDefault(r =>
                        r.Type == RelationshipType.FriendRequest && r.Member.Id == memberId
                    )
                );
            }

            relationships.Add(newRelationship);
            return newRelationship;
        }
        else
        {
            throw new FriendsServiceException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Deletes an incoming friend request.
    /// </summary>
    /// <param name="memberId">The ID of the user that sent the friend request.</param>
    /// <exception cref="ArgumentException">Represents an error that occurs when an argument is incorrectly setup.</exception>
    /// <exception cref="FriendsServiceException">An exception containing the HttpClientResponse with headers, response code, and string of error.</exception>
    /// <exception cref="InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    /// <exception cref="RelationshipNotFoundException">Represents an error that occurs when a relationship is not found.</exception>
    public async Task DeleteIncomingFriendRequestAsync(string memberId)
    {
        var incomingFriendRequest = Relationships.FirstOrDefault(r =>
            r.Type == RelationshipType.FriendRequest && r.Member.Role == MemberRole.Source && r.Member.Id == memberId
        );

        if (incomingFriendRequest == null)
            throw new RelationshipNotFoundException(memberId);

        await DeleteRelationshipAsync(incomingFriendRequest.Id);
    }

    /// <summary>
    /// Deletes an outgoing friend request.
    /// </summary>
    /// <param name="memberId">The ID of the user that a friend request was sent to.</param>
    /// <exception cref="ArgumentException">Represents an error that occurs when an argument is incorrectly setup.</exception>
    /// <exception cref="FriendsServiceException">An exception containing the HttpClientResponse with headers, response code, and string of error.</exception>
    /// <exception cref="InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    /// <exception cref="RelationshipNotFoundException">Represents an error that occurs when a relationship is not found.</exception>
    public async Task DeleteOutgoingFriendRequestAsync(string memberId)
    {
        var outgoingFriendRequest = Relationships.FirstOrDefault(r =>
            r.Type == RelationshipType.FriendRequest && r.Member.Role == MemberRole.Target && r.Member.Id == memberId
        );

        if (outgoingFriendRequest == null)
            throw new RelationshipNotFoundException(memberId);

        await DeleteRelationshipAsync(outgoingFriendRequest.Id);
    }

    /// <summary>
    /// Deletes a friend.
    /// </summary>
    /// <param name="memberId">The ID of the friend to be deleted.</param>
    /// <exception cref="ArgumentException">Represents an error that occurs when an argument is incorrectly setup.</exception>
    /// <exception cref="FriendsServiceException">An exception containing the HttpClientResponse with headers, response code, and string of error.</exception>
    /// <exception cref="InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    /// <exception cref="RelationshipNotFoundException">Represents an error that occurs when a relationship is not found.</exception>
    public async Task DeleteFriendAsync(string memberId)
    {
        var friendship = Relationships.FirstOrDefault(r =>
            r.Type == RelationshipType.Friend && r.Member.Id == memberId
        );

        if (friendship == null)
            throw new RelationshipNotFoundException(memberId);

        await DeleteRelationshipAsync(friendship.Id);
    }

    /// <summary>
    /// Deletes a block.
    /// </summary>
    /// <param name="memberId">The ID of the user that will be unblocked.</param>
    /// <exception cref="ArgumentException">Represents an error that occurs when an argument is incorrectly setup.</exception>
    /// <exception cref="FriendsServiceException">An exception containing the HttpClientResponse with headers, response code, and string of error.</exception>
    /// <exception cref="InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    /// <exception cref="RelationshipNotFoundException">Represents an error that occurs when a relationship is not found.</exception>
    public async Task DeleteBlockAsync(string memberId)
    {
        var block = relationships.FirstOrDefault(r => r.Type == RelationshipType.Block && r.Member.Id == memberId);
        if (block == null)
            throw new RelationshipNotFoundException(memberId);

        await DeleteRelationshipAsync(block.Id);
    }

    /// <summary>
    /// Deletes a relationship.
    /// </summary>
    /// <param name="relationshipId">The ID of the relationship to be deleted.</param>
    /// <exception cref="ArgumentException">Represents an error that occurs when an argument is incorrectly setup.</exception>
    /// <exception cref="FriendsServiceException">An exception containing the HttpClientResponse with headers, response code, and string of error.</exception>
    /// <exception cref="InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    public async Task DeleteRelationshipAsync(string relationshipId)
    {
        Validate();

        if (string.IsNullOrEmpty(relationshipId))
            throw new ArgumentException("Relationship ID cannot be null or empty.");

        var request = new RestRequest($"/relationships/{relationshipId}", Method.Delete)
        {
            RequestFormat = DataFormat.Json
        };

        var response = await friendsClient.ExecuteAsync(request);
        if (response.IsSuccessful)
        {
            var foundRelationship = relationships.FirstOrDefault(r => r.Id == relationshipId);
            if (foundRelationship != null && !relationships.Remove(foundRelationship))
                await ForceRelationshipsRefreshAsync();
        }
        else
        {
            throw new FriendsServiceException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Forcefully refreshes the list of relationships in case they were not refreshed automatically by the service
    /// </summary>
    /// <exception cref="FriendsServiceException">An exception containing the HttpClientResponse with headers, response code, and string of error.</exception>
    /// <exception cref="InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    public async Task<List<Relationship>> ForceRelationshipsRefreshAsync(PaginationOptions paginationOptions = null)
    {
        Validate();

        var request = new RestRequest("/relationships") { RequestFormat = DataFormat.Json }
            .AddQueryParameter("limit", paginationOptions?.Limit ?? 25)
            .AddQueryParameter("offset", paginationOptions?.Offset ?? 0)
            .AddQueryParameter("withPresence", initializeOptions.MemberOptions.IncludePresence)
            .AddQueryParameter("withProfile", initializeOptions.MemberOptions.IncludeProfile);

        var response = await friendsClient.ExecuteAsync<RelationshipList>(request);
        if (response.IsSuccessful)
        {
            var relationships = response.Data.Members.ConvertAll(member => new Relationship(
                response.Data.Id,
                response.Data.Type,
                member
            ));
            this.relationships = relationships;
            return relationships;
        }
        else
        {
            throw new FriendsServiceException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Sets the presence for the user.
    /// </summary>
    /// <param name="availabilityOption">The type of availability to be set for the user's presence.</param>
    /// <param name="activity">The activity value to be set for the user's presence.</param>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="ArgumentException">Represents an error that occurs when an argument is incorrectly setup.</exception>
    /// <exception cref="FriendsServiceException">An exception containing the HttpClientResponse with headers, response code, and string of error.</exception>
    /// <exception cref="InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    public async Task SetPresenceAsync<T>(Availability availabilityOption, T activity)
        where T : new()
    {
        Validate();

        var request = new RestRequest("/presence", Method.Post) { RequestFormat = DataFormat.Json }.AddJsonBody(
            new InternalPresence { Availability = availabilityOption, Activity = activity }
        );

        var response = await friendsClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw new FriendsServiceException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    /// <summary>
    /// Gets the presence for a specified user by id.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="ArgumentException">Represents an error that occurs when an argument is incorrectly setup.</exception>
    /// <exception cref="FriendsServiceException">An exception containing the HttpClientResponse with headers, response code, and string of error.</exception>
    /// <exception cref="InvalidOperationException">Represents an error that occurs when the service has not been initialized.</exception>
    public async Task<Presence> GetPresenceAsync(string userId)
    {
        Validate();

        if (string.IsNullOrEmpty(userId))
            throw new ArgumentException("User ID cannot be null or empty.");

        var request = new RestRequest($"/presence/{userId}") { RequestFormat = DataFormat.Json };

        var response = await friendsClient.ExecuteAsync<InternalPresence>(request);
        if (response.IsSuccessful)
            return new Presence { Activity = response.Data.Activity, Availability = response.Data.Availability };
        else
            throw new FriendsServiceException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    private void Validate()
    {
        if (!isInitialized)
            throw new InvalidOperationException("Friends service has not been initialized.");

        if (!AuthenticationService.Instance.IsSignedIn)
            throw new InvalidOperationException("User must be signed in to use the Friends service.");
    }

    public override void _ExitTree()
    {
        AuthenticationService.Instance.SignedIn -= OnSignIn;
    }
}
