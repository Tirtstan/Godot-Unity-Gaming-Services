namespace Unity.Services.CloudSave.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Internal.Models;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;

public class CustomDataService
{
    private RestClient customDataClient;
    private const string CustomDataURL = "https://cloud-save.services.api.unity.com/v1/data";
    private string ProjectId { get; }
    private string PlayerId { get; }

    public CustomDataService(string projectId, string playerId)
    {
        ProjectId = projectId;
        PlayerId = playerId;

        var options = new RestClientOptions(CustomDataURL)
        {
            Authenticator = new JwtAuthenticator(AuthenticationService.Instance.AccessToken)
        };
        customDataClient = new RestClient(
            options,
            configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions { })
        );

        customDataClient.AddDefaultHeaders(
            new Dictionary<string, string>
            {
                { "ProjectId", ProjectId },
                { "UnityEnvironment", UnityServices.Instance.Environment }
            }
        );
    }

    /// <summary>
    /// Returns all keys stored in Cloud Save for the specified custom data ID.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <returns>A list of keys and their metadata as stored in the server for the logged in player.</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public async Task<List<ItemKey>> ListAllKeysAsync(string customDataID)
    {
        var request = new RestRequest($"/projects/{ProjectId}/custom/{customDataID}/keys")
        {
            RequestFormat = DataFormat.Json
        };

        var response = await customDataClient.ExecuteAsync<ItemKeyList>(request);
        if (response.IsSuccessful)
            return response.Data.Results;
        else
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    /// <summary>
    /// Downloads items from Cloud Save for the custom data ID and keys provided.
    /// There is no client validation in place.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <returns>The dictionary of all key-value pairs that represents the current state of data on the server</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public async Task<Dictionary<string, Item>> LoadAsync(string customDataID, ISet<string> keys)
    {
        var request = new RestRequest($"/projects/{ProjectId}/custom/{customDataID}/items")
        {
            RequestFormat = DataFormat.Json
        };

        var keyArray = keys.ToArray();
        foreach (string key in keyArray)
            request.AddQueryParameter("keys", key);

        var response = await customDataClient.ExecuteAsync<ItemBatch>(request);
        if (response.IsSuccessful)
        {
            var dict = new Dictionary<string, Item>();
            foreach (var internalItem in response.Data.Results)
                dict.Add(internalItem.Key, new Item(internalItem));

            return dict;
        }
        else
        {
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Downloads all items from Cloud Save for the custom data ID.
    /// There is no client validation in place.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <returns>The dictionary of all key-value pairs that represents the current state of data on the server</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public async Task<Dictionary<string, Item>> LoadAllAsync(string customDataID)
    {
        var request = new RestRequest($"/projects/{ProjectId}/custom/{customDataID}/items")
        {
            RequestFormat = DataFormat.Json
        };

        var response = await customDataClient.ExecuteAsync<ItemBatch>(request);
        if (response.IsSuccessful)
        {
            var dict = new Dictionary<string, Item>();
            foreach (var internalItem in response.Data.Results)
                dict.Add(internalItem.Key, new Item(internalItem));

            return dict;
        }
        else
        {
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Queries indexed custom data from Cloud Save, and returns the requested keys for matching items.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// </summary>
    /// <param name="query">The query conditions to apply, including field filters and sort orders</param>
    /// <param name="options">Options to modify the behavior of the method</param>
    /// <returns>The dictionary of all key-value pairs that represents the current state of data on the server including their write locks</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    public async Task<List<EntityData>> QueryAsync(Query query)
    {
        var request = new RestRequest($"/projects/{ProjectId}/custom/query", Method.Post).AddJsonBody(query);

        var response = await customDataClient.ExecuteAsync<List<EntityData>>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }
}
