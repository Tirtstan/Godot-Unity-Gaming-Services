// Reference: https://services.docs.unity.com/docs/client-auth/

using System;
using System.Text;
using System.Threading.Tasks;
using Godot;
using RestSharp;
using RestSharp.Authenticators;
using Unity.Services.Core;

namespace Unity.Services.Authentication;

public partial class AuthenticationService : HttpRequest
{
    public static AuthenticationService Instance { get; private set; }
    private RestClient authClient;
    private const string URL = "https://services.api.unity.com";
    private const string AuthURL = "https://player-auth.services.api.unity.com/v1/authentication";
    private const string UsernamePasswordURL =
        "https://player-auth.services.api.unity.com/v1/authentication/usernamepassword";
    private const string SignUpURL =
        "https://player-auth.services.api.unity.com/v1/authentication/usernamepassword/sign-up";
    private const string SignInURL =
        "https://player-auth.services.api.unity.com/v1/authentication/usernamepassword/sign-in";
    private const string UpdatePasswordURL =
        "https://player-auth.services.api.unity.com/v1/authentication/usernamepassword/update-password";
    private const string JsonHeader = "Content-Type: application/json";
    private string IdToken;

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        var authOptions = new RestClientOptions(AuthURL) { ThrowOnAnyError = true };
        authClient = new RestClient(authOptions);
    }

    public async Task SignInAnonymously()
    {
        var request = new RestRequest("/anonymous", Method.Post).AddHeader(
            $"ProjectId",
            UnityServices.Instance.ProjectId
        );
        request.RequestFormat = DataFormat.Json;

        if (!string.IsNullOrEmpty(IdToken))
            request.AddHeader("Authorization", $"Bearer {IdToken}");

        var response = await authClient.ExecuteAsync(request);
        GD.Print("Content: " + response.Content);
    }

    public Error SignUpWithUsernamePassword(string username, string password)
    {
        if (ValidUsername(username) && ValidPassword(password))
            return UsernamePassword(SignUpURL, username, password);

        return Error.InvalidData;
    }

    public Error SignInWithUsernamePassword(string username, string password) =>
        UsernamePassword(SignInURL, username, password);

    public Error UpdatePassword(string currentPassword, string newPassword)
    {
        if (!ValidPassword(newPassword))
            return Error.InvalidData;

        try
        {
            string requestData =
                "{"
                + $@"""password"": ""{currentPassword}"", ""newPassword"": ""{newPassword}"""
                + "}";

            return Request(
                UpdatePasswordURL,
                new string[]
                {
                    JsonHeader,
                    $"ProjectId: {UnityServices.Instance.ProjectId}",
                    $"Authorization: Bearer {IdToken}"
                },
                HttpClient.Method.Post,
                requestData
            );
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
        }

        return Error.Failed;
    }

    /// <summary>
    /// <para> Username constraints:</para>
    /// <para>- Must be between 3-20 characters long.</para>
    /// <para>- Can only contain the following characters: a-z, 0-9, and the symbols [.][-][@][_].</para>
    /// <para>- Is case-insensitive.</para>
    /// </summary>
    /// <returns>Whether the username is valid</returns>
    private static bool ValidUsername(string username) // TODO
    {
        return true;
    }

    /// <summary>
    /// <para>Password Constraints:</para>
    /// <para>- Must be between 8-30 characters long.</para>
    /// <para>- Must contain at least one uppercase letter.</para>
    /// <para>- Must contain at least one lowercase letter.</para>
    /// <para>- Must contain at least one number.</para>
    /// <para>- Must contain at least one symbol.</para>
    /// </summary>
    /// <returns></returns>
    private static bool ValidPassword(string password) // TODO
    {
        return true;
    }

    private Error UsernamePassword(string url, string username, string password)
    {
        try
        {
            string requestData =
                "{" + $@"""username"": ""{username}"", ""password"": ""{password}""" + "}";

            return Request(
                url,
                new string[] { JsonHeader, $"ProjectId: {UnityServices.Instance.ProjectId}" },
                HttpClient.Method.Post,
                requestData
            );
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
        }

        return Error.Failed;
    }

    private void HttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body) // TODO: set idToken from body here
    {
        Godot.Collections.Dictionary json = Json.ParseString(Encoding.UTF8.GetString(body))
            .AsGodotDictionary();

        // idToken = json["idToken"].ToString();

        // GD.Print(
        //     $"Error: {(Error)result}\nResponse Code: {responseCode}\nHeaders: {PrintHeaders(headers)} \nBody: {json}"
        // );
    }

    private static string PrintHeaders(string[] headers)
    {
        string output = "";
        for (int i = 0; i < headers.Length; i++)
            output += i == 0 ? headers[i] : $", {headers[i]}";

        return output;
    }
}
