namespace Unity.Services.CloudSave.Internal;

using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using Unity.Services.CloudSave.Models;
using Unity.Services.CloudSave.Models.Data.Player;

public class PlayerDataService
{
    /// <summary>
    /// Returns all keys stored in Cloud Save for the logged in player.
    /// </summary>
    /// <param name="options">Options to modify the behavior of the method, specifying AccessClass and PlayerId</param>
    /// <returns>A list of keys and their metadata as stored in the server for the logged in player.</returns>
    public async Task<List<ItemKey>> ListAllKeysAsync(ListAllKeysOptions options)
    {
        return null;
    }

    /// <summary>
    /// Downloads data from Cloud Save for the keys provided.
    /// There is no client validation in place for the provided keys.
    /// </summary>
    /// <param name="keys">The optional set of keys to load data for</param>
    /// <param name="options">Options to modify the behavior of the method, specifying AccessClass and PlayerId</param>
    /// <returns>The dictionary of all key-value pairs that represents the current state of data on the server including their write locks</returns>
    public async Task<Dictionary<string, Item>> LoadAsync(ISet<string> keys, LoadOptions options)
    {
        return null;
    }

    /// <summary>
    /// Downloads data from Cloud Save for all keys.
    /// </summary>
    /// <param name="options">Options to modify the behavior of the method, specifying AccessClass and PlayerId</param>
    /// <returns>The dictionary of all key-value pairs that represents the current state of data on the server including their write locks</returns>
    public async Task<Dictionary<string, Item>> LoadAllAsync(LoadAllOptions options)
    {
        return null;
    }

    /// <summary>
    /// Upload one or more key-value pairs to the Cloud Save service, with optional write lock validation.
    /// If a write lock is provided on an item and it does not match with the existing write lock, will throw a conflict exception.
    /// If the write lock for an item is set to null, the write lock validation for that item will be skipped and any existing value
    /// currently stored for that key will be overwritten.
    /// Keys can only contain alphanumeric characters, dashes, and underscores and be up to a length of 255 characters.
    /// <code>Dictionary</code> as a parameter ensures the uniqueness of given keys.
    /// There is no client validation in place, which means the API can be called regardless if data or keys are incorrect, invalid, and/or missing.
    /// </summary>
    /// <param name="data">The dictionary of keys and corresponding values to upload, together with optional write lock to check conflict</param>
    /// <param name="options">Options to modify the behavior of the method, specifying AccessClass and PlayerId</param>
    /// <returns>The dictionary of saved keys and the corresponding updated write lock</returns>
    public async Task<Dictionary<string, string>> SaveAsync(IDictionary<string, SaveItem> data, SaveOptions options)
    {
        return null;
    }

    /// <summary>
    /// Upload one or more key-value pairs to the Cloud Save service without write lock validation, overwriting any values
    /// that are currently stored under the given keys.
    /// Key can only contain alphanumeric characters, dashes, and underscores and be up to a length of 255 characters.
    /// <code>Dictionary</code> as a parameter ensures the uniqueness of given keys.
    /// There is no client validation in place, which means the API can be called regardless if data is incorrect, invalid, and/or missing.
    /// </summary>
    /// <param name="data">The dictionary of keys and corresponding values to upload</param>
    /// <param name="options">Options to modify the behavior of the method, specifying AccessClass</param>
    /// <returns>The dictionary of saved keys and the corresponding updated write lock</returns>
    public async Task<Dictionary<string, string>> SaveAsync(IDictionary<string, object> data, SaveOptions options)
    {
        return null;
    }

    /// <summary>
    /// Removes one key at a time, with optional write lock validation. If the given key doesn't exist, there is no feedback in place to inform a developer about it.
    /// If a write lock is provided and it does not match with the existing write lock, will throw a conflict exception.
    /// There is no client validation on the arguments for this method.
    /// </summary>
    /// <param name="key">The key to be removed from the server</param>
    /// <param name="options">The optional options object for specifying the write lock to check conflict in the server, as well as AccessClass</param>
    public async Task DeleteAsync(string key, DeleteOptions options) { }

    /// <summary>
    /// Removes all keys for the player without write lock validation.
    /// </summary>
    /// <param name="options">Options to modify the behavior of the method, specifying AccessClass and PlayerId</param>
    public async Task DeleteAllAsync(DeleteAllOptions options) { }

    /// <summary>
    /// Queries indexed player data from Cloud Save, and returns the requested keys for matching items.
    /// </summary>
    /// <param name="options">The query conditions to apply, including field filters and sort orders</param>
    /// <param name="options">Options to modify the behavior of the method, specifying AccessClass</param>
    /// <returns>The dictionary of all key-value pairs that represents the current state of data on the server including their write locks</returns>
    public async Task<List<EntityData>> QueryAsync(Query query, QueryOptions options)
    {
        return null;
    }
}
