namespace Unity.Services.Economy;

using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Economy.Models;

public class EconomyConfiguration
{
    private RestClient configClient;
    private const string ConfigURL = "https://economy.services.api.unity.com/v2";
    private string ProjectId { get; }
    private string PlayerId { get; }
    private string ConfigAssignmentHash;
    private List<ConfigurationItemDefinition> cachedConfig;
    private bool isSynced;

    private const string CurrencyTypeString = "CurrencyResource";
    private const string InventoryItemTypeString = "InventoryItemResource";
    private const string VirtualPurchaseTypeString = "VirtualPurchaseResource";
    private const string RealMoneyPurchaseTypeString = "RealMoneyPurchaseResource";

    private const string CurrencyType = "CURRENCY";
    private const string InventoryItemType = "INVENTORY_ITEM";
    private const string VirtualPurchaseType = "VIRTUAL_PURCHASE";
    private const string RealMoneyPurchaseType = "MONEY_PURCHASE";

    public EconomyConfiguration(string projectId, string playerId)
    {
        ProjectId = projectId;
        PlayerId = playerId;

        var options = new RestClientOptions(ConfigURL)
        {
            Authenticator = new JwtAuthenticator(AuthenticationService.Instance.AccessToken)
        };
        configClient = new RestClient(
            options,
            configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions { })
        );

        configClient.AddDefaultHeaders(UnityServices.Instance.DefaultHeaders);
    }

    /// <summary>
    /// Gets the currently published Economy configuration and caches it.
    /// </summary>
    /// <returns>A list of ConfigurationItemDefinition</returns>
    /// <exception cref="EconomyException"></exception>
    public async Task<List<ConfigurationItemDefinition>> SyncConfigurationAsync()
    {
        var request = new RestRequest($"/projects/{ProjectId}/players/{PlayerId}/config/resources")
        {
            RequestFormat = DataFormat.Json
        };

        var response = await configClient.ExecuteAsync<ConfigurationItemDefinitionList>(request);
        if (response.IsSuccessful)
        {
            isSynced = true;
            ConfigAssignmentHash = response.Data.Metadata.ConfigAssignmentHash;
            cachedConfig = response.Data.Results;
            return response.Data.Results;
        }
        else
        {
            throw new EconomyException(response.Content, response.ErrorMessage, response.ErrorException);
        }
    }

    /// <summary>
    /// Gets the currencies from the cached configuration.
    /// </summary>
    /// <returns>A list of CurrencyDefinition</returns>
    /// <exception cref="EconomyException"></exception>
    // public List<CurrencyDefinition> GetCurrencies() { }

    /// <summary>
    /// Gets the inventory items from the cached configuration.
    /// </summary>
    /// <returns>A list of InventoryItemDefinition</returns>
    /// <exception cref="EconomyException"></exception>
    // public List<InventoryItemDefinition> GetInventoryItems() { }

    /// <summary>
    /// Gets the virtual purchases from the cached configuration.
    /// </summary>
    /// <returns>A list of VirtualPurchaseDefinition</returns>
    /// <exception cref="EconomyException"></exception>
    // public List<VirtualPurchaseDefinition> GetVirtualPurchases() { }

    /// <summary>
    /// Gets the real money purchases from the cached configuration.
    /// </summary>
    /// <returns>A list of RealMoneyPurchaseDefinition</returns>
    /// <exception cref="EconomyException"></exception>
    // public List<RealMoneyPurchaseDefinition> GetRealMoneyPurchases() { }

    /// <summary>
    /// Gets a specific currency from the cached config.
    /// </summary>
    /// <param name="id">The ID of the currency to fetch.</param>
    /// <returns>A CurrencyDefinition or null if the currency doesn't exist.</returns>
    /// <exception cref="EconomyException">Thrown if request is unsuccessful</exception>
    // public CurrencyDefinition GetCurrency(string id) { }

    /// <summary>
    /// Gets a specific inventory item from the cached config.
    /// </summary>
    /// <param name="id">The ID of the inventory item to fetch.</param>
    /// <returns>A InventoryItemDefinition or null if the currency doesn't exist.</returns>
    /// <exception cref="EconomyException">Thrown if request is unsuccessful</exception>
    // public InventoryItemDefinition GetInventoryItem(string id) { }

    /// <summary>
    /// Gets a specific virtual purchase from the cached config.
    /// </summary>
    /// <param name="id">The ID of the virtual purchase to fetch.</param>
    /// <returns>A VirtualPurchaseDefinition or null if the currency doesn't exist.</returns>
    /// <exception cref="EconomyException">Thrown if request is unsuccessful</exception>
    // public VirtualPurchaseDefinition GetVirtualPurchase(string id) { }

    /// <summary>
    /// Gets a specific real money purchase from the cached config.
    /// </summary>
    /// <param name="id">The ID of the real money purchase to fetch.</param>
    /// <returns>A RealMoneyPurchaseDefinition or null if the currency doesn't exist.</returns>
    /// <exception cref="EconomyException">Thrown if request is unsuccessful</exception>
    // public RealMoneyPurchaseDefinition GetRealMoneyPurchase(string id) { }

    /// <summary>
    /// Returns the cached config assignment hash.
    /// </summary>
    /// <returns>A config assignment hash or null if Economy doesn't have one</returns>
    public string GetConfigAssignmentHash()
    {
        if (!isSynced)
            return null;

        return ConfigAssignmentHash;
    }
}
