namespace Unity.Services.Ugc;

using System;
using System.Text.Json;
using System.Threading.Tasks;
using Godot;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
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
        var request = new RestRequest($"/projects/{ProjectId}/environments/{EnvironmentId}/content/search")
        {
            RequestFormat = DataFormat.Json
        }
            .AddQueryParameter("offset", getContentsArgs?.Offset ?? 0)
            .AddQueryParameter("limit", getContentsArgs?.Limit ?? 25)
            .AddQueryParameter("sortBys", "");
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
