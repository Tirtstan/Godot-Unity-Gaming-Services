// References: https://services.docs.unity.com/docs/client-auth && https://restsharp.dev/docs/usage/basics

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using RestSharp;
using RestSharp.Authenticators;
using Unity.Services.Authentication.Models;
using Unity.Services.Core;

namespace Unity.Services.Authentication;

public partial class AuthenticationService : Node
{
    public static AuthenticationService Instance { get; private set; }
    public event Action SignedIn;
    public event Action SignedOut;
    public string AccessToken => UserSession.IdToken;
    public string PlayerId => UserSession.User.Id;
    public string PlayerName => UserSession.User.Username;
    public string Profile => currentProfile;
    public bool SessionTokenExists => !string.IsNullOrEmpty(SessionToken);
    public string LastNotificationDate =>
        DateTime.UnixEpoch.AddMilliseconds(UserSession.LastNotificationDate).ToString();

    private RestClient authClient;
    private ConfigFile config = new();
    private UserSession UserSession = new();
    private string SessionToken => UserSession.SessionToken;
    private const string AuthURL = "https://player-auth.services.api.unity.com/v1";
    private const string CachePath = "user://GodotUGS_UserCache.cfg";
    private const string Persistents = "Persistents";
    private string currentProfile = "DefaultProfile";

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

        LoadPersistents();
        LoadCache();
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
            if (SessionTokenExists)
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
            SaveCache();
            SignedIn?.Invoke();
        }
        else
        {
            throw response.ErrorException;
        }
    }

    private async Task SignInWithSessionToken(string sessionToken)
    {
        var request = new RestRequest("/authentication/session-token", Method.Post).AddJsonBody(new { sessionToken });

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (response.IsSuccessful)
        {
            UserSession = response.Data;
            SaveCache();
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
        var request = new RestRequest("/authentication/usernamepassword/sign-in", Method.Post).AddJsonBody(
            new { username, password }
        );

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (response.IsSuccessful)
        {
            UserSession = response.Data;
            SaveCache();
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
        var request = new RestRequest("/authentication/usernamepassword/sign-up", Method.Post).AddJsonBody(
            new { username, password }
        );

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (response.IsSuccessful)
        {
            UserSession = response.Data;
            SaveCache();
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

        var request = new RestRequest("/authentication/usernamepassword/sign-up", Method.Post).AddJsonBody(
            new { username, password }
        );
        request.Authenticator = new JwtAuthenticator(AccessToken);

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (response.IsSuccessful)
        {
            UserSession = response.Data;
            SaveCache();
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
        var request = new RestRequest("/authentication/usernamepassword/update-password", Method.Post).AddJsonBody(
            new { password = currentPassword, newPassword }
        );
        request.Authenticator = new JwtAuthenticator(AccessToken);

        var response = await authClient.ExecuteAsync<UserSession>(request);
        if (response.IsSuccessful)
        {
            UserSession = response.Data;
            SaveCache();
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
        var request = new RestRequest($"/users/{PlayerId}", Method.Delete)
        {
            Authenticator = new JwtAuthenticator(AccessToken),
            RequestFormat = DataFormat.Json
        };

        var response = await authClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw response.ErrorException;

        SignOut(true);
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
    /// Returns the info of the logged in player, which includes the player's id, creation time and linked identities.
    /// </summary>
    /// <returns>Task for the operation</returns>
    public async Task<PlayerInfo> GetPlayerInfoAsync()
    {
        var request = new RestRequest($"/users/{PlayerId}").AddHeader("Authorization", $"Bearer {AccessToken}");
        request.RequestFormat = DataFormat.Json;

        var response = await authClient.ExecuteAsync<PlayerInfo>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw response.ErrorException;
    }

    /// <summary>
    /// Retrieves the Notifications that were created for the signed in player
    /// </summary>
    public async Task<List<Notification>> GetNotificationsAsync()
    {
        var request = new RestRequest($"/users/{PlayerId}/notifications")
        {
            Authenticator = new JwtAuthenticator(AccessToken),
            RequestFormat = DataFormat.Json
        };

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

    public void SwitchProfile(string profileName)
    {
        if (string.IsNullOrEmpty(profileName))
            currentProfile = "DefaultProfile";
        else
            currentProfile = profileName;

        SavePersistents();
        LoadCache();
    }

    // public async void ProcessAuthenticationTokens(string accessToken, string sessionToken = null)
    // {
    //     const string UnityServicesURL = "https://services.api.unity.com";
    //     var request = new RestRequest() { Authenticator = new JwtAuthenticator(accessToken) };

    //     var response = await new RestClient(UnityServicesURL).ExecuteAsync(request);
    //     if (response.IsSuccessful)
    //     {
    //         GD.Print("All good, not expired.");
    //     }
    //     else
    //     {
    //         GD.PrintErr(response.Content);
    //         throw response.ErrorException;
    //     }
    // }

    /// <summary>
    /// Deletes the session token if it exists.
    /// </summary>
    public void ClearSessionToken()
    {
        Error error = config.Load(CachePath);
        if (error != Error.Ok)
            return;

        config.EraseSectionKey(currentProfile, "sessionToken");
    }

    private void ClearAccessToken()
    {
        Error error = config.Load(CachePath);
        if (error != Error.Ok)
            return;

        config.EraseSectionKey(currentProfile, "idToken");
    }

    private void SaveCache()
    {
        config.SetValue(currentProfile, "idToken", AccessToken);
        config.SetValue(currentProfile, "sessionToken", SessionToken);

        config.Save(CachePath);
    }

    private void LoadCache()
    {
        Error error = config.Load(CachePath);
        if (error != Error.Ok)
            return;

        UserSession = new UserSession();
        if (config.HasSection(currentProfile))
        {
            UserSession = new UserSession
            {
                IdToken = (string)config.GetValue(currentProfile, "idToken"),
                SessionToken = (string)config.GetValue(currentProfile, "sessionToken")
            };
        }
    }

    private void SavePersistents()
    {
        config.SetValue(Persistents, "lastUsedProfile", currentProfile);

        config.Save(CachePath);
    }

    private void LoadPersistents()
    {
        Error error = config.Load(CachePath);
        if (error != Error.Ok)
            return;

        if (config.HasSection(Persistents))
            currentProfile = (string)config.GetValue(Persistents, "lastUsedProfile");
    }
}
