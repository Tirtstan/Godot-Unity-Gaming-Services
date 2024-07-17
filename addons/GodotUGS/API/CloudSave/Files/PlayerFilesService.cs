namespace Unity.Services.CloudSave.Internal;

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
using Unity.Services.CloudSave.Internal.Models;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;

public class PlayerFilesService
{
    private RestClient playerFilesClient;
    private const string PlayerFilesURL = "https://cloud-save.services.api.unity.com/v1/files";
    private const int ByteLengthDifference = 180; // no idea why, but RestSharp or something is adding 180 bytes to the file size when sent
    private string ProjectId { get; }
    private string PlayerId { get; }

    public PlayerFilesService(string projectId, string playerId)
    {
        ProjectId = projectId;
        PlayerId = playerId;

        var options = new RestClientOptions(PlayerFilesURL)
        {
            Authenticator = new JwtAuthenticator(AuthenticationService.Instance.AccessToken)
        };
        playerFilesClient = new RestClient(
            options,
            configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions { })
        );

        playerFilesClient.AddDefaultHeaders(
            new Dictionary<string, string>
            {
                { "ProjectId", ProjectId },
                { "UnityEnvironment", UnityServices.Instance.Environment }
            }
        );
    }

    /// <summary>
    /// Returns all player-scoped files stored in Cloud Save for the logged in player.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// This method includes pagination.
    /// </summary>
    /// <returns>A list of file metadata for the files stored in Cloud Save for the logged in player.</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public async Task<List<FileItem>> ListAllAsync()
    {
        var request = new RestRequest($"projects/{ProjectId}/players/{PlayerId}/items")
        {
            RequestFormat = DataFormat.Json
        };

        var response = await playerFilesClient.ExecuteAsync<FileItemList>(request);
        if (response.IsSuccessful)
        {
            var results = response.Data.Results;
            return results.ConvertAll(item => new FileItem(item));
        }
        else
        {
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Returns the metadata of a file stored in Cloud Save for the logged in player.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <returns>The metadata of the specified file stored in Cloud Save for the logged in player</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public async Task<FileItem> GetMetadataAsync(string key)
    {
        var request = new RestRequest($"/projects/{ProjectId}/players/{PlayerId}/items/{key}/metadata")
        {
            RequestFormat = DataFormat.Json
        };

        var response = await playerFilesClient.ExecuteAsync<InternalFileItem>(request);
        if (response.IsSuccessful)
            return new FileItem(response.Data);
        else
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    /// <summary>
    /// Upload a player-scoped file to the Cloud Save service, overwriting if the file already exists.
    /// File name can only contain alphanumeric characters, dashes, and underscores and be up to a length of 255 characters.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <param name="key">The key at which to upload the file</param>
    /// <param name="path">The path to the file (DO NOT USE res:// or user://)</param>
    /// <param name="options">Options object with "WriteLock", the expected stored writeLock of the file - if this value is provided and is not a match then the operation will not succeed. If it is not provided then the operation will be performed regardless of the stored writeLock value.</param>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    /// <exception cref="InvalidOperationException">Thrown if request is invalid.</exception>
    public async Task SaveAsync(string key, string path, WriteLockOptions options = null)
    {
        if (path.StartsWith("res://") || path.StartsWith("user://"))
            throw new InvalidOperationException(
                "Path cannot start with res:// or user://, use an absolute path instead."
            );

        var request = new RestRequest($"/projects/{ProjectId}/players/{PlayerId}/items/{key}", Method.Post);
        byte[] fileBytes = System.IO.File.ReadAllBytes(path);

        string md5Hash;
        using (var md5 = MD5.Create())
        {
            var md5Bytes = md5.ComputeHash(fileBytes);
            md5Hash = Convert.ToBase64String(md5Bytes);
        }

        using (var file = FileAccess.Open(path, FileAccess.ModeFlags.Read))
        {
            var fileDetails = new FileDetails(
                ContentType.Binary,
                fileBytes.Length + ByteLengthDifference,
                md5Hash,
                options?.WriteLock
            );
            request.AddJsonBody(fileDetails);
        }

        var response = await playerFilesClient.ExecuteAsync<SignedUrlResponse>(request);
        if (response.IsSuccessful)
            await HandleUploadFileUrl(fileBytes, response.Data);
        else
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    /// <summary>
    /// Upload a player-scoped file to the Cloud Save service, overwriting if the file already exists.
    /// File name can only contain alphanumeric characters, dashes, and underscores and be up to a length of 255 characters.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <param name="key">The key at which to upload the file</param>
    /// <param name="bytes">The byte array containing the file data</param>
    /// <param name="options">Options object with "WriteLock", the expected stored writeLock of the file - if this value is provided and is not a match then the operation will not succeed. If it is not provided then the operation will be performed regardless of the stored writeLock value.</param>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public async Task SaveAsync(string key, byte[] bytes, WriteLockOptions options = null) { }

    /// <summary>
    /// Upload a player-scoped file to the Cloud Save service, overwriting if the file already exists.
    /// File name can only contain alphanumeric characters, dashes, and underscores and be up to a length of 255 characters.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <returns>A Stream containing the downloaded file data</returns>
    /// <param name="key">The key of the saved file to be loaded.</param>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    // public async Task<Stream> LoadStreamAsync(string key) { }

    /// <summary>
    /// Upload a player-scoped file to the Cloud Save service, overwriting if the file already exists.
    /// File name can only contain alphanumeric characters, dashes, and underscores and be up to a length of 255 characters.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <returns>A byte array containing the downloaded file data</returns>
    /// <param name="key">The key of the saved file to be loaded.</param>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public async Task<byte[]> LoadBytesAsync(string key)
    {
        return null;
    }

    /// <summary>
    /// Delete a player-scoped file form the Cloud Save service.
    /// File name can only contain alphanumeric characters, dashes, and underscores and be up to a length of 255 characters.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <param name="key">The key of the saved file to be deleted.</param>
    /// <param name="options">Options object with "WriteLock", the expected stored writeLock of the file - if this value is provided and is not a match then the operation will not succeed. If it is not provided then the operation will be performed regardless of the stored writeLock value.</param>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public async Task DeleteAsync(string key, WriteLockOptions options = null) { }

    private async Task HandleUploadFileUrl(byte[] bytes, SignedUrlResponse signedUrlResponse)
    {
        var method = signedUrlResponse.HttpMethod switch
        {
            "GET" => Method.Get,
            "POST" => Method.Post,
            "DELETE" => Method.Delete,
            "HEAD" => Method.Head,
            "OPTIONS" => Method.Options,
            "PATCH" => Method.Patch,
            "MERGE" => Method.Merge,
            "COPY" => Method.Copy,
            "SEARCH" => Method.Search,
            _ => Method.Put,
        };

        var request = new RestRequest(signedUrlResponse.SignedUrl, method).AddHeaders(
            signedUrlResponse.RequiredHeaders
        );
        request.RequestFormat = DataFormat.Json;
        request.AddFile("", bytes, "", ContentType.Binary);

        var response = await playerFilesClient.ExecuteAsync(request);
        GD.Print(response.Content);
        if (!response.IsSuccessful)
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);

        GD.Print("DONE!");
    }
}
