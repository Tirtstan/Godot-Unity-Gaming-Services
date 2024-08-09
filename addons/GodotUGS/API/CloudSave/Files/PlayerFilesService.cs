namespace Unity.Services.CloudSave.Internal;

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using Unity.Services.Authentication;
using Unity.Services.CloudSave.Internal.Models;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;

public interface IPlayerFilesService
{
    /// <summary>
    /// Returns all player-scoped files stored in Cloud Save for the logged in player.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// This method includes pagination.
    /// </summary>
    /// <returns>A list of file metadata for the files stored in Cloud Save for the logged in player.</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public Task<List<FileItem>> ListAllAsync();

    /// <summary>
    /// Returns the metadata of a file stored in Cloud Save for the logged in player.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <returns>The metadata of the specified file stored in Cloud Save for the logged in player</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public Task<FileItem> GetMetadataAsync(string key);

    /// <summary>
    /// Upload a player-scoped file to the Cloud Save service, overwriting if the file already exists.
    /// File name can only contain alphanumeric characters, dashes, and underscores and be up to a length of 255 characters.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <param name="key">The key at which to upload the file</param>
    /// <param name="stream">The path to the file (DO NOT USE res:// or user://)</param>
    /// <param name="options">Options object with "WriteLock", the expected stored writeLock of the file - if this value is provided and is not a match then the operation will not succeed. If it is not provided then the operation will be performed regardless of the stored writeLock value.</param>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    /// <exception cref="InvalidOperationException">Thrown if request is invalid.</exception>
    public Task SaveAsync(string key, Stream stream, WriteLockOptions options = null);

    /// <summary>
    /// Upload a player-scoped file to the Cloud Save service, overwriting if the file already exists.
    /// File name can only contain alphanumeric characters, dashes, and underscores and be up to a length of 255 characters.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <param name="key">The key at which to upload the file</param>
    /// <param name="bytes">The byte array containing the file data</param>
    /// <param name="options">Options object with "WriteLock", the expected stored writeLock of the file - if this value is provided and is not a match then the operation will not succeed. If it is not provided then the operation will be performed regardless of the stored writeLock value.</param>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public Task SaveAsync(string key, byte[] bytes, WriteLockOptions options = null);

    /// <summary>
    /// Upload a player-scoped file to the Cloud Save service, overwriting if the file already exists.
    /// File name can only contain alphanumeric characters, dashes, and underscores and be up to a length of 255 characters.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <returns>A Stream containing the downloaded file data</returns>
    /// <param name="key">The key of the saved file to be loaded.</param>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public Task<Stream> LoadStreamAsync(string key);

    /// <summary>
    /// Upload a player-scoped file to the Cloud Save service, overwriting if the file already exists.
    /// File name can only contain alphanumeric characters, dashes, and underscores and be up to a length of 255 characters.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <returns>A byte array containing the downloaded file data</returns>
    /// <param name="key">The key of the saved file to be loaded.</param>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public Task<byte[]> LoadBytesAsync(string key);

    /// <summary>
    /// Delete a player-scoped file form the Cloud Save service.
    /// File name can only contain alphanumeric characters, dashes, and underscores and be up to a length of 255 characters.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <param name="key">The key of the saved file to be deleted.</param>
    /// <param name="options">Options object with "WriteLock", the expected stored writeLock of the file - if this value is provided and is not a match then the operation will not succeed. If it is not provided then the operation will be performed regardless of the stored writeLock value.</param>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public Task DeleteAsync(string key, WriteLockOptions options = null);
}

public class PlayerFilesService : IPlayerFilesService
{
    private RestClient playerFilesClient;
    private const string PlayerFilesURL = "https://cloud-save.services.api.unity.com/v1/files";
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
            configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions { IncludeFields = true })
        );

        playerFilesClient.AddDefaultHeaders(UnityServices.Instance.DefaultHeaders);
    }

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

    public async Task SaveAsync(string key, Stream stream, WriteLockOptions options = null)
    {
        byte[] bytes;
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            bytes = memoryStream.ToArray();
        }

        await Save(key, bytes, options);
    }

    public async Task SaveAsync(string key, byte[] bytes, WriteLockOptions options = null) =>
        await Save(key, bytes, options);

    public async Task<Stream> LoadStreamAsync(string key)
    {
        var bytes = await Load(key);
        return new MemoryStream(bytes);
    }

    public async Task<byte[]> LoadBytesAsync(string key) => await Load(key);

    public async Task DeleteAsync(string key, WriteLockOptions options = null)
    {
        var request = new RestRequest(
            $"/projects/{ProjectId}/players/{PlayerId}/items/{key}",
            Method.Delete
        ).AddQueryParameter("writeLock", options?.WriteLock);
        request.RequestFormat = DataFormat.Json;

        var response = await playerFilesClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    private async Task Save(string key, byte[] bytes, WriteLockOptions options = null)
    {
        var request = new RestRequest($"/projects/{ProjectId}/players/{PlayerId}/items/{key}", Method.Post);

        string md5Hash;
        using (var md5 = MD5.Create())
        {
            var md5Bytes = md5.ComputeHash(bytes);
            md5Hash = Convert.ToBase64String(md5Bytes);
        }

        var fileDetails = new FileDetails(ContentType.Binary, bytes.Length, md5Hash, options?.WriteLock);
        request.AddJsonBody(fileDetails);

        var response = await playerFilesClient.ExecuteAsync<SignedUrlResponse>(request);
        if (response.IsSuccessful)
            await UploadFile(bytes, response.Data);
        else
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    private async Task<byte[]> Load(string key)
    {
        var request = new RestRequest($"/projects/{ProjectId}/players/{PlayerId}/items/{key}")
        {
            RequestFormat = DataFormat.Json
        };

        var response = await playerFilesClient.ExecuteAsync<SignedUrlResponse>(request);
        if (response.IsSuccessful)
            return await DownloadFile(response.Data);
        else
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    private async Task UploadFile(byte[] bytes, SignedUrlResponse signedUrlResponse)
    {
        var request = new RestRequest(signedUrlResponse.SignedUrl, StringToMethod(signedUrlResponse.HttpMethod))
        {
            RequestFormat = DataFormat.Json,
            AlwaysSingleFileAsContent = true
        }.AddFile("file", bytes, "", ContentType.Binary);

        if (signedUrlResponse?.RequiredHeaders != null)
            request.AddHeaders(signedUrlResponse.RequiredHeaders);

        var response = await playerFilesClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    private async Task<byte[]> DownloadFile(SignedUrlResponse signedUrlResponse)
    {
        var request = new RestRequest(signedUrlResponse.SignedUrl, StringToMethod(signedUrlResponse.HttpMethod))
        {
            RequestFormat = DataFormat.Json
        };

        if (signedUrlResponse?.RequiredHeaders != null)
            request.AddHeaders(signedUrlResponse.RequiredHeaders);

        var response = await playerFilesClient.ExecuteAsync(request);
        if (response.IsSuccessful)
            return response?.RawBytes;
        else
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    private static Method StringToMethod(string method)
    {
        return method switch
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
    }
}