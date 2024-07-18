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
    public string ProjectId { get; private set; }
    public string PlayerId { get; private set; }

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
        return null;
    }
}
