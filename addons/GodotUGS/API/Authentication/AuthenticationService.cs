namespace Unity.Services.Authentication;

// References: https://services.docs.unity.com/docs/client-auth && https://restsharp.dev/docs/usage/basics

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Godot;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using Unity.Services.Authentication.Internal;
using Unity.Services.Authentication.Internal.Models;
using Unity.Services.Authentication.Models;
using Unity.Services.Core;

public interface IAuthenticationService
{
    /// <summary>
    /// Invoked when a sign-in attempt has completed successfully.
    /// </summary>
    public event Action SignedIn;

    /// <summary>
    /// Invoked when a sign-out attempt has completed successfully.
    /// </summary>
    public event Action SignedOut;

    /// <summary>
    /// Checks whether the player is signed in or not.
    /// A player can remain signed in but have an expired session.
    /// </summary>
    /// <returns>Returns true if player is signed in, else false.</returns>
    public bool IsSignedIn { get; }

    /// <summary>
    /// Returns the current player's access token when they are signed in, otherwise null.
    /// </summary>
    public string AccessToken { get; }

    /// <summary>
    /// Returns the current player's ID. This value is cached between sessions.
    /// </summary>
    public string PlayerId { get; }

    /// <summary>
    /// Returns the current player's name. This value is cached between sessions.
    /// </summary>
    public string PlayerName { get; }

    /// <summary>
    /// The profile isolates the values saved to the PlayerPrefs.
    /// You can use profiles to sign in to multiple accounts on a single device.
    /// Use the <see cref="SwitchProfile(string)"/> method to change this value.
    /// </summary>
    public string Profile { get; }

    /// <summary>
    /// Check if there is an existing session token stored for the current profile.
    /// </summary>
    public bool SessionTokenExists { get; }

    /// <summary>
    /// The date the last notification for the player was created or null if there are no notifications
    /// </summary>
    public string LastNotificationDate { get; }

    /// <summary>
    /// Returns the current player's info, including linked identities.
    /// </summary>
    public PlayerInfo PlayerInfo { get; }

    /// <summary>
    /// Returns player's notifications after GetNotificationsAsync is called successfully.
    /// </summary>
    public List<Notification> Notifications { get; }

    /// <summary>
    /// Signs in the current player anonymously. No credentials are required and the session is confined to the current device.
    /// </summary>
    /// <remarks>
    /// If a player has signed in previously with a session token stored on the device, they are signed back in regardless of if they're an anonymous player or not.
    /// </remarks>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task SignInAnonymouslyAsync();

    /// <summary>
    /// Sign in using Apple's ID token.
    /// If no options are used, this will create an account if none exist.
    /// </summary>
    /// <param name="idToken">Apple's ID token</param>
    /// <param name="options">Options for the operation</param>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.</exception>
    public Task SignInWithAppleAsync(string idToken, SignInOptions options = null);

    /// <summary>
    /// Link the current player with the Apple account using Apple's ID token.
    /// </summary>
    /// <param name="idToken">Apple's ID token</param>
    /// <param name="options">Options for the link operations.</param>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task LinkWithAppleAsync(string idToken, LinkOptions options = null);

    /// <summary>
    /// Unlinks the Apple account from the current player account.
    /// </summary>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task UnlinkAppleAsync();

    /// <summary>
    /// Sign in using AppleGameCenter's teamPlayerId.
    /// If no options are used, this will create an account if none exist.
    /// </summary>
    /// <param name="signature">AppleGameCenter's signature</param>
    /// <param name="teamPlayerId">AppleGameCenter's teamPlayerId</param>
    /// <param name="publicKeyURL">AppleGameCenter's publicKeyURL</param>
    /// <param name="salt">AppleGameCenter's salt</param>
    /// <param name="timestamp">AppleGameCenter's timestamp</param>
    /// <param name="options">Options for the operation</param>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task SignInWithAppleGameCenterAsync(
        string signature,
        string teamPlayerId,
        string publicKeyURL,
        string salt,
        ulong timestamp,
        SignInOptions options = null
    );

    /// <summary>
    /// Link the current player with the AppleGameCenter account using AppleGameCenter's teamPlayerId.
    /// </summary>
    /// <param name="signature">AppleGameCenter's signature</param>
    /// <param name="teamPlayerId">AppleGameCenter's teamPlayerId</param>
    /// <param name="publicKeyURL">AppleGameCenter's publicKeyURL</param>
    /// <param name="salt">AppleGameCenter's salt</param>
    /// <param name="timestamp">AppleGameCenter's timestamp</param>
    /// <param name="options">Options for the link operations.</param>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task LinkWithAppleGameCenterAsync(
        string signature,
        string teamPlayerId,
        string publicKeyURL,
        string salt,
        ulong timestamp,
        LinkOptions options = null
    );

    /// <summary>
    /// Unlinks the AppleGameCenter account from the current player account.
    /// </summary>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task UnlinkAppleGameCenterAsync();

    /// <summary>
    /// Sign in using Google's ID token.
    /// If no options are used, this will create an account if none exist.
    /// </summary>
    /// <param name="idToken">Google's ID token</param>
    /// <param name="options">Options for the operation</param>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task SignInWithGoogleAsync(string idToken, SignInOptions options = null);

    /// <summary>
    /// Link the current player with the Google account using Google's ID token.
    /// </summary>
    /// <param name="idToken">Google's ID token</param>
    /// <param name="options">Options for the link operations.</param>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task LinkWithGoogleAsync(string idToken, LinkOptions options = null);

    /// <summary>
    /// Unlinks the Google account from the current player account.
    /// </summary>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task UnlinkGoogleAsync();

    /// <summary>
    /// Sign in using Google Play Games' authorization code.
    /// If no options are used, this will create an account if none exist.
    /// </summary>
    /// <param name="authCode">Google Play Games' authorization code</param>
    /// <param name="options">Options for the operation</param>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task SignInWithGooglePlayGamesAsync(string authCode, SignInOptions options = null);

    /// <summary>
    /// Link the current player with the Google play games account using Google play games' authorization code.
    /// </summary>
    /// <param name="authCode">Google play games' authorization code</param>
    /// <param name="options">Options for the link operations.</param>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task LinkWithGooglePlayGamesAsync(string authCode, LinkOptions options = null);

    /// <summary>
    /// Unlinks the Google play games account from the current player account.
    /// </summary>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task UnlinkGooglePlayGamesAsync();

    /// <summary>
    /// Sign in using Facebook's access token.
    /// If no options are used, this will create an account if none exist.
    /// </summary>
    /// <param name="accessToken">Facebook's access token</param>
    /// <param name="options">Options for the operation</param>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task SignInWithFacebookAsync(string accessToken, SignInOptions options = null);

    /// <summary>
    /// Link the current player with the Facebook account using Facebook's access token.
    /// </summary>
    /// <param name="accessToken">Facebook's access token</param>
    /// <param name="options">Options for the link operations.</param>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task LinkWithFacebookAsync(string accessToken, LinkOptions options = null);

    /// <summary>
    /// Unlinks the Facebook account from the current player account.
    /// </summary>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task UnlinkFacebookAsync();

    /// <summary>
    /// Sign in using Steam's session ticket.
    /// If no options are used, this will create an account if none exist.
    /// </summary>
    /// <param name="sessionTicket">Steam's session ticket</param>
    /// <param name="identity">The identity of the calling service</param>
    /// <param name="options">Options for the operation</param>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task SignInWithSteamAsync(string sessionTicket, string identity, SignInOptions options = null);

    /// <summary>
    /// Link the current player with the Steam account using Steam's session ticket.
    /// </summary>
    /// <param name="sessionTicket">Steam's session ticket</param>
    /// <param name="identity">The identity of the calling service</param>
    /// <param name="options">Options for the link operations.</param>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task LinkWithSteamAsync(string sessionTicket, string identity, LinkOptions options = null);

    /// <summary>
    /// Sign in using Steam's session ticket.
    /// If no options are used, this will create an account if none exist.
    /// </summary>
    /// <param name="sessionTicket">Steam's session ticket</param>
    /// <param name="identity">The identity of the calling service</param>
    /// <param name="appId">App Id that was used to generate the ticket. Only required for additional app ids (e.g.: PlayTest, Demo, etc)</param>
    /// <param name="options">Options for the operation</param>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task SignInWithSteamAsync(string sessionTicket, string identity, string appId, SignInOptions options = null);

    /// <summary>
    /// Link the current player with the Steam account using Steam's session ticket.
    /// </summary>
    /// <param name="sessionTicket">Steam's session ticket</param>
    /// <param name="identity">The identity of the calling service</param>
    /// <param name="appId">App Id that was used to generate the ticket. Only required for additional app ids (e.g.: PlayTest, Demo, etc)</param>
    /// <param name="options">Options for the link operations.</param>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task LinkWithSteamAsync(string sessionTicket, string identity, string appId, LinkOptions options = null);

    /// <summary>
    /// Unlinks the Steam account from the current player account.
    /// </summary>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task UnlinkSteamAsync();

    /// <summary>
    /// Sign in using Username and Password credentials.
    /// </summary>
    /// <param name="username">Username of the player. Note that it must be unique per project and contains 3-20 characters of alphanumeric and/or these special characters [. - @ _].</param>
    /// <param name="password">Password of the player. Note that it must contain 8-30 characters with at least 1 upper case, 1 lower case, 1 number, and 1 special character.</param>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task SignInWithUsernamePasswordAsync(string username, string password);

    /// <summary>
    /// Sign up using Username and Password credentials.
    /// </summary>
    /// <param name="username">Username of the player. Note that it must be unique per project and contains 3-20 characters of alphanumeric and/or these special characters [. - @ _].</param>
    /// <param name="password">Password of the player. Note that it must contain 8-30 characters with at least 1 upper case, 1 lower case, 1 number, and 1 special character.</param>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task SignUpWithUsernamePasswordAsync(string username, string password);

    /// <summary>
    /// Sign up with a new Username/Password and add it to the current logged in user.
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task AddUsernamePasswordAsync(string username, string password);

    /// <summary>
    /// Update Password credentials for username/password user.
    /// </summary>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task UpdatePasswordAsync(string currentPassword, string newPassword);

    /// <summary>
    /// Deletes the currently signed in player permanently.
    /// </summary>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task DeleteAccountAsync();

    /// <summary>
    /// Sign out the current player.
    /// </summary>
    /// <param name="clearCredentials">Option to clear the session token that enables logging in to the same account</param>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public void SignOut(bool clearCredentials = false);

    /// <summary>
    /// Returns the info of the logged in player, which includes the player's id, creation time and linked identities.
    /// </summary>
    /// <returns>Task for the operation</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task<PlayerInfo> GetPlayerInfoAsync();

    /// <summary>
    /// Retrieves the Notifications that were created for the signed in player
    /// </summary>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task<List<Notification>> GetNotificationsAsync();

    /// <summary>
    /// Switch the current profile.
    /// You can use profiles to sign in to multiple accounts on a single device.
    /// </summary>
    /// <param name="profile">The profile to switch to.</param>
    public void SwitchProfile(string profileName);

    /// <summary>
    /// Returns the name of the logged in player if it has been set.
    /// If no name has been set, this will return null if autoGenerate is set to false.
    /// </summary>
    /// <param name="autoGenerate">Option auto generate a player name if none already exist. Defaults to true</param>
    /// <returns>Task for the operation with the resulting player name</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task<string> GetPlayerNameAsync(bool autoGenerate = true);

    /// <summary>
    /// Updates the player name of the logged in player.
    /// </summary>
    /// <param name="name">The new name for the player. It must not contain spaces.</param>
    /// <returns>Task for the operation with the resulting player name</returns>
    /// <exception cref="AuthenticationException">
    /// The task fails with the exception when the task cannot complete successfully due to Authentication specific errors.
    /// </exception>
    public Task<string> UpdatePlayerNameAsync(string name);

    /// <summary>
    /// Deletes the session token if it exists.
    /// </summary>
    public void ClearSessionToken();
}

public partial class AuthenticationService : Node, IAuthenticationService
{
    public static AuthenticationService Instance { get; private set; }
    public event Action SignedIn;
    public event Action SignedOut;
    public bool IsSignedIn { get; private set; }
    public string AccessToken => SignInResponse.IdToken;
    internal string EnvironmentId { get; private set; }
    public string PlayerId => SignInResponse.User.Id;
    public string PlayerName => SignInResponse.User.Username;
    public string Profile => currentProfile;
    public bool SessionTokenExists => !string.IsNullOrEmpty(SessionToken);
    public string LastNotificationDate =>
        DateTime.UnixEpoch.AddMilliseconds(SignInResponse.LastNotificationDate).ToString();
    public PlayerInfo PlayerInfo { get; private set; }
    public List<Notification> Notifications { get; private set; }

    private RestClient authClient;
    private ConfigFile config = new();
    private SignInResponse SignInResponse = new();
    private AccessToken accessToken = new();
    private string SessionToken => SignInResponse.SessionToken;
    private const string AuthURL = "https://player-auth.services.api.unity.com/v1";
    private const string SteamIdentityRegex = @"^[a-zA-Z0-9]{5,30}$";
    private const string CachePath = "user://GodotUGS_UserCache.cfg";
    private const string Persistents = "Persistents";
    private string currentProfile = "DefaultProfile";

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        var options = new RestClientOptions(AuthURL);
        authClient = new RestClient(
            options,
            configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions { IncludeFields = true })
        );
        authClient.AddDefaultHeaders(UnityServices.Instance.DefaultHeaders);

        LoadPersistents();
        LoadCache();

        SignedIn += OnSignIn;
        SignedOut += OnSignOut;
    }

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

        var response = await authClient.ExecuteAsync<SignInResponse>(request);
        if (response.IsSuccessful)
        {
            SignInResponse = response.Data;
            SaveCache();
            SignedIn?.Invoke();
        }
        else
        {
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    private async Task SignInWithSessionToken(string sessionToken)
    {
        var request = new RestRequest("/authentication/session-token", Method.Post).AddJsonBody(new { sessionToken });

        var response = await authClient.ExecuteAsync<SignInResponse>(request);
        if (response.IsSuccessful)
        {
            SignInResponse = response.Data;
            SaveCache();
            SignedIn?.Invoke();
        }
        else
        {
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    public async Task SignInWithAppleAsync(string idToken, SignInOptions options = null)
    {
        await SignInWithExternalTokenAsync(
            IdProviderKeys.Apple,
            new SignInWithExternalTokenRequest
            {
                IdProvider = IdProviderKeys.Apple,
                Token = idToken,
                SignInOnly = !options?.CreateAccount ?? false
            }
        );
    }

    public async Task LinkWithAppleAsync(string idToken, LinkOptions options = null)
    {
        await LinkWithExternalTokenAsync(
            IdProviderKeys.Apple,
            new LinkWithExternalTokenRequest
            {
                IdProvider = IdProviderKeys.Apple,
                Token = idToken,
                ForceLink = options?.ForceLink ?? false
            }
        );
    }

    public async Task UnlinkAppleAsync()
    {
        await UnlinkExternalTokenAsync(IdProviderKeys.Apple);
    }

    public async Task SignInWithAppleGameCenterAsync(
        string signature,
        string teamPlayerId,
        string publicKeyURL,
        string salt,
        ulong timestamp,
        SignInOptions options = null
    )
    {
        await SignInWithExternalTokenAsync(
            IdProviderKeys.AppleGameCenter,
            new SignInWithAppleGameCenterRequest
            {
                IdProvider = IdProviderKeys.AppleGameCenter,
                Token = signature,
                AppleGameCenterConfig = new AppleGameCenterConfig
                {
                    TeamPlayerId = teamPlayerId,
                    PublicKeyURL = publicKeyURL,
                    Salt = salt,
                    Timestamp = timestamp
                },
                SignInOnly = !options?.CreateAccount ?? false
            }
        );
    }

    public async Task LinkWithAppleGameCenterAsync(
        string signature,
        string teamPlayerId,
        string publicKeyURL,
        string salt,
        ulong timestamp,
        LinkOptions options = null
    )
    {
        await LinkWithExternalTokenAsync(
            IdProviderKeys.AppleGameCenter,
            new LinkWithAppleGameCenterRequest
            {
                IdProvider = IdProviderKeys.AppleGameCenter,
                Token = signature,
                AppleGameCenterConfig = new AppleGameCenterConfig
                {
                    TeamPlayerId = teamPlayerId,
                    PublicKeyURL = publicKeyURL,
                    Salt = salt,
                    Timestamp = timestamp
                },
                ForceLink = options?.ForceLink ?? false
            }
        );
    }

    public async Task UnlinkAppleGameCenterAsync()
    {
        await UnlinkExternalTokenAsync(IdProviderKeys.AppleGameCenter);
    }

    public async Task SignInWithGoogleAsync(string idToken, SignInOptions options = null)
    {
        await SignInWithExternalTokenAsync(
            IdProviderKeys.Google,
            new SignInWithExternalTokenRequest
            {
                IdProvider = IdProviderKeys.Google,
                Token = idToken,
                SignInOnly = !options?.CreateAccount ?? false
            }
        );
    }

    public async Task LinkWithGoogleAsync(string idToken, LinkOptions options = null)
    {
        await LinkWithExternalTokenAsync(
            IdProviderKeys.Google,
            new LinkWithExternalTokenRequest
            {
                IdProvider = IdProviderKeys.Google,
                Token = idToken,
                ForceLink = options?.ForceLink ?? false
            }
        );
    }

    public async Task UnlinkGoogleAsync()
    {
        await UnlinkExternalTokenAsync(IdProviderKeys.Google);
    }

    public async Task SignInWithGooglePlayGamesAsync(string authCode, SignInOptions options = null)
    {
        await SignInWithExternalTokenAsync(
            IdProviderKeys.GooglePlayGames,
            new SignInWithExternalTokenRequest
            {
                IdProvider = IdProviderKeys.GooglePlayGames,
                Token = authCode,
                SignInOnly = !options?.CreateAccount ?? false
            }
        );
    }

    public async Task LinkWithGooglePlayGamesAsync(string authCode, LinkOptions options = null)
    {
        await LinkWithExternalTokenAsync(
            IdProviderKeys.GooglePlayGames,
            new LinkWithExternalTokenRequest
            {
                IdProvider = IdProviderKeys.GooglePlayGames,
                Token = authCode,
                ForceLink = options?.ForceLink ?? false
            }
        );
    }

    public async Task UnlinkGooglePlayGamesAsync()
    {
        await UnlinkExternalTokenAsync(IdProviderKeys.GooglePlayGames);
    }

    public async Task SignInWithFacebookAsync(string accessToken, SignInOptions options = null)
    {
        await SignInWithExternalTokenAsync(
            IdProviderKeys.Facebook,
            new SignInWithExternalTokenRequest
            {
                IdProvider = IdProviderKeys.Facebook,
                Token = accessToken,
                SignInOnly = !options?.CreateAccount ?? false
            }
        );
    }

    public async Task LinkWithFacebookAsync(string accessToken, LinkOptions options = null)
    {
        await LinkWithExternalTokenAsync(
            IdProviderKeys.Facebook,
            new LinkWithExternalTokenRequest
            {
                IdProvider = IdProviderKeys.Facebook,
                Token = accessToken,
                ForceLink = options?.ForceLink ?? false
            }
        );
    }

    public async Task UnlinkFacebookAsync()
    {
        await UnlinkExternalTokenAsync(IdProviderKeys.Facebook);
    }

    public async Task SignInWithSteamAsync(string sessionTicket, string identity, SignInOptions options = null)
    {
        ValidateSteamIdentity(identity);

        await SignInWithExternalTokenAsync(
            IdProviderKeys.Steam,
            new SignInWithSteamRequest
            {
                IdProvider = IdProviderKeys.Steam,
                Token = sessionTicket,
                SteamConfig = new SteamConfig { identity = identity },
                SignInOnly = !options?.CreateAccount ?? false
            }
        );
    }

    public async Task LinkWithSteamAsync(string sessionTicket, string identity, LinkOptions options = null)
    {
        ValidateSteamIdentity(identity);

        await LinkWithExternalTokenAsync(
            IdProviderKeys.Steam,
            new LinkWithSteamRequest
            {
                IdProvider = IdProviderKeys.Steam,
                Token = sessionTicket,
                SteamConfig = new SteamConfig { identity = identity },
                ForceLink = options?.ForceLink ?? false
            }
        );
    }

    public async Task SignInWithSteamAsync(
        string sessionTicket,
        string identity,
        string appId,
        SignInOptions options = null
    )
    {
        ValidateSteamIdentity(identity);

        await SignInWithExternalTokenAsync(
            IdProviderKeys.Steam,
            new SignInWithSteamRequest
            {
                IdProvider = IdProviderKeys.Steam,
                Token = sessionTicket,
                SteamConfig = new SteamConfig { identity = identity, appId = appId },
                SignInOnly = !options?.CreateAccount ?? false
            }
        );
    }

    public async Task LinkWithSteamAsync(
        string sessionTicket,
        string identity,
        string appId,
        LinkOptions options = null
    )
    {
        ValidateSteamIdentity(identity);

        await LinkWithExternalTokenAsync(
            IdProviderKeys.Steam,
            new LinkWithSteamRequest
            {
                IdProvider = IdProviderKeys.Steam,
                Token = sessionTicket,
                SteamConfig = new SteamConfig { identity = identity, appId = appId },
                ForceLink = options?.ForceLink ?? false
            }
        );
    }

    public async Task UnlinkSteamAsync()
    {
        await UnlinkExternalTokenAsync(IdProviderKeys.Steam);
    }

    public async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        var request = new RestRequest("/authentication/usernamepassword/sign-in", Method.Post).AddJsonBody(
            new { username, password }
        );

        var response = await authClient.ExecuteAsync<SignInResponse>(request);
        if (response.IsSuccessful)
        {
            SignInResponse = response.Data;
            SaveCache();
            SignedIn?.Invoke();
        }
        else
        {
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    public async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        var request = new RestRequest("/authentication/usernamepassword/sign-up", Method.Post).AddJsonBody(
            new { username, password }
        );

        var response = await authClient.ExecuteAsync<SignInResponse>(request);
        if (response.IsSuccessful)
        {
            SignInResponse = response.Data;
            SaveCache();
            SignedIn?.Invoke();
        }
        else
        {
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

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

        var response = await authClient.ExecuteAsync<SignInResponse>(request);
        if (response.IsSuccessful)
        {
            SignInResponse = response.Data;
            SaveCache();
            SignedIn?.Invoke();
        }
        else
        {
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    public async Task UpdatePasswordAsync(string currentPassword, string newPassword)
    {
        var request = new RestRequest("/authentication/usernamepassword/update-password", Method.Post).AddJsonBody(
            new { password = currentPassword, newPassword }
        );
        request.Authenticator = new JwtAuthenticator(AccessToken);

        var response = await authClient.ExecuteAsync<SignInResponse>(request);
        if (response.IsSuccessful)
        {
            SignInResponse = response.Data;
            SaveCache();
            SignedIn?.Invoke();
        }
        else
        {
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    public async Task DeleteAccountAsync()
    {
        var request = new RestRequest($"/users/{PlayerId}", Method.Delete)
        {
            Authenticator = new JwtAuthenticator(AccessToken),
            RequestFormat = DataFormat.Json
        };

        var response = await authClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);

        SignOut(true);
    }

    public void SignOut(bool clearCredentials = false)
    {
        if (clearCredentials)
        {
            ClearAccessToken();
            ClearSessionToken();
        }

        SignInResponse = new SignInResponse();
        SignedOut?.Invoke();
    }

    public async Task<PlayerInfo> GetPlayerInfoAsync()
    {
        var request = new RestRequest($"/users/{PlayerId}")
        {
            RequestFormat = DataFormat.Json,
            Authenticator = new JwtAuthenticator(AccessToken)
        };

        var response = await authClient.ExecuteAsync<PlayerInfoResponse>(request);
        if (response.IsSuccessful)
            return PlayerInfo = new PlayerInfo(response.Data);
        else
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);
    }

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
            return Notifications = response.Data.Notifications;
        }
        else
        {
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);
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

    public async Task<string> GetPlayerNameAsync(bool autoGenerate = true)
    {
        var request = new RestRequest($"https://social.services.api.unity.com/v1/names/{PlayerId}").AddQueryParameter(
            "autoGenerate",
            autoGenerate
        );
        request.Authenticator = new JwtAuthenticator(AccessToken);
        request.RequestFormat = DataFormat.Json;

        var response = await authClient.ExecuteAsync<PlayerName>(request);
        if (response.IsSuccessful)
            return response.Data.Name;
        else
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task<string> UpdatePlayerNameAsync(string name)
    {
        var request = new RestRequest(
            $"https://social.services.api.unity.com/v1/names/{PlayerId}",
            Method.Post
        ).AddJsonBody(new { name });
        request.Authenticator = new JwtAuthenticator(AccessToken);

        var response = await authClient.ExecuteAsync<PlayerName>(request);
        if (response.IsSuccessful)
            return response.Data.Name;
        else
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    private async Task GetJWKSAsync()
    {
        var request = new RestRequest("https://player-auth.services.api.unity.com/.well-known/jwks.json");
        var response = await authClient.ExecuteAsync(request);
        GD.Print(response.Content);

        if (response.IsSuccessful)
            GD.Print("Success!");
        else
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public void ClearSessionToken()
    {
        Error error = config.Load(CachePath);
        if (error != Error.Ok)
            return;

        try
        {
            config.EraseSectionKey(currentProfile, "sessionToken");
            config.Save(CachePath);
        }
        catch { }
    }

    private void ClearAccessToken()
    {
        Error error = config.Load(CachePath);
        if (error != Error.Ok)
            return;

        try
        {
            config.EraseSectionKey(currentProfile, "idToken");
            config.Save(CachePath);
        }
        catch { }
    }

    private async Task SignInWithExternalTokenAsync(string idProvider, SignInWithExternalTokenRequest tokenRequest)
    {
        var request = new RestRequest($"/authentication/external-token/{idProvider}", Method.Post).AddJsonBody(
            tokenRequest
        );

        var response = await authClient.ExecuteAsync<SignInResponse>(request);
        if (response.IsSuccessful)
        {
            SignInResponse = response.Data;
            SaveCache();
            SignedIn?.Invoke();
        }
        else
        {
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    private async Task LinkWithExternalTokenAsync(string idProvider, LinkWithExternalTokenRequest tokenRequest)
    {
        var request = new RestRequest($"/authentication/link/{idProvider}", Method.Post)
        {
            Authenticator = new JwtAuthenticator(AccessToken)
        }.AddJsonBody(tokenRequest);

        var response = await authClient.ExecuteAsync<LinkResponse>(request);
        if (response.IsSuccessful)
        {
            PlayerInfo?.AddExternalIdentity(
                response.Data.User?.ExternalIds?.FirstOrDefault(x => x.ProviderId == tokenRequest.IdProvider)
            );
        }
        else
        {
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    private async Task UnlinkExternalTokenAsync(string idProvider)
    {
        var request = new RestRequest($"/authentication/unlink/{idProvider}", Method.Post)
        {
            Authenticator = new JwtAuthenticator(AccessToken)
        }.AddJsonBody(new { externalId = PlayerInfo?.GetIdentityId(idProvider) });

        var response = await authClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw new AuthenticationException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    private void ValidateSteamIdentity(string identity)
    {
        if (string.IsNullOrEmpty(identity))
        {
            throw new InvalidOperationException("Identity cannot be null or empty.");
        }

        if (!Regex.IsMatch(identity, SteamIdentityRegex))
        {
            throw new InvalidOperationException(
                "The provided identity must only contain alphanumeric characters and be between 5 and 30 characters in length."
            );
        }
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

        SignInResponse = new SignInResponse();
        if (config.HasSection(currentProfile))
        {
            SignInResponse = new SignInResponse
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

    private void OnSignIn()
    {
        IsSignedIn = true;
        accessToken = JwtDecoder.Decode<AccessToken>(AccessToken);
        EnvironmentId = accessToken.Audience.FirstOrDefault(s => s.StartsWith("envId:"))?.Substring(6);
    }

    private void OnSignOut()
    {
        IsSignedIn = false;
    }

    public override void _ExitTree()
    {
        SignedIn -= OnSignIn;
        SignedOut -= OnSignOut;
    }
}
