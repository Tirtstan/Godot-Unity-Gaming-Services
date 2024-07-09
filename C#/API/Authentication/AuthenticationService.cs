// References: https://services.docs.unity.com/docs/client-auth && https://restsharp.dev/docs/usage/basics

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using RestSharp;
using Unity.Services.Authentication.Models;
using Unity.Services.Core;

namespace Unity.Services.Authentication;

public partial class AuthenticationService : Node
{
    public static AuthenticationService Instance { get; private set; }
    public UserSession UserSession { get; private set; } = new();
    private RestClient authClient;
    private const string AuthURL = "https://player-auth.services.api.unity.com/v1/authentication";
    private const string Path = "user://GodotUGS_UserCache.cfg";

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        authClient = new RestClient(AuthURL);
        authClient.AddDefaultHeaders(
            new Dictionary<string, string>
            {
                { "ProjectId", UnityServices.Instance.ProjectId },
                { "UnityEnvironment", UnityServices.Instance.Environment }
            }
        );

        LoadUserTokens();
    }

    public async Task SignInAnonymouslyAsync()
    {
        if (!string.IsNullOrEmpty(UserSession.sessionToken))
        {
            await SignInWithSessionToken(UserSession.sessionToken);
            return;
        }

        var request = new RestRequest("/anonymous", Method.Post)
        {
            RequestFormat = DataFormat.Json
        };

        var response = await authClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw response.ErrorException;
    }

    private async Task SignInWithSessionToken(string sessionToken)
    {
        string requestData = "{" + $@"""sessionToken"": ""{sessionToken}""" + "}";
        var request = new RestRequest("/session-token", Method.Post).AddJsonBody(requestData);

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (!response.IsSuccessful)
        {
            throw response.ErrorException;
        }
        else
        {
            UserSession = response.Data;
            SaveUserTokens();
        }
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
        var request = new RestRequest("/usernamepassword/sign-up", Method.Post).AddJsonBody(
            requestData
        );

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (!response.IsSuccessful)
        {
            throw response.ErrorException;
        }
        else
        {
            UserSession = response.Data;
            SaveUserTokens();
        }
    }

    public async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        string requestData =
            "{" + $@"""username"": ""{username}"", ""password"": ""{password}""" + "}";

        var request = new RestRequest("/usernamepassword/sign-in", Method.Post).AddJsonBody(
            requestData
        );

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (!response.IsSuccessful)
        {
            throw response.ErrorException;
        }
        else
        {
            UserSession = response.Data;
            SaveUserTokens();
        }
    }

    public async Task UpdatePasswordAsync(string currentPassword, string newPassword)
    {
        string requestData =
            "{" + $@"""password"": ""{currentPassword}"", ""newPassword"": ""{newPassword}""" + "}";

        var request = new RestRequest("/usernamepassword/update-password", Method.Post)
            .AddHeader("Authorization", $"Bearer {UserSession.idToken}")
            .AddJsonBody(requestData);

        var response = await authClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw response.ErrorException;
    }

    private void SaveUserTokens()
    {
        var config = new ConfigFile();

        config.SetValue("GodotUser", "idToken", UserSession.idToken);
        config.SetValue("GodotUser", "sessionToken", UserSession.sessionToken);

        config.Save(Path);
    }

    private void LoadUserTokens()
    {
        var config = new ConfigFile();
        Error error = config.Load(Path);
        if (error != Error.Ok)
            return;

        UserSession = new UserSession();
        foreach (string section in config.GetSections())
        {
            UserSession.idToken = (string)config.GetValue(section, "idToken");
            UserSession.sessionToken = (string)config.GetValue(section, "sessionToken");
        }
    }
}
