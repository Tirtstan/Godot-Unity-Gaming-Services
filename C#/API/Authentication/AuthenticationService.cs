// References: https://services.docs.unity.com/docs/client-auth && https://restsharp.dev/docs/usage/basics

using System.Threading.Tasks;
using Godot;
using RestSharp;
using Unity.Services.Authentication.Models;
using Unity.Services.Core;

namespace Unity.Services.Authentication;

public partial class AuthenticationService : Node
{
    public static AuthenticationService Instance { get; private set; }
    public UserSession UserSession { get; private set; }
    private RestClient authClient;
    private const string AuthURL = "https://player-auth.services.api.unity.com/v1/authentication";

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        authClient = new RestClient(AuthURL);
    }

    public async Task SignInAnonymouslyAsync()
    {
        var request = new RestRequest("/anonymous", Method.Post).AddHeader(
            "ProjectId",
            UnityServices.Instance.ProjectId
        );
        request.RequestFormat = DataFormat.Json;

        var response = await authClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw response.ErrorException;
    }

    /// <summary>
    /// <para> Username constraints:</para>
    /// <para>- Must be between 3-20 characters long.</para>
    /// <para>- Can only contain the following characters: a-z, 0-9, and the symbols [.][-][@][_].</para>
    /// <para>- Is case-insensitive.</para>
    /// <para>Password Constraints:</para>
    /// <para>- Must be between 8-30 characters long.</para>
    /// <para>- Must contain at least one uppercase letter.</para>
    /// <para>- Must contain at least one lowercase letter.</para>
    /// <para>- Must contain at least one number.</para>
    /// <para>- Must contain at least one symbol.</para>
    /// </summary>
    public async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        string requestData =
            "{" + $@"""username"": ""{username}"", ""password"": ""{password}""" + "}";

        var request = new RestRequest("/usernamepassword/sign-up", Method.Post)
            .AddHeader("ProjectId", UnityServices.Instance.ProjectId)
            .AddJsonBody(requestData);

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (!response.IsSuccessful)
            throw response.ErrorException;
        else
            UserSession = response.Data;
    }

    public async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        string requestData =
            "{" + $@"""username"": ""{username}"", ""password"": ""{password}""" + "}";

        var request = new RestRequest("/usernamepassword/sign-in", Method.Post)
            .AddHeader("ProjectId", UnityServices.Instance.ProjectId)
            .AddJsonBody(requestData);

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (!response.IsSuccessful)
            throw response.ErrorException;
        else
            UserSession = response.Data;
    }

    public async Task UpdatePasswordAsync(string currentPassword, string newPassword)
    {
        string requestData =
            "{" + $@"""password"": ""{currentPassword}"", ""newPassword"": ""{newPassword}""" + "}";

        var request = new RestRequest("/usernamepassword/update-password", Method.Post)
            .AddHeader("ProjectId", UnityServices.Instance.ProjectId)
            .AddHeader("Authorization", $"Bearer {UserSession}")
            .AddJsonBody(requestData);

        var response = await authClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw response.ErrorException;
    }
}
