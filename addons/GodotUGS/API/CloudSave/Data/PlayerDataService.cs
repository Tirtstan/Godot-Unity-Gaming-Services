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
using Unity.Services.CloudSave.Internal.Models;
using Unity.Services.CloudSave.Models;
using Unity.Services.CloudSave.Models.Data.Player;
using Unity.Services.Core;

public interface IPlayerDataService
{
    /// <summary>
    /// Returns all keys stored in Cloud Save for the logged in player.
    /// </summary>
    /// <param name="options">Options to modify the behavior of the method, specifying AccessClass and PlayerId</param>
    /// <returns>A list of keys and their metadata as stored in the server for the logged in player.</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    /// <exception cref="InvalidOperationException">Thrown if request is unsuccessful.</exception>
    public Task<List<ItemKey>> ListAllKeysAsync(ListAllKeysOptions options = null);

    /// <summary>
    /// Downloads data from Cloud Save for the keys provided.
    /// There is no client validation in place for the provided keys.
    /// </summary>
    /// <param name="keys">The optional set of keys to load data for</param>
    /// <param name="options">Options to modify the behavior of the method, specifying AccessClass and PlayerId</param>
    /// <returns>The dictionary of all key-value pairs that represents the current state of data on the server including their write locks</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    /// <exception cref="InvalidOperationException">Thrown if request is unsuccessful.</exception>
    public Task<Dictionary<string, Item>> LoadAsync(ISet<string> keys, LoadOptions options = null);

    /// <summary>
    /// Downloads data from Cloud Save for all keys.
    /// </summary>
    /// <param name="options">Options to modify the behavior of the method, specifying AccessClass and PlayerId</param>
    /// <returns>The dictionary of all key-value pairs that represents the current state of data on the server including their write locks</returns>
    /// <returns>The dictionary of saved keys and the corresponding updated write lock</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    /// <exception cref="InvalidOperationException">Thrown if request is unsuccessful.</exception>
    public Task<Dictionary<string, Item>> LoadAllAsync(LoadAllOptions options = null);

    /// <summary>
    /// Upload one or more key-value pairs to the Cloud Save service, with optional write lock validation.
    /// If a write lock is provided on an item and it does not match with the existing write lock, will throw a conflict exception.
    /// If the write lock for an item is set to null, the write lock validation for that item will be skipped and any existing value
    /// currently stored for that key will be overwritten (Max 20 Items).
    /// Keys can only contain alphanumeric characters, dashes, and underscores and be up to a length of 255 characters.
    /// Throws a CloudSaveException with a reason code and explanation of what happened.
    /// <code>Dictionary</code> as a parameter ensures the uniqueness of given keys.
    /// There is no client validation in place, which means the API can be called regardless if data or keys are incorrect, invalid, and/or missing.
    /// </summary>
    /// <param name="data">The dictionary of keys and corresponding values to upload, together with optional write lock to check conflict</param>
    /// <param name="options">Options to modify the behavior of the method, specifying AccessClass and PlayerId</param>
    /// <returns>The dictionary of saved keys and the corresponding updated write lock</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    /// <exception cref="InvalidOperationException">Thrown if request is unsuccessful.</exception>
    public Task SaveAsync(IDictionary<string, SaveItem> data, SaveOptions options = null);

    /// <summary>
    /// Upload one or more key-value pairs to the Cloud Save service without write lock validation, overwriting any values
    /// that are currently stored under the given keys (Max 20 Items).
    /// Key can only contain alphanumeric characters, dashes, and underscores and be up to a length of 255 characters.
    /// <code>Dictionary</code> as a parameter ensures the uniqueness of given keys.
    /// There is no client validation in place, which means the API can be called regardless if data is incorrect, invalid, and/or missing.
    /// </summary>
    /// <param name="data">The dictionary of keys and corresponding values to upload</param>
    /// <param name="options">Options to modify the behavior of the method, specifying AccessClass</param>
    /// <returns>The dictionary of saved keys and the corresponding updated write lock</returns>
    /// <returns>The dictionary of saved keys and the corresponding updated write lock</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    /// <exception cref="InvalidOperationException">Thrown if request is unsuccessful.</exception>
    public Task SaveAsync(IDictionary<string, object> data, SaveOptions options = null);

    /// <summary>
    /// Removes one key at a time, with optional write lock validation. If the given key doesn't exist, there is no feedback in place to inform a developer about it.
    /// If a write lock is provided and it does not match with the existing write lock, will throw a conflict exception.
    /// There is no client validation on the arguments for this method.
    /// </summary>
    /// <param name="key">The key to be removed from the server</param>
    /// <param name="options">The optional options object for specifying the write lock to check conflict in the server, as well as AccessClass</param>
    /// <returns>The dictionary of saved keys and the corresponding updated write lock</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    /// <exception cref="InvalidOperationException">Thrown if request is unsuccessful.</exception>
    public Task DeleteAsync(string key, DeleteOptions options = null);

    /// <summary>
    /// Removes all keys for the player without write lock validation.
    /// </summary>
    /// <param name="options">Options to modify the behavior of the method, specifying AccessClass and PlayerId</param>
    /// <returns>The dictionary of saved keys and the corresponding updated write lock</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful.</exception>
    /// <exception cref="InvalidOperationException">Thrown if request is unsuccessful.</exception>
    public Task DeleteAllAsync(DeleteAllOptions options = null);

    /// <summary>
    /// Queries indexed player data from Cloud Save, and returns the requested keys for matching items.
    /// </summary>
    /// <param name="options">The query conditions to apply, including field filters and sort orders</param>
    /// <param name="options">Options to modify the behavior of the method, specifying AccessClass</param>
    /// <returns>The dictionary of all key-value pairs that represents the current state of data on the server including their write locks</returns>
    /// <returns>The dictionary of saved keys and the corresponding updated write lock</returns>
    /// <exception cref="CloudSaveException">Thrown if request is unsuccessful. </exception>
    /// <exception cref="InvalidOperationException">Thrown if request is unsuccessful.</exception>
    public Task<List<EntityData>> QueryAsync(Query query, QueryOptions options);
}

public class PlayerDataService : IPlayerDataService
{
    private RestClient playerDataClient;
    private const string PlayerDataURL = "https://cloud-save.services.api.unity.com/v1/data";
    private string ProjectId { get; }
    private string PlayerId { get; }

    public PlayerDataService(string projectId, string playerId)
    {
        ProjectId = projectId;
        PlayerId = playerId;

        var options = new RestClientOptions(PlayerDataURL)
        {
            Authenticator = new JwtAuthenticator(AuthenticationService.Instance.AccessToken)
        };
        playerDataClient = new RestClient(
            options,
            configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions { IncludeFields = true })
        );

        playerDataClient.AddDefaultHeaders(UnityServices.Instance.DefaultHeaders);
    }

    public async Task<List<ItemKey>> ListAllKeysAsync(ListAllKeysOptions options = null)
    {
        string access = options?.AccessClassOptions.AccessClass switch
        {
            AccessClass.Protected => "protected/keys",
            AccessClass.Public => "public/keys",
            AccessClass.Private
                => throw new InvalidOperationException(
                    "ListAllKeysAsync can only be called with Default, Protected or Public."
                ),
            _ => "keys",
        };

        var request = new RestRequest($"/projects/{ProjectId}/players/{PlayerId}/{access}")
        {
            RequestFormat = DataFormat.Json
        };

        var response = await playerDataClient.ExecuteAsync<ItemKeyList>(request);
        if (response.IsSuccessful)
            return response.Data.Results;
        else
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task<Dictionary<string, Item>> LoadAsync(ISet<string> keys, LoadOptions options = null)
    {
        string access = options?.AccessClassOptions.AccessClass switch
        {
            AccessClass.Protected => "protected/items",
            AccessClass.Public => "public/items",
            AccessClass.Private
                => throw new InvalidOperationException(
                    "LoadAsync can only be called with Default, Protected or Public."
                ),
            _ => "items",
        };

        var request = new RestRequest($"/projects/{ProjectId}/players/{PlayerId}/{access}")
        {
            RequestFormat = DataFormat.Json
        };

        var keyArray = keys.ToArray();
        foreach (string key in keyArray)
            request.AddQueryParameter("keys", key);

        var response = await playerDataClient.ExecuteAsync<ItemBatch>(request);
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

    public async Task<Dictionary<string, Item>> LoadAllAsync(LoadAllOptions options = null)
    {
        string access = options?.AccessClassOptions.AccessClass switch
        {
            AccessClass.Protected => "protected/items",
            AccessClass.Public => "public/items",
            AccessClass.Private
                => throw new InvalidOperationException(
                    "LoadAllAsync can only be called with Default, Protected or Public."
                ),
            _ => "items",
        };

        var request = new RestRequest($"/projects/{ProjectId}/players/{PlayerId}/{access}")
        {
            RequestFormat = DataFormat.Json
        };

        var response = await playerDataClient.ExecuteAsync<ItemBatch>(request);
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

    public async Task SaveAsync(IDictionary<string, SaveItem> data, SaveOptions options = null)
    {
        if (data == null || data.Count is 0 or > 20)
            return;

        string access = options?.AccessClassOptions.AccessClass switch
        {
            AccessClass.Public => "public/item-batch",
            AccessClass.Protected
            or AccessClass.Private
                => throw new InvalidOperationException("SaveAsync can only be called with Default or Public."),
            _ => "item-batch",
        };

        var itemBodies = new List<SetItemBody>();
        foreach (var item in data)
            itemBodies.Add(new SetItemBody(item.Key, item.Value.Value, item.Value.WriteLock));

        var items = new SetItemBatchBody(itemBodies);

        var request = new RestRequest($"/projects/{ProjectId}/players/{PlayerId}/{access}", Method.Post).AddJsonBody(
            items
        );

        var response = await playerDataClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task SaveAsync(IDictionary<string, object> data, SaveOptions options = null)
    {
        if (data == null || data.Count is 0 or > 20)
            return;

        string access = options?.AccessClassOptions.AccessClass switch
        {
            AccessClass.Public => "public/item-batch",
            AccessClass.Protected
            or AccessClass.Private
                => throw new InvalidOperationException("SaveAsync can only be called with Default or Public."),
            _ => "item-batch",
        };

        var itemBodies = new List<SetItemBody>();
        foreach (var item in data)
            itemBodies.Add(new SetItemBody(item.Key, item.Value, null));

        var items = new SetItemBatchBody(itemBodies);

        var request = new RestRequest($"/projects/{ProjectId}/players/{PlayerId}/{access}", Method.Post).AddJsonBody(
            items
        );

        var response = await playerDataClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task DeleteAsync(string key, DeleteOptions options = null)
    {
        string access = options?.AccessClassOptions.AccessClass switch
        {
            AccessClass.Public => "public/items",
            AccessClass.Protected
            or AccessClass.Private
                => throw new InvalidOperationException("DeleteAsync can only be called with Default or Public."),
            _ => "items",
        };

        var request = new RestRequest($"/projects/{ProjectId}/players/{PlayerId}/{access}/{key}", Method.Delete)
        {
            RequestFormat = DataFormat.Json
        };

        var response = await playerDataClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task DeleteAllAsync(DeleteAllOptions options = null)
    {
        string access = options?.AccessClassOptions.AccessClass switch
        {
            AccessClass.Public => "public/items",
            AccessClass.Protected
            or AccessClass.Private
                => throw new InvalidOperationException("DeleteAllAsync can only be called with Default or Public."),
            _ => "items",
        };

        var request = new RestRequest($"/projects/{ProjectId}/players/{PlayerId}/{access}", Method.Delete)
        {
            RequestFormat = DataFormat.Json
        };

        var response = await playerDataClient.ExecuteAsync(request);
        if (!response.IsSuccessful)
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }

    public async Task<List<EntityData>> QueryAsync(Query query, QueryOptions options)
    {
        string access = options.AccessClassOptions.AccessClass switch
        {
            AccessClass.Public => "public/query",
            _ => throw new InvalidOperationException("QueryAsync can only be called publicly."),
        };

        var request = new RestRequest($"/projects/{ProjectId}/players/{access}", Method.Post).AddJsonBody(query);

        var response = await playerDataClient.ExecuteAsync<List<EntityData>>(request);
        if (response.IsSuccessful)
            return response.Data;
        else
            throw new CloudSaveException(response.Content, response.ErrorMessage, response.ErrorException);
    }
}
