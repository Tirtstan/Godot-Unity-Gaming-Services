// References: https://services.docs.unity.com/docs/client-auth && https://restsharp.dev/docs/usage/basics

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using RestSharp;
using Unity.Services.Authentication.Models;
using Unity.Services.Core;

namespace Unity.Services.Authentication;

public partial class AuthenticationService : Node
{
    public static AuthenticationService Instance { get; private set; }
    public event Action SignedIn;
    public event Action SignedOut;
    public string AccessToken => UserSession.idToken;
    public string PlayerId => UserSession.user.id;
    public string PlayerName => UserSession.user.username;
    public bool SessionTokenExists => !string.IsNullOrEmpty(SessionToken);
    public string LastNotificationDate =>
        DateTime.UnixEpoch.AddMilliseconds(UserSession.lastNotificationDate).ToString();

    private RestClient authClient;
    private UserSession UserSession = new();
    private string SessionToken => UserSession.sessionToken;
    private const string AuthURL = "https://player-auth.services.api.unity.com/v1/";
    private const string CachePath = "user://GodotUGS_UserCache.cfg";

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

    /// <summary>
    /// Signs in the current player anonymously. No credentials are required and the session is confined to the current device.
    /// </summary>
    /// <remarks>
    /// If a player has signed in previously with a session token stored on the device, they are signed back in regardless of if they're an anonymous player or not.
    /// </remarks>
    public async Task SignInAnonymouslyAsync()
    {
        try
        {
            if (!string.IsNullOrEmpty(SessionToken))
            {
                await SignInWithSessionToken(SessionToken);
                return;
            }
        }
        catch { }

        var request = new RestRequest("/authentication/anonymous", Method.Post) { RequestFormat = DataFormat.Json };

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (response.IsSuccessful)
        {
            UserSession = response.Data;
            SaveUserTokens();
            SignedIn?.Invoke();
        }
        else
        {
            throw response.ErrorException;
        }
    }

    private async Task SignInWithSessionToken(string sessionToken)
    {
        string requestData = "{" + $@"""sessionToken"": ""{sessionToken}""" + "}";
        var request = new RestRequest("/authentication/session-token", Method.Post).AddJsonBody(requestData);

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (response.IsSuccessful)
        {
            UserSession = response.Data;
            SaveUserTokens();
            SignedIn?.Invoke();
        }
        else
        {
            throw response.ErrorException;
        }
    }

    /// <summary>
    /// Sign in using Username and Password credentials.
    /// </summary>
    /// <param name="username">Username of the player. Note that it must be unique per project and contains 3-20 characters of alphanumeric and/or these special characters [. - @ _].</param>
    /// <param name="password">Password of the player. Note that it must contain 8-30 characters with at least 1 upper case, 1 lower case, 1 number, and 1 special character.</param>
    public async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        string requestData = "{" + $@"""username"": ""{username}"", ""password"": ""{password}""" + "}";

        var request = new RestRequest("/authentication/usernamepassword/sign-in", Method.Post).AddJsonBody(requestData);

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (response.IsSuccessful)
        {
            UserSession = response.Data;
            SaveUserTokens();
            SignedIn?.Invoke();
        }
        else
        {
            throw response.ErrorException;
        }
    }

    /// <summary>
    /// Sign up using Username and Password credentials.
    /// </summary>
    /// <param name="username">Username of the player. Note that it must be unique per project and contains 3-20 characters of alphanumeric and/or these special characters [. - @ _].</param>
    /// <param name="password">Password of the player. Note that it must contain 8-30 characters with at least 1 upper case, 1 lower case, 1 number, and 1 special character.</param>
    public async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        string requestData = "{" + $@"""username"": ""{username}"", ""password"": ""{password}""" + "}";
        var request = new RestRequest("/authentication/usernamepassword/sign-up", Method.Post).AddJsonBody(requestData);

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (response.IsSuccessful)
        {
            UserSession = response.Data;
            SaveUserTokens();
            SignedIn?.Invoke();
        }
        else
        {
            throw response.ErrorException;
        }
    }

    /// <summary>
    /// Sign up with a new Username/Password and add it to the current logged in user.
    /// </summary>
    public async Task AddUsernamePasswordAsync(string username, string password)
    {
        if (string.IsNullOrEmpty(AccessToken))
            throw new NullReferenceException(
                "User must be signed in an already existant account to add a username and password."
            );

        string requestData = "{" + $@"""username"": ""{username}"", ""password"": ""{password}""" + "}";
        var request = new RestRequest("/authentication/usernamepassword/sign-up", Method.Post)
            .AddHeader("Authorization", $"Bearer {AccessToken}")
            .AddJsonBody(requestData);

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (response.IsSuccessful)
        {
            UserSession = response.Data;
            SaveUserTokens();
            SignedIn?.Invoke();
        }
        else
        {
            throw response.ErrorException;
        }
    }

    /// <summary>
    /// Update Password credentials for username/password user.
    /// </summary>
    public async Task UpdatePasswordAsync(string currentPassword, string newPassword)
    {
        string requestData = "{" + $@"""password"": ""{currentPassword}"", ""newPassword"": ""{newPassword}""" + "}";
        var request = new RestRequest("/authentication/usernamepassword/update-password", Method.Post)
            .AddHeader("Authorization", $"Bearer {AccessToken}")
            .AddJsonBody(requestData);

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (response.IsSuccessful)
        {
            UserSession = response.Data;
            SaveUserTokens();
            SignedIn?.Invoke();
        }
        else
        {
            throw response.ErrorException;
        }
    }

    /// <summary>
    /// Deletes the currently signed in player permanently.
    /// </summary>
    public async Task DeleteAccountAsync()
    {
        var request = new RestRequest($"/users/{PlayerId}", Method.Delete).AddHeader(
            "Authorization",
            $"Bearer {AccessToken}"
        );
        request.RequestFormat = DataFormat.Json;

        var response = await authClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw response.ErrorException;

        ClearAccessToken();
        ClearSessionToken();
    }

    /// <summary>
    /// Sign out the current player.
    /// </summary>
    /// <param name="clearCredentials">Option to clear the session token that enables logging in to the same account</param>
    public void SignOut(bool clearCredentials = false)
    {
        if (clearCredentials)
        {
            ClearAccessToken();
            ClearSessionToken();
        }

        UserSession = new UserSession();
        SignedOut?.Invoke();
    }

    /// <summary>
    /// Retrieves the Notifications that were created for the signed in player
    /// </summary>
    public async Task<List<Notification>> GetNotificationsAsync()
    {
        var request = new RestRequest($"/users/{PlayerId}/notifications").AddHeader(
            "Authorization",
            $"Bearer {AccessToken}"
        );
        request.RequestFormat = DataFormat.Json;

        var response = await authClient.ExecuteAsync<NotificationList>(request);
        if (response.IsSuccessful)
        {
            return response.Data.notifications;
        }
        else
        {
            throw response.ErrorException;
        }
    }

    /// <summary>
    /// Deletes the session token if it exists.
    /// </summary>
    public void ClearSessionToken()
    {
        UserSession.sessionToken = "";
        SaveUserTokens();
    }

    private void ClearAccessToken()
    {
        UserSession.idToken = "";
        SaveUserTokens();
    }

    private void SaveUserTokens()
    {
        var config = new ConfigFile();
        const string Section = "GodotUGS";

        config.SetValue(Section, "idToken", AccessToken);
        config.SetValue(Section, "sessionToken", SessionToken);

        config.Save(CachePath);
    }

    private void LoadUserTokens()
    {
        var config = new ConfigFile();
        Error error = config.Load(CachePath);
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
